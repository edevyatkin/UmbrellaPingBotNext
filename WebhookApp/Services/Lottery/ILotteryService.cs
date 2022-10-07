using System.Collections.Generic;
using System.Threading.Tasks;
using WebhookApp.Abstractions;

namespace WebhookApp.Services.Lottery;

public interface ILotteryService
{
    Task<bool> AddPingAsync(User user, long chatId);
    Task<bool> RemovePingAsync(User user, long chatId);
    Task<List<User>> GetUsersToPingAsync(long chatId);
}