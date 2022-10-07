using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using WebhookApp.Abstractions;

namespace WebhookApp.Services.Ping;

public class PingService : IPingService
{
    private readonly BotService _botService;

    public PingService(BotService botService)
    {
        _botService = botService;
    }

    public async Task<string> PingUserAsync(User user, long chatId)
    {
        var message = await _botService.Client.SendTextMessageAsync(
            chatId: chatId,
            text: user.ToString());
        return message.Text;
    }

    public async Task<List<string>> PingUsersAsync(ICollection<User> users, long chatId)
    {
        var usernamesToPing = users
            .Select(u => $"@{u.Username}")
            .OrderBy(s => s)
            .Chunk(5)
            .Select(u => string.Join(' ', u))
            .ToList();

        foreach (var names in usernamesToPing)
        {
            await _botService.Client.SendTextMessageAsync(
                chatId: chatId,
                text: names);
        }

        return usernamesToPing;
    }
}