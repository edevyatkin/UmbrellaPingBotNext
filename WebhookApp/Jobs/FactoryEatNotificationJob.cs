using System;
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
        private readonly ConfigService _configService;
        private readonly ILogger<FactoryEatNotificationJob> _logger;

        public FactoryEatNotificationJob(BotService botService, ConfigService configService, ILogger<FactoryEatNotificationJob> logger) {
            _botService = botService;
            _configService = configService;
            _logger = logger;
        }
        public async Task Do() {
            BotConfig botConfig = await _configService.LoadAsync();
            var chats = botConfig.Chats;
            foreach (var chatId in chats) {
                _logger.LogInformation($"Factory Eat Notification, chatId: {chatId.ToString()}");

                var message = await _botService.Client.SendTextMessageAsync(
                    chatId: chatId,
                    text: "Кушаем после фабрики 🍔",
                    parseMode: ParseMode.Html);
            }
        }
    }
}
