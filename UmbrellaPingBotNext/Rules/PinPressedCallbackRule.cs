using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace UmbrellaPingBotNext.Rules
{
    class PinPressedCallbackRule : IUpdateRule
    {
        public bool IsMatch(Update update) {
            return (update.Type == UpdateType.CallbackQuery &&
                update.CallbackQuery.Message.Chat.Id == PollHelper.ChatId &&
                update.CallbackQuery.Message.MessageId == PollHelper.MessageId &&
                update.CallbackQuery.Data == "pin_is_pressed");
        }

        public async Task ProcessAsync(Update update) {
            Console.WriteLine("Processing pin pressed callback...");

            var client = await ClientFactory.GetAsync();
            
            bool userListUpdated = PollHelper.AddActiveVote(update.CallbackQuery.From);

            PollView pollView = PollHelper.AsView();

            if (userListUpdated) {
                await client.EditMessageTextAsync(
                    chatId: PollHelper.ChatId,
                    messageId: PollHelper.MessageId,
                    text: pollView.Text,
                    parseMode: ParseMode.Html,
                    replyMarkup: pollView.ReplyMarkup);
            }

            await client.AnswerCallbackQueryAsync(
                callbackQueryId: update.CallbackQuery.Id,
                text: pollView.ActiveCallbackQueryAnswer);
        }
    }
}
