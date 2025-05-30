﻿using System;
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
    internal class SleepCallbackRule : IUpdateRule
    {
        private readonly BotService _botService;
        private readonly PollCallbackQueryRule _queryRule;
        private readonly ILogger<SleepCallbackRule> _logger;

        public SleepCallbackRule(BotService botService, PollCallbackQueryRule queryRule, ILogger<SleepCallbackRule> logger) {
            _botService = botService;
            _queryRule = queryRule;
            _logger = logger;
        }
        
        public async Task<bool> IsMatch(Update update) {
            return await _queryRule.IsMatch(update)
                    && update.CallbackQuery.Data == "sleep_is_pressed";
        }

        public async Task ProcessAsync(Update update) {
            _logger.LogInformation($"[ {DateTime.Now.ToLocalTime().ToString(CultureInfo.InvariantCulture)} ] Processing sleep callback... {update.CallbackQuery.From.Username}, chatId: {update.CallbackQuery.Message.Chat.Id.ToString()}");

            Poll poll = PollsHelper.GetPoll(update.CallbackQuery.Message.Chat.Id);
            bool userListUpdated = poll.AddSleepVote(update.CallbackQuery.From);
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
                text: pollView.SleepCallbackQueryAnswer);
        }
    }
}
