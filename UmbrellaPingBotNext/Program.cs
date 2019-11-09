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
        private static TelegramBotClient _client;
        private static readonly JobServer _jobServer = new JobServer();

        static async Task Main(string[] args) {
            try {
                _client = await ClientFactory.GetAsync();
                var me = await _client.GetMeAsync();
                Console.WriteLine(me.Username);

                _jobServer.AddBattleNotification(9,30);
                _jobServer.AddBattleNotificationPing(9,31);
                _jobServer.AddBattleNotificationPing(9,55);
                _jobServer.AddFinishBattleOperation(10,00);

                _jobServer.AddBattleNotification(12,30);
                _jobServer.AddBattleNotificationPing(12,31);
                _jobServer.AddBattleNotificationPing(12,55);
                _jobServer.AddFinishBattleOperation(13,00);

                _jobServer.AddBattleNotification(15,30);
                _jobServer.AddBattleNotificationPing(15,31);
                _jobServer.AddBattleNotificationPing(15,55);
                _jobServer.AddFinishBattleOperation(16,00);

                _jobServer.AddBattleNotification(18,30);
                _jobServer.AddBattleNotificationPing(18,31);
                _jobServer.AddBattleNotificationPing(18,55);
                _jobServer.AddFinishBattleOperation(19,00);

                _jobServer.AddBattleNotification(21,30);
                _jobServer.AddBattleNotificationPing(21,31);
                _jobServer.AddBattleNotificationPing(21,55);
                _jobServer.AddFinishBattleOperation(22,00);

                _jobServer.DisplayJobs();

                Console.CancelKeyPress += Console_CancelKeyPressAsync;

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

        private static void Console_CancelKeyPressAsync(object sender, ConsoleCancelEventArgs e) {
            _client.DeleteWebhookAsync();
            _jobServer.Dispose();
        }
    }
}
