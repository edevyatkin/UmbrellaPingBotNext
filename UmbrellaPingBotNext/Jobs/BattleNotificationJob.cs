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

            Console.WriteLine("Battle Notification");

            if (!PollHelper.Exists())
                return;

            PollView pollView = PollHelper.AsView();
            await client.DeleteMessageAsync(
                chatId: PollHelper.ChatId,
                messageId: PollHelper.MessageId);

            var message = await client.SendTextMessageAsync(
                chatId: PollHelper.ChatId,
                text: pollView.Text,
                parseMode: ParseMode.Html,
                replyMarkup: pollView.ReplyMarkup);
            PollHelper.ChatId = message.Chat.Id;
            PollHelper.MessageId = message.MessageId;
        }
    }
}
