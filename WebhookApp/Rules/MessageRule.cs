using System;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using WebhookApp.Services;

namespace WebhookApp.Rules
{
    internal class MessageRule : IUpdateRule
    {
        private readonly ConfigService _configService;

        public MessageRule(ConfigService configService) {
            _configService = configService;
        }
        
        public async Task<bool> IsMatch(Update update) {
            BotConfig config = await _configService.LoadAsync();
            return update.Type == UpdateType.Message
                && update.Message.Type == MessageType.Text
                && config.Chats.Contains(update.Message.Chat.Id);
        }

        public Task ProcessAsync(Update update) => Task.CompletedTask;
    }
}