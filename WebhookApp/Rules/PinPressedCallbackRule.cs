using System;
using System.Globalization;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using WebhookApp.Services;

namespace WebhookApp.Rules
{
    internal class PinPressedCallbackRule : IUpdateRule
    {
        private readonly BotService _botService;
        private readonly PollCallbackQueryRule _queryRule;

        public PinPressedCallbackRule(BotService botService, PollCallbackQueryRule queryRule) {
            _botService = botService;
            _queryRule = queryRule;
        }
        
        public async Task<bool> IsMatch(Update update) {
            return await _queryRule.IsMatch(update)
                    && update.CallbackQuery.Data == "pin_is_pressed";
        }

        public async Task ProcessAsync(Update update) {
            Console.WriteLine($"[ {DateTime.Now.ToLocalTime().ToString(CultureInfo.InvariantCulture)} ] Processing pin pressed callback... {update.CallbackQuery.From.Username}, chatId: {update.CallbackQuery.Message.Chat.Id.ToString()}");

            Poll poll = PollsHelper.GetPoll(update.CallbackQuery.Message.Chat.Id);
            bool userListUpdated = poll.AddActiveVote(update.CallbackQuery.From);
            PollView pollView = poll.AsView();

            if (userListUpdated) {
                await PollVoteThrottle.Acquire();
                await _botService.Client.EditMessageTextAsync(
                    chatId: poll.ChatId,
                    messageId: poll.MessageId,
                    text: pollView.Text,
                    parseMode: ParseMode.Html,
                    replyMarkup: pollView.ReplyMarkup);
            }

            await _botService.Client.AnswerCallbackQueryAsync(
                callbackQueryId: update.CallbackQuery.Id,
                text: pollView.ActiveCallbackQueryAnswer);
        }
    }
}
