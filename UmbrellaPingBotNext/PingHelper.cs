using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UmbrellaPingBotNext
{
    class PingHelper
    {
        private const int partCount = 5;

        public static IEnumerable<string> ConstructMessages(List<string> usernames) {
            var list = SplitList(usernames);
            foreach (var s in list) {
                yield return string.Join(' ', s);
            }
        }

        // https://stackoverflow.com/a/11463800
        private static IEnumerable<List<T>> SplitList<T>(List<T> items) {
            items.Sort();
            for (int i = 0; i < items.Count; i += partCount) {
                yield return items.GetRange(i, Math.Min(partCount, items.Count - i));
            }
        }
    }
}
