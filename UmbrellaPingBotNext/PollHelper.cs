using System;
using System.Collections.Generic;
using System.Linq;
using Telegram.Bot.Types;

namespace UmbrellaPingBotNext
{
    internal class PollHelper
    {
        public static long ChatId { get; internal set; }
        public static int MessageId { get; internal set; }
        public static List<Vote> Votes => _poll.Votes;
        public static Pin Pin => _poll.Pin;

        private static Poll _poll;

        internal static void Create(Pin pin) {
            if (_poll == null || !pin.Equals(Pin))
                _poll = new Poll(pin);
            else
                _poll = new Poll(pin, _poll.Votes);
        }

        internal static bool AddActiveVote(User user) {
            ThrowIfPollNull();
            return _poll.AddVote(user, VoteType.Active);
        }

        internal static bool AddSleepVote(User user) {
            ThrowIfPollNull();
            return _poll.AddVote(user, VoteType.Sleep);
        }

        internal static PollView AsView() {
            ThrowIfPollNull();
            return new PollView(_poll);
        }

        private static void ThrowIfPollNull() {
            if (_poll == null) {
                throw new Exception("Poll is not created");
            }
        }

        internal static bool Exists() => ChatId != 0 && MessageId != 0;

        internal static void Reset() {
            ChatId = 0;
            MessageId = 0;
            _poll = null;
        }
    }
}
