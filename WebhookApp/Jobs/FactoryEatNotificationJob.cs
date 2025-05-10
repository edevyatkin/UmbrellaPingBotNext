using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using WebhookApp.Services;

namespace WebhookApp.Jobs
{
    class FactoryEatNotificationJob : IJob
    {
        private readonly BotService _botService;
        private readonly BotConfig _botConfig;
        private readonly ILogger<FactoryEatNotificationJob> _logger;

        public FactoryEatNotificationJob(BotService botService, BotConfig botConfig, ILogger<FactoryEatNotificationJob> logger) {
            _botService = botService;
            _botConfig = botConfig;
            _logger = logger;
        }
        public async Task Do() {
            foreach (var chatId in _botConfig.Chats) {
                _logger.LogInformation($"Factory Eat Notification, chatId: {chatId.ToString()}");

                var message = await _botService.Client.SendMessage(
                    chatId: chatId,
                    text: "Кушаем после фабрики 🍔",
                    parseMode: ParseMode.Html);
            }
        }
    }
}