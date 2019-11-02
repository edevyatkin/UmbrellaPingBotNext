using System.Collections.Generic;

namespace UmbrellaPingBotNext
{
    internal class Poll
    {
        private List<PollUser> _usersPressedPin = new List<PollUser>();
        private Pin _pin;
        internal IReadOnlyList<PollUser> UsersPressedPin => _usersPressedPin;
        internal Pin Pin => _pin;

        public Poll(Pin pin) {
            _pin = pin;
        }

        public bool AddVote(PollUser user) {
            if (!_usersPressedPin.Contains(user)) {
                _usersPressedPin.Add(user);
                return true;
            }
            return false;
        }
    }
}