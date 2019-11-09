using System.Text.RegularExpressions;
using Telegram.Bot.Types;

namespace UmbrellaPingBotNext
{
    class PinHelper
    {
        private const string pattern = "(.*)(В атаку на |Все в защиту - )(.*)";

        public static Pin Parse(Message pinMessage) {
            Match match = Regex.Match(pinMessage.Text, pattern);
            return new Pin {
                MessageId = pinMessage.MessageId,
                ChatId = pinMessage.Chat.Id,
                MessageDate = pinMessage.ForwardDate ?? pinMessage.Date,
                Type = new PinType(match.Groups[1].Value),
                Company = new PinCompany(match.Groups[3].Value)
            };
        }

        internal static bool IsPin(Message message) {
            return Regex.IsMatch(message.Text, pattern);
        }
    }
}
