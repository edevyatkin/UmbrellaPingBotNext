using System;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace UmbrellaPingBotNext.Rules
{
    internal class DotMessageRule : IUpdateRule
    {
        public bool IsMatch(Update update) {
            var config = ConfigHelper.Get();
            if (update.Type == UpdateType.Message
                && update.Message.Type == MessageType.Text
                && update.Message.Text.Equals(".")
                && update.Message.Chat.Type == ChatType.Supergroup
                && update.Message.Chat.Id == long.Parse(config.ChatId)
                && update.Message.ReplyToMessage != null
                && update.Message.ReplyToMessage.From != null
                && update.Message.ReplyToMessage.From.Username.Equals("StartupWarsInfoBot") 
                && PinHelper.IsPin(update.Message.ReplyToMessage))
                return true;

            return false;
        }

        public void Process(Update update) {
            Console.WriteLine("Processing . message...");
            var client = ClientFactory.GetAsync().GetAwaiter().GetResult();

            Pin pin = PinHelper.Parse(update.Message.ReplyToMessage);
            if (pin.IsActual()) {
                PollHelper.Create(pin);
                PollView pollView = PollHelper.AsView();

                if (PollHelper.Exists())
                    client.DeleteMessageAsync(
                        chatId: PollHelper.ChatId,
                        messageId: PollHelper.MessageId)
                        .GetAwaiter().GetResult();

                var message = client.SendTextMessageAsync(
                    chatId: update.Message.Chat.Id,
                    text: pollView.Text,
                    parseMode: ParseMode.Html,
                    replyMarkup: pollView.ReplyMarkup)
                    .GetAwaiter().GetResult();
                PollHelper.ChatId = message.Chat.Id;
                PollHelper.MessageId = message.MessageId;
            }

            client.DeleteMessageAsync(
                chatId: update.Message.Chat.Id,
                messageId: update.Message.MessageId)
                .GetAwaiter().GetResult();
        }
    }
}
