using System.Net;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using Telegram.Bot;

namespace WebhookApp.Services
{
    public class BotService
    {
        public TelegramBotClient Client { get; }

        public BotService(BotConfig config, ILogger<BotService> logger) {
            logger.LogInformation("Loading telegram client...");
            if (!string.IsNullOrEmpty(config.ProxyHost))
            {
                WebProxy proxy = new (config.ProxyHost);
                HttpClient httpClient = new (
                    new SocketsHttpHandler { Proxy = proxy, UseProxy = true }
                );
                Client = new TelegramBotClient(config.Token, httpClient);
            }
            else
            {
                Client = new TelegramBotClient(config.Token);
            }
            Client.SetWebhook(config.WebhookUrl).GetAwaiter().GetResult();
            logger.LogInformation("Telegram client started!");
        }
    }
}