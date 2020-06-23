using System;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using WebhookApp.Services;

namespace WebhookApp.Rules
{
    internal class PollCommandRule : IUpdateRule
    {
        private readonly BotService _botService;
        private readonly MessageRule _messageRule;
        private readonly ConfigService _configService;

        public PollCommandRule(BotService botService, MessageRule messageRule, ConfigService configService) {
            _botService = botService;
            _messageRule = messageRule;
            _configService = configService;
        }
        
        public async Task<bool> IsMatch(Update update) {
            BotConfig config = await _configService.LoadAsync();
            return await _messageRule.IsMatch(update) 
                   && (update.Message.Text.Equals("/poll") 
                       || update.Message.Text.Equals($"/poll@{config.Bot}"));
        }

        public async Task ProcessAsync(Update update) {
            Console.WriteLine($"Processing /poll message..., chatId: {update.Message.Chat.Id.ToString()}");
            
            if (PollsHelper.HasPoll(update.Message.Chat.Id)) {
                var poll = PollsHelper.GetPoll(update.Message.Chat.Id);
                var text = $"⇧ <a href='https://t.me/c/{poll.ChatId.ToString().Substring(4)}/{poll.MessageId.ToString()}'>К голосованию</a> ⇧";
                await _botService.Client.SendTextMessageAsync(
                    chatId: poll.ChatId,
                    text: text,
                    parseMode: ParseMode.Html);
            }
            else {
                await _botService.Client.SendTextMessageAsync(
                    chatId: update.Message.Chat.Id,
                    text: "Голосования ещё нет");
            }
        }
    }
}
