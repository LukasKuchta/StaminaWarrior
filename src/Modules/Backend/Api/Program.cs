using Backend.Api.Extensions;
using Backend.Application;
using Backend.Application.Contracts;
using Backend.Infrastructure;
using Backend.Infrastructure.DistributedCaching;
using Backend.Infrastructure.Messaging.InternalCommands;
using Backend.Infrastructure.Messaging.Outbox;
using Serilog;
using ILogger = Serilog.ILogger;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IBackendModule, BackendModule>();
//builder.Services.AddScoped<ISynchronizationModule, SynchronizationModule>();


RedisOptions? redisOption = builder.Configuration.GetSection("RedisOptions").Get<RedisOptions>();
ProcessOutboxMessagesOptions? processOutboxMessagesOptions = builder.Configuration.GetSection("ProcessOutboxMessagesOptions").Get<ProcessOutboxMessagesOptions>();
ProcessInternalCommandsOptions? processInternalCommandsOptions = builder.Configuration.GetSection("ProcessInternalCommandsOptions").Get<ProcessInternalCommandsOptions>();

if (redisOption is null)
{
    throw new ArgumentNullException(nameof(redisOption), "Missing configuration");
}

if (processOutboxMessagesOptions is null)
{
    throw new ArgumentNullException(nameof(processOutboxMessagesOptions), "Missing configuration");
}

if (processInternalCommandsOptions is null)
{
    throw new ArgumentNullException(nameof(processInternalCommandsOptions), "Missing configuration");
}

string? connectionString = builder.Configuration.GetConnectionString("Database");

if (string.IsNullOrEmpty(connectionString))
{
    throw new ArgumentNullException(nameof(connectionString), "Missing connection string");
}


BackendModuleInitializer.Initialize(

    connectionString,
    redisOption,
    processOutboxMessagesOptions,
    processInternalCommandsOptions);

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    //app.ApplyMigrations();
}

app.UseHttpsRedirection();

app.UseCustomExpcetionHandling();

app.MapControllers();

app.Run();
