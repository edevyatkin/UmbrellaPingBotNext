using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace UmbrellaPingBotNext.Rules
{
    internal class PinCommandRule : IUpdateRule
    {
        public bool IsMatch(Update update) {
            return UpdateProcessor.GetRule<MessageRule>().IsMatch(update) 
                   && (update.Message.Text.Equals("/pin") 
                       || update.Message.Text.Equals($"/pin@{ConfigHelper.Get().Bot}"));
        }

        public async Task ProcessAsync(Update update) {
            Console.WriteLine($"Processing /pin message..., chatId: {update.Message.Chat.Id.ToString()}");
            
            var client = await ClientFactory.GetAsync();
            
            if (PollsHelper.HasPoll(update.Message.Chat.Id)) {
                var poll = PollsHelper.GetPoll(update.Message.Chat.Id);
                var pressPinText = $"{poll.Pin.Type}{poll.Pin.Company.Logo} Прожимаемся в 📌<a href='{poll.Pin.LinkToMessage}'>пин</a>";
                await client.SendTextMessageAsync(
                    chatId: poll.ChatId,
                    text: pressPinText,
                    parseMode: ParseMode.Html);
            }
            else {
                await client.SendTextMessageAsync(
                    chatId: update.Message.Chat.Id,
                    text: "Пина на битву ещё нет");
            }
        }
    }
}
