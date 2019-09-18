using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;

namespace WebhookApp.Controllers
{
    [Route("bot")]
    [ApiController]
    public class WebhookController : ControllerBase
    {
        private readonly IUpdateServerStream stream;

        public WebhookController(IUpdateServerStream stream) {
            this.stream = stream;
        }

        [HttpPost]
        public async Task PostAsync([FromBody] Update update) {
            if (update == null) {
                return;
            }
            await stream.SendUpdateAsync(update);
        }
    }
}
