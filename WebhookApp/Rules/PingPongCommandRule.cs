using System;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using WebhookApp.Services;

namespace WebhookApp.Rules
{
    internal class PingPongCommandRule : IUpdateRule
    {
        private readonly BotService _botService;
        private readonly MessageRule _messageRule;
        private readonly ConfigService _configService;

        public PingPongCommandRule(BotService botService, MessageRule messageRule, ConfigService configService) {
            _botService = botService;
            _messageRule = messageRule;
            _configService = configService;
        }
        
        public async Task<bool> IsMatch(Update update) {
            BotConfig config = await _configService.LoadAsync();
            return await _messageRule.IsMatch(update)
                   && (update.Message.Text.Equals("/ping") 
                       || update.Message.Text.Equals($"/ping@{config.Bot}"));
        }

        public async Task ProcessAsync(Update update) {
            Console.WriteLine($"Processing /ping message..., chatId: {update.Message.Chat.Id.ToString()}");

            await _botService.Client.SendTextMessageAsync(
                  chatId: update.Message.Chat.Id,
                  text: "Pong!");
        }
    }
}
