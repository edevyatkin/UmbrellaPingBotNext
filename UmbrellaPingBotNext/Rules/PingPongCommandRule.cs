﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace UmbrellaPingBotNext.Rules
{
    internal class PingPongCommandRule : IUpdateRule
    {
        public bool IsMatch(Update update) {
            return UpdateProcessor.GetRule<MessageRule>().IsMatch(update)
                && update.Message.Text.Equals("/ping");
        }

        public async Task ProcessAsync(Update update) {
            Console.WriteLine("Processing /ping message...");
            var client = await ClientFactory.GetAsync();

            await client.SendTextMessageAsync(
                  chatId: update.Message.Chat.Id,
                  text: "Pong!");
        }
    }
}