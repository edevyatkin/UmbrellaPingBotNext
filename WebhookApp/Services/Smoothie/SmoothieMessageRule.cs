using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using WebhookApp.Rules;

namespace WebhookApp.Services.Smoothie; 

public class SmoothieMessageRule : IUpdateRule {
    
    private readonly ConfigService _configService;
    private readonly ISmoothieService _smoothieService;
    private readonly BotService _botService;

    public SmoothieMessageRule(ConfigService configService, ISmoothieService smoothieService, BotService botService) {
        _configService = configService;
        _smoothieService = smoothieService;
        _botService = botService;
    }

    public async Task<bool> IsMatch(Update update) {
        BotConfig config = await _configService.LoadAsync();
        return update.Type == UpdateType.Message
               && update.Message.Type == MessageType.Text
               && update.Message.ForwardFrom?.Username == "StartupWarsBot"
               && config.Chats.Contains(update.Message.Chat.Id)
               && update.Message.Text.Contains("Ты приготовил 🍹Смузи")
               && update.Message.ForwardDate.Value.Date == DateTime.UtcNow.Date;
    }

    public async Task ProcessAsync(Update update) {
        var ingredientNumbers = new Dictionary<string, int>() {
            ["🍋"] = 0,
            ["🍇"] = 1,
            ["🍏"] = 2,
            ["🥕"] = 3,
            ["🍅"] = 4,
        };
        var text = update.Message.Text.Split('\n'); 
        var smoothie = new Smoothie(
            text[1].EnumerateRunes().Select(c => ingredientNumbers[c.ToString()]).ToArray(),
            text.Length > 6 ? text[6] : string.Empty
        );
        var smoothieStatus = text[3] switch {
            var t when t.Contains("Самый шикарный") => SmoothieStatus.Best,
            var t when t.Contains("Отличный") => SmoothieStatus.Excellent,
            var t when t.Contains("Хороший") => SmoothieStatus.Good,
            var t when t.Contains("Неплохой") => SmoothieStatus.Normal,
            _ => SmoothieStatus.Poor
        };
        _smoothieService.Filter(smoothie, smoothieStatus);
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
