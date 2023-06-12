namespace WebhookApp.Services.Polls.Entities;

public class BattlePin
{
    public int Id { get; set; }
    public long TgChatId { get; set; }
    public long TgMessageId { get; set; }
    public int BattlePollId { get; set; }
    
    public BattlePoll Poll { get; set; } = null!;
}