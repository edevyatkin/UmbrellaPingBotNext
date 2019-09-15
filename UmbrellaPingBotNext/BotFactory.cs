using MihaZupan;
using System;
using Telegram.Bot;

namespace UmbrellaPingBotNext
{
    internal class BotFactory
    {
        internal static TelegramBotClient Create(BotConfig config) {
            if (config.Proxy != null) {
                var proxy = new HttpToSocks5Proxy(
                    socks5Hostname: config.Proxy.Server,
                    socks5Port:     config.Proxy.Port,
                    username:       config.Proxy.Login,
                    password:       config.Proxy.Password
                );
                return new TelegramBotClient(config.Token, proxy);
            }
            return new TelegramBotClient(config.Token);
        }
    }
}