﻿using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using WebhookApp.Services;
using WebhookApp.Services.Polls;

namespace WebhookApp.Jobs
{
    class BattleNotificationJob : IJob
    {
        private readonly BotService _botService;
        private readonly ILogger<BattleNotificationJob> _logger;

        public BattleNotificationJob(BotService botService, ILogger<BattleNotificationJob> logger) {
            _botService = botService;
            _logger = logger;
        }
        public async Task Do() {
            var polls = PollsHelper.Polls;
            foreach (var poll in polls.Values) {
                _logger.LogInformation($"Battle Notification, chatId: {poll.ChatId.ToString()}");
                
                PollView pollView = poll.AsView();
                await _botService.Client.DeleteMessage(
                    chatId: poll.ChatId,
                    messageId: poll.MessageId);

                var message = await _botService.Client.SendMessage(
                    chatId: poll.ChatId,
                    text: pollView.Text,
                    parseMode: ParseMode.Html,
                    replyMarkup: pollView.ReplyMarkup);
                
                poll.ChatId = message.Chat.Id;
                poll.MessageId = message.Id;
            }
        }
    }
}
