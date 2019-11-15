using System;
using System.Threading.Tasks;
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

        public async Task ProcessAsync(Update update) {
            Console.WriteLine("Processing . message...");
            var client = await ClientFactory.GetAsync();

            Pin pin = PinHelper.Parse(update.Message.ReplyToMessage);
            if (pin.IsActual()) {
                PollHelper.Create(pin);
                PollView pollView = PollHelper.AsView();

                if (PollHelper.Exists())
                    await client.DeleteMessageAsync(
                        chatId: PollHelper.ChatId,
                        messageId: PollHelper.MessageId);

                var message = await client.SendTextMessageAsync(
                    chatId: update.Message.Chat.Id,
                    text: pollView.Text,
                    parseMode: ParseMode.Html,
                    replyMarkup: pollView.ReplyMarkup);
                PollHelper.ChatId = message.Chat.Id;
                PollHelper.MessageId = message.MessageId;
            }

            await client.DeleteMessageAsync(
                chatId: update.Message.Chat.Id,
                messageId: update.Message.MessageId);
        }
    }
}
