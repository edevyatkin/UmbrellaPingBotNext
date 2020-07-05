using System;
using System.Threading.Tasks;
using Telegram.Bot.Types.Enums;
using WebhookApp.Services;

namespace WebhookApp.Jobs
{
    class FactoryEatNotificationJob : IJob
    {
        private readonly BotService _botService;
        private readonly ConfigService _configService;

        public FactoryEatNotificationJob(BotService botService, ConfigService configService) {
            _botService = botService;
            _configService = configService;
        }
        public async Task Do() {
            BotConfig botConfig = await _configService.LoadAsync();
            var chats = botConfig.Chats;
            foreach (var chatId in chats) {
                Console.WriteLine($"Factory Eat Notification, chatId: {chatId.ToString()}");

                var message = await _botService.Client.SendTextMessageAsync(
                    chatId: chatId,
                    text: "Кушаем после фабрики 🍔",
                    parseMode: ParseMode.Html);
            }
        }
    }
}
