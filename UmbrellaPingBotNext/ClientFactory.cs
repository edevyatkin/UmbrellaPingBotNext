using MihaZupan;
using System.Threading.Tasks;
using Telegram.Bot;

namespace UmbrellaPingBotNext
{
    internal class ClientFactory
    {
        private static TelegramBotClient _client;

        internal static async Task<TelegramBotClient> GetAsync() {
            if (_client != null)
                return _client;

            BotConfig config = await ConfigHelper.LoadAsync("config.json");
            if (config.Proxy != null) {
                var proxy = new HttpToSocks5Proxy(
                    socks5Hostname: config.Proxy.Server,
                    socks5Port:     config.Proxy.Port,
                    username:       config.Proxy.Login,
                    password:       config.Proxy.Password
                );
                _client = new TelegramBotClient(config.Token, proxy);
            }
            else
                _client = new TelegramBotClient(config.Token);

            await _client.DeleteWebhookAsync();
            if (!string.IsNullOrEmpty(config.WebhookUrl)) {
                await _client.SetWebhookAsync(config.WebhookUrl);
            }

            return _client;
        }
    }
}