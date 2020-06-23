using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using WebhookApp.Services;

namespace WebhookApp.Rules
{
    internal class RemovePingCommandRule : IUpdateRule
    {
        private readonly BotService _botService;
        private readonly MessageRule _messageRule;
        private readonly ConfigService _configService;

        public RemovePingCommandRule(BotService botService, MessageRule messageRule, ConfigService configService) {
            _botService = botService;
            _messageRule = messageRule;
            _configService = configService;
        }
        
        public async Task<bool> IsMatch(Update update) {
            BotConfig config = await _configService.LoadAsync();
            return await _messageRule.IsMatch(update)
                   && config.ChatAdmins.ContainsKey(update.Message.Chat.Id)
                   && config.ChatAdmins[update.Message.Chat.Id].Contains($"@{update.Message.From.Username}")
                   && Regex.IsMatch(update.Message.Text, @"^\/removeping[\n\s]+\@\w+([\n\s]+\@\w+)*$");
        }

        public async Task ProcessAsync(Update update) {
            Console.WriteLine($"Processing /removeping message..., chatId: {update.Message.Chat.Id.ToString()}");
            
            BotConfig config = await _configService.LoadAsync();
            var usernames = config.Usernames;
            var inputUsernames = update.Message.Text
                .Split(new[] {'\n', ' '}, StringSplitOptions.RemoveEmptyEntries)
                .Where(s => s.StartsWith("@"))
                .Distinct()
                .ToList();
            if (usernames.ContainsKey(update.Message.Chat.Id)) {
                inputUsernames = inputUsernames.Intersect(usernames[update.Message.Chat.Id]).ToList();
                foreach (var username in inputUsernames) {
                    usernames[update.Message.Chat.Id].Remove(username);
                }
                if (inputUsernames.Count > 0) {
                    await _configService.SaveAsync();
                    await _botService.Client.SendTextMessageAsync(
                        chatId: update.Message.Chat.Id,
                        replyToMessageId: update.Message.MessageId,
                        text: $"Удалены пинги:\n{string.Join('\n', inputUsernames.Select(u => $"🗡{u.Substring(1)}"))}");
                }
                else {
                    await _botService.Client.SendTextMessageAsync(
                        chatId: update.Message.Chat.Id,
                        replyToMessageId: update.Message.MessageId,
                        text: $"Эти бойцы не пингуются на битву"); 
                }

                if (usernames[update.Message.Chat.Id].Count == 0)
                    usernames.Remove(update.Message.Chat.Id);
            }
        }
    }
}