namespace WebhookApp.Services.Polls.Infrastructure.Dtos {
    public class VoteDto {
        public long UserId { get; set; }
        public int ItemId { get; set; }
    }
}