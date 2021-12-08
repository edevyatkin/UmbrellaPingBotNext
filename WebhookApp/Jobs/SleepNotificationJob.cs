using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using WebhookApp.Services;

namespace WebhookApp.Jobs
{
    class SleepNotificationJob : IJob
    {
        private readonly BotService _botService;
        private readonly ConfigService _configService;
        private readonly ILogger<SleepNotificationJob> _logger;

        public SleepNotificationJob(BotService botService, ConfigService configService, ILogger<SleepNotificationJob> logger) {
            _botService = botService;
            _configService = configService;
            _logger = logger;
        }
        public async Task Do() {
            BotConfig botConfig = await _configService.LoadAsync();
            var chats = botConfig.Chats;
            foreach (var chatId in chats) {
                _logger.LogInformation($"Sleep Notification, chatId: {chatId.ToString()}");

                var message = await _botService.Client.SendTextMessageAsync(
                    chatId: chatId,
                    text: "Проверьте время до сна 🛌",
                    parseMode: ParseMode.Html);
            }
        }
    }
}
