using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace UmbrellaPingBotNext.Rules
{
    internal class MessageRule : IUpdateRule
    {
        public bool IsMatch(Update update) {
            var config = ConfigHelper.Get();
            return update.Type == UpdateType.Message
                && update.Message.Type == MessageType.Text
                && config.Chats.Contains(update.Message.Chat.Id);
        }

        public Task ProcessAsync(Update update) => Task.CompletedTask;
    }
}