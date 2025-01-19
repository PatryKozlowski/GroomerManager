using GroomerManager.Application.Salon;
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
    public async Task<ActionResult> GetUserSalons()
    {
        var result = await _mediator.Send(new GetUserSalonCommand.Request() {});
        return Ok(result);
    }
}