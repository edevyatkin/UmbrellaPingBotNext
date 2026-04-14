using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace WebhookApp.Rules
{
    internal class PingPongCommandRule : IUpdateRule
    {
        private readonly MessageRule _messageRule;
        private readonly BotConfig _botConfig;
        private readonly ILogger<PingPongCommandRule> _logger;
        private readonly ITelegramBotClient _botClient;

        public PingPongCommandRule(ITelegramBotClient botClient, MessageRule messageRule, BotConfig botConfig, ILogger<PingPongCommandRule> logger) {
            _botClient = botClient;
            _messageRule = messageRule;
            _botConfig = botConfig;
            _logger = logger;
        }
        
        public async Task<bool> IsMatch(Update update) {
            return await _messageRule.IsMatch(update)
                   && (update.Message.Text.Equals("/ping") 
                       || update.Message.Text.Equals($"/ping@{_botConfig.Bot}"));
        }

        public async Task ProcessAsync(Update update) {
            _logger.LogInformation($"Processing /ping message..., chatId: {update.Message.Chat.Id.ToString()}");
            
            await _botClient.SendMessage(
                chatId: update.Message.Chat.Id,
                text: "Pong!");
        }
    }
}
