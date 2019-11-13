using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> PostAsync() {
            await stream.SendUpdateAsync(Request.Body);
            return Ok();
        }
    }
}
