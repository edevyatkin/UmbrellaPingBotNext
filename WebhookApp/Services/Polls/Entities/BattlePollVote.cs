using System;

namespace WebhookApp.Services.Polls.Entities;

public class BattlePollVote
{
    public int Id { get; set; }
    public int BattlePollId { get; set; }
    public DateTime DateTime { get; set; }

    public BattlePoll Poll { get; set; } = null!;
}