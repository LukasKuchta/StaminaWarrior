using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.Infrastructure.Messaging.InternalCommands;
using Backend.Infrastructure.Messaging.Outbox;
using Quartz.Impl;
using Quartz;

namespace Backend.Infrastructure.Quartz;
internal static class QuartzInitializer
{
    private const string messageProcessingJobGroup = "MessageProcessingJobGroup";

    public static async void Initialize(ProcessInternalCommandsOptions internalCommandsOptions,
        ProcessOutboxMessagesOptions outboxOptions)
    {
        ISchedulerFactory schedulerFactory = new StdSchedulerFactory();
        IScheduler scheduler = await schedulerFactory.GetScheduler().ConfigureAwait(false);
        scheduler.JobFactory = new UseMsServiceProviderJobFactory(); // create jobs from DI
        await scheduler.Start().ConfigureAwait(false);

        var outboxJob = JobBuilder.Create<ProcessOutboxMessagesJob>().WithIdentity(nameof(ProcessOutboxMessagesJob), messageProcessingJobGroup).Build();

        var internalCommandsJob = JobBuilder.Create<ProcessInternalCommandsJob>().WithIdentity(nameof(ProcessInternalCommandsJob), messageProcessingJobGroup).Build();

        await scheduler.ScheduleJob(
            internalCommandsJob,
            CreateJobTrigger(
                 nameof(ProcessInternalCommandsJob),
                 outboxOptions.JobProcessingInterval)).ConfigureAwait(false);

        await scheduler.ScheduleJob(
            outboxJob,
            CreateJobTrigger(
                nameof(ProcessOutboxMessagesJob),
                internalCommandsOptions.JobProcessingInterval)).ConfigureAwait(false);
    }


    private static async Task StartSchedulerAsync(IScheduler scheduler, CancellationToken cancellationToken)
    {
        if (scheduler is null)
        {
            throw new InvalidOperationException("The scheduler should have been initialized first.");
        }

        await scheduler.Start(cancellationToken).ConfigureAwait(false);
    }

    private static ITrigger CreateJobTrigger(string id, int processingInternval)
    {
        return TriggerBuilder
                   .Create()
                     .WithIdentity(id, messageProcessingJobGroup)
                     .StartNow()
                      .WithSimpleSchedule(x => x
                           .WithIntervalInSeconds(processingInternval)
                           .RepeatForever())
                   .Build();
    }
}
