using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Telegram.Bot.Types;

namespace UmbrellaPingBotNext
{
    internal class Pin {
        private readonly int _messageId;
        private readonly long _chatId;
        private readonly DateTime _messageDate;
        private readonly PinType _type;
        private readonly PinCompany _company;
        private readonly BattleInterval _battleInterval;
        private const string pattern = "(.*)(В атаку на |Все в защиту - )(.*)";

        public PinType Type => _type;
        public PinCompany Company => _company;
        public int BattleHour => _battleInterval.End.Hour;
        public string LinkToMessage => $"https://t.me/c/{_chatId.ToString().Substring(4)}/{_messageId}";

        public Pin(Message message) {
            _messageId = message.MessageId;
            _chatId = message.Chat.Id;
            _messageDate = message.Date;
            (_type, _company) = Parse(message);
            _battleInterval = BattleInterval.FromDateTimeUtc(_messageDate.ToUniversalTime());
        }

        private (PinType, PinCompany) Parse(Message message) {
            Match match = Regex.Match(message.Text, pattern);
            var type = new PinType(match.Groups[1].Value);
            var company = new PinCompany(match.Groups[3].Value);
            return (type, company);
        }

        internal static bool IsPinMessage(Message message) => Regex.IsMatch(message.Text, pattern);

        internal bool IsAttack() => _type.ToString() == PinType.Attack;

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
