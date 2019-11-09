using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UmbrellaPingBotNext.Jobs
{
    class BattleNotificationPingJob : IJob
    {
        public void Do() {
            var client = ClientFactory.GetAsync().GetAwaiter().GetResult();

            Console.WriteLine("Battle Notification Ping");

            if (!PollHelper.Exists())
                return;

            var list = ConfigHelper.Get().Usernames.Except(PollHelper.Votes.Select(u => $"@{u.Username}").ToList()).ToList();
            
            var usernamesToPing = PingHelper.ConstructMessages(list);

            foreach (var usernames in usernamesToPing) {
                client.SendTextMessageAsync(
                    chatId: PollHelper.ChatId,
                    text: usernames)
                    .GetAwaiter().GetResult();
            }
        }
    }
}
