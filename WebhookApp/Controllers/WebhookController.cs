using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using WebhookApp.Services;

namespace WebhookApp.Controllers
{
    [Route("bot/update")]
    [ApiController]
    public class WebhookController : ControllerBase
    {
        private readonly ILogger<WebhookController> _logger;

        public WebhookController(ILogger<WebhookController> logger) {
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] Update update, [FromServices] ITelegramBotClient botClient, [FromServices] UpdateHandler handler, CancellationToken token) {
            _logger.LogDebug("Update processing...");
            try
            {
                await handler.HandleUpdateAsync(botClient, update, token);
            }
            catch (Exception exception)
            {
                await handler.HandleErrorAsync(botClient, exception, Telegram.Bot.Polling.HandleErrorSource.HandleUpdateError, token);
            }
            return Ok();
        }
    }
}
