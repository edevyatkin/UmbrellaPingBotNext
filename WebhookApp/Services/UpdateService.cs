using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;
using WebhookApp.Rules;

namespace WebhookApp.Services
{
    public class UpdateService
    {
        private readonly IEnumerable<IUpdateRule> _rules;
        private readonly ILogger<UpdateService> _logger;

        public UpdateService(IEnumerable<IUpdateRule> rules, ILogger<UpdateService> logger) {
            _rules = rules;
            _logger = logger;
        }
        
        internal async Task ProcessAsync(Update update) {
            foreach (IUpdateRule rule in _rules) {
                _logger.LogDebug($"Checking rule: {rule.GetType().Name}");
                if (await rule.IsMatch(update)) {
                    _logger.LogDebug($"Match rule: {rule.GetType().Name}");
                    await rule.ProcessAsync(update);
                }
            }
        }
    }
}
