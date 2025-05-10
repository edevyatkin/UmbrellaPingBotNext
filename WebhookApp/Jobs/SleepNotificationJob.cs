using System.Collections.Generic;
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
        private readonly BotConfig _botConfig;
        private readonly ILogger<SleepNotificationJob> _logger;

        public SleepNotificationJob(BotService botService, BotConfig botConfig, ILogger<SleepNotificationJob> logger) {
            _botService = botService;
            _botConfig = botConfig;
            _logger = logger;
        }
        public async Task Do() {
            foreach (var chatId in _botConfig.Chats) {
                _logger.LogInformation($"Sleep Notification, chatId: {chatId.ToString()}");

                var message = await _botService.Client.SendMessage(
                    chatId: chatId,
                    text: "Проверьте время до сна 🛌",
                    parseMode: ParseMode.Html);
            }
        }
    }
}
