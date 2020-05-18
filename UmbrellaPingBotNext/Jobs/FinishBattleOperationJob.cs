using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace UmbrellaPingBotNext.Jobs
{
    class FinishBattleOperationJob : IJob
    {
        public async Task Do() {
            var client = await ClientFactory.GetAsync();
            var polls = PollsHelper.Polls;
            foreach (var poll in polls.Values) {
                Console.WriteLine($"Finish Battle Operation, chatId: {poll.ChatId.ToString()}");
                
                await client.DeleteMessageAsync(
                    chatId: poll.ChatId,
                    messageId: poll.MessageId);
            }
            PollsHelper.RemovePolls();
        }
    }
}
