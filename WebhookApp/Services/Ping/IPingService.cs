using System.Collections.Generic;
using System.Threading.Tasks;
using WebhookApp.Abstractions;

namespace WebhookApp.Services.Ping;

public interface IPingService
{
    Task<string> PingUserAsync(User user, long chatId);
    Task<List<string>> PingUsersAsync(ICollection<User> users, long chatId);
}