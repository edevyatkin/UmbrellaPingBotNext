using System;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace UmbrellaPingBotNext.Rules
{
    internal class DotMessageRule : IUpdateRule
    {
        public bool IsMatch(Update update) {
            var tmr = UpdateProcessor.GetRule<TextMessageRule>();
            var config = ConfigHelper.Get();
            var message = update.Message;
            if (tmr.IsMatch(update)
                && message.Text.Equals(".")
                && message.Chat.Type == ChatType.Supergroup
                && message.Chat.Id == long.Parse(config.ChatId)
                && message.ReplyToMessage != null
                && message.ReplyToMessage.ForwardFrom != null
                && message.ReplyToMessage.ForwardFrom.Username.Equals("StartupWarsInfoBot") 
                && PinHelper.IsPin(message.ReplyToMessage))
                return true;

            return false;
        }

        public void Process(Update update) {
            Console.WriteLine("Processing . message...");
            var client = ClientFactory.GetAsync().GetAwaiter().GetResult();

            PollHelper.CreatePoll(update.Message);

            PollView pollView = PollHelper.AsView();
            string pollText = pollView.Text;

            var message = client.SendTextMessageAsync(
                chatId: update.Message.Chat.Id, 
                text: pollText,
                parseMode: ParseMode.Html,
                replyMarkup: pollView.ReplyMarkup)
                .GetAwaiter().GetResult();
            PollHelper.ChatId = message.Chat.Id;
            PollHelper.MessageId = message.MessageId;

            client.DeleteMessageAsync(
                chatId: update.Message.Chat.Id,
                messageId: update.Message.MessageId)
                .GetAwaiter().GetResult();
        }
    }
}
