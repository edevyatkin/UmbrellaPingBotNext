using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace UmbrellaPingBotNext.Rules
{
    internal class DelCommandRule : IUpdateRule
    {
        public bool IsMatch(Update update) {
            return UpdateProcessor.GetRule<ReplyToBotMessageRule>().IsMatch(update)
                && update.Message.Text.Equals("/del")
                && update.Message.ReplyToMessage.Text.Contains("Битва")
                && update.Message.ReplyToMessage.MessageId != PollHelper.MessageId;
        }

        public void Process(Update update) {
            Console.WriteLine("Processing /del message...");
            var client = ClientFactory.GetAsync().GetAwaiter().GetResult();

            client.DeleteMessageAsync(
                chatId: update.Message.Chat.Id,
                messageId: update.Message.MessageId)
                .GetAwaiter().GetResult();

            client.DeleteMessageAsync(
                chatId: update.Message.ReplyToMessage.Chat.Id,
                messageId: update.Message.ReplyToMessage.MessageId)
                .GetAwaiter().GetResult();
        }
    }
}
