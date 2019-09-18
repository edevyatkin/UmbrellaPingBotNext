using Newtonsoft.Json;
using System;
using System.IO;
using System.IO.Pipes;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace UmbrellaPingBotNext
{
    class Program
    {
        static async Task Main(string[] args) {
            try {
                BotConfig config = ConfigHelper.Load("config.json");
                TelegramBotClient client = BotFactory.Create(config);
                var me = await client.GetMeAsync();
                Console.WriteLine(me.Username);

                ////
                JsonSerializer jsonSerializer = JsonSerializer.CreateDefault();
                using (var pipeServer = new NamedPipeServerStream("telegrambot_upstream", PipeDirection.In)) {
                    using (var streamReader = new StreamReader(pipeServer)) {
                        while (true) {
                            if (!pipeServer.IsConnected)
                                await pipeServer.WaitForConnectionAsync();
                            var jsonReader = new JsonTextReader(streamReader);
                            Update update = jsonSerializer.Deserialize<Update>(jsonReader);
                            UpdateProcessor.Process(update);
                        }
                    }
                }
                ////
            }
            catch (Exception e) {
                Console.WriteLine($"Something went wrong:{Environment.NewLine}{e.Message}{Environment.NewLine}{e.StackTrace}");
            }
        }
    }
}
