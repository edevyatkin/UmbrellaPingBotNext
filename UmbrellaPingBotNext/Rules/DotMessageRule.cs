using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace UmbrellaPingBotNext.Rules
{
    internal class DotMessageRule : IUpdateRule
    {
        public bool IsMatch(Update update) {
            var tmr = UpdateProcessor.GetRule<TextMessageRule>();
            if (tmr.IsMatch(update)
                && update.Message.Text == "."
                && update.Message.Chat.Type == ChatType.Supergroup)
                return true;

            return false;
        }

        public void Process(Update update) {
            Console.WriteLine("Processing . message...");
        }
    }
}
