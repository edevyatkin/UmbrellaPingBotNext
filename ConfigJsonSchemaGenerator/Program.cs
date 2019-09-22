using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Schema.Generation;
using Newtonsoft.Json.Serialization;
using System;
using System.IO;
using System.Reflection;
using UmbrellaPingBotNext;

namespace ConfigJsonSchemaGenerator
{
    class Program
    {
        static void Main(string[] args) {
            string name = Assembly.GetExecutingAssembly().GetName().Name;
            string help = $"Usage: {Environment.NewLine}" +
                $"./{name}.exe config.json [config-schema.json]";
            if (args.Length == 0) {
                Console.WriteLine("Please specify config.json file location");
                Console.WriteLine(help);
                return;
            }
            if (args.Length > 2) {
                Console.WriteLine("Too many parameters");
                Console.WriteLine(help);
                return;
            }

            try {
                string configPath = args[0];
                if (!File.Exists(configPath) && Path.GetExtension(configPath) != ".json") {
                    Console.WriteLine($"File not found or invalid path at \"{configPath}\"");
                    Console.WriteLine(help);
                    return;
                }

                string schemaPath = "config-schema.json"; // default
                if (args.Length == 2) {
                    string arg = args[1];
                    var dirName = Path.GetDirectoryName(arg);
                    if (Path.GetExtension(arg) != ".json") {
                        Console.WriteLine($"Invalid path \"{arg}\". Please specify output file path with *.json");
                        Console.WriteLine(help);
                        return;
                    }
                    if (dirName != string.Empty && !Directory.Exists(dirName)) {
                        Console.WriteLine($"Directory \"{dirName}\" does not exist");
                        Console.WriteLine(help);
                        return;
                    }
                    schemaPath = arg;
                }

                Console.WriteLine($"Process {configPath} file...{Environment.NewLine}");

                JSchemaGenerator generator = new JSchemaGenerator() {
                    SchemaReferenceHandling = SchemaReferenceHandling.None,
                    ContractResolver = new CamelCasePropertyNamesContractResolver() {
                        NamingStrategy = new SnakeCaseNamingStrategy()
                    },
                    DefaultRequired = Required.DisallowNull
                };

                JSchema schema = generator.Generate(typeof(BotConfig));

                using (TextWriter tw = File.CreateText(schemaPath))
                using (JsonWriter jw = new JsonTextWriter(tw)) {
                    jw.Formatting = Formatting.Indented;
                    schema.WriteTo(jw);
                    Console.WriteLine($"Done!{Environment.NewLine}" +
                        $"Schema file located at:{Environment.NewLine}{ Path.GetFullPath(schemaPath)}");
                }
            }
            catch (Exception e) {
                Console.WriteLine($"Something went wrong: {e.Message}{Environment.NewLine}{e.StackTrace}");
            }

        }
    }
}
