using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using WebhookApp.Rules;

namespace WebhookApp.Services.Smoothie; 

public class SmoothieCommandRule : IUpdateRule {
    
    private readonly ConfigService _configService;
    private readonly ISmoothieService _smoothieService;
    private readonly BotService _botService;

    public SmoothieCommandRule(ConfigService configService, ISmoothieService smoothieService, BotService botService) {
        _configService = configService;
        _smoothieService = smoothieService;
        _botService = botService;
    }

    public async Task<bool> IsMatch(Update update) {
        BotConfig config = await _configService.LoadAsync();
        return update.Type == UpdateType.Message
               && update.Message.Type == MessageType.Text
               && config.Chats.Contains(update.Message.Chat.Id)
               && (update.Message.Text.Equals("/smoothie") 
                       || update.Message.Text.Equals($"/smoothie@{config.Bot}"));
    }

    public async Task ProcessAsync(Update update) {
        if (_smoothieService.BestSmoothieStatus == SmoothieStatus.Best) {
            await _botService.Client.SendTextMessageAsync(
                chatId: update.Message.Chat.Id,
                text:
                $"<b>Поиск лучшего смузи</b>\nНайден cамый лучший смузи!\n{_smoothieService.BestSmoothie}\n\n{_smoothieService.BestSmoothieDescription}",
                parseMode: ParseMode.Html
            );
        } else if (_smoothieService.BestSmoothieStatus >= SmoothieStatus.Poor) {
            await _botService.Client.SendTextMessageAsync(
                chatId: update.Message.Chat.Id,
                text:
                $"<b>Поиск лучшего смузи</b>\n<i>Не найден</i>\n\nЕщё не проверено комбинаций: {_smoothieService.ElapsedCombinations} шт.\nНужно проверить:\n{_smoothieService.Peek()}",
                parseMode: ParseMode.Html
            );
        } else {
            await _botService.Client.SendTextMessageAsync(
                chatId: update.Message.Chat.Id,
                text:
                $"<b>Поиск лучшего смузи</b>\nНайден:\n{_smoothieService.BestSmoothie}\n\n{_smoothieService.BestSmoothieDescription}\n\nЕщё не проверено комбинаций: {_smoothieService.ElapsedCombinations} шт.\nНужно проверить:\n{_smoothieService.Peek()}",
                parseMode: ParseMode.Html
            );
        }
    }
}
