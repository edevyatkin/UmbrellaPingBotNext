using System;
using System.Globalization;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using WebhookApp.Services;

namespace WebhookApp.Rules
{
    internal class DotMessageRule : IUpdateRule
    {
        private readonly BotService _botService;
        private readonly ReplyToInfoBotMessageRule _messageRule;

        public DotMessageRule(BotService botService, ReplyToInfoBotMessageRule messageRule) {
            _botService = botService;
            _messageRule = messageRule;
        }
        
        public async Task<bool> IsMatch(Update update) {
            return await _messageRule.IsMatch(update)
                && update.Message.Text.Equals(".")
                && Pin.IsPinMessage(update.Message.ReplyToMessage);
        }

        public async Task ProcessAsync(Update update) {
            Console.WriteLine($"[ {DateTime.Now.ToLocalTime().ToString(CultureInfo.InvariantCulture)} ] Processing . message... , chatId: {update.Message.Chat.Id.ToString()}, {update.Message.From.Username}");
            
            Pin pin = new Pin(update.Message.ReplyToMessage);
            if (pin.IsActual()) {
                Poll poll;
                
                if (PollsHelper.HasPoll(update.Message.Chat.Id)) {
                    poll = PollsHelper.GetPoll(update.Message.Chat.Id);
                    await _botService.Client.DeleteMessageAsync(
                        chatId: poll.ChatId,
                        messageId: poll.MessageId);
                    PollsHelper.UpdatePoll(poll.ChatId, pin);
                }
                else {
                    poll = PollsHelper.CreatePoll(update.Message.Chat.Id, pin);
                }
                
                PollView pollView = poll.AsView();

                var message = await _botService.Client.SendTextMessageAsync(
                    chatId: update.Message.Chat.Id,
                    text: pollView.Text,
                    parseMode: ParseMode.Html,
                    replyMarkup: pollView.ReplyMarkup);
                
                poll.ChatId = message.Chat.Id;
                poll.MessageId = message.MessageId;
            }

            await _botService.Client.DeleteMessageAsync(
                chatId: update.Message.Chat.Id,
                messageId: update.Message.MessageId);
        }
    }
}
