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
                && update.Message.Chat.Type == ChatType.Supergroup
                && update.Message.Chat.Id == long.Parse(config.ChatId);
        }

        public Task ProcessAsync(Update update) => Task.CompletedTask;
    }
}