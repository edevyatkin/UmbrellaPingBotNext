using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;

namespace UmbrellaPingBotNext
{
    public class BotConfig
    {
        [JsonRequired]
        [RegularExpression("^[0-9]{9}:[-_a-zA-Z0-9]{0,35}$")]
        public string Token { get; set; }
        [JsonRequired]
        public string Bot { get; set; }
        [JsonRequired]
        public string SwInfoBot { get; set; }
        [JsonRequired]
        [RegularExpression("^-100[0-9]{10}$")]
        public List<long> Chats { get; set; }
        public BotProxy Proxy { get; set; }
        [JsonProperty(Required = Required.Always)]
        public Dictionary<long, List<string>> Usernames { get; set; }
        public string WebhookUrl { get; set; }
        public Dictionary<long, List<string>> ChatAdmins { get; set; }
    }

    //  Socks5 proxy settings
    public class BotProxy
    {
        [JsonRequired]
        public string Server { get; set; }
        [JsonRequired]
        public int Port { get; set; }
        [JsonRequired]
        public string Login { get; set; }
        [JsonRequired]
        public string Password { get; set; }
    }

    internal class ConfigHelper
    {
        private const string schemaFile = "config-schema.json";
        private static BotConfig _config;
        private static JsonSerializer _serializer;

        static ConfigHelper() {
            _serializer = new JsonSerializer() {
                ContractResolver = new CamelCasePropertyNamesContractResolver() {
                    NamingStrategy = new SnakeCaseNamingStrategy()
                },
                Formatting = Formatting.Indented
            };
        }

        internal static async Task<BotConfig> LoadAsync(string path) {
            Console.WriteLine("Loading config file...");
            var configObj = JObject.Parse(await File.ReadAllTextAsync(path));
            var jSchema = JSchema.Parse(await File.ReadAllTextAsync(schemaFile));

            if (!configObj.IsValid(jSchema, out IList<string> errorMessages)) {
                Console.WriteLine("There are some validation errors:");
                foreach (var error in errorMessages) {
                    Console.WriteLine(error);
                }
                throw new Exception($"Loading config file failed");
            }
            
            _config = configObj.ToObject<BotConfig>(_serializer);
            Console.WriteLine("Config loaded");
            return _config;
        }

        public static async Task SaveAsync() {
            await using StreamWriter file = File.CreateText("config.json");
            _serializer.Serialize(file, _config);
        }

        public static BotConfig Get() => _config;
    }
}
