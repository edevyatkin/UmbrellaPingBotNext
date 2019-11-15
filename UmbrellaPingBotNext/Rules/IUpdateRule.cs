using Telegram.Bot.Types;
using System.Threading.Tasks;

namespace UmbrellaPingBotNext.Rules
{
    internal interface IUpdateRule
    {
        bool IsMatch(Update update);
        Task ProcessAsync(Update update);
    }
}