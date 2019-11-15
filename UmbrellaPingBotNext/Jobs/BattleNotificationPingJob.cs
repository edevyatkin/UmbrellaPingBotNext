using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UmbrellaPingBotNext.Jobs
{
    class BattleNotificationPingJob : IJob
    {
        public async Task Do() {
            var client = await ClientFactory.GetAsync();

            Console.WriteLine("Battle Notification Ping");

            if (!PollHelper.Exists())
                return;

            var list = ConfigHelper.Get().Usernames.Except(PollHelper.Votes.Select(u => $"@{u.Username}").ToList()).ToList();
            
            var usernamesToPing = PingHelper.ConstructMessages(list);

            foreach (var usernames in usernamesToPing) {
                await client.SendTextMessageAsync(
                    chatId: PollHelper.ChatId,
                    text: usernames);
            }
        }
    }
}
