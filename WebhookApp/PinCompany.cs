using System;
using System.Collections.Generic;
using System.Linq;

namespace WebhookApp
{
    internal class PinCompany
    {
        private static readonly HashSet<string> Companies;
        private readonly string _text;

        public string Logo => _text.Substring(0,2);

        static PinCompany() {
            Companies = new HashSet<string> {
                "📯Pied Piper", "🤖Hooli", "⚡️Stark Ind.", 
                "☂️Umbrella", "🎩Wayne Ent."
            };
        }

        public PinCompany(string company) {
            if (!Companies.Contains(company))
                throw new ArgumentException("Incorrect pin company", nameof(company));
            _text = company;
        }

        public override string ToString() {
            return _text;
        }

        public override bool Equals(object obj) {
            return obj is PinCompany company &&
                   _text == company._text;
        }

        public override int GetHashCode() {
            return HashCode.Combine(_text);
        }
    }
}
