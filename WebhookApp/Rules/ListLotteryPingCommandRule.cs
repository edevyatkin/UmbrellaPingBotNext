using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using WebhookApp.Services;
using WebhookApp.Services.Lottery;

namespace WebhookApp.Rules
{
    internal class ListLotteryPingCommandRule : IUpdateRule
    {
        private readonly BotService _botService;
        private readonly MessageRule _messageRule;
        private readonly ConfigService _configService;
        private readonly ILogger<ListLotteryPingCommandRule> _logger;
        private readonly ILotteryService _lotteryService;

        public ListLotteryPingCommandRule(BotService botService, MessageRule messageRule, ConfigService configService, ILogger<ListLotteryPingCommandRule> logger, ILotteryService lotteryService) {
            _botService = botService;
            _messageRule = messageRule;
            _configService = configService;
            _logger = logger;
            _lotteryService = lotteryService;
        }
        public async Task<bool> IsMatch(Update update) {
            BotConfig botConfig = await _configService.LoadAsync();
            return await _messageRule.IsMatch(update)
                   && botConfig.ChatAdmins.ContainsKey(update.Message.Chat.Id)
                   && botConfig.ChatAdmins[update.Message.Chat.Id].Contains($"@{update.Message.From.Username}")
                   && Regex.IsMatch(update.Message.Text, @"^\/listlotping$");
        }

        public async Task ProcessAsync(Update update) {
            _logger.LogInformation($"Processing /listlotping message..., chatId: {update.Message.Chat.Id.ToString()}");

            var users = await _lotteryService.GetUsersToPingAsync(update.Message.Chat.Id);
            if (users.Count > 0) {
                await _botService.Client.SendTextMessageAsync(
                    chatId: update.Message.Chat.Id,
                    replyToMessageId: update.Message.MessageId,
                    text: $"Список пингов на лотерею:\n{string.Join('\n', users.OrderBy(u => u.Username).Select(u => $"👊{u.Username}"))}");
            }
            else {
                await _botService.Client.SendTextMessageAsync(
                    chatId: update.Message.Chat.Id,
                    replyToMessageId: update.Message.MessageId,
                    text: $"На лотерею никто не пингуется"); 
            }
        }
    }
}