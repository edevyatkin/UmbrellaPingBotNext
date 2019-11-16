using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types;

namespace UmbrellaPingBotNext.Rules
{
    internal class PingPongCommandRule : IUpdateRule
    {
        public bool IsMatch(Update update) {
            return UpdateProcessor.GetRule<MessageRule>().IsMatch(update)
                && update.Message.Text.Equals("/ping");
        }

        public void Process(Update update) {
            Console.WriteLine("Processing /ping message...");
            var client = ClientFactory.GetAsync().GetAwaiter().GetResult();

            client.SendTextMessageAsync(
                  chatId: update.Message.Chat.Id,
                  text: "Pong!")
                  .GetAwaiter().GetResult();
        }
    }
}
