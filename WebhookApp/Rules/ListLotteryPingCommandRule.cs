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
        private readonly BotConfig _botConfig;
        private readonly ILogger<ListLotteryPingCommandRule> _logger;
        private readonly ILotteryService _lotteryService;

        public ListLotteryPingCommandRule(BotService botService, MessageRule messageRule, BotConfig botConfig, ILogger<ListLotteryPingCommandRule> logger, ILotteryService lotteryService) {
            _botService = botService;
            _messageRule = messageRule;
            _botConfig = botConfig;
            _logger = logger;
            _lotteryService = lotteryService;
        }
        public async Task<bool> IsMatch(Update update) {
            return await _messageRule.IsMatch(update)
                   && _botConfig.ChatAdmins.ContainsKey(update.Message.Chat.Id)
                   && _botConfig.ChatAdmins[update.Message.Chat.Id].Contains($"@{update.Message.From.Username}")
                   && Regex.IsMatch(update.Message.Text, @"^\/listlotping$");
        }

        public async Task ProcessAsync(Update update) {
            _logger.LogInformation($"Processing /listlotping message..., chatId: {update.Message.Chat.Id.ToString()}");

            var users = await _lotteryService.GetUsersToPingAsync(update.Message.Chat.Id);
            if (users.Count > 0) {
                await _botService.Client.SendMessage(
                    chatId: update.Message.Chat.Id,
                    replyParameters: new ReplyParameters()
                    {
                        MessageId = update.Message.Id
                    },
                    text: $"Список пингов на лотерею:\n{string.Join('\n', users.OrderBy(u => u.Username).Select(u => $"👊{u.Username}"))}");
            }
            else {
                await _botService.Client.SendMessage(
                    chatId: update.Message.Chat.Id,
                    replyParameters: new ReplyParameters()
                    {
                        MessageId = update.Message.Id
                    },
                    text: $"На лотерею никто не пингуется"); 
            }
        }
    }
}