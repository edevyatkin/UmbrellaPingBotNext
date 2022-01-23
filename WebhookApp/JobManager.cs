using System;
using System.Runtime.InteropServices;
using Hangfire;
using WebhookApp.Common;

namespace WebhookApp
{
    public class JobManager
    {
        public JobManager() {
            
        }
        
        public void AddDaily<T>(int hour, int minute) where T: IJob {
            RecurringJob.AddOrUpdate<T>(
                    recurringJobId: $"{typeof(T).Name}{hour}{minute}",
                    methodCall: j => j.Do(),
                    cronExpression: Cron.Daily(hour, minute),
                    timeZone: Constants.BotTimeZoneInfo
                ); 
        }
    }
}
