using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace WebhookApp.Jobs
{
    class FactoryEatNotificationJob : IJob
    {
        private readonly ITelegramBotClient _botClient;
        private readonly BotConfig _botConfig;
        private readonly ILogger<FactoryEatNotificationJob> _logger;

        public FactoryEatNotificationJob(ITelegramBotClient botClient, BotConfig botConfig, ILogger<FactoryEatNotificationJob> logger) {
            _botClient = botClient;
            _botConfig = botConfig;
            _logger = logger;
        }
        public async Task Do() {
            foreach (var chatId in _botConfig.Chats) {
                _logger.LogInformation($"Factory Eat Notification, chatId: {chatId.ToString()}");

                var message = await _botClient.SendMessage(
                    chatId: chatId,
                    text: "Кушаем после фабрики 🍔",
                    parseMode: ParseMode.Html);
            }
        }
    }
}