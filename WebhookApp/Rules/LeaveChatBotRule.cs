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
        private readonly ConfigService _configService;
        private readonly ILogger<LeaveChatBotRule> _logger;

        public LeaveChatBotRule(BotService botService, ConfigService configService, ILogger<LeaveChatBotRule> logger) {
            _botService = botService;
            _configService = configService;
            _logger = logger;
        }
        
        public async Task<bool> IsMatch(Update update) {
            BotConfig config = await _configService.LoadAsync();
            return update.Type == UpdateType.Message
                && update.Message.Type == MessageType.ChatMembersAdded
                && update.Message.NewChatMembers.Any(u => u.Username == config.Bot)
                && !config.Chats.Contains(update.Message.Chat.Id);
        }

        public async Task ProcessAsync(Update update) {
            _logger.LogInformation($"Processing leave chat bot message..., chatId: {update.Message.Chat.Id.ToString()}");

            await _botService.Client.LeaveChatAsync(
                chatId: update.Message.Chat.Id);
        }
    }
}
