using System;
using System.Globalization;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace UmbrellaPingBotNext.Rules
{
    internal class DotMessageRule : IUpdateRule
    {
        public bool IsMatch(Update update) {
            return UpdateProcessor.GetRule<ReplyToInfoBotMessageRule>().IsMatch(update)
                && update.Message.Text.Equals(".")
                && Pin.IsPinMessage(update.Message.ReplyToMessage);
        }

        public async Task ProcessAsync(Update update) {
            Console.WriteLine($"[ {DateTime.Now.ToLocalTime().ToString(CultureInfo.InvariantCulture)} ] Processing . message... , chatId: {update.Message.Chat.Id.ToString()}, {update.Message.From.Username}");
            var client = await ClientFactory.GetAsync();
            
            Pin pin = new Pin(update.Message.ReplyToMessage);
            if (pin.IsActual()) {
                Poll poll;
                
                if (PollsHelper.HasPoll(update.Message.Chat.Id)) {
                    poll = PollsHelper.GetPoll(update.Message.Chat.Id);
                    await client.DeleteMessageAsync(
                        chatId: poll.ChatId,
                        messageId: poll.MessageId);
                    PollsHelper.UpdatePoll(poll.ChatId, pin);
                }
                else {
                    poll = PollsHelper.CreatePoll(update.Message.Chat.Id, pin);
                }
                
                PollView pollView = poll.AsView();

                var message = await client.SendTextMessageAsync(
                    chatId: update.Message.Chat.Id,
                    text: pollView.Text,
                    parseMode: ParseMode.Html,
                    replyMarkup: pollView.ReplyMarkup);
                
                poll.ChatId = message.Chat.Id;
                poll.MessageId = message.MessageId;
            }

            await client.DeleteMessageAsync(
                chatId: update.Message.Chat.Id,
                messageId: update.Message.MessageId);
        }
    }
}
