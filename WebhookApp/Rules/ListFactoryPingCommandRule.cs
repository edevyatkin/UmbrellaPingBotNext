using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using WebhookApp.Services.Factory;

namespace WebhookApp.Rules
{
    internal class ListFactoryPingCommandRule : IUpdateRule
    {
        private readonly ITelegramBotClient _botClient;
        private readonly MessageRule _messageRule;
        private readonly BotConfig _botConfig;
        private readonly ILogger<ListFactoryPingCommandRule> _logger;
        private readonly IFactoryService _factoryService;

        public ListFactoryPingCommandRule(ITelegramBotClient botClient, MessageRule messageRule, BotConfig botConfig, ILogger<ListFactoryPingCommandRule> logger, IFactoryService factoryService) {
            _botClient = botClient;
            _messageRule = messageRule;
            _botConfig = botConfig;
            _logger = logger;
            _factoryService = factoryService;
        }
        public async Task<bool> IsMatch(Update update) {
            return await _messageRule.IsMatch(update)
                   && _botConfig.ChatAdmins.ContainsKey(update.Message.Chat.Id)
                   && _botConfig.ChatAdmins[update.Message.Chat.Id].Contains($"@{update.Message.From.Username}")
                   && Regex.IsMatch(update.Message.Text, @"^\/listfactping$");
        }

        public async Task ProcessAsync(Update update) {
            _logger.LogInformation($"Processing /listfactping message..., chatId: {update.Message.Chat.Id.ToString()}");

            var users = await _factoryService.GetUsersToPingAsync(update.Message.Chat.Id);
            if (users.Count > 0) {
                await _botClient.SendMessage(
                    chatId: update.Message.Chat.Id,
                    replyParameters: new ReplyParameters()
                    {
                        MessageId = update.Message.Id
                    },
                    text: $"Список пингов на фабрику:\n{string.Join('\n', users.OrderBy(u => u.Username).Select(u => $"👊{u.Username}"))}");
            }
            else {
                await _botClient.SendMessage(
                    chatId: update.Message.Chat.Id,
                    replyParameters: new ReplyParameters()
                    {
                        MessageId = update.Message.Id
                    },
                    text: $"На фабрику никто не пингуется"); 
            }
        }
    }
}