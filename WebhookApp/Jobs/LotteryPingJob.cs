using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using WebhookApp.Services;
using WebhookApp.Services.Polls;

namespace WebhookApp.Jobs
{
    class LotteryPingJob : IJob
    {
        private readonly BotService _botService;
        private readonly ConfigService _configService;
        private readonly ILogger<LotteryPingJob> _logger;

        public LotteryPingJob(BotService botService, ConfigService configService, ILogger<LotteryPingJob> logger) {
            _botService = botService;
            _configService = configService;
            _logger = logger;
        }
        
        public async Task Do() {
            var polls = PollsHelper.Polls;
            BotConfig config = await _configService.LoadAsync();
            var usernames = config.LotteryUsernames;
            foreach (var chatId in usernames.Keys) {
                _logger.LogInformation($"Lottery Ping, chatId: {chatId}");

                var usernamesToPing = usernames[chatId]
                    .OrderBy(u => u).Chunk(5).Select(users => string.Join(' ',users));

                var ticketsBuyText = "`🤑Купить все`";
                await _botService.Client.SendTextMessageAsync(
                        chatId: chatId,
                        text: ticketsBuyText,
                        parseMode: ParseMode.MarkdownV2);

                foreach (var names in usernamesToPing) {
                    await _botService.Client.SendTextMessageAsync(
                        chatId: chatId,
                        text: names);
                }
            }
        }
    }
}
