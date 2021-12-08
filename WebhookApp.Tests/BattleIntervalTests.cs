using System;
using System.Collections.Generic;
using Xunit;

namespace WebhookApp.Tests {
    public class BattleIntervalTests {
        public static IEnumerable<object[]> Data =>
            new List<object[]>
            {
                new object[] {
                    new DateTime(2021, 12, 08, 0, 11, 34, DateTimeKind.Utc),
                    new BattleInterval(
                        new DateTimeOffset(2021, 12, 07, 22, 0, 0,TimeSpan.FromHours(3)),
                        new DateTimeOffset(2021, 12, 08, 10, 0, 0,TimeSpan.FromHours(3)))
                },
                new object[] {
                    new DateTime(2021, 12, 08, 8, 37, 8, DateTimeKind.Utc),
                    new BattleInterval(
                        new DateTimeOffset(2021, 12, 08, 10, 0, 0,TimeSpan.FromHours(3)),
                        new DateTimeOffset(2021, 12, 08, 13, 0, 0,TimeSpan.FromHours(3)))
                },
                new object[] {
                    new DateTime(2021, 12, 08, 11, 15, 48, DateTimeKind.Utc),
                    new BattleInterval(
                        new DateTimeOffset(2021, 12, 08, 13, 0, 0,TimeSpan.FromHours(3)),
                        new DateTimeOffset(2021, 12, 08, 16, 0, 0,TimeSpan.FromHours(3)))
                },
                new object[] {
                    new DateTime(2021, 12, 08, 14, 25, 31, DateTimeKind.Utc),
                    new BattleInterval(
                        new DateTimeOffset(2021, 12, 08, 16, 0, 0,TimeSpan.FromHours(3)),
                        new DateTimeOffset(2021, 12, 08, 19, 0, 0,TimeSpan.FromHours(3)))
                },
                new object[] {
                    new DateTime(2021, 12, 08, 18, 9, 59, DateTimeKind.Utc),
                    new BattleInterval(
                        new DateTimeOffset(2021, 12, 08, 19, 0, 0,TimeSpan.FromHours(3)),
                        new DateTimeOffset(2021, 12, 08, 22, 0, 0,TimeSpan.FromHours(3)))
                },
                new object[] {
                    new DateTime(2021, 12, 08, 20, 38, 14, DateTimeKind.Utc),
                    new BattleInterval(
                        new DateTimeOffset(2021, 12, 08, 22, 0, 0,TimeSpan.FromHours(3)),
                        new DateTimeOffset(2021, 12, 09, 10, 0, 0,TimeSpan.FromHours(3)))
                },
                new object[] {
                    new DateTime(2021, 12, 08, 22, 51, 46, DateTimeKind.Utc),
                    new BattleInterval(
                        new DateTimeOffset(2021, 12, 08, 22, 0, 0,TimeSpan.FromHours(3)),
                        new DateTimeOffset(2021, 12, 09, 10, 0, 0,TimeSpan.FromHours(3)))
                },
                new object[] {
                    new DateTime(2021, 12, 08, 23, 1, 8, DateTimeKind.Utc),
                    new BattleInterval(
                        new DateTimeOffset(2021, 12, 08, 22, 0, 0,TimeSpan.FromHours(3)),
                        new DateTimeOffset(2021, 12, 09, 10, 0, 0,TimeSpan.FromHours(3)))
                },
                new object[] {
                    new DateTime(2021, 12, 08, 0, 0, 0, DateTimeKind.Utc),
                    new BattleInterval(
                        new DateTimeOffset(2021, 12, 07, 22, 0, 0,TimeSpan.FromHours(3)),
                        new DateTimeOffset(2021, 12, 08, 10, 0, 0,TimeSpan.FromHours(3)))
                },
                new object[] {
                    new DateTime(2021, 12, 08, 23, 59, 59, DateTimeKind.Utc),
                    new BattleInterval(
                        new DateTimeOffset(2021, 12, 08, 22, 0, 0,TimeSpan.FromHours(3)),
                        new DateTimeOffset(2021, 12, 09, 10, 0, 0,TimeSpan.FromHours(3)))
                },
                new object[] {
                    new DateTime(2021, 12, 07, 21, 0, 0, DateTimeKind.Utc),
                    new BattleInterval(
                        new DateTimeOffset(2021, 12, 07, 22, 0, 0,TimeSpan.FromHours(3)),
                        new DateTimeOffset(2021, 12, 08, 10, 0, 0,TimeSpan.FromHours(3)))
                },
                new object[] {
                    new DateTime(2021, 12, 07, 20, 59, 59, DateTimeKind.Utc),
                    new BattleInterval(
                        new DateTimeOffset(2021, 12, 07, 22, 0, 0,TimeSpan.FromHours(3)),
                        new DateTimeOffset(2021, 12, 08, 10, 0, 0,TimeSpan.FromHours(3)))
                }
            };

        [Theory]
        [MemberData(nameof(Data))]
        public void TestFromDateTimeUtcConversion(DateTime dt, BattleInterval realBi) {
            var bi = BattleInterval.FromDateTimeUtc(dt);
            Assert.StrictEqual(realBi, bi);
        }
        
        [Fact]
        public void TestFromDateTimeUtcTestThrowsWhenUnspecifiedDateTimeKind() {
            var dt = new DateTime(2021, 12, 07, 20, 59, 59);
            Assert.Throws<ArgumentException>(() => BattleInterval.FromDateTimeUtc(dt));
        }
        
        [Fact]
        public void TestFromDateTimeUtcTestThrowsWhenLocalDateTimeKind() {
            var dt = new DateTime(2021, 12, 07, 20, 59, 59, DateTimeKind.Local);
            Assert.Throws<ArgumentException>(() => BattleInterval.FromDateTimeUtc(dt));
        }
    }
}
