using Telegram.Bot.Types;

namespace WebhookApp.Services.Polls
{
    internal class Vote
    {
        public long UserId { get;  }
        public string Username { get; }
        public string DisplayName { get; }
        public VoteType Type { get; }

        public Vote(User user, VoteType type) {
            UserId = user.Id;
            Username = user.Username;
            DisplayName = string.Join(' ', user.FirstName, user.LastName);
            Type = type;
        }
    }
}
