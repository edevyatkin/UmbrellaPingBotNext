using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types.Enums;
using WebhookApp.Services;
using WebhookApp.Services.PollsService;

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
                await _botService.Client.DeleteMessageAsync(
                    chatId: poll.ChatId,
                    messageId: poll.MessageId);

                var message = await _botService.Client.SendTextMessageAsync(
                    chatId: poll.ChatId,
                    text: pollView.Text,
                    parseMode: ParseMode.Html,
                    replyMarkup: pollView.ReplyMarkup);
                
                poll.ChatId = message.Chat.Id;
                poll.MessageId = message.MessageId;
            }
        }
    }
}
