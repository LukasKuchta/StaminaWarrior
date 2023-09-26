using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Quartz.Spi;
using Quartz;

namespace Backend.Infrastructure.Quartz;
internal class UseMsServiceProviderJobFactory : IJobFactory
{
    public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
    {
        using (IServiceScope scope = BackendCompositionRoot.CreateScope())
        {
            return (IJob)scope.ServiceProvider.GetRequiredService(bundle.JobDetail.JobType);
        }
    }

    public void ReturnJob(IJob job)
    {
        (job as IDisposable)?.Dispose();
    }
}
