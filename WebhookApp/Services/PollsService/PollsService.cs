namespace WebhookApp.Services.PollsService {
    class PollsService : IPollsService {
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