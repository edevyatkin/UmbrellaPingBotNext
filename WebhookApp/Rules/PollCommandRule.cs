using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using WebhookApp.Services;
using WebhookApp.Services.Polls;

namespace WebhookApp.Rules
{
    internal class PollCommandRule : IUpdateRule
    {
        private readonly BotService _botService;
        private readonly MessageRule _messageRule;
        private readonly BotConfig _botConfig;
        private readonly ILogger<PollCommandRule> _logger;

        public PollCommandRule(BotService botService, MessageRule messageRule, BotConfig botConfig, ILogger<PollCommandRule> logger) {
            _botService = botService;
            _messageRule = messageRule;
            _botConfig = botConfig;
            _logger = logger;
        }
        
        public async Task<bool> IsMatch(Update update) {
            return await _messageRule.IsMatch(update) 
                   && (update.Message.Text.Equals("/poll") 
                       || update.Message.Text.Equals($"/poll@{_botConfig.Bot}"));
        }

        public async Task ProcessAsync(Update update) {
            _logger.LogInformation($"Processing /poll message..., chatId: {update.Message.Chat.Id.ToString()}");
            
            if (PollsHelper.HasPoll(update.Message.Chat.Id)) {
                var poll = PollsHelper.GetPoll(update.Message.Chat.Id);
                var text = $"⇧ <a href='https://t.me/c/{poll.ChatId.ToString().Substring(4)}/{poll.MessageId.ToString()}'>К голосованию</a> ⇧";
                await _botService.Client.SendMessage(
                    chatId: poll.ChatId,
                    text: text,
                    parseMode: ParseMode.Html);
            }
            else {
                await _botService.Client.SendMessage(
                    chatId: update.Message.Chat.Id,
                    text: "Голосования ещё нет");
            }
        }
    }
}
