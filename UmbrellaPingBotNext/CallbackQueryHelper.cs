using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types;

namespace UmbrellaPingBotNext
{
    internal static class CallbackQueryHelper
    {
        public static bool MayAnswer(CallbackQuery callbackQuery) =>
            DateTime.UtcNow - callbackQuery.Message.EditDate < TimeSpan.FromSeconds(15);
    }
}
