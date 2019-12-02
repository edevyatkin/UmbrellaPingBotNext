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
            if (!PollHelper.Exists())
                return;

            Console.WriteLine("Battle Notification Ping");

            var client = await ClientFactory.GetAsync();

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
