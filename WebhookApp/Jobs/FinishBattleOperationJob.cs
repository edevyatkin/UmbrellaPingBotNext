using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using WebhookApp.Services;

namespace WebhookApp.Jobs
{
    class FinishBattleOperationJob : IJob
    {
        private readonly BotService _botService;
        private readonly ILogger<FinishBattleOperationJob> _logger;

        public FinishBattleOperationJob(BotService botService, ILogger<FinishBattleOperationJob> logger) {
            _botService = botService;
            _logger = logger;
        }
        
        public async Task Do() {
            var polls = PollsHelper.Polls;
            foreach (var poll in polls.Values) {
                _logger.LogInformation($"Finish Battle Operation, chatId: {poll.ChatId.ToString()}");
                
                await _botService.Client.DeleteMessageAsync(
                    chatId: poll.ChatId,
                    messageId: poll.MessageId);
            }
            PollsHelper.RemovePolls();
        }
    }
}
