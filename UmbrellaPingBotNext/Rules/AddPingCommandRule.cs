using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace UmbrellaPingBotNext.Rules
{
    internal class AddPingCommandRule : IUpdateRule
    {
        public bool IsMatch(Update update) {
            var botConfig = ConfigHelper.Get();
            return UpdateProcessor.GetRule<MessageRule>().IsMatch(update)
                   && botConfig.ChatAdmins.ContainsKey(update.Message.Chat.Id)
                   && botConfig.ChatAdmins[update.Message.Chat.Id].Contains($"@{update.Message.From.Username}")
                   && Regex.IsMatch(update.Message.Text, @"^\/addping[\n\s]+\@\w+([\n\s]+\@\w+)*$");
        }

        public async Task ProcessAsync(Update update) {
            Console.WriteLine($"Processing /addping message..., chatId: {update.Message.Chat.Id.ToString()}");
            
            var client = await ClientFactory.GetAsync();
            var usernames = ConfigHelper.Get().Usernames;
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
                await ConfigHelper.SaveAsync();
                await client.SendTextMessageAsync(
                    chatId: update.Message.Chat.Id,
                    replyToMessageId: update.Message.MessageId,
                    text:
                    $"Добавлены пинги:\n{string.Join('\n', inputUsernames.Select(u => $"👊{u.Substring(1)}"))}");
            }
            else {
                await client.SendTextMessageAsync(
                    chatId: update.Message.Chat.Id,
                    replyToMessageId: update.Message.MessageId,
                    text: $"Эти бойцы уже пингуются на битву"); 
            }
        }
    }
}