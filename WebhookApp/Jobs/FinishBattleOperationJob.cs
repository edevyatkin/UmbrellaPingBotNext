using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using WebhookApp.Services.Polls;

namespace WebhookApp.Jobs
{
    class FinishBattleOperationJob : IJob
    {
        private readonly ITelegramBotClient _botClient;
        private readonly ILogger<FinishBattleOperationJob> _logger;

        public FinishBattleOperationJob(ITelegramBotClient botClient, ILogger<FinishBattleOperationJob> logger) {
            _botClient = botClient;
            _logger = logger;
        }
        
        public async Task Do() {
            var polls = PollsHelper.Polls;
            foreach (var poll in polls.Values) {
                _logger.LogInformation($"Finish Battle Operation, chatId: {poll.ChatId.ToString()}");
                
                await _botClient.DeleteMessage(
                    chatId: poll.ChatId,
                    messageId: poll.MessageId);
            }
            PollsHelper.RemovePolls();
        }
    }
}
