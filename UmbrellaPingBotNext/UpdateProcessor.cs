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
        private const string _rulesNamespace = "UmbrellaPingBotNext.Rules";
        private static Dictionary<string, IUpdateRule> _rules = new Dictionary<string, IUpdateRule>();

        static UpdateProcessor() {
            Console.WriteLine("Loading rules...");
            Type[] types = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => string.Equals(t.Namespace, _rulesNamespace, StringComparison.Ordinal) && t.IsClass)
                .ToArray();
            Array.ForEach(types, t => _rules.Add(t.FullName, (IUpdateRule)Activator.CreateInstance(t)));
            Console.WriteLine("Rules loaded!");
        }

        public static async Task Start() {
            using (var pipeServer = new NamedPipeServerStream("telegrambot_upstream",
                PipeDirection.InOut, 1, PipeTransmissionMode.Byte, PipeOptions.Asynchronous)) {
                using (var binaryReader = new BinaryReader(pipeServer, Encoding.UTF8)) {
                    while (true) {
                        if (!pipeServer.IsConnected)
                            await pipeServer.WaitForConnectionAsync();
                        try {
                            string json = binaryReader.ReadString();
                            Update update = JsonConvert.DeserializeObject<Update>(json);
                            if (update == null)
                                Console.WriteLine("Incorrect Update object");
                            else {
                                Process(update);
                            }
                        }
                        catch (EndOfStreamException) {
                            Console.WriteLine("End of stream. Disconnect!");
                            pipeServer.Disconnect();
                        }
                    }
                }
            }
        }

        internal static void Process(Update update) {
            if (_rules.Count == 0)
                return;
            foreach (IUpdateRule rule in _rules.Values) {
                if (rule.IsMatch(update)) {
                    rule.Process(update);
                }
            }
        }

        public static T GetRule<T>() => (T)_rules[typeof(T).FullName];
    }
}
