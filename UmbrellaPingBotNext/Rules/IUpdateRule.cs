using Telegram.Bot.Types;

namespace UmbrellaPingBotNext.Rules
{
    internal interface IUpdateRule
    {
        bool IsMatch(Update update);
        void Process(Update update);
    }
}