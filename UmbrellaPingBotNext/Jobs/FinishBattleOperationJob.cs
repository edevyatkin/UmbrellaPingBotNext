using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace UmbrellaPingBotNext.Jobs
{
    class FinishBattleOperationJob : IJob
    {
        public async Task Do() {
            if (!PollHelper.Exists())
                return;
            
            Console.WriteLine("Finish Battle Operation");

            var client = await ClientFactory.GetAsync();

            await client.DeleteMessageAsync(
                chatId: PollHelper.ChatId,
                messageId: PollHelper.MessageId);
            PollHelper.Reset();
        }
    }
}
