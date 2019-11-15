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

            Console.WriteLine("Finish Battle Operation");

            if (PollHelper.Exists()) {
                await client.DeleteMessageAsync(
                    chatId: PollHelper.ChatId,
                    messageId: PollHelper.MessageId);
                PollHelper.Reset();
            }
        }
    }
}
