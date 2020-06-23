using System.Threading.Tasks;
using Telegram.Bot.Types;
using WebhookApp.Services;

namespace WebhookApp.Rules
{
    internal class ReplyToBotMessageRule : IUpdateRule
    {
        private readonly MessageRule _messageRule;
        private readonly ConfigService _configService;

        public ReplyToBotMessageRule(MessageRule messageRule, ConfigService configService) {
            _messageRule = messageRule;
            _configService = configService;
        }
        
        public async Task<bool> IsMatch(Update update) {
            BotConfig config = await _configService.LoadAsync();
            return await _messageRule.IsMatch(update)
                && update.Message.ReplyToMessage?.From.Username == config.Bot;
        }

        public Task ProcessAsync(Update update) => Task.CompletedTask;
    }
}
