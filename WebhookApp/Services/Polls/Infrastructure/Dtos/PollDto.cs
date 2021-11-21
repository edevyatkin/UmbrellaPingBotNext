using System.Collections.Generic;

namespace WebhookApp.Services.Polls.Infrastructure.Dtos {
    public class PollDto {
        public List<VoteDto> Votes { get; set; }
        public List<PollOptionDto> Options { get; set; }
        public long MessageId { get; set; }
        public long ChatId { get; set; }
    }
}