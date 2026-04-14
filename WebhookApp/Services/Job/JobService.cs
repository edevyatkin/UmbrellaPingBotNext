using Hangfire;
using Hangfire.Storage;
using WebhookApp.Common;

namespace WebhookApp.Services.Job
{
    public class JobService
    {
        private readonly IRecurringJobManager _recurringJobManager;

        public JobService(IRecurringJobManager recurringJobManager, JobStorage jobStorage)
        {
            _recurringJobManager = recurringJobManager;
            using var connection = jobStorage.GetConnection();
            foreach (var recurringJob in connection.GetRecurringJobs())
            {
                recurringJobManager.RemoveIfExists(recurringJob.Id);
            }
        }
        
        public void AddDaily<T>(int hour, int minute) where T: IJob {
            
            _recurringJobManager.AddOrUpdate<T>(
                    recurringJobId: $"{typeof(T).Name}{hour}{minute}",
                    methodCall: j => j.Do(),
                    cronExpression: Cron.Daily(hour, minute),
                    options: new RecurringJobOptions
                    {
                        TimeZone = Constants.BotTimeZoneInfo
                    }
                ); 
        }
    }
}
