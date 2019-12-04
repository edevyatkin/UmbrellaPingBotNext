using System;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using UmbrellaPingBotNext.Rules;
using Newtonsoft.Json;
using System.IO.Pipes;
using System.IO;
using System.Threading.Tasks;
using System.Text;

namespace UmbrellaPingBotNext
{
    internal class UpdateProcessor
    {
        private static Dictionary<string, IUpdateRule> _rules = new Dictionary<string, IUpdateRule>();

        static UpdateProcessor() {
            Console.WriteLine("Loading rules...");
            Type[] types = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => typeof(IUpdateRule).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
                .ToArray();
            Array.ForEach(types, t => _rules.Add(t.FullName, (IUpdateRule)Activator.CreateInstance(t)));
            Console.WriteLine("Rules loaded!");
        }

        public static async Task StartAsync() {
            Console.WriteLine("Start update processing...");
            using (var pipeServer = new NamedPipeServerStream("telegrambot_upstream",
                PipeDirection.InOut, 1, PipeTransmissionMode.Byte, PipeOptions.Asynchronous)) {
                using (var streamReader = new StreamReader(pipeServer, new UTF8Encoding(false))) {
                    do {
                        if (!pipeServer.IsConnected)
                            await pipeServer.WaitForConnectionAsync();
                        string json = await streamReader.ReadLineAsync();
                        Update update = JsonConvert.DeserializeObject<Update>(json);
                        await ProcessAsync(update);
                    } while (!streamReader.EndOfStream);
                }
            }
            Console.WriteLine("Exit");
        }

        internal static async Task ProcessAsync(Update update) {
            if (_rules.Count == 0)
                return;
            foreach (IUpdateRule rule in _rules.Values) {
                if (rule.IsMatch(update)) {
                    await rule.ProcessAsync(update);
                }
            }
        }

        public static T GetRule<T>() => (T)_rules[typeof(T).FullName];
    }
}
