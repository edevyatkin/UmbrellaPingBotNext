using System;
using System.Threading.Tasks;
using Telegram.Bot;

namespace UmbrellaPingBotNext
{
    class Program {
        static async Task Main(string[] args) {
            try {
                BotConfig config = ConfigHelper.Load("config.json");
                TelegramBotClient client = BotFactory.Create(config);
                var me = await client.GetMeAsync();
                Console.WriteLine(me.FirstName);
            }
            catch (Exception e) {
                Console.WriteLine($"Something went wrong:{Environment.NewLine}{e.Message}{Environment.NewLine}{e.StackTrace}");
            }
        }
    }
}
