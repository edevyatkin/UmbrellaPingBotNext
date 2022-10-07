using System;
using System.Collections.Generic;
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
    internal class AddLotteryPingCommandRule : IUpdateRule
    {
        private readonly BotService _botService;
        private readonly MessageRule _messageRule;
        private readonly ConfigService _configService;
        private readonly ILogger<AddLotteryPingCommandRule> _logger;
        private readonly ILotteryService _lotteryService;

        public AddLotteryPingCommandRule(BotService botService, MessageRule messageRule, ConfigService configService, ILogger<AddLotteryPingCommandRule> logger, ILotteryService lotteryService) {
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
                   && Regex.IsMatch(update.Message.Text, @"^\/addlotping[\n\s]+\@\w+([\n\s]+\@\w+)*$");
        }

        public async Task ProcessAsync(Update update) {
            _logger.LogInformation($"Processing /addlotping message..., chatId: {update.Message.Chat.Id.ToString()}");

            var inputUsernames = update.Message.Text
                .Split(new[] {'\n', ' '}, StringSplitOptions.RemoveEmptyEntries)
                .Where(s => s.StartsWith("@"))
                .Distinct()
                .Select(u => new Abstractions.User(u.Substring(1)))
                .ToList();
            var addedUsers = new List<Abstractions.User>();
            foreach (var user in inputUsernames)
                if (await _lotteryService.AddPingAsync(user, update.Message.Chat.Id))
                    addedUsers.Add(user);
            if (addedUsers.Count > 0) {
                await _botService.Client.SendTextMessageAsync(
                    chatId: update.Message.Chat.Id,
                    replyToMessageId: update.Message.MessageId,
                    text: $"Добавлены пинги на лотерею:\n{string.Join('\n', addedUsers.Select(u => $"👊{u.Username}"))}");
            }
            else {
                await _botService.Client.SendTextMessageAsync(
                    chatId: update.Message.Chat.Id,
                    replyToMessageId: update.Message.MessageId,
                    text: $"Эти бойцы уже пингуются на лотерею"); 
            }
        }
    }
}