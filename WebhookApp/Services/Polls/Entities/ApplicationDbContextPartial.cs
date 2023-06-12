using Microsoft.EntityFrameworkCore;
using WebhookApp.Services.Polls.Entities;

namespace WebhookApp.Data;

public partial class ApplicationDbContext
{
    public DbSet<BattlePoll> BattlePolls { get; set; }
}