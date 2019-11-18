using System;
using System.Collections.Generic;

namespace UmbrellaPingBotNext
{
    public class PinCompany
    {
        private static readonly Dictionary<string, string> _companies;
        private string _text;

        public string Logo => _text.Substring(0,2);

        public static string Piper => _companies["piper"];
        public static string Hooli => _companies["hooli"];
        public static string Stark => _companies["stark"];
        public static string Umbrella => _companies["umbrl"];
        public static string Wayne => _companies["wayne"];

        static PinCompany() {
            _companies = new Dictionary<string, string> {
                ["piper"] = "📯Pied Piper",
                ["hooli"] = "🤖Hooli",
                ["stark"] = "⚡️Stark Ind.",
                ["umbrl"] = "☂️Umbrella",
                ["wayne"] = "🎩Wayne Ent."
            };
        }

        public PinCompany(string company) {
            bool isCorrect = false;
            foreach (string c in _companies.Values) {
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
