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
                && update.Message.Text.Equals($"/pin@{Constants.Bot}");
        }

        public async Task ProcessAsync(Update update) {
            Console.WriteLine("Processing /pin message...");
            var client = await ClientFactory.GetAsync();

            if (PollHelper.Exists()) {
                var pressPinText = $"{PollHelper.Pin.Type}{PollHelper.Pin.Company.Logo} Прожимаемся в 📌<a href='{PollHelper.Pin.LinkToMessage}'>пин</a>";
                await client.SendTextMessageAsync(
                    chatId: update.Message.Chat.Id,
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
