using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace WebhookApp.Jobs
{
    class SleepNotificationJob : IJob
    {
        private readonly ITelegramBotClient _botClient;
        private readonly BotConfig _botConfig;
        private readonly ILogger<SleepNotificationJob> _logger;

        public SleepNotificationJob(ITelegramBotClient botClient, BotConfig botConfig, ILogger<SleepNotificationJob> logger) {
            _botClient = botClient;
            _botConfig = botConfig;
            _logger = logger;
        }
        public async Task Do() {
            foreach (var chatId in _botConfig.Chats) {
                _logger.LogInformation($"Sleep Notification, chatId: {chatId.ToString()}");

                var message = await _botClient.SendMessage(
                    chatId: chatId,
                    text: "Проверьте время до сна 🛌",
                    parseMode: ParseMode.Html);
            }
        }
    }
}
