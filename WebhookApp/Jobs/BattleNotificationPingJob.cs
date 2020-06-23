using System;
using System.Linq;
using System.Threading.Tasks;
using WebhookApp.Services;

namespace WebhookApp.Jobs
{
    class BattleNotificationPingJob : IJob
    {
        private readonly BotService _botService;
        private readonly ConfigService _configService;

        public BattleNotificationPingJob(BotService botService, ConfigService configService) {
            _botService = botService;
            _configService = configService;
        }
        
        public async Task Do() {
            var polls = PollsHelper.Polls;
            BotConfig config = await _configService.LoadAsync();
            var usernames = config.Usernames;
            foreach (var poll in polls.Values) {
                if (!usernames.ContainsKey(poll.ChatId))
                    continue;
                
                Console.WriteLine($"Battle Notification Ping, chatId: {poll.ChatId.ToString()}");

                var list = usernames[poll.ChatId].Except(poll.Votes.Select(u => $"@{u.Username}")).ToList();
            
                var usernamesToPing = PingHelper.ConstructMessages(list);

                foreach (var names in usernamesToPing) {
                    await _botService.Client.SendTextMessageAsync(
                        chatId: poll.ChatId,
                        text: names);
                }
            }
        }
    }
}
