using Microsoft.Extensions.Logging;
using Telegram.Bot;

namespace WebhookApp.Services
{
    public class BotService
    {
        public TelegramBotClient Client { get; }

        public BotService(BotOptions options, ILogger<BotService> logger) {
            logger.LogInformation("Loading telegram client...");
            Client = new TelegramBotClient(options.Token);
            Client.SetWebhookAsync(options.WebhookUrl).GetAwaiter().GetResult();
            logger.LogInformation("Telegram client started!");
        }
    }
}