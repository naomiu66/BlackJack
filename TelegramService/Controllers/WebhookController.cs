using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;

namespace TelegramService.Controllers
{
    [Route("telegram")]
    [ApiController]
    public class WebhookController : ControllerBase
    {
        [HttpPost("webhook")]
        public async Task<IActionResult> ReceiveUpdate([FromBody] Update update)
        {
            Console.WriteLine($"Received update: {update?.Message?.Text}");
            return Ok();
        }
    }
}
