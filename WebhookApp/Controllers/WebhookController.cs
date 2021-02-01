using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;
using WebhookApp.Services;

namespace WebhookApp.Controllers
{
    [Route("bot/update")]
    [ApiController]
    public class WebhookController : ControllerBase
    {
        private readonly ILogger<WebhookController> _logger;
        private readonly UpdateService _service;

        public WebhookController(ILogger<WebhookController> logger, UpdateService service) {
            _logger = logger;
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] Update update) {
            _logger.LogDebug("Update processing...");
            await _service.ProcessAsync(update);
            return Ok();
        }
    }
}
