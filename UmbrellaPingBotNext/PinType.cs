using System;

namespace UmbrellaPingBotNext
{
    internal class PinType
    {
        private readonly string _text;
        public static string Defence => "🛡";
        public static string Attack => "⚔";

        public PinType(string type) {
            _text = type switch
            {
                var x when x == Defence || x == Attack => x,
                _ => throw new ArgumentException("Incorrect pin type", nameof(type))
            };
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
