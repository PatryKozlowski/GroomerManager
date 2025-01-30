using GroomerManager.Application.Note.Client;
using GroomerManager.Domain.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GroomerManager.API.Controller;

[Route("api/[controller]/[action]")]
[ApiController]
public class NoteController : BaseController
{
    private readonly IMediator _mediator;
    
    public NoteController(ILogger<NoteController> logger, IMediator mediator) : base(logger, mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    public async Task<ActionResult<ClientNoteResponseDto>> GetClientNote([FromQuery] Guid salonId, Guid clientId)
    {
        var request = new GetClientNoteCommand.Request
        {
            SalonId = salonId,
            ClientId = clientId
        };
        
        var result = await _mediator.Send(request);
        return Ok(result);
    }
    
    [HttpPost]
    public async Task<ActionResult<ClientNoteResponseDto>> AddNewClientNote([FromQuery] Guid salonId, Guid clientId, [FromBody] AddNoteForClientRequestDto request)
    {
        var command = new AddNewNoteCommand.Request
        {
            SalonId = salonId,
            ClientId = clientId,
            Note = request.Note
        };
        
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}