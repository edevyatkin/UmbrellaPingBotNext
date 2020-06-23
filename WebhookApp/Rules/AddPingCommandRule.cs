using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using WebhookApp.Services;

namespace WebhookApp.Rules
{
    internal class AddPingCommandRule : IUpdateRule
    {
        private readonly BotService _botService;
        private readonly MessageRule _messageRule;
        private readonly ConfigService _configService;

        public AddPingCommandRule(BotService botService, MessageRule messageRule, ConfigService configService) {
            _botService = botService;
            _messageRule = messageRule;
            _configService = configService;
        }
        public async Task<bool> IsMatch(Update update) {
            BotConfig botConfig = await _configService.LoadAsync();
            return await _messageRule.IsMatch(update)
                   && botConfig.ChatAdmins.ContainsKey(update.Message.Chat.Id)
                   && botConfig.ChatAdmins[update.Message.Chat.Id].Contains($"@{update.Message.From.Username}")
                   && Regex.IsMatch(update.Message.Text, @"^\/addping[\n\s]+\@\w+([\n\s]+\@\w+)*$");
        }

        public async Task ProcessAsync(Update update) {
            Console.WriteLine($"Processing /addping message..., chatId: {update.Message.Chat.Id.ToString()}");
            
            BotConfig botConfig = await _configService.LoadAsync();
            var usernames = botConfig.Usernames;
            var inputUsernames = update.Message.Text
                .Split(new[] {'\n', ' '}, StringSplitOptions.RemoveEmptyEntries)
                .Where(s => s.StartsWith("@"))
                .Distinct()
                .ToList();
            if (!usernames.ContainsKey(update.Message.Chat.Id)) {
                usernames.Add(update.Message.Chat.Id, inputUsernames);
            }
            else {
                inputUsernames = inputUsernames.Except(usernames[update.Message.Chat.Id]).ToList();
                usernames[update.Message.Chat.Id].AddRange(inputUsernames);
            }

            if (inputUsernames.Count > 0) {
                await _configService.SaveAsync();
                await _botService.Client.SendTextMessageAsync(
                    chatId: update.Message.Chat.Id,
                    replyToMessageId: update.Message.MessageId,
                    text:
                    $"Добавлены пинги:\n{string.Join('\n', inputUsernames.Select(u => $"👊{u.Substring(1)}"))}");
            }
            else {
                await _botService.Client.SendTextMessageAsync(
                    chatId: update.Message.Chat.Id,
                    replyToMessageId: update.Message.MessageId,
                    text: $"Эти бойцы уже пингуются на битву"); 
            }
        }
    }
}