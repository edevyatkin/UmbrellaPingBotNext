using System;
using System.Collections.Generic;

namespace UmbrellaPingBotNext
{
    public class BattleInterval
    {
        public DateTimeOffset Start { get; }
        public DateTimeOffset End { get; }

        private BattleInterval(DateTimeOffset start, DateTimeOffset end) {
            Start = start;
            End = end;
        }

        internal static BattleInterval FromDateTimeUtc(DateTime utcDateTime) {
            DateTimeOffset dateTimeOffset = new DateTimeOffset(utcDateTime).ToOffset(TimeSpan.FromHours(3));
            int hour = dateTimeOffset.Hour;
            List<int> hourList = new List<int>() { 0, 10, 13, 16, 19, 22, 24 };
            (int left, int right) = GetBounds(hour, hourList);

            DateTimeOffset leftDateTimeOffset, rightDateTimeOffset;

            if (hourList[left] == 0) {
                leftDateTimeOffset = new DateTimeOffset((dateTimeOffset - TimeSpan.FromDays(1)).Date + new TimeSpan(22, 0, 0));
                rightDateTimeOffset = new DateTimeOffset(dateTimeOffset.Date + new TimeSpan(10, 0, 0));
            }
            else if (hourList[right] == 24) {
                leftDateTimeOffset = new DateTimeOffset(dateTimeOffset.Date + new TimeSpan(22, 0, 0));
                rightDateTimeOffset = new DateTimeOffset((dateTimeOffset + TimeSpan.FromDays(1)).Date + new TimeSpan(10, 0, 0));
            }
            else {
                leftDateTimeOffset = new DateTimeOffset(dateTimeOffset.Date + new TimeSpan(hourList[left], 0, 0));
                rightDateTimeOffset = new DateTimeOffset(dateTimeOffset.Date + new TimeSpan(hourList[right], 0, 0));
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
