using System;
using System.Collections.Generic;
using System.IO;
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
            using (var reader = new StreamReader(Request.Body)) {
                var updateJson = await reader.ReadToEndAsync();
                var isSend = await stream.SendUpdateAsync(updateJson);
                return isSend == true ? Ok() : StatusCode(500);
            }
        }
    }
}
