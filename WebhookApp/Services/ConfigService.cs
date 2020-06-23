using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Serialization;

namespace WebhookApp.Services
{
    public class ConfigService
    {
        private readonly ILogger<ConfigService> _logger;
        private JsonSerializer _serializer;
        private const string SchemaFile = "config-schema.json";
        private const string ConfigFile = "config.json";
        private BotConfig _config;

        public ConfigService(ILogger<ConfigService> logger) {
            _logger = logger;
            _serializer = new JsonSerializer() {
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver() {
                    NamingStrategy = new SnakeCaseNamingStrategy()
                },
                Formatting = Formatting.Indented
            };
        }

        public async Task SaveAsync() {
            await using StreamWriter file = File.CreateText(ConfigFile);
            _serializer.Serialize(file, _config);
        }
        
        public async Task<BotConfig> LoadAsync() {
            if (_config != null)
                return _config;
            
            _logger.LogInformation("Loading config file...");
            var configObj = JObject.Parse(await File.ReadAllTextAsync(ConfigFile));
            var jSchema = JSchema.Parse(await File.ReadAllTextAsync(SchemaFile));

            if (!configObj.IsValid(jSchema, out IList<string> errorMessages)) {
                _logger.LogError("There are some validation errors:");
                foreach (var error in errorMessages) {
                    _logger.LogError(error);
                }
                throw new Exception($"Loading config file failed");
            }
            
            _config = configObj.ToObject<BotConfig>(_serializer);
            _logger.LogInformation("Config loaded");
            
            return _config;
        }
    }
}