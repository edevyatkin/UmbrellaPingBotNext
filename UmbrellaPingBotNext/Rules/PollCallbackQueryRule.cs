using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace UmbrellaPingBotNext.Rules
{
    internal class PollCallbackQueryRule : IUpdateRule
    {
        public bool IsMatch(Update update) {
            if (update.Type != UpdateType.CallbackQuery)
                return false;
            
            Message message = update.CallbackQuery.Message;
            return PollsHelper.HasPoll(message.Chat.Id) &&
                   PollsHelper.GetPoll(message.Chat.Id).MessageId == message.MessageId;
        }

        public Task ProcessAsync(Update update) => Task.CompletedTask;
    }
}
