using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace UmbrellaPingBotNext.Rules
{
    internal class LeaveChatBotRule : IUpdateRule
    {
        public bool IsMatch(Update update) {
            var config = ConfigHelper.Get();
            return update.Type == UpdateType.Message
                && update.Message.NewChatMembers != null
                && update.Message.NewChatMembers.Length != 0 
                && update.Message.NewChatMembers.Where(u => u.Username == "UmbrellaPingBot") != null
                && update.Message.Chat.Id != long.Parse(config.ChatId);
        }

        public void Process(Update update) {
            Console.WriteLine("Processing leave chat bot message...");
            var client = ClientFactory.GetAsync().GetAwaiter().GetResult();

            client.LeaveChatAsync(
                chatId: update.Message.Chat.Id)
                .GetAwaiter().GetResult();
        }
    }
}
