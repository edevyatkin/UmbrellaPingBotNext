using Microsoft.EntityFrameworkCore;
using WebhookApp.Services.Lottery;

namespace WebhookApp.Data;

public partial class ApplicationDbContext
{
    public DbSet<LotteryPing> LotteryPings { get; set; }
}