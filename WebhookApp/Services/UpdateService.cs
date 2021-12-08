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
        private readonly HashSet<int> _updatesIds;

        public UpdateService(IEnumerable<IUpdateRule> rules, ILogger<UpdateService> logger) {
            _rules = rules;
            _logger = logger;
            _updatesIds = new HashSet<int>();
        }
        
        internal async Task ProcessAsync(Update update) {
            if (_updatesIds.Count == 50)
                _updatesIds.Clear();
            if (_updatesIds.Contains(update.Id))
                return;
            _updatesIds.Add(update.Id);
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
