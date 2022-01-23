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
            List<int> hourList = new List<int>() { 0, 10, 13, 16, 19, 22, 24 };
            (int left, int right) = GetBounds(hour, hourList);

            DateTimeOffset leftDateTimeOffset, rightDateTimeOffset;

            if (hourList[left] == 0) {
                leftDateTimeOffset = new DateTimeOffset((dateTimeOffset - TimeSpan.FromDays(1)).Date + new TimeSpan(22, 0, 0), botTimeOffset);
                rightDateTimeOffset = new DateTimeOffset(dateTimeOffset.Date + new TimeSpan(10, 0, 0), botTimeOffset);
            }
            else if (hourList[right] == 24) {
                leftDateTimeOffset = new DateTimeOffset(dateTimeOffset.Date + new TimeSpan(22, 0, 0), botTimeOffset);
                rightDateTimeOffset = new DateTimeOffset((dateTimeOffset + TimeSpan.FromDays(1)).Date + new TimeSpan(10, 0, 0), botTimeOffset);
            }
            else {
                leftDateTimeOffset = new DateTimeOffset(dateTimeOffset.Date + new TimeSpan(hourList[left], 0, 0), botTimeOffset);
                rightDateTimeOffset = new DateTimeOffset(dateTimeOffset.Date + new TimeSpan(hourList[right], 0, 0), botTimeOffset);
            }

            return new BattleInterval(leftDateTimeOffset, rightDateTimeOffset);
        }

        private static (int left, int right) GetBounds(int hour, List<int> hourList) {
            int left = 0, right = hourList.Count;
            while (left < right) {
                int index = left + ((right - left) >> 1);
                if (hour < hourList[index])
                    right = index;
                else if (hour > hourList[index])
                    left = index + 1;
                else {
                    right = index + 1;
                    break;
                }
            }
            left = right - 1;
            return (left, right);
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
