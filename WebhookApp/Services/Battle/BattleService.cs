using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebhookApp.Abstractions;
using WebhookApp.Data;

namespace WebhookApp.Services.Battle;

class BattleService : IBattleService
{
    private readonly ApplicationDbContext _context;

    public BattleService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> AddPingAsync(User user, long chatId)
    {
        var count = await _context.BattleNotificationPings
            .CountAsync(ping => ping.Username == user.Username && ping.ChatId == chatId);
        if (count == 0)
        {
            var entity = new BattleNotificationPing
            {
                Username = user.Username,
                ChatId = chatId
            };
            await _context.AddAsync(entity);
        }
        return await _context.SaveChangesAsync() == 1;
    }

    public async Task<bool> RemovePingAsync(User user, long chatId)
    {
        var entity = await _context.BattleNotificationPings
            .Where(ping => ping.Username == user.Username && ping.ChatId == chatId).FirstOrDefaultAsync();
        if (entity is not null)
        {
            _context.Remove(entity);
        }
        return await _context.SaveChangesAsync() == 1;
    }

    public async Task<List<User>> GetUsersToPingAsync(long chatId)
    {
        return await _context.BattleNotificationPings.AsNoTracking()
            .Where(ping => ping.ChatId == chatId).Select(ping => new User(ping.Username)).ToListAsync();
    }
}