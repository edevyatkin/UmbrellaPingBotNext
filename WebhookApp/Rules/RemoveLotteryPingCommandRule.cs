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
    internal class RemoveLotteryPingCommandRule : IUpdateRule
    {
        private readonly BotService _botService;
        private readonly MessageRule _messageRule;
        private readonly BotConfig _botConfig;
        private readonly ILogger<RemoveLotteryPingCommandRule> _logger;
        private readonly ILotteryService _lotteryService;

        public RemoveLotteryPingCommandRule(BotService botService, MessageRule messageRule, BotConfig botConfig, ILogger<RemoveLotteryPingCommandRule> logger, ILotteryService lotteryService) {
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
                   && Regex.IsMatch(update.Message.Text, @"^\/removelotping[\n\s]+\@\w+([\n\s]+\@\w+)*$");
        }

        public async Task ProcessAsync(Update update) {
            _logger.LogInformation($"Processing /removelotping message..., chatId: {update.Message.Chat.Id.ToString()}");
            
            var inputUsernames = update.Message.Text
                .Split(new[] {'\n', ' '}, StringSplitOptions.RemoveEmptyEntries)
                .Where(s => s.StartsWith("@"))
                .Distinct()
                .Select(u => new Abstractions.User((u.Substring(1))))
                .ToList();
            var removedUsers = new List<Abstractions.User>();
            foreach (var user in inputUsernames)
                if (await _lotteryService.RemovePingAsync(user, update.Message.Chat.Id))
                    removedUsers.Add(user);
            if (removedUsers.Count > 0) {
                await _botService.Client.SendTextMessageAsync(
                    chatId: update.Message.Chat.Id,
                    replyParameters: new ReplyParameters()
                    {
                        MessageId = update.Message.MessageId
                    },
                    text: $"Удалены пинги на лотерею:\n{string.Join('\n', removedUsers.Select(u => $"🗡{u.Username}"))}");
            }
            else {
                await _botService.Client.SendTextMessageAsync(
                    chatId: update.Message.Chat.Id,
                    replyParameters: new ReplyParameters()
                    {
                        MessageId = update.Message.MessageId
                    },
                    text: $"Эти бойцы не пингуются на лотерею"); 
            }
        }
    }
}