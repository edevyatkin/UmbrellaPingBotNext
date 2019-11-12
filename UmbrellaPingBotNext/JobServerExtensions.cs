using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telegram.Bot.Types.Enums;
using UmbrellaPingBotNext.Jobs;

namespace UmbrellaPingBotNext
{
    internal static class JobServerExtensions
    {
        internal static void AddBattleNotification(this JobServer server, int hour, int minute) {
            server.AddDaily<BattleNotificationJob>(hour, minute);
        }

        internal static void AddBattleNotificationPing(this JobServer server, int hour, int minute) {
            server.AddDaily<BattleNotificationPingJob>(hour, minute);
        }

        internal static void AddFinishBattleOperation(this JobServer server, int hour, int minute) {
            server.AddDaily<FinishBattleOperationJob>(hour, minute);
        }
    }
}
