namespace WebhookApp.Services.Polls.Entities;

public class BattlePollOption
{
    public int Id { get; set; }
    public int BattlePollId { get; set; }
    public int OrderInOptions { get; set; }
    public int OrderInVotes { get; set; }
    public string Text { get; set; }
    
    public BattlePoll Poll { get; set; }  = null!;
}