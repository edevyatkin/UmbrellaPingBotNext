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
            return update.Type == UpdateType.CallbackQuery &&
                update.CallbackQuery.Message.Chat.Id == PollHelper.ChatId &&
                update.CallbackQuery.Message.MessageId == PollHelper.MessageId;
        }

        public Task ProcessAsync(Update update) => Task.CompletedTask;
    }
}
