using Microsoft.Extensions.Logging;
using WebhookApp.Services.Polls.Infrastructure;

namespace WebhookApp.Services.Polls {
    class PollsService : IPollsService {
        private readonly ILogger<IPollsService> _logger;
        private readonly IPollsStorage _storage;

        public PollsService(ILogger<PollsService> logger, IPollsStorage storage) {
            _logger = logger;
            _storage = storage;
        }
        
        Poll IPollsService.CreatePoll(long chatId, Pin pin) {
            return PollsHelper.CreatePoll(chatId, pin);
        }

        void IPollsService.UpdatePoll(long chatId, Pin pin) {
            PollsHelper.UpdatePoll(chatId, pin);
        }

        public void RemovePolls() {
            PollsHelper.RemovePolls();
        }

        Poll IPollsService.GetPoll(long chatId) {
            return PollsHelper.GetPoll(chatId);
        }

        public bool HasPoll(long chatId) {
            return PollsHelper.HasPoll(chatId);
        }
    }
}