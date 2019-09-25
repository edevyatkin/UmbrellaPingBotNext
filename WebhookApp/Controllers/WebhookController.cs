using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;

namespace WebhookApp.Controllers
{
    [Route("bot/update")]
    [ApiController]
    public class WebhookController : ControllerBase
    {
        private readonly IUpdateServerStream stream;

        public WebhookController(IUpdateServerStream stream) {
            this.stream = stream;
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] Update update) {
            if (update == null) {
                return BadRequest();
            }
            await stream.SendUpdateAsync(update);
            return Ok();
        }
    }
}
