using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using WebhookApp.Services;
using WebhookApp.Services.Laboratory;

namespace WebhookApp.Rules {
    internal class LabMoneyCommandRule : IUpdateRule {
        private readonly MessageRule _messageRule;
        private readonly BotService _botService;
        private readonly ConfigService _config;
        private readonly ILaboratoryService _laboratoryService;
        private readonly ILogger<LabMoneyCommandRule> _logger;

        public LabMoneyCommandRule(ILogger<LabMoneyCommandRule> logger, MessageRule messageRule, BotService botService,
            ConfigService config, ILaboratoryService laboratoryService) {
            _messageRule = messageRule;
            _botService = botService;
            _config = config;
            _laboratoryService = laboratoryService;
            _logger = logger;
        }

        public async Task<bool> IsMatch(Update update) {
            return await _messageRule.IsMatch(update) &&
                   Regex.IsMatch(update.Message.Text, "^/labmoney.*");
        }

        public async Task ProcessAsync(Update update) {
            var text = update.Message.Text;
            var config = await _config.LoadAsync();

            if (text == "/labmoney" || text == $"/labmoney@{config.Bot}") {
                await _botService.Client.SendTextMessageAsync(
                    chatId: update.Message.Chat.Id,
                    text:
                    "<b>Расчет затрат лаборатории</b>\nФормат команды:\n/labmoney <i>ваш_уровень</i> <i>кол-во_нанимаемых_1 кол-во_нанимаемых_2 ...</i>\nУровень от 1 до 99; количество от 1 до 99, до 6 чисел через пробел",
                    parseMode: ParseMode.Html
                );
                return;
            }

            var numsStrs = text.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (!numsStrs[1..].All(n => int.TryParse(n, out _)))
                return;

            var nums = numsStrs[1..].Select(int.Parse).ToArray();

            var level = nums[0];
            if (level is < 1 or > 99) {
                await _botService.Client.SendTextMessageAsync(
                    chatId: update.Message.Chat.Id,
                    text: "Уровень должен быть от 1 до 99"
                );
                return;
            }

            var workers = nums[1..];
            if (workers.All(n => n is < 1 or > 99) || workers.Length > 6) {
                await _botService.Client.SendTextMessageAsync(
                    chatId: update.Message.Chat.Id,
                    text: "Каждое количество нанимаемых должно быть от 1 до 99, не более 6"
                );
                return;
            }

            await _botService.Client.SendTextMessageAsync(
                chatId: update.Message.Chat.Id,
                text: $"<b>Расчет затрат лаборатории</b>\n" +
                      $"Уровень: {level}\nКоличество нанимаемых: {string.Join("+", workers)}\n" +
                      $"Сумма на день: {_laboratoryService.CalculateSalary(level, workers):N0} $",
                parseMode: ParseMode.Html
            );
        }


    }
}