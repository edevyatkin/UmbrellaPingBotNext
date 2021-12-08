using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using WebhookApp.Services;

namespace WebhookApp.Rules
{
    internal class ListPingCommandRule : IUpdateRule
    {
        private readonly BotService _botService;
        private readonly MessageRule _messageRule;
        private readonly ConfigService _configService;
        private readonly ILogger<AddPingCommandRule> _logger;

        public ListPingCommandRule(BotService botService, MessageRule messageRule, ConfigService configService, ILogger<AddPingCommandRule> logger) {
            _botService = botService;
            _messageRule = messageRule;
            _configService = configService;
            _logger = logger;
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
            
            BotConfig botConfig = await _configService.LoadAsync();
            var usernames = botConfig.Usernames;
            if (usernames.ContainsKey(update.Message.Chat.Id) && usernames[update.Message.Chat.Id].Count > 0) {
                var names = usernames[update.Message.Chat.Id];
                await _botService.Client.SendTextMessageAsync(
                    chatId: update.Message.Chat.Id,
                    replyToMessageId: update.Message.MessageId,
                    text:
                    $"Список пингов:\n{string.Join('\n', names.Select(u => $"👊{u.Substring(1)}").OrderBy(x => x))}");
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