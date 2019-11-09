using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types.Enums;

namespace UmbrellaPingBotNext.Jobs
{
    class BattleNotificationJob : IJob
    {
        public void Do() {
            var client = ClientFactory.GetAsync().GetAwaiter().GetResult();

            Console.WriteLine("Battle Notification");

            if (!PollHelper.Exists())
                return;

            PollView pollView = PollHelper.AsView();
            client.DeleteMessageAsync(
                chatId: PollHelper.ChatId,
                messageId: PollHelper.MessageId)
                .GetAwaiter().GetResult();

            var message = client.SendTextMessageAsync(
                chatId: PollHelper.ChatId,
                text: pollView.Text,
                parseMode: ParseMode.Html,
                replyMarkup: pollView.ReplyMarkup)
                .GetAwaiter().GetResult();
            PollHelper.ChatId = message.Chat.Id;
            PollHelper.MessageId = message.MessageId;
        }
    }
}
