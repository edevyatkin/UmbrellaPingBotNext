using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.Enums;

namespace UmbrellaPingBotNext.Jobs
{
    class BattleNotificationJob : IJob
    {
        public async Task Do() {
            var client = await ClientFactory.GetAsync();
            var polls = PollsHelper.Polls;
            foreach (var poll in polls.Values) {
                Console.WriteLine($"Battle Notification, chatId: {poll.ChatId.ToString()}");
                
                PollView pollView = poll.AsView();
                await client.DeleteMessageAsync(
                    chatId: poll.ChatId,
                    messageId: poll.MessageId);

                var message = await client.SendTextMessageAsync(
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
