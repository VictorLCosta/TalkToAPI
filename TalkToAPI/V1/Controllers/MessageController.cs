using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using TalkToAPI.V1.Models;
using TalkToAPI.V1.Models.DTO;
using TalkToAPI.V1.Repositories.Contracts;

namespace TalkToAPI.V1.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class MessageController : ControllerBase
    {
        private readonly IMessageRepository _repo;
        private readonly IMapper _mapper;

        public MessageController(IMessageRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpPost("", Name = "CreateNew")]
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

                    var dtomessage = _mapper.Map<Message, DTOMessage>(message);
                    dtomessage.Links.Add(new DTOLink("self", Url.Link("CreateNew", null), "POST"));
                    dtomessage.Links.Add(new DTOLink("update", Url.Link("PartialUpdate", new { id = message.Id }), "PATCH"));

                    return Created($"api/[controller]/{message.Id}", dtomessage);
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

        [HttpGet("{userOneId}/{userTwoId}", Name = "FindAllMessages")]
        public IActionResult FindAllMessages(string userOneId, string userTwoId, [FromHeader(Name = "Accept")]string mediaType)
        {
            if(userOneId == userTwoId)
            {
                return UnprocessableEntity();
            }

            var messages = _repo.FindAll(userOneId, userTwoId).ToList();

            if(mediaType == "application/vnd.talkto.hateoas+json")
            {
                var dtomessages = _mapper.Map<List<Message>, List<DTOMessage>>(messages);

                var list = new DTOList<DTOMessage> { Results = dtomessages };
                list.Links.Add(new DTOLink("self", Url.Link("FindAllMessages", null), "GET"));

                return Ok(list);
            }
            else
            {
                return Ok(messages);
            }

        }

        [HttpPatch("{id}", Name = "PartialUpdate")]
        public async Task<IActionResult> PartialUpdate(int id, [FromBody]JsonPatchDocument<Message> jsonPatch)
        {
            //JSONPatch
            if(jsonPatch == null)
            {
                return BadRequest();
            }

            var message = await _repo.FindMessageAsync(id);

            jsonPatch.ApplyTo(message);
            message.Updated = DateTime.UtcNow;

            await _repo.UpdateAsync(message);

            var dtomessage = _mapper.Map<Message, DTOMessage>(message);
            dtomessage.Links.Add(new DTOLink("update", Url.Link("PartialUpdate", new { id = message.Id }), "PATCH"));

            return NoContent();
        }
    }
}