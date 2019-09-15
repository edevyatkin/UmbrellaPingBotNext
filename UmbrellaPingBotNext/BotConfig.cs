using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace UmbrellaPingBotNext
{
    public class BotConfig
    {
        [JsonRequired]
        [RegularExpression("^[0-9]{9}:[-_a-zA-Z0-9]{0,35}$")]
        public string Token { get; set; }
        [JsonRequired]
        [RegularExpression("^-100[0-9]{10}$")]
        public string ChatId { get; set; }
        [JsonProperty(Required = Required.DisallowNull)]
        public BotProxy Proxy { get; set; }
        [JsonProperty(Required = Required.Always)]
        public List<string> Usernames { get; set; }
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

        internal static BotConfig Load(string path) {
            BotConfig config = default;
            List<string> validationErrors = new List<string>();
            Console.WriteLine("Loading config file...");
            try {
                using (var tr = File.OpenText(path))
                using (var jtr = new JsonTextReader(tr))
                using (var vr = new JSchemaValidatingReader(jtr)) {
                    vr.Schema = JSchema.Parse(File.ReadAllText(schemaFile));

                    vr.ValidationEventHandler += (o, a) => validationErrors.Add(a.Message);

                    JsonSerializer serializer = new JsonSerializer() {
                        ContractResolver = new CamelCasePropertyNamesContractResolver() {
                            NamingStrategy = new SnakeCaseNamingStrategy()
                        }
                    };
                    config = serializer.Deserialize<BotConfig>(vr);
                    if (validationErrors.Count > 0) {
                        Console.WriteLine($"There are some validation errors:");
                        foreach (var error in validationErrors) {
                            Console.WriteLine(error);
                        }
                        return default;
                    }
                }
            }
            catch (Exception e) {
                throw e;
            }
            Console.WriteLine("Config loaded");
            return config;
        }
    }
}
