using System.Net.Http;
using Microsoft.Extensions.Logging;
using MihaZupan;
using Telegram.Bot;

namespace WebhookApp.Services
{
    public class BotService
    {
        private readonly ILogger<BotService> _logger;
        public TelegramBotClient Client { get; }

        public BotService(ConfigService configService, ILogger<BotService> logger) {
            _logger = logger;
            
            _logger.LogInformation("Loading telegram client...");
            BotConfig config = configService.LoadAsync().Result;
            if (config.Proxy != null) {
                var proxy = new HttpToSocks5Proxy(
                    socks5Hostname: config.Proxy.Server,
                    socks5Port:     config.Proxy.Port,
                    username:       config.Proxy.Login,
                    password:       config.Proxy.Password
                );
                proxy.ResolveHostnamesLocally = true;
                var httpClient = new HttpClient(
                    new HttpClientHandler { Proxy = proxy, UseProxy = true }
                );
                Client = new TelegramBotClient(config.Token, httpClient);
            }
            else
                Client = new TelegramBotClient(config.Token);

            Client.DeleteWebhookAsync();
            if (!string.IsNullOrEmpty(config.WebhookUrl)) {
                Client.SetWebhookAsync(config.WebhookUrl);
            }
            _logger.LogInformation("Telegram client started!");
            _logger.LogInformation(Client.GetMeAsync().Result.Username);
        }

        public void Run()
        {
            _logger.LogInformation("Starting telegram client!");
        }
    }
}