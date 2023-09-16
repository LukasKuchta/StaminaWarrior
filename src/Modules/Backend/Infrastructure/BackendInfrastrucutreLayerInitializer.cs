using System.Net;
using Backend.Application;
using Backend.Application.Abstractions.Clock;
using Backend.Application.Abstractions.Commands;
using Backend.Domain.Users;
using Backend.Domain.WarriorPaths;
using Backend.Domain.Warriors;
using Backend.Infrastructure.Clock;
using Backend.Infrastructure.Data;
using Backend.Infrastructure.DistributedCaching;
using Backend.Infrastructure.Exceptions;
using Backend.Infrastructure.Messaging.DomainEvents;
using Backend.Infrastructure.Messaging.InternalCommands;
using Backend.Infrastructure.Messaging.Outbox;
using Backend.Infrastructure.Messaging.PublicEvents;
using Backend.Infrastructure.Repositories;
using Backend.Infrastructure.Serialization;
using BuildingBlocks.Application.Data;
using BuildingBlocks.Application.InternalCommands;
using BuildingBlocks.Application.PublicEvents;
using BuildingBlocks.Domain;
using BuildingBlocks.Infrastructure.DomainEventsDispatching;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Quartz;
using RedLockNet;
using RedLockNet.SERedis;
using RedLockNet.SERedis.Configuration;

namespace Backend.Infrastructure;

public static class BackendInfrastrucutreLayerInitializer
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        if (configuration is null)
        {
            throw new ArgumentNullException(nameof(configuration));
        }
        
        string? connectionString = configuration.GetConnectionString("Database");
        if (connectionString is null)
        {
            throw new ConnectionStringMissingException();
        }

        services.AddScoped<IRequestHandler<ProcessOutboxCommand>, ProcessOutboxCommandHandler>();
        services.AddScoped<IRequestHandler<ProcessInternalCommandsCommand>, ProcessInternalCommandsCommandHandler>();
        services.AddScoped<IInternalCommandMarker, InternalCommandMarker>();

        services.AddScoped<IPublicEventPublisher, PublicEventPublisher>();

        // BUG https://github.com/jbogard/MediatR/issues/718
        services.Decorate(typeof(INotificationHandler<>), typeof(DomainEventHandlerDecorator<>));

        services.Decorate(typeof(INotificationHandler<>), typeof(PublicEventHandlerDecorator<>));

        services.Configure<RedisOptions>(configuration.GetSection("RedisOptions"));
        services.Configure<ProcessOutboxMessagesOptions>(configuration.GetSection("Outbox"));
        services.Configure<ProcessInternalCommandsOptions>(configuration.GetSection("InternalCommands"));

        services.AddStackExchangeRedisCache(options =>
        {
            var opt = new RedisOptions();
            configuration.GetSection(nameof(RedisOptions)).Bind(opt);

            options.Configuration = opt.GetConnectionString();
        });

        services.AddSingleton<IDistributedLockFactory, RedLockFactory>(x =>
        {
            var opt = x.GetRequiredService<IOptions<RedisOptions>>().Value;

            return RedLockFactory.Create(new List<RedLockEndPoint> { new DnsEndPoint(opt.Endpoint, opt.Port) });
        });

        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

      
        services.AddDbContext<ApplicationDbContext>(builder =>
        {
            // EFC uses titleCase for name of the tables and columns
            // pgsql uses snake case
            builder
            .UseNpgsql(connectionString)
            .UseSnakeCaseNamingConvention();
        });

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IWarriorRepository, WarriorRepository>();
        services.AddScoped<IWarriorPathRepository, WarriorPathRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IDomainEventsDispatcher, DomainEventsDispatcher>();
        services.AddScoped<ICommandExecutor, CommandExecutor>();
        services.AddTransient<ISerializer, NewtonsoftJsonSerializer>();

        services.AddScoped<ICommandScheduler, CommandScheduler>();

        services.AddSingleton<ISqlConnectionFactory>(_ => new SqlConnectionFactory(connectionString));

        services.AddSingleton<IPublicEventRegister>(ctx =>
        {

            IDictionary<string, IList<Type>> register = new Dictionary<string, IList<Type>>();

            IEnumerable<Type> publicEvents = BackendApplicationAssembly.Instance
              .GetTypes()
              .Where(s =>
                s.Name.EndsWith("PublicEvent", StringComparison.InvariantCulture)
                && !s.IsInterface)
              .ToList();

            foreach (Type type in publicEvents)
            {
                var evt = type.GetInterfaces()
                  .Where(i =>
                      i.IsGenericType
                      && i.GetGenericTypeDefinition() == typeof(IPublicEvent<>)
                      && i.GetGenericArguments().Length == 1)
                   .Select(o => new
                   {
                       DomainEventTypeName = o.GetGenericArguments()[0].Name,
                       PublicDomaineEventType = type,
                   })
                  .FirstOrDefault();

                if (evt is not null)
                {
                    if (!register.ContainsKey(evt.DomainEventTypeName))
                    {
                        register.Add(evt.DomainEventTypeName, new List<Type>());
                    }

                    register[evt.DomainEventTypeName].Add(evt.PublicDomaineEventType);
                }
            }

            return new PublicEventRegister(register);
        });

        AddBackgroundJobs(services, configuration);

        return services;
    }

    public static void AddBackgroundJobs(IServiceCollection services, IConfiguration configuration)
    {
        services.AddQuartz(q =>
        {
            //q.UseMicrosoftDependencyInjectionJobFactory();
        });

        services.AddQuartzHostedService(opt => opt.WaitForJobsToComplete = true);

        services.ConfigureOptions<ProcessOutboxMessageJobSetup>();
        services.ConfigureOptions<ProcessInternalCommandsJobSetup>();
    }
}
