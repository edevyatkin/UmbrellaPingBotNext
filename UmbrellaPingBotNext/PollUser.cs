using System;
using Telegram.Bot.Types;

namespace UmbrellaPingBotNext
{
    internal class PollUser
    {
        private readonly User _user;
        public int Id => _user.Id;
        public string Username => _user.Username;
        public string DisplayName => string.Join(' ', _user.FirstName, _user.LastName);
        public PollUserStatus Status { get;  }

        public PollUser(User user, PollUserStatus status) {
            _user = user;
            Status = status;
        }

        public override bool Equals(object obj) {
            return obj is PollUser user &&
                   Id == user.Id;
        }

        public override int GetHashCode() {
            return HashCode.Combine(Id);
        }

        public override string ToString() {
            return DisplayName;
        }
    }
}