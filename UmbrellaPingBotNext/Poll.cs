using System.Collections.Generic;
using Telegram.Bot.Types;

namespace UmbrellaPingBotNext
{
    internal class Poll
    {
        internal List<PollUser> Votes { get; } = new List<PollUser>();
        internal Pin Pin { get; }

        public Poll(Pin pin) {
            Pin = pin;
        }

        public Poll(Pin pin, List<PollUser> votes) : this(pin) {
            Votes.AddRange(votes);
        }

        public bool AddVote(PollUser user) {
            if (!Votes.Contains(user)) {
                Votes.Add(user);
                return true;
            }
            var voteUser = Votes.Find(u => u.Equals(user));
            if (voteUser.Status != user.Status) {
                Votes.Remove(voteUser);
                Votes.Add(user);
                return true;
            }
            return false;
        }

        public void ClearVotes() {
            Votes.Clear();
        }
    }
}