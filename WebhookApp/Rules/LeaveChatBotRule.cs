using System;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using WebhookApp.Services;

namespace WebhookApp.Rules
{
    internal class LeaveChatBotRule : IUpdateRule
    {
        private readonly BotService _botService;
        private readonly ConfigService _configService;

        public LeaveChatBotRule(BotService botService, ConfigService configService) {
            _botService = botService;
            _configService = configService;
        }
        
        public async Task<bool> IsMatch(Update update) {
            BotConfig config = await _configService.LoadAsync();
            return update.Type == UpdateType.Message
                && update.Message.Type == MessageType.ChatMembersAdded
                && update.Message.NewChatMembers.Any(u => u.Username == config.Bot)
                && !config.Chats.Contains(update.Message.Chat.Id);
        }

        public async Task ProcessAsync(Update update) {
            Console.WriteLine($"Processing leave chat bot message..., chatId: {update.Message.Chat.Id.ToString()}");

            await _botService.Client.LeaveChatAsync(
                chatId: update.Message.Chat.Id);
        }
    }
}
