using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using WebhookApp.Services.PollsService;

namespace WebhookApp.Rules
{
    internal class PollCallbackQueryRule : IUpdateRule
    {
        public Task<bool> IsMatch(Update update) {
            if (update.Type != UpdateType.CallbackQuery)
                return Task.FromResult(false);
            
            Message message = update.CallbackQuery.Message;
            return Task.FromResult(PollsHelper.HasPoll(message.Chat.Id) &&
                   PollsHelper.GetPoll(message.Chat.Id).MessageId == message.MessageId);
        }

        public Task ProcessAsync(Update update) => Task.CompletedTask;
    }
}
