using System;

namespace WebhookApp.Services.Polls.Infrastructure.Dtos {
    public class PinDto {
        public string Direction { get; set; }
        public long MessageId { get; set; }
        public DateTime DateTime { get; set; }
        public long ChatId { get; set; }
    }
}