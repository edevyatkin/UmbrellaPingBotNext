using System.Collections.Generic;
using Telegram.Bot.Types;

namespace UmbrellaPingBotNext
{
    internal class Poll
    {
        internal List<Vote> Votes { get; } = new List<Vote>();
        internal Pin Pin { get; set; }
        public long ChatId { get; set; }
        public int MessageId { get; set; }

        public Poll(Pin pin) {
            Pin = pin;
        }

        public bool AddVote(User user, VoteType type) {
            var foundVote = Votes.Find(v => v.UserId == user.Id);
            if (foundVote != null) {
                if (foundVote.Type != type)
                    Votes.Remove(foundVote);
                else 
                    return false;
            }
            Votes.Add(new Vote(user, type));
            return true;
        }

        public void ClearVotes() {
            Votes.Clear();
        }
        
    }
}
