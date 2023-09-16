using Microsoft.Extensions.Options;
using Quartz;

namespace Backend.Infrastructure.Messaging.Outbox;
internal sealed class ProcessOutboxMessageJobSetup : IConfigureOptions<QuartzOptions>
{
    private readonly ProcessOutboxMessagesOptions _outboxOptions;

    public ProcessOutboxMessageJobSetup(IOptions<ProcessOutboxMessagesOptions> outboxOptions)
    {
        _outboxOptions = outboxOptions.Value;
    }

    public void Configure(QuartzOptions options)
    {
        const string jobName = nameof(ProcessOutboxMessagesJob);

        options
            .AddJob<ProcessOutboxMessagesJob>(config => config.WithIdentity(jobName))
            .AddTrigger(config =>
                config.ForJob(jobName)
                .WithSimpleSchedule(
                     schedule => schedule.WithIntervalInSeconds(_outboxOptions.JobProcessingInterval).RepeatForever()));
    }
}
