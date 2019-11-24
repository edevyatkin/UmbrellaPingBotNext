using System.Threading;
using System.Threading.Tasks;

namespace UmbrellaPingBotNext
{
    internal static class PollVoteThrottle
    {
        private static int _count = 15; // Telegram allows 20 in a minute, 
                                        // but maybe bot sent 5 messages itself

        public static async Task Acquire() {
            while (_count == 0) {
                await Task.Delay(500);
            }
            Interlocked.Decrement(ref _count);
            StartTimer();
        }

        private static void StartTimer() {
            Timer t = new Timer((state) => {
                Interlocked.Increment(ref _count);
                Timer t = (Timer)state;
                t.Dispose();
            });
            t.Change(60000, 0);
        }
    }
}
