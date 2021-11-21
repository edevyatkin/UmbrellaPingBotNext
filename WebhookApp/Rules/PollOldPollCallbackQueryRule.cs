using System;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using WebhookApp.Services;
using WebhookApp.Services.Polls;

namespace WebhookApp.Rules
{
    internal class PollOldPollCallbackQueryRule : IUpdateRule
    {
        private readonly BotService _botService;
        private readonly ILogger<PollOldPollCallbackQueryRule> _logger;

        public PollOldPollCallbackQueryRule(BotService botService, ILogger<PollOldPollCallbackQueryRule> logger) {
            _botService = botService;
            _logger = logger;
        }
        
        public Task<bool> IsMatch(Update update) {
            if (update.Type != UpdateType.CallbackQuery)
                return Task.FromResult(false);
            
            Message message = update.CallbackQuery.Message;
            return Task.FromResult(!PollsHelper.HasPoll(message.Chat.Id) ||
                   PollsHelper.GetPoll(message.Chat.Id).MessageId != message.MessageId);
        }

        public async Task ProcessAsync(Update update) {
            _logger.LogInformation($"[ {DateTime.Now.ToLocalTime().ToString(CultureInfo.InvariantCulture)} ] Processing old pressing callback... {update.CallbackQuery.From.Username}, chatId: {update.CallbackQuery.Message.Chat.Id.ToString()}");

            await _botService.Client.AnswerCallbackQueryAsync(
                callbackQueryId: update.CallbackQuery.Id,
                text: "Голосование устарело");
        }
    }
}
