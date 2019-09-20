using MihaZupan;
using System;
using Telegram.Bot;

namespace UmbrellaPingBotNext
{
    internal class BotFactory
    {
        private static TelegramBotClient _client;

        internal static TelegramBotClient Create(BotConfig config) {
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
            return _client;
        }

        public static TelegramBotClient Get() => _client;
    }
}