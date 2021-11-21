namespace WebhookApp.Services.Polls {
    public interface IPollsService {
        internal Poll CreatePoll(long chatId, Pin pin);
        internal void UpdatePoll(long chatId, Pin pin);
        void RemovePolls();
        internal Poll GetPoll(long chatId);
        bool HasPoll(long chatId);
    }
}