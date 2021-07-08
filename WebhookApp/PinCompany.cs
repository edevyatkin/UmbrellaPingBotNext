using System;
using System.Collections.Generic;
using System.Linq;

namespace WebhookApp
{
    internal class PinCompany
    {
        private static readonly Dictionary<string, string> Companies;
        private readonly string _text;

        public string Logo => _text.Substring(0,2);

        public static string Piper => Companies["piper"];
        public static string Hooli => Companies["hooli"];
        public static string Stark => Companies["stark"];
        public static string Umbrella => Companies["umbrl"];
        public static string Wayne => Companies["wayne"];

        static PinCompany() {
            Companies = new Dictionary<string, string> {
                ["piper"] = "📯Pied Piper",
                ["hooli"] = "🤖Hooli",
                ["stark"] = "⚡️Stark Ind.",
                ["umbrl"] = "☂️Umbrella",
                ["wayne"] = "🎩Wayne Ent."
            };
        }

        public PinCompany(string company) {
            if (!Companies.Values.Contains(company))
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
