using System;
using Telegram.Bot.Types;

namespace UmbrellaPingBotNext
{
    internal class PollHelper
    {
        public static Pin Pin { get; private set; }
        public static long ChatId { get; internal set; }
        public static int MessageId { get; internal set; }

        private static Poll _poll;

        internal static void CreatePoll(Message message) {
            Pin pin = PinHelper.Parse(message.ReplyToMessage);
            if (!pin.Equals(Pin)) {
                _poll = new Poll(pin);
                Pin = pin;
            }
        }

        internal static bool AddVote(User user) {
            var puser = new PollUser(user);
            return _poll.AddVote(puser);
        }

        internal static PollView AsView() {
            return new PollView(_poll);
        }
    }
}