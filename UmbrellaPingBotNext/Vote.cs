using System;
using Telegram.Bot.Types;

namespace UmbrellaPingBotNext
{
    internal class Vote
    {
        public int UserId { get;  }
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
