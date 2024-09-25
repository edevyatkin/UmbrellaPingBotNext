using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using WebhookApp.Services;

namespace WebhookApp.Rules
{
    internal class LeaveChatBotRule : IUpdateRule
    {
        private readonly BotService _botService;
        private readonly BotConfig _botConfig;
        private readonly ILogger<LeaveChatBotRule> _logger;

        public LeaveChatBotRule(BotService botService, BotConfig botConfig, ILogger<LeaveChatBotRule> logger) {
            _botService = botService;
            _botConfig = botConfig;
            _logger = logger;
        }
        
        public async Task<bool> IsMatch(Update update) {
            return update.Type == UpdateType.Message
                && update.Message.Type == MessageType.ChatMembersAdded
                && update.Message.NewChatMembers.Any(u => u.Username == _botConfig.Bot)
                && !_botConfig.Chats.Contains(update.Message.Chat.Id);
        }

        public async Task ProcessAsync(Update update) {
            _logger.LogInformation($"Processing leave chat bot message..., chatId: {update.Message.Chat.Id.ToString()}");

            await _botService.Client.LeaveChatAsync(
                chatId: update.Message.Chat.Id);
        }
    }
}
