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
        private readonly IUpdateProxy proxy;

        public WebhookController(IUpdateProxy proxy) {
            this.proxy = proxy;
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync() {
            using (var reader = new StreamReader(Request.Body)) {
                var updateJson = await reader.ReadToEndAsync();
                var isSend = await proxy.SendUpdateAsync(updateJson);
                return isSend == true ? Ok() : StatusCode(500);
            }
        }
    }
}
