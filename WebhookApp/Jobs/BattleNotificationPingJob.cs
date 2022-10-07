using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using WebhookApp.Services.Battle;
using WebhookApp.Services.Ping;
using WebhookApp.Services.Polls;

namespace WebhookApp.Jobs
{
    class BattleNotificationPingJob : IJob
    {
        private readonly ILogger<BattleNotificationPingJob> _logger;
        private readonly IBattleService _battleService;
        private readonly IPingService _pingService;

        public BattleNotificationPingJob(ILogger<BattleNotificationPingJob> logger, IBattleService battleService, IPingService pingService) {
            _logger = logger;
            _battleService = battleService;
            _pingService = pingService;
        }
        
        public async Task Do() {
            var polls = PollsHelper.Polls;
            foreach (var poll in polls.Values) {
                var allUsersToPing = await _battleService.GetUsersToPingAsync(poll.ChatId);
                _logger.LogInformation($"Battle Notification Ping, chatId: {poll.ChatId.ToString()}");
                var usersToPing = allUsersToPing.Except(poll.Votes.Select(v => new Abstractions.User(v.Username))).ToList();
                await _pingService.PingUsersAsync(usersToPing, poll.ChatId);
            }
        }
    }
}
