using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace UmbrellaPingBotNext.Rules
{
    class SleepCallbackRule : IUpdateRule
    {
        public bool IsMatch(Update update) {
            return (update.Type == UpdateType.CallbackQuery &&
                    update.CallbackQuery.Message.Chat.Id == PollHelper.ChatId &&
                    update.CallbackQuery.Message.MessageId == PollHelper.MessageId &&
                    update.CallbackQuery.Data == "sleep_is_pressed");
        }

        public void Process(Update update) {
            Console.WriteLine("Processing sleep callback...");

            var client = ClientFactory.GetAsync().GetAwaiter().GetResult();

            bool userListUpdated = PollHelper.AddSleepVote(update.CallbackQuery.From);

            PollView pollView = PollHelper.AsView();

            if (userListUpdated) {
                client.EditMessageTextAsync(
                    chatId: PollHelper.ChatId,
                    messageId: PollHelper.MessageId,
                    text: pollView.Text,
                    parseMode: ParseMode.Html,
                    replyMarkup: pollView.ReplyMarkup)
                    .GetAwaiter().GetResult();
            }

            client.AnswerCallbackQueryAsync(
                callbackQueryId: update.CallbackQuery.Id,
                text: pollView.SleepCallbackQueryAnswer)
                .GetAwaiter().GetResult();
        }
    }
}
