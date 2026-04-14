using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using WebhookApp.Rules;

namespace WebhookApp.Services;

public class UpdateHandler : IUpdateHandler
{
    private readonly Queue<int> _updateIds = new ();
    private readonly object _lockObj = new ();
    private readonly IEnumerable<IUpdateRule> _rules;
    private readonly ILogger<UpdateHandler> _logger;

    public UpdateHandler(ILogger<UpdateHandler> logger, IEnumerable<IUpdateRule> rules)
    {
        _logger = logger;
        _rules = rules;
    }

    public async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, HandleErrorSource source, CancellationToken cancellationToken)
    {
        _logger.LogInformation("HandleError: {Exception}", exception);
        if (exception is RequestException)
            await Task.Delay(TimeSpan.FromSeconds(2), cancellationToken);
    }

    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        lock (_lockObj)
        {
            if (_updateIds.Contains(update.Id))
                return;
            if (_updateIds.Count == 100)
                _updateIds.TryDequeue(out _);
            _updateIds.Enqueue(update.Id);
        }
        foreach (IUpdateRule rule in _rules) {
            _logger.LogDebug($"Checking rule: {rule.GetType().Name}");
            if (await rule.IsMatch(update)) {
                _logger.LogDebug($"Match rule: {rule.GetType().Name}");
                await rule.ProcessAsync(update);
            }
        }
    }
}