using Microsoft.EntityFrameworkCore;
using WebhookApp.Services.Factory;

namespace WebhookApp.Data;

public partial class ApplicationDbContext
{
    public DbSet<FactoryPing> FactoryPings { get; set; }
}