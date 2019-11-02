using System;
using Telegram.Bot.Types;

namespace UmbrellaPingBotNext
{
    internal class PollUser
    {
        private readonly User _user;
        public string Username => _user.Username;
        public string DisplayName => string.Join(' ', _user.FirstName, _user.LastName);

        public PollUser(User user) {
            _user = user;
        }

        public override bool Equals(object obj) {
            return obj is PollUser user &&
                   Username == user.Username;
        }

        public override int GetHashCode() {
            return HashCode.Combine(Username);
        }

        public override string ToString() {
            return DisplayName;
        }
    }
}