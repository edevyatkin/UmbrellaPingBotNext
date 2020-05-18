using Telegram.Bot.Types;

namespace UmbrellaPingBotNext
{
    internal static class PollExtensions
    {
        internal static bool AddActiveVote(this Poll poll, User user) {
            return poll.AddVote(user, VoteType.Active);
        }

        internal static bool AddSleepVote(this Poll poll, User user) {
            return poll.AddVote(user, VoteType.Sleep);
        }

        internal static PollView AsView(this Poll poll) {
            return new PollView(poll);
        }
    }
}