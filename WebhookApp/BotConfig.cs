using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebhookApp;

public class BotConfig {
    [Required, RegularExpression("^[0-9]+:[-_a-zA-Z0-9]{0,35}$")]
    public string Token { get; set; }
    [Required, Url]
    public string WebhookUrl { get; set; }
    [Required]
    public string Bot { get; set; }
    [Required]
    public string SwInfoBot { get; set; }
    [Required]
    public List<long> Chats { get; set; }
    [Required]
    public Dictionary<long, List<string>> ChatAdmins { get; set; }
}
