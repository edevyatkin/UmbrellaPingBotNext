using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Text;
using Hangfire;
using Hangfire.LiteDB;
using Hangfire.Storage;
using UmbrellaPingBotNext.Jobs;

namespace UmbrellaPingBotNext
{
    class JobServer : IDisposable
    {
        private readonly BackgroundJobServer _server;
        private readonly TimeZoneInfo _timeZoneInfo;

        public JobServer() {
            GlobalConfiguration.Configuration.UseLiteDbStorage();
            var options = new BackgroundJobServerOptions() {
                SchedulePollingInterval = TimeSpan.FromSeconds(5)
            };
            _server = new BackgroundJobServer(options);

            using (var connection = JobStorage.Current.GetConnection())
                foreach (var recurringJob in StorageConnectionExtensions.GetRecurringJobs(connection)) {
                    RecurringJob.RemoveIfExists(recurringJob.Id);
                }

            try {
                string timezoneId = string.Empty;
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    timezoneId = "Russian Standard Time";
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                    timezoneId = "Europe/Moscow";
                else
                    throw new TimeZoneNotFoundException("Timezone not found");
                _timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timezoneId);
            }
            catch (Exception) {
                throw;
            }
        }
        
        public void AddDaily<T>(int hour, int minute) where T: IJob {
            RecurringJob.AddOrUpdate<T>(
                    recurringJobId: $"{typeof(T).Name}{hour}{minute}",
                    methodCall: j => j.Do(),
                    cronExpression: Cron.Daily(hour, minute),
                    timeZone: _timeZoneInfo
                ); 
        }

        public void DisplayJobs() {
            var jobs = JobStorage.Current.GetConnection().GetRecurringJobs();
            jobs.ForEach(dto => Console.WriteLine(dto.NextExecution.Value.ToLocalTime()));
        }

        public void Dispose() {
            _server.Dispose();
        }
    }
}
