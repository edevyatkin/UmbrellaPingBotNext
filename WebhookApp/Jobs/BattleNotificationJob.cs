using System;
using System.Threading.Tasks;
using Telegram.Bot.Types.Enums;
using WebhookApp.Services;

namespace WebhookApp.Jobs
{
    class BattleNotificationJob : IJob
    {
        private readonly BotService _botService;

        public BattleNotificationJob(BotService botService) {
            _botService = botService;
        }
        public async Task Do() {
            var polls = PollsHelper.Polls;
            foreach (var poll in polls.Values) {
                Console.WriteLine($"Battle Notification, chatId: {poll.ChatId.ToString()}");
                
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
