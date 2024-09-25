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
    
    private readonly BotConfig _botConfig;
    private readonly ISmoothieService _smoothieService;
    private readonly BotService _botService;

    public SmoothieCommandRule(BotConfig botConfig, ISmoothieService smoothieService, BotService botService) {
        _botConfig = botConfig;
        _smoothieService = smoothieService;
        _botService = botService;
    }

    public async Task<bool> IsMatch(Update update) {
        return update.Type == UpdateType.Message
               && update.Message.Type == MessageType.Text
               && _botConfig.Chats.Contains(update.Message.Chat.Id)
               && (update.Message.Text.Equals("/smoothie") 
                       || update.Message.Text.Equals($"/smoothie@{_botConfig.Bot}"));
    }

    public async Task ProcessAsync(Update update) {
        if (_smoothieService.BestSmoothieStatus == SmoothieStatus.Best) {
            await _botService.Client.SendTextMessageAsync(
                chatId: update.Message.Chat.Id,
                text:
                $"\ud83c\udf79<b>Поиск лучшего смузи</b>\nНайден cамый лучший смузи!\n<code>{_smoothieService.BestSmoothie}</code>\n\n{_smoothieService.BestSmoothieDescription}\n\n/smoothie",
                parseMode: ParseMode.Html
            );
        } else if (_smoothieService.BestSmoothieStatus >= SmoothieStatus.Poor) {
            await _botService.Client.SendTextMessageAsync(
                chatId: update.Message.Chat.Id,
                text:
                $"\ud83c\udf79<b>Поиск лучшего смузи</b>\n<i>Не найден</i>\n\nНе проверено комбинаций: <b>{_smoothieService.ElapsedCombinations}</b> шт.\nПопробуйте эти:\n{string.Join('\n',_smoothieService.Peek(3).Select(c => $"<code>{c}</code>"))}\n\nПодобрать другие — /smoothie",
                parseMode: ParseMode.Html
            );
        } else {
            await _botService.Client.SendTextMessageAsync(
                chatId: update.Message.Chat.Id,
                text:
                $"\ud83c\udf79<b>Поиск лучшего смузи</b>\nНайден:\n{_smoothieService.BestSmoothie}\n\n{_smoothieService.BestSmoothieDescription}\n\nНе проверено комбинаций: <b>{_smoothieService.ElapsedCombinations}</b> шт.\nПопробуйте эти:\n{string.Join('\n',_smoothieService.Peek(3).Select(c => $"<code>{c}</code>"))}\n\nПодобрать другие — /smoothie",
                parseMode: ParseMode.Html
            );
        }
    }
}
