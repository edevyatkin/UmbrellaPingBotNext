using System;
using System.Runtime.InteropServices;
using Hangfire;

namespace WebhookApp
{
    public class JobManager
    {
        private readonly TimeZoneInfo _timeZoneInfo;

        public JobManager() {
            string timezoneId;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                timezoneId = "Russian Standard Time";
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                timezoneId = "Europe/Moscow";
            else
                throw new TimeZoneNotFoundException("Timezone not found");
            _timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timezoneId);
        }
        
        public void AddDaily<T>(int hour, int minute) where T: IJob {
            RecurringJob.AddOrUpdate<T>(
                    recurringJobId: $"{typeof(T).Name}{hour}{minute}",
                    methodCall: j => j.Do(),
                    cronExpression: Cron.Daily(hour, minute),
                    timeZone: _timeZoneInfo
                ); 
        }
    }
}
