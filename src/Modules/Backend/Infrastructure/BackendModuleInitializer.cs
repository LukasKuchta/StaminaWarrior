using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading;
using Backend.Application;
using Backend.Application.Abstractions.Clock;
using Backend.Application.Abstractions.Commands;
using Backend.Application.Contracts;
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
using Backend.Infrastructure.Quartz;
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
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Quartz;
using Quartz.Impl;
using Quartz.Logging;
using Quartz.Spi;
using RedLockNet;
using RedLockNet.SERedis;
using RedLockNet.SERedis.Configuration;
using Serilog.Core;
using static Quartz.Logging.OperationName;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace Backend.Infrastructure;

public static class BackendModuleInitializer
{
    public static void Initialize(
        string connectionString,
        RedisOptions redisOptions,
        ProcessOutboxMessagesOptions outboxOptions,
        ProcessInternalCommandsOptions internalCommandsOptions)
    {
        if (redisOptions is null)
        {
            throw new ArgumentNullException(nameof(redisOptions));
        }

        if (outboxOptions is null)
        {
            throw new ArgumentNullException(nameof(outboxOptions));
        }

        if (internalCommandsOptions is null)
        {
            throw new ArgumentNullException(nameof(internalCommandsOptions));
        }

        var serviceCollection = new ServiceCollection();

        serviceCollection.AddApplication();
        serviceCollection.AddInfrastructure(
            connectionString,
            redisOptions,
            outboxOptions,
            internalCommandsOptions);

        IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

        QuartzInitializer.Initialize(internalCommandsOptions, outboxOptions);

        BackendCompositionRoot.SetContainer(serviceProvider);
    }


    private static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        [NotNull] string connectionString,
        [NotNull] RedisOptions redisOptions,
        [NotNull] ProcessOutboxMessagesOptions processOutboxMessagesOptions,
        [NotNull] ProcessInternalCommandsOptions processInternalCommandsOptions)
    {
        services.AddScoped<IRequestHandler<ProcessOutboxCommand>, ProcessOutboxCommandHandler>();
        services.AddScoped<IRequestHandler<ProcessInternalCommandsCommand>, ProcessInternalCommandsCommandHandler>();
        services.AddScoped<IInternalCommandMarker, InternalCommandMarker>();

        services.AddScoped<IPublicEventPublisher, PublicEventPublisher>();

        // BUG https://github.com/jbogard/MediatR/issues/718
        services.Decorate(typeof(INotificationHandler<>), typeof(DomainEventHandlerDecorator<>));

        services.Decorate(typeof(INotificationHandler<>), typeof(PublicEventHandlerDecorator<>));


        services.AddSingleton<IOptions<RedisOptions>>(_ => Options.Create<RedisOptions>(redisOptions));
        services.AddSingleton<IOptions<ProcessOutboxMessagesOptions>>(_ => Options.Create<ProcessOutboxMessagesOptions>(processOutboxMessagesOptions));
        services.AddSingleton<IOptions<ProcessInternalCommandsOptions>>(_ => Options.Create<ProcessInternalCommandsOptions>(processInternalCommandsOptions));

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = redisOptions.GetConnectionString;
        });

        services.AddSingleton<IDistributedLockFactory, RedLockFactory>(x =>
        {
            var opt = x.GetRequiredService<IOptions<RedisOptions>>().Value;

            return RedLockFactory.Create(new List<RedLockEndPoint> { new DnsEndPoint(opt.Endpoint, opt.Port) });
        });

        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();


        services.AddDbContext<BackendApplicationDbContext>(builder =>
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
        services.AddScoped<IUnitOfWork, BackendUnitOfWork>();
        services.AddScoped<IDomainEventsDispatcher, DomainEventsDispatcher>();
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

        // add jobs
        services.AddTransient<ProcessOutboxMessagesJob>();
        services.AddTransient<ProcessInternalCommandsJob>();

        return services;
    }

}


