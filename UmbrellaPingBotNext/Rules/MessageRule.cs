using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace UmbrellaPingBotNext.Rules
{
    internal class MessageRule : IUpdateRule
    {
        public bool IsMatch(Update update) {
            return update.Type == UpdateType.Message;
        }

        public void Process(Update update) {
            Console.WriteLine("Processing message...");
        }
    }
}
