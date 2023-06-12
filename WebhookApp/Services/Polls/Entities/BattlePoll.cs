using System.Collections.Generic;

namespace WebhookApp.Services.Polls.Entities;

public class BattlePoll
{
    public int Id { get; set; }
    public long TgChatId { get; set; }
    public long TgMessageId { get; set; }
    public BattlePin Pin { get; set; }
    public ICollection<BattlePollOption> Options { get; } = new List<BattlePollOption>();
    public ICollection<BattlePollVote> Votes { get; } = new List<BattlePollVote>();
}