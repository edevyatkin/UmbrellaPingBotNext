using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace UmbrellaPingBotNext.Rules
{
    internal class PollOldPollCallbackQueryRule : IUpdateRule
    {
        public bool IsMatch(Update update) {
            if (update.Type != UpdateType.CallbackQuery)
                return false;
            
            Message message = update.CallbackQuery.Message;
            return !PollsHelper.HasPoll(message.Chat.Id) ||
                   PollsHelper.GetPoll(message.Chat.Id).MessageId != message.MessageId;
        }

        public async Task ProcessAsync(Update update) {
            Console.WriteLine($"[ {DateTime.Now.ToLocalTime().ToString(CultureInfo.InvariantCulture)} ] Processing old pressing callback... {update.CallbackQuery.From.Username}, chatId: {update.CallbackQuery.Message.Chat.Id.ToString()}");

            var client = await ClientFactory.GetAsync();
            await client.AnswerCallbackQueryAsync(
                callbackQueryId: update.CallbackQuery.Id,
                text: "Голосование устарело");
        }
    }
}
