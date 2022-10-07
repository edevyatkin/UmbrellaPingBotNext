using Microsoft.EntityFrameworkCore;
using WebhookApp.Services.Battle;

namespace WebhookApp.Data;

public partial class ApplicationDbContext
{
    public DbSet<BattleNotificationPing> BattleNotificationPings { get; set; }
}