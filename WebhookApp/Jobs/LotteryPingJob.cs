using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using WebhookApp.Services;
using WebhookApp.Services.Lottery;
using WebhookApp.Services.Ping;

namespace WebhookApp.Jobs
{
    class LotteryPingJob : IJob
    {
        private readonly BotService _botService;
        private readonly BotConfig _botConfig;
        private readonly ILogger<LotteryPingJob> _logger;
        private readonly ILotteryService _lotteryService;
        private readonly IPingService _pingService;

        public LotteryPingJob(BotService botService, BotConfig botConfig, ILogger<LotteryPingJob> logger, ILotteryService lotteryService, IPingService pingService) {
            _botService = botService;
            _botConfig = botConfig;
            _logger = logger;
            _lotteryService = lotteryService;
            _pingService = pingService;
        }
        
        public async Task Do() {
             foreach (var chatId in _botConfig.Chats) {
                 var usersToPing = await _lotteryService.GetUsersToPingAsync(chatId);
                 _logger.LogInformation($"Lottery Ping, chatId: {chatId}");
                 await _botService.Client.SendTextMessageAsync(
                     chatId: chatId,
                     text: "`🤑Купить все`",
                     parseMode: ParseMode.MarkdownV2);                
                 await _pingService.PingUsersAsync(usersToPing, chatId);
             }
        }
    }
}
