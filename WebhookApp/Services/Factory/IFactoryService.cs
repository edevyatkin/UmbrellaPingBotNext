using System.Collections.Generic;
using System.Threading.Tasks;
using WebhookApp.Abstractions;

namespace WebhookApp.Services.Factory;

public interface IFactoryService
{
    Task<bool> AddPingAsync(User user, long chatId);
    Task<bool> RemovePingAsync(User user, long chatId);
    Task<List<User>> GetUsersToPingAsync(long chatId);
}