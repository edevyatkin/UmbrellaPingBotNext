using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Telegram.Bot.Types;

namespace UmbrellaPingBotNext
{
    class PinHelper
    {
        private const string pattern = "(.)(В атаку на |Все в защиту - )(.*)";

        public static Pin Parse(Message pinMessage) {
            Match match = Regex.Match(pinMessage.Text, pattern);
            return new Pin {
                MessageId = pinMessage.MessageId,
                ChatId = pinMessage.Chat.Id,
                Type = new PinType(match.Groups[1].Value),
                Company = new PinCompany(match.Groups[3].Value)
            };
        }

        internal static bool IsPin(Message message) {
            return Regex.IsMatch(message.Text, pattern);
        }
    }

    public class Pin
    {
        public int MessageId { get; set; }
        public long ChatId { get; set; }
        public PinType Type { get; set; }
        public PinCompany Company { get; set; }
    }

    public class PinCompany
    {
        private static readonly Dictionary<string, string> _companies;
        private string _text;

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
                throw new ArgumentException("Incorrect pin company", "company");
            }
        }

        public override string ToString() {
            return _text;
        }
    }

    public class PinType
    {
        private readonly string _text;
        public static string Defence => "🛡";
        public static string Attack => "⚔️";

        public PinType(string type) {
            if (type == Defence) {
                _text = Defence;
            }
            else if (type == Attack) {
                _text = Attack;
            }
            else
                throw new ArgumentException("Incorrect pin type", "type");
        }

        public override string ToString() {
            return _text;
        }
    }
}
