using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using WebhookApp.Rules;

namespace WebhookApp.Services
{
    public class UpdateService
    {
        private readonly IEnumerable<IUpdateRule> _rules;
        private readonly ILogger<UpdateService> _logger;
        private readonly Queue<int> _updateIds = new ();
        private readonly object _lockObj = new ();

        public UpdateService(IEnumerable<IUpdateRule> rules, ILogger<UpdateService> logger) {
            _rules = rules;
            _logger = logger;
        }
        
        internal async Task ProcessAsync(Update update) {
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
                    try {
                        await rule.ProcessAsync(update);
                    }
                    catch (ApiRequestException ex) {
                        _logger.LogError("Rule processing error: {0}", ex.Message);
                    }
                }
            }
        }
    }
}
