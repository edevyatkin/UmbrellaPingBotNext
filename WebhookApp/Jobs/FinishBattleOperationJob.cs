using System;
using System.Threading.Tasks;
using WebhookApp.Services;

namespace WebhookApp.Jobs
{
    class FinishBattleOperationJob : IJob
    {
        private readonly BotService _botService;

        public FinishBattleOperationJob(BotService botService) {
            _botService = botService;
        }
        
        public async Task Do() {
            var polls = PollsHelper.Polls;
            foreach (var poll in polls.Values) {
                Console.WriteLine($"Finish Battle Operation, chatId: {poll.ChatId.ToString()}");
                
                await _botService.Client.DeleteMessageAsync(
                    chatId: poll.ChatId,
                    messageId: poll.MessageId);
            }
            PollsHelper.RemovePolls();
        }
    }
}
