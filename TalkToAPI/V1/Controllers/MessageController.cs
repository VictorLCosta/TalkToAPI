using Microsoft.AspNetCore.Mvc;

namespace TalkToAPI.V1.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class MessageController : ControllerBase
    {
        public IActionResult CreateNew()
        {
            return Ok();
        }

        public IActionResult FindMessage()
        {
            return Ok();
        }
    }
}