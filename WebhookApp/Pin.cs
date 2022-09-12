using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Telegram.Bot.Types;
using WebhookApp.Common;

namespace WebhookApp
{
    internal class Pin {
        private readonly int _messageId;
        private readonly long _chatId;
        private readonly BattleInterval _battleInterval;
        private const string Pattern = "(.*)(В атаку на |Все в защиту - )(.*)";
        public PinType Type { get; }
        public PinCompany Company { get; }

        public int BattleHour => _battleInterval.End.Hour;
        public string LinkToMessage => Utils.LinkToMessage(_chatId, _messageId);

        public Pin(Message message) {
            _messageId = message.MessageId;
            _chatId = message.Chat.Id;
            (Type, Company) = Parse(message);
            _battleInterval = BattleInterval.FromDateTimeUtc(message.Date.ToUniversalTime());
        }

        private static (PinType, PinCompany) Parse(Message message) {
            Match match = Regex.Match(message.Text, Pattern);
            var type = new PinType(match.Groups[1].Value);
            var company = new PinCompany(match.Groups[3].Value);
            return (type, company);
        }

        internal static bool IsPinMessage(Message message) => Regex.IsMatch(message.Text, Pattern);

        internal bool IsAttack() => Type.ToString() == PinType.Attack;

        internal bool IsActual() => _battleInterval == BattleInterval.FromDateTimeUtc(DateTime.UtcNow);

        public override bool Equals(object obj) {
            return obj is Pin pin &&
                   EqualityComparer<PinType>.Default.Equals(Type, pin.Type) &&
                   EqualityComparer<PinCompany>.Default.Equals(Company, pin.Company);
        }

        public override int GetHashCode() {
            return HashCode.Combine(Type, Company);
        }
    }
}
