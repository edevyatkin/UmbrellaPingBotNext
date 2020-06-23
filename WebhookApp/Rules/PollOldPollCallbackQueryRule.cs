using System;
using System.Globalization;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using WebhookApp.Services;

namespace WebhookApp.Rules
{
    internal class PollOldPollCallbackQueryRule : IUpdateRule
    {
        private readonly BotService _botService;

        public PollOldPollCallbackQueryRule(BotService botService) {
            _botService = botService;
        }
        
        public Task<bool> IsMatch(Update update) {
            if (update.Type != UpdateType.CallbackQuery)
                return Task.FromResult(false);
            
            Message message = update.CallbackQuery.Message;
            return Task.FromResult(!PollsHelper.HasPoll(message.Chat.Id) ||
                   PollsHelper.GetPoll(message.Chat.Id).MessageId != message.MessageId);
        }

        public async Task ProcessAsync(Update update) {
            Console.WriteLine($"[ {DateTime.Now.ToLocalTime().ToString(CultureInfo.InvariantCulture)} ] Processing old pressing callback... {update.CallbackQuery.From.Username}, chatId: {update.CallbackQuery.Message.Chat.Id.ToString()}");

            await _botService.Client.AnswerCallbackQueryAsync(
                callbackQueryId: update.CallbackQuery.Id,
                text: "Голосование устарело");
        }
    }
}
