using System;
using System.Collections.Generic;

namespace WebhookApp.Services.PollsService
{
    internal static class PollsHelper
    {
        internal static readonly Dictionary<long, Poll> Polls = new Dictionary<long, Poll>();

        internal static Poll CreatePoll(long chatId, Pin pin) {
            if (pin == null)
                throw new ArgumentException(nameof(pin));
            
            Polls[chatId] = new Poll(pin);
            return Polls[chatId];
        }

        internal static void UpdatePoll(long chatId, Pin pin) {
            if (!HasPoll(chatId))
                throw new ArgumentException(nameof(chatId));
            if (pin == null)
                throw new ArgumentException(nameof(pin));
            
            Poll poll = Polls[chatId];
            if (!pin.Equals(poll.Pin)) {
                poll.ClearVotes();
            }
            poll.Pin = pin;
        }
        
        internal static void RemovePolls() {
            Polls.Clear();
        }

        internal static Poll GetPoll(long chatId) {
            if (!HasPoll(chatId))
                throw new ArgumentException(nameof(chatId));
            
            return Polls[chatId];
        }

        internal static bool HasPoll(long chatId) {
            return Polls.ContainsKey(chatId);
        }
    }
}
