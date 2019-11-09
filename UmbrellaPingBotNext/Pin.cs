using System;
using System.Collections.Generic;

namespace UmbrellaPingBotNext
{
    public class Pin
    {
        public int MessageId { get; set; }
        public long ChatId { get; set; }
        public DateTime MessageDate { get; set; }
        public PinType Type { get; set; }
        public PinCompany Company { get; set; }

        public int BattleHour => GetBattleInterval().End.Hour;

        public override bool Equals(object obj) {
            return obj is Pin pin &&
                   EqualityComparer<PinType>.Default.Equals(Type, pin.Type) &&
                   EqualityComparer<PinCompany>.Default.Equals(Company, pin.Company);
        }

        public override int GetHashCode() {
            return HashCode.Combine(Type, Company);
        }

        public bool IsAttack() {
            return Type.ToString() == PinType.Attack;
        }

        internal bool IsActual() {
            var battleInterval = BattleInterval.FromDateTimeUtc(DateTime.UtcNow);
            var pinBattleInverval = GetBattleInterval();
            return battleInterval == pinBattleInverval;
        }

        private BattleInterval GetBattleInterval() {
            return BattleInterval.FromDateTimeUtc(MessageDate.ToUniversalTime());
        }

    }
}
