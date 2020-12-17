using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TalkToAPI.V1.Models;
using TalkToAPI.V1.Repositories.Contracts;

namespace TalkToAPI.V1.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class MessageController : ControllerBase
    {
        private readonly IMessageRepository _repo;

        public MessageController(IMessageRepository repo)
        {
            _repo = repo;
        }

        [HttpPost("")]
        public async Task<IActionResult> CreateNew([FromBody]Message message)
        {
            if(message == null)
            {
                return BadRequest();
            }

            if(ModelState.IsValid)
            {
                try
                {
                    await _repo.CreateAsync(message);
                    return Created($"api/[controller]/{message.Id}", message);
                }
                catch(Exception e)
                {
                    return UnprocessableEntity(e);
                }
            }
            else
            {
                return UnprocessableEntity();
            }
        }

        [HttpGet("{userOneId}/{userTwoId}")]
        public IActionResult FindAllMessage(string userOneId, string userTwoId)
        {
            if(userOneId == userTwoId)
            {
                return UnprocessableEntity();
            }

            var result = _repo.FindAll(userOneId, userTwoId);

            return Ok(result);
        }
    }
}