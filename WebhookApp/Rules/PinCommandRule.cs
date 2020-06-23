using System;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using WebhookApp.Services;

namespace WebhookApp.Rules
{
    internal class PinCommandRule : IUpdateRule
    {
        private readonly BotService _botService;
        private readonly MessageRule _messageRule;
        private readonly ConfigService _configService;

        public PinCommandRule(BotService botService, MessageRule messageRule, ConfigService configService) {
            _botService = botService;
            _messageRule = messageRule;
            _configService = configService;
        }
        
        public async Task<bool> IsMatch(Update update) {
            BotConfig config = await _configService.LoadAsync();
            return await _messageRule.IsMatch(update) 
                   && (update.Message.Text.Equals("/pin") 
                       || update.Message.Text.Equals($"/pin@{config.Bot}"));
        }

        public async Task ProcessAsync(Update update) {
            Console.WriteLine($"Processing /pin message..., chatId: {update.Message.Chat.Id.ToString()}");
            
            if (PollsHelper.HasPoll(update.Message.Chat.Id)) {
                var poll = PollsHelper.GetPoll(update.Message.Chat.Id);
                var pressPinText = $"{poll.Pin.Type}{poll.Pin.Company.Logo} Прожимаемся в 📌<a href='{poll.Pin.LinkToMessage}'>пин</a>";
                await _botService.Client.SendTextMessageAsync(
                    chatId: poll.ChatId,
                    text: pressPinText,
                    parseMode: ParseMode.Html);
            }
            else {
                await _botService.Client.SendTextMessageAsync(
                    chatId: update.Message.Chat.Id,
                    text: "Пина на битву ещё нет");
            }
        }
    }
}
