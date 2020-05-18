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
            var polls = PollsHelper.Polls;
            var usernames = ConfigHelper.Get().Usernames;
            foreach (var poll in polls.Values) {
                if (!usernames.ContainsKey(poll.ChatId))
                    continue;
                
                Console.WriteLine($"Battle Notification Ping, chatId: {poll.ChatId.ToString()}");

                var list = usernames[poll.ChatId].Except(poll.Votes.Select(u => $"@{u.Username}")).ToList();
            
                var usernamesToPing = PingHelper.ConstructMessages(list);

                foreach (var names in usernamesToPing) {
                    await client.SendTextMessageAsync(
                        chatId: poll.ChatId,
                        text: names);
                }
            }
        }
    }
}
