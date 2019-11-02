using System;

namespace UmbrellaPingBotNext
{
    public class PinType
    {
        private readonly string _text;
        public static string Defence => "🛡";
        public static string Attack => "⚔";

        public PinType(string type) {
            if (type == Defence) {
                _text = Defence;
            }
            else if (type == Attack) {
                _text = Attack;
            }
            else
                throw new ArgumentException("Incorrect pin type", nameof(type));
        }

        public override string ToString() {
            return _text;
        }

        public override bool Equals(object obj) {
            return obj is PinType type &&
                   _text == type._text;
        }

        public override int GetHashCode() {
            return HashCode.Combine(_text);
        }
    }
}
