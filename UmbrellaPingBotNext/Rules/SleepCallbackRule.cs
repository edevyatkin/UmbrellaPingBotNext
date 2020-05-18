using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace UmbrellaPingBotNext.Rules
{
    internal class SleepCallbackRule : IUpdateRule
    {
        public bool IsMatch(Update update) {
            return UpdateProcessor.GetRule<PollCallbackQueryRule>().IsMatch(update)
                    && update.CallbackQuery.Data == "sleep_is_pressed";
        }

        public async Task ProcessAsync(Update update) {
            Console.WriteLine($"[ {DateTime.Now.ToLocalTime().ToString(CultureInfo.InvariantCulture)} ] Processing sleep callback... {update.CallbackQuery.From.Username}, chatId: {update.CallbackQuery.Message.Chat.Id.ToString()}");

            var client = await ClientFactory.GetAsync();
            
            Poll poll = PollsHelper.GetPoll(update.CallbackQuery.Message.Chat.Id);
            bool userListUpdated = poll.AddSleepVote(update.CallbackQuery.From);
            PollView pollView = poll.AsView();

            if (userListUpdated) {
                await PollVoteThrottle.Acquire();
                await client.EditMessageTextAsync(
                    chatId: poll.ChatId,
                    messageId: poll.MessageId,
                    text: pollView.Text,
                    parseMode: ParseMode.Html,
                    replyMarkup: pollView.ReplyMarkup);
            }

            await client.AnswerCallbackQueryAsync(
                callbackQueryId: update.CallbackQuery.Id,
                text: pollView.SleepCallbackQueryAnswer);
        }
    }
}
