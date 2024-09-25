using System.Threading.Tasks;
using Telegram.Bot.Types;
using WebhookApp.Services;

namespace WebhookApp.Rules
{
    internal class ReplyToInfoBotMessageRule : IUpdateRule
    {
        private readonly MessageRule _messageRule;
        private readonly BotConfig _botConfig;

        public ReplyToInfoBotMessageRule(MessageRule messageRule, BotConfig botConfig) {
            _messageRule = messageRule;
            _botConfig = botConfig;
        }
        
        public async Task<bool> IsMatch(Update update) {
            return await _messageRule.IsMatch(update)
                && update.Message.ReplyToMessage?.From.Username == _botConfig.SwInfoBot;
        }

        public Task ProcessAsync(Update update) => Task.CompletedTask;
    }
}
