﻿using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace UmbrellaPingBotNext.Rules
{
    internal class ReplyToInfoBotMessageRule : IUpdateRule
    {
        public bool IsMatch(Update update) {
            return UpdateProcessor.GetRule<ReplyToMessageRule>().IsMatch(update)
                && update.Message.ReplyToMessage.From != null
                && update.Message.ReplyToMessage.From.Username == "StartupWarsInfoBot";
        }

        public void Process(Update update) { }
    }
}
