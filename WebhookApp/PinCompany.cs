using System;
using System.Collections.Generic;

namespace WebhookApp
{
    internal class PinCompany
    {
        private static readonly Dictionary<string, string> Companies;
        private string _text;

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
            bool isCorrect = false;
            foreach (string c in Companies.Values) {
                if (string.Equals(c, company)) {
                    isCorrect = true;
                    _text = company;
                    break;
                }
            }
            if (!isCorrect) {
                throw new ArgumentException("Incorrect pin company", nameof(company));
            }
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
