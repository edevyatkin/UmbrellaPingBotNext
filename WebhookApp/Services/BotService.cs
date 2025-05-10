using Microsoft.Extensions.Logging;
using Telegram.Bot;

namespace WebhookApp.Services
{
    public class BotService
    {
        public TelegramBotClient Client { get; }

        public BotService(BotConfig config, ILogger<BotService> logger) {
            logger.LogInformation("Loading telegram client...");
            Client = new TelegramBotClient(config.Token);
            Client.SetWebhook(config.WebhookUrl).GetAwaiter().GetResult();
            logger.LogInformation("Telegram client started!");
        }
    }
}