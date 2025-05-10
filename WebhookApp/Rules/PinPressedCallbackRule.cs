using System;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using WebhookApp.Services;
using WebhookApp.Services.Polls;
using Poll = WebhookApp.Services.Polls.Poll;

namespace WebhookApp.Rules
{
    internal class PinPressedCallbackRule : IUpdateRule
    {
        private readonly BotService _botService;
        private readonly PollCallbackQueryRule _queryRule;
        private readonly ILogger<PinPressedCallbackRule> _logger;

        public PinPressedCallbackRule(BotService botService, PollCallbackQueryRule queryRule, ILogger<PinPressedCallbackRule> logger) {
            _botService = botService;
            _queryRule = queryRule;
            _logger = logger;
        }
        
        public async Task<bool> IsMatch(Update update) {
            return await _queryRule.IsMatch(update)
                    && update.CallbackQuery.Data == "pin_is_pressed";
        }

        public async Task ProcessAsync(Update update) {
            _logger.LogInformation($"[ {DateTime.Now.ToLocalTime().ToString(CultureInfo.InvariantCulture)} ] Processing pin pressed callback... {update.CallbackQuery.From.Username}, chatId: {update.CallbackQuery.Message.Chat.Id.ToString()}");

            Poll poll = PollsHelper.GetPoll(update.CallbackQuery.Message.Chat.Id);
            bool userListUpdated = poll.AddActiveVote(update.CallbackQuery.From);
            PollView pollView = poll.AsView();

            if (userListUpdated) {
                await PollVoteThrottle.Acquire();
                await _botService.Client.EditMessageText(
                    chatId: poll.ChatId,
                    messageId: poll.MessageId,
                    text: pollView.Text,
                    parseMode: ParseMode.Html,
                    replyMarkup: pollView.ReplyMarkup);
            }

            await _botService.Client.AnswerCallbackQuery(
                callbackQueryId: update.CallbackQuery.Id,
                text: pollView.ActiveCallbackQueryAnswer);
        }
    }
}
