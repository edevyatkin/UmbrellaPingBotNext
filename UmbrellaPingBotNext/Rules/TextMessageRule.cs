using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace UmbrellaPingBotNext.Rules
{
    internal class TextMessageRule : IUpdateRule
    {
        public bool IsMatch(Update update) {
            return update.Type == UpdateType.Message
                && update.Message.Type == MessageType.Text;
        }

        public void Process(Update update) {
            Console.WriteLine("Processing text message...");
        }
    }
}
