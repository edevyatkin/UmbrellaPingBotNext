using WebhookApp.Jobs;
using WebhookApp.Services.Smoothie;

namespace WebhookApp.Services.Job;

public static class JobServiceExtensions
{
    public static void AddAllJobs(this JobService jobService)
    {
        jobService.AddDaily<SmoothieResetJob>(3,0);

        jobService.AddDaily<BattleNotificationJob>(9, 30);
        jobService.AddDaily<BattleNotificationPingJob>(9, 31);
        jobService.AddDaily<BattleNotificationPingJob>(9, 55);
        jobService.AddDaily<FinishBattleOperationJob>(10, 00);
            
        jobService.AddDaily<BattleNotificationJob>(12, 30);
        jobService.AddDaily<BattleNotificationPingJob>(12, 31);
        jobService.AddDaily<BattleNotificationPingJob>(12, 55);
        jobService.AddDaily<FinishBattleOperationJob>(13, 00);
            
        jobService.AddDaily<BattleNotificationJob>(15, 30);
        jobService.AddDaily<BattleNotificationPingJob>(15, 31);
        jobService.AddDaily<BattleNotificationPingJob>(15, 55);
        jobService.AddDaily<FinishBattleOperationJob>(16, 00);
            
        jobService.AddDaily<BattleNotificationJob>(18, 30);
        jobService.AddDaily<BattleNotificationPingJob>(18, 31);
        jobService.AddDaily<FactoryEatNotificationJob>(18,32);
        jobService.AddDaily<BattleNotificationPingJob>(18, 55);
        jobService.AddDaily<FinishBattleOperationJob>(19, 00);

        jobService.AddDaily<LotteryPingJob>(19,17);
            
        jobService.AddDaily<BattleNotificationJob>(21, 30);
        jobService.AddDaily<BattleNotificationPingJob>(21, 31);
        jobService.AddDaily<BattleNotificationPingJob>(21, 55);
        jobService.AddDaily<FinishBattleOperationJob>(22, 00);
        jobService.AddDaily<SleepNotificationJob>(22,05);
        jobService.AddDaily<SleepNotificationJob>(23,00);
    }
}