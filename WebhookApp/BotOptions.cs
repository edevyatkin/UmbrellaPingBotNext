using System.ComponentModel.DataAnnotations;

namespace WebhookApp;

public class BotOptions {
    [Required, RegularExpression("^[0-9]{10}:[-_a-zA-Z0-9]{0,35}$")]
    public string Token { get; set; }
    [Url]
    public string WebhookUrl { get; set; }
    [Required]
    public string Bot { get; set; }
    [Required]
    public string SwInfoBot { get; set; }
}
