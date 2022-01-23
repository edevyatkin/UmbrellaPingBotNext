using System;
using System.Collections.Generic;
using WebhookApp.Common;

namespace WebhookApp
{
    public class BattleInterval
    {
        public DateTimeOffset Start { get; }
        public DateTimeOffset End { get; }

        internal BattleInterval(DateTimeOffset start, DateTimeOffset end) {
            Start = start;
            End = end;
        }

        internal static BattleInterval FromDateTimeUtc(DateTime utcDateTime) {
            if (utcDateTime.Kind != DateTimeKind.Utc)
                throw new ArgumentException(nameof(utcDateTime));
            
            var botTimeOffset = Constants.BotTimeZoneInfo.BaseUtcOffset;
            var dateTimeOffset = new DateTimeOffset(utcDateTime).ToOffset(botTimeOffset);
            int hour = dateTimeOffset.Hour;
            List<int> hourList = new List<int>() { 10, 13, 16, 19, 22 };

            int ix = hourList.BinarySearch(hour);
            if (ix < 0)
                ix = ~ix - 1;

            DateTimeOffset leftDateTimeOffset, rightDateTimeOffset;

            if (ix == -1) {
                leftDateTimeOffset = new DateTimeOffset((dateTimeOffset - TimeSpan.FromDays(1)).Date + new TimeSpan(hourList[^1], 0, 0), botTimeOffset);
                rightDateTimeOffset = new DateTimeOffset(dateTimeOffset.Date + new TimeSpan(hourList[0], 0, 0), botTimeOffset);
            }
            else if (ix == hourList.Count-1) {
                leftDateTimeOffset = new DateTimeOffset(dateTimeOffset.Date + new TimeSpan(hourList[^1], 0, 0), botTimeOffset);
                rightDateTimeOffset = new DateTimeOffset((dateTimeOffset + TimeSpan.FromDays(1)).Date + new TimeSpan(hourList[0], 0, 0), botTimeOffset);
            }
            else {
                leftDateTimeOffset = new DateTimeOffset(dateTimeOffset.Date + new TimeSpan(hourList[ix], 0, 0), botTimeOffset);
                rightDateTimeOffset = new DateTimeOffset(dateTimeOffset.Date + new TimeSpan(hourList[ix+1], 0, 0), botTimeOffset);
            }

            return new BattleInterval(leftDateTimeOffset, rightDateTimeOffset);
        }

        public override bool Equals(object obj) {
            return obj is BattleInterval interval &&
                   Start.Equals(interval.Start) &&
                   End.Equals(interval.End);
        }

        public override int GetHashCode() {
            return HashCode.Combine(Start, End);
        }

        public static bool operator ==(BattleInterval left, BattleInterval right) {
            if (left is null)
                return false;

            return left.Equals(right);
        }

        public static bool operator !=(BattleInterval left, BattleInterval right) {
            return !(left == right);
        }
    }
}
