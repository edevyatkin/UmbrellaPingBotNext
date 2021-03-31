using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using WebhookApp.Rules;
using WebhookApp.Services;

internal class LabMoneyCommandRule : IUpdateRule
{
    private readonly MessageRule _messageRule;
    private readonly BotService _botService;
    private readonly ConfigService _config;
    private readonly ILogger<LabMoneyCommandRule> _logger;

    public LabMoneyCommandRule(ILogger<LabMoneyCommandRule> logger, MessageRule messageRule, BotService botService, ConfigService config)
    {
        _messageRule = messageRule;
        _botService = botService;
        _config = config;
        _logger = logger;
    }
    
    public async Task<bool> IsMatch(Update update)
    {
        return await _messageRule.IsMatch(update) &&
            Regex.IsMatch(update.Message.Text, "^/labmoney.*");
    }

    public async Task ProcessAsync(Update update)
    {
        var text = update.Message.Text;
        var config = await _config.LoadAsync();
        var pattern = @"^\/labmoney (?'level'\d{1,2}) (?'count'\d{1,2})$";
        if (text == "/labmoney" || text == $"/labmoney@{config.Bot}") {
            await _botService.Client.SendTextMessageAsync(
                chatId: update.Message.Chat.Id,
                text: "<b>Расчет затрат лаборатории</b>\nФормат команды:\n/labmoney <i>ваш_уровень</i> <i>количество_нанимаемых</i>\nУровень от 1 до 99, количество от 1 до 99",
                parseMode: ParseMode.Html
            );
        } else if (Regex.IsMatch(text, pattern)) {
            var match = Regex.Match(text, pattern);
            var level = int.Parse(match.Groups["level"].Value);
            if (level == 0) {
                await _botService.Client.SendTextMessageAsync(
                    chatId: update.Message.Chat.Id,
                    text: "Уровень должен быть больше 0"
                );
                return;               
            }
            var count = int.Parse(match.Groups["count"].Value);
            if (count == 0) {
                await _botService.Client.SendTextMessageAsync(
                    chatId: update.Message.Chat.Id,
                    text: "Количество нанимаемых должно быть больше 0"
                );
                return;               
            } 
            await _botService.Client.SendTextMessageAsync(
                chatId: update.Message.Chat.Id,
                text: $"<b>Расчет затрат лаборатории</b>\nУровень: {level}\nКоличество нанимаемых: {count}\nСумма на день: {CalculateMoney(level,count):N0} $",
                parseMode: ParseMode.Html
            );       
        }
    }

    private int CalculateMoney(int level, int count) => (20*level+level*(count-1))*count/2;
}