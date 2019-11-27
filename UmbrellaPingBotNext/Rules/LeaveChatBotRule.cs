using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace UmbrellaPingBotNext.Rules
{
    internal class LeaveChatBotRule : IUpdateRule
    {
        public bool IsMatch(Update update) {
            var config = ConfigHelper.Get();
            return update.Type == UpdateType.Message
                && update.Message.Type == MessageType.ChatMembersAdded
                && update.Message.NewChatMembers.Any(u => u.Username == Constants.Bot)
                && update.Message.Chat.Id != long.Parse(config.ChatId);
        }

        public async Task ProcessAsync(Update update) {
            Console.WriteLine("Processing leave chat bot message...");
            var client = await ClientFactory.GetAsync();

            await client.LeaveChatAsync(
                chatId: update.Message.Chat.Id);
        }
    }
}
