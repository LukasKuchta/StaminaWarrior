using Microsoft.Extensions.Options;
using Quartz;

namespace Backend.Infrastructure.Messaging.InternalCommands;
internal sealed class ProcessInternalCommandsJobSetup : IConfigureOptions<QuartzOptions>
{
    private readonly ProcessInternalCommandsOptions _options;

    public ProcessInternalCommandsJobSetup(IOptions<ProcessInternalCommandsOptions> options)
    {
        _options = options.Value;
    }

    public void Configure(QuartzOptions options)
    {
        const string jobName = nameof(ProcessInternalCommandsJob);

        options
            .AddJob<ProcessInternalCommandsJob>(config => config.WithIdentity(jobName))
            .AddTrigger(config =>
                config.ForJob(jobName)
                .WithSimpleSchedule(
                     schedule => 
                        schedule.WithIntervalInSeconds(_options.JobProcessingInterval).RepeatForever()));
    }
}
