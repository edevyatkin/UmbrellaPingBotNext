using Hangfire;
using Hangfire.Storage;
using WebhookApp.Common;

namespace WebhookApp
{
    public class JobManager
    {
        public JobManager()
        {
            using var connection = JobStorage.Current.GetConnection();
            foreach (var recurringJob in connection.GetRecurringJobs())
            {
                RecurringJob.RemoveIfExists(recurringJob.Id);
            }
        }
        
        public void AddDaily<T>(int hour, int minute) where T: IJob {
            RecurringJob.AddOrUpdate<T>(
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
