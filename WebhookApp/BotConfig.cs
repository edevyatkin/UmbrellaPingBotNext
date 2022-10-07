using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace WebhookApp
{
    public class BotConfig
    {
        public string Token { get; set; }
        public string Bot { get; set; }
        public string SwInfoBot { get; set; }
        [JsonRequired]
        [RegularExpression("^-100[0-9]{10}$")]
        public List<long> Chats { get; set; }
        public BotProxy Proxy { get; set; }
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
}
