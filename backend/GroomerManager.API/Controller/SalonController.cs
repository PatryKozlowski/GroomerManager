using GroomerManager.Application.Salon;
using GroomerManager.Domain.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GroomerManager.API.Controller;

[Route("api/[controller]/[action]")]
[ApiController]
public class SalonController : BaseController
{
    private readonly IMediator _mediator;
    
    public SalonController(ILogger<SalonController> logger, IMediator mediator) : base(logger, mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    public async Task<ActionResult<List<SalonResponseDto>>> GetUserSalons()
    {
        var result = await _mediator.Send(new GetUserSalonCommand.Request() {});
        return Ok(result);
    }
    
    [HttpPost]
    public async Task<ActionResult<AddSalonResponseDto>> CreateSalon([FromForm] AddSalonRequestDto dto)
    {
        var request = new CreateSalonCommand.Request
        {
            Name = dto.Name,
            Logo = dto.Logo
        };
        
        Console.WriteLine($"Odebrano nazwę: {request.Name}");
        Console.WriteLine($"Odebrano logo: {request.Logo?.FileName}");

        var result = await _mediator.Send(request);
        return Ok(result);
    }
}