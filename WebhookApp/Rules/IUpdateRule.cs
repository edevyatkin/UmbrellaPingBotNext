using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace WebhookApp.Rules
{
    public interface IUpdateRule
    {
        Task<bool> IsMatch(Update update);
        Task ProcessAsync(Update update);
    }
}