using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using WebhookApp.Services.Factory;

namespace WebhookApp.Rules
{
    internal class RemoveFactoryPingCommandRule : IUpdateRule
    {
        private readonly ITelegramBotClient _botClient;
        private readonly MessageRule _messageRule;
        private readonly BotConfig _botConfig;
        private readonly ILogger<RemoveFactoryPingCommandRule> _logger;
        private readonly IFactoryService _factoryService;

        public RemoveFactoryPingCommandRule(ITelegramBotClient botClient, MessageRule messageRule, BotConfig botConfig, ILogger<RemoveFactoryPingCommandRule> logger, IFactoryService factoryService) {
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
                   && Regex.IsMatch(update.Message.Text, @"^\/removefactping[\n\s]+\@\w+([\n\s]+\@\w+)*$");
        }

        public async Task ProcessAsync(Update update) {
            _logger.LogInformation($"Processing /removefactping message..., chatId: {update.Message.Chat.Id.ToString()}");
            
            var inputUsernames = update.Message.Text
                .Split(new[] {'\n', ' '}, StringSplitOptions.RemoveEmptyEntries)
                .Where(s => s.StartsWith("@"))
                .Distinct()
                .Select(u => new Abstractions.User((u.Substring(1))))
                .ToList();
            var removedUsers = new List<Abstractions.User>();
            foreach (var user in inputUsernames)
                if (await _factoryService.RemovePingAsync(user, update.Message.Chat.Id))
                    removedUsers.Add(user);
            if (removedUsers.Count > 0) {
                await _botClient.SendMessage(
                    chatId: update.Message.Chat.Id,
                    replyParameters: new ReplyParameters()
                    {
                        MessageId = update.Message.Id
                    },
                    text: $"Удалены пинги на фабрику:\n{string.Join('\n', removedUsers.Select(u => $"🗡{u.Username}"))}");
            }
            else {
                await _botClient.SendMessage(
                    chatId: update.Message.Chat.Id,
                    replyParameters: new ReplyParameters()
                    {
                        MessageId = update.Message.Id
                    },
                    text: $"Эти бойцы не пингуются на фабрику"); 
            }
        }
    }
}