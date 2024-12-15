using GroomerManager.Application.Auth;
using GroomerManager.Infrastructure.Auth;
using GroomerManager.Shared.DTOs.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GroomerManager.API.Controller;

[Route("api/[controller]/[action]")]
[ApiController]
public class AuthController( 
    IMediator mediator,
    JwtManager jwtManager
) : BaseController(mediator)
{
    private readonly IMediator _mediator = mediator;

    [HttpPost]
    public async Task<ActionResult> CreateUser([FromBody] CreateUserCommand.Request request)
    {
        var result = await _mediator.Send(request);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginCommand.Request request)
    {
        var result = await _mediator.Send(request);
        var token = jwtManager.GenerateUserToken(result, false);
        var refreshToken = jwtManager.GenerateUserToken(result, true);
        
        return Ok(new LoginResponseDto
        {
            Token = token,
            TokenExpired = 0,
            RefreshToken = refreshToken
        });
    }
}