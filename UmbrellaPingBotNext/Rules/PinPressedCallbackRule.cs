using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace UmbrellaPingBotNext.Rules
{
    internal class PinPressedCallbackRule : IUpdateRule
    {
        public bool IsMatch(Update update) {
            return UpdateProcessor.GetRule<PollCallbackQueryRule>().IsMatch(update)
                    && update.CallbackQuery.Data == "pin_is_pressed";
        }

        public async Task ProcessAsync(Update update) {
            Console.WriteLine($"[ {DateTime.Now.ToLocalTime()} ] Processing pin pressed callback... {update.CallbackQuery.From.Username}");

            var client = await ClientFactory.GetAsync();
            
            bool userListUpdated = PollHelper.AddActiveVote(update.CallbackQuery.From);

            PollView pollView = PollHelper.AsView();

            if (userListUpdated) {
                await PollVoteThrottle.Acquire();
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
