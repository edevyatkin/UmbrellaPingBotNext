using WebhookApp.Jobs;

namespace WebhookApp
{
    internal static class JobManagerExtensions
    {
        internal static void AddBattleNotification(this JobManager manager, int hour, int minute) {
            manager.AddDaily<BattleNotificationJob>(hour, minute);
        }

        internal static void AddBattleNotificationPing(this JobManager manager, int hour, int minute) {
            manager.AddDaily<BattleNotificationPingJob>(hour, minute);
        }

        internal static void AddFinishBattleOperation(this JobManager manager, int hour, int minute) {
            manager.AddDaily<FinishBattleOperationJob>(hour, minute);
        }
    }
}
