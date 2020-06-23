using System.Threading.Tasks;
using Telegram.Bot.Types;
using WebhookApp.Services;

namespace WebhookApp.Rules
{
    internal class ReplyToInfoBotMessageRule : IUpdateRule
    {
        private readonly MessageRule _messageRule;
        private readonly ConfigService _configService;

        public ReplyToInfoBotMessageRule(MessageRule messageRule, ConfigService configService) {
            _messageRule = messageRule;
            _configService = configService;
        }
        
        public async Task<bool> IsMatch(Update update) {
            BotConfig config = await _configService.LoadAsync();
            return await _messageRule.IsMatch(update)
                && update.Message.ReplyToMessage?.From.Username == config.SwInfoBot;
        }

        public Task ProcessAsync(Update update) => Task.CompletedTask;
    }
}
