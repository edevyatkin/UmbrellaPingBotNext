using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace UmbrellaPingBotNext.Rules
{
    internal class ReplyToBotMessageRule : IUpdateRule
    {
        public bool IsMatch(Update update) {
            return UpdateProcessor.GetRule<MessageRule>().IsMatch(update)
                && update.Message.ReplyToMessage?.From.Username == ConfigHelper.Get().Bot;
        }

        public Task ProcessAsync(Update update) => Task.CompletedTask;
    }
}
