using System;
using System.Collections.Generic;
using System.Text;

namespace UmbrellaPingBotNext.Jobs
{
    class AddFinishBattleOperationJob : IJob
    {
        public void Do() {
            var client = ClientFactory.GetAsync().GetAwaiter().GetResult();

            Console.WriteLine("Finish Battle Operation");

            if (PollHelper.Exists()) {
                client.DeleteMessageAsync(
                    chatId: PollHelper.ChatId,
                    messageId: PollHelper.MessageId)
                    .GetAwaiter().GetResult();
                PollHelper.Reset();
            }
        }
    }
}
