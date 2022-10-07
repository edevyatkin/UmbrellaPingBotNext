using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using WebhookApp.Services;
using WebhookApp.Services.Battle;

namespace WebhookApp.Rules
{
    internal class ListPingCommandRule : IUpdateRule
    {
        private readonly BotService _botService;
        private readonly MessageRule _messageRule;
        private readonly ConfigService _configService;
        private readonly ILogger<ListPingCommandRule> _logger;
        private readonly IBattleService _battleService;

        public ListPingCommandRule(BotService botService, MessageRule messageRule, ConfigService configService, ILogger<ListPingCommandRule> logger, IBattleService battleService) {
            _botService = botService;
            _messageRule = messageRule;
            _configService = configService;
            _logger = logger;
            _battleService = battleService;
        }
        public async Task<bool> IsMatch(Update update) {
            BotConfig botConfig = await _configService.LoadAsync();
            return await _messageRule.IsMatch(update)
                   && botConfig.ChatAdmins.ContainsKey(update.Message.Chat.Id)
                   && botConfig.ChatAdmins[update.Message.Chat.Id].Contains($"@{update.Message.From.Username}")
                   && Regex.IsMatch(update.Message.Text, @"^\/listping$");
        }

        public async Task ProcessAsync(Update update) {
            _logger.LogInformation($"Processing /listping message..., chatId: {update.Message.Chat.Id.ToString()}");

            var users = await _battleService.GetUsersToPingAsync(update.Message.Chat.Id);
            if (users.Count > 0) {
                await _botService.Client.SendTextMessageAsync(
                    chatId: update.Message.Chat.Id,
                    replyToMessageId: update.Message.MessageId,
                    text: $"Список пингов:\n{string.Join('\n', users.OrderBy(u => u.Username).Select(u => $"👊{u.Username}"))}");
            }
            else {
                await _botService.Client.SendTextMessageAsync(
                    chatId: update.Message.Chat.Id,
                    replyToMessageId: update.Message.MessageId,
                    text: $"На битву никто не пингуется"); 
            }
        }
    }
}