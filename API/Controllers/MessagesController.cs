using Application.Commands;
using Application.DTOs.Response;
using Application.Queries;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MessagesController : ControllerBase
{
    private readonly ILogger<MessagesController> _logger;
    private readonly IMediator _mediator;

    public MessagesController(
        ILogger<MessagesController> logger, 
        IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> Details([FromRoute] Guid id)
    {
        try
        {
            var query = new GetMessageByIdQuery { Id = id };
            var result = await _mediator.Send(query);

            if (result == null)
                return NotFound();

            return Ok(result);
        }
        catch (Exception ex)
        {

            throw;
        }
    }

    [HttpGet("conversation")]
    public async Task<ActionResult<IEnumerable<MessageDto>>> GetConversation(string user1Id, string user2Id)
    {
        var query = new GetConversationQuery { User1Id = user1Id, User2Id = user2Id };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("user/{userId}")]
    public async Task<ActionResult<IEnumerable<MessageDto>>> GetUserMessages(string userId)
    {
        var query = new GetUserMessagesQuery { UserId = userId };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<MessageDto>> SendMessage(SendMessageCommand command)
    {
        var result = await _mediator.Send(command);
        return Created(result.Id.ToString(),result);
    }

    [HttpPut("{id}/status")]
    public async Task<ActionResult<MessageDto>> UpdateStatus(Guid id, [FromBody] Message.EStatus newStatus)
    {
        var command = new UpdateMessageStatusCommand { MessageId = id, NewStatus = newStatus };
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}
