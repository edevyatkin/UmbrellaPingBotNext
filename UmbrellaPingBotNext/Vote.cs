using System;
using Telegram.Bot.Types;

namespace UmbrellaPingBotNext
{
    internal class Vote
    {
        private readonly User _user;
        public int UserId => _user.Id;
        public string Username => _user.Username;
        public string DisplayName => string.Join(' ', _user.FirstName, _user.LastName);
        public VoteType Type { get; }

        public Vote(User user, VoteType type) {
            _user = user;
            Type = type;
        }
    }
}
