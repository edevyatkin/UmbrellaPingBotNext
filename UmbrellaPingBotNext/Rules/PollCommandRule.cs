using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace UmbrellaPingBotNext.Rules
{
    internal class PollCommandRule : IUpdateRule
    {
        public bool IsMatch(Update update) {
            return UpdateProcessor.GetRule<MessageRule>().IsMatch(update) 
                   && (update.Message.Text.Equals("/poll") 
                       || update.Message.Text.Equals($"/poll@{ConfigHelper.Get().Bot}"));
        }

        public async Task ProcessAsync(Update update) {
            Console.WriteLine($"Processing /poll message..., chatId: {update.Message.Chat.Id.ToString()}");
            
            var client = await ClientFactory.GetAsync();
            
            if (PollsHelper.HasPoll(update.Message.Chat.Id)) {
                var poll = PollsHelper.GetPoll(update.Message.Chat.Id);
                var text = $"⇧ <a href='https://t.me/c/{poll.ChatId.ToString().Substring(4)}/{poll.MessageId.ToString()}'>К голосованию</a> ⇧";
                await client.SendTextMessageAsync(
                    chatId: poll.ChatId,
                    text: text,
                    parseMode: ParseMode.Html);
            }
            else {
                await client.SendTextMessageAsync(
                    chatId: update.Message.Chat.Id,
                    text: "Голосования ещё нет");
            }
        }
    }
}
