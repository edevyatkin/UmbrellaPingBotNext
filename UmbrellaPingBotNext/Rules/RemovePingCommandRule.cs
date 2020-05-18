using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace UmbrellaPingBotNext.Rules
{
    internal class RemovePingCommandRule : IUpdateRule
    {
        public bool IsMatch(Update update) {
            var botConfig = ConfigHelper.Get();
            return UpdateProcessor.GetRule<MessageRule>().IsMatch(update)
                   && botConfig.ChatAdmins.ContainsKey(update.Message.Chat.Id)
                   && botConfig.ChatAdmins[update.Message.Chat.Id].Contains($"@{update.Message.From.Username}")
                   && Regex.IsMatch(update.Message.Text, @"^\/removeping[\n\s]+\@\w+([\n\s]+\@\w+)*$");
        }

        public async Task ProcessAsync(Update update) {
            Console.WriteLine($"Processing /removeping message..., chatId: {update.Message.Chat.Id.ToString()}");
            
            var client = await ClientFactory.GetAsync();
            var usernames = ConfigHelper.Get().Usernames;
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
                    await ConfigHelper.SaveAsync();
                    await client.SendTextMessageAsync(
                        chatId: update.Message.Chat.Id,
                        replyToMessageId: update.Message.MessageId,
                        text: $"Удалены пинги:\n{string.Join('\n', inputUsernames.Select(u => $"🗡{u.Substring(1)}"))}");
                }
                else {
                    await client.SendTextMessageAsync(
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