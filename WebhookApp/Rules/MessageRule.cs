using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using WebhookApp.Services;

namespace WebhookApp.Rules
{
    internal class MessageRule : IUpdateRule
    {
        private readonly BotConfig _botConfig;

        public MessageRule(BotConfig botConfig) {
            _botConfig = botConfig;
        }
        
        public async Task<bool> IsMatch(Update update) {
            return update.Type == UpdateType.Message
                && update.Message.Type == MessageType.Text
                && _botConfig.Chats.Contains(update.Message.Chat.Id);
        }

        public Task ProcessAsync(Update update) => Task.CompletedTask;
    }
}