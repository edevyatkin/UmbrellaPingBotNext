using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using WebhookApp.Services;
using WebhookApp.Services.Polls;

namespace WebhookApp.Rules
{
    internal class PinCommandRule : IUpdateRule
    {
        private readonly BotService _botService;
        private readonly MessageRule _messageRule;
        private readonly BotConfig _botConfig;
        private readonly ILogger<PinCommandRule> _logger;

        public PinCommandRule(BotService botService, MessageRule messageRule, BotConfig botConfig, ILogger<PinCommandRule> logger) {
            _botService = botService;
            _messageRule = messageRule;
            _botConfig = botConfig;
            _logger = logger;
        }
        
        public async Task<bool> IsMatch(Update update) {
            return await _messageRule.IsMatch(update) 
                   && (update.Message.Text.Equals("/pin") 
                       || update.Message.Text.Equals($"/pin@{_botConfig.Bot}"));
        }

        public async Task ProcessAsync(Update update) {
            _logger.LogInformation($"Processing /pin message..., chatId: {update.Message.Chat.Id.ToString()}");
            
            if (PollsHelper.HasPoll(update.Message.Chat.Id)) {
                var poll = PollsHelper.GetPoll(update.Message.Chat.Id);
                var pressPinText = $"{poll.Pin.Type}{poll.Pin.Company.Logo} Прожимаемся в 📌<a href='{poll.Pin.LinkToMessage}'>пин</a>";
                await _botService.Client.SendMessage(
                    chatId: poll.ChatId,
                    text: pressPinText,
                    parseMode: ParseMode.Html);
            }
            else {
                await _botService.Client.SendMessage(
                    chatId: update.Message.Chat.Id,
                    text: "Пина на битву ещё нет");
            }
        }
    }
}
