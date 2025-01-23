using GroomerManager.Application.Client;
using GroomerManager.Domain.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GroomerManager.API.Controller;

[Route("api/[controller]/[action]")]
[ApiController]
public class ClientController :  BaseController
{
    private readonly IMediator _mediator;

    public ClientController(ILogger<ClientController> logger, IMediator mediator) : base(logger, mediator)
    {
        _mediator = mediator;
    }
    
    // [HttpPost]
    // public async Task<ActionResult<ClientNoteResponseDto>> AddNoteForClient([FromBody] AddNewNoteForClientCommand.Request request)
    // {
    //     var result = await _mediator.Send(request);
    //     return Ok(result);
    // }
    
    [HttpPost]
    public async Task<ActionResult<NewClientResponseDto>> AddNewClient([FromQuery] Guid salonId, [FromBody] NewClientRequestDto dto)
    {
        var request = new AddNewClientCommand.Request
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            PhoneNumber = dto.PhoneNumber,
            Email = dto.Email,
            SalonId = salonId,
        };
        
        var result = await _mediator.Send(request);
        return Ok(result);
    }
    

    [HttpGet]
    public async Task<ActionResult<List<ClientsResponseDto>>> GetClients([FromQuery] GetClientsCommand.Request request)
    {
        var result = await _mediator.Send(request);
        return Ok(result);
    }
    
    // [HttpGet]
    // public async Task<ActionResult> GetClient([FromQuery] GetClientWithNotesAndPetsCommand.Request request)
    // {
    //     var result = await _mediator.Send(request);
    //     return Ok(result);
    // }
}