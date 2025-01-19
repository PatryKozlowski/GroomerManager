using GroomerManager.API.Auth;
using GroomerManager.Application.Auth;
using GroomerManager.Application.Common.Interfaces;
using GroomerManager.Domain.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace GroomerManager.API.Controller;

[Route("api/[controller]/[action]")]
[ApiController]
public class AuthController: BaseController
{
    private readonly IMediator _mediator;
    private readonly CookieSettings? _cookieSetting;
    private readonly IDateTime _dateTime;


    public AuthController(ILogger<AuthController> logger, IMediator mediator, IOptions<CookieSettings> cookieSetting, IDateTime dateTime) : base(logger, mediator)
    {
        _mediator = mediator;
        _cookieSetting = cookieSetting != null ? cookieSetting.Value : null;
        _dateTime = dateTime;
    }

    [HttpPost]
    public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginRequestDto dto)
    {
        var request = new LoginCommand.Request
        {
            Email = dto.Email,
            Password = dto.Password
        };

        var result = await _mediator.Send(request);
        SetTokenCookie(result.Token);
        SetTokenCookie(result.RefreshToken, true);
        return Ok(result);
    }
    
    private void SetTokenCookie(string token, bool isRefreshToken = false)
    {
        var cookieOption = new CookieOptions()
        {
            HttpOnly = true,
            Secure = true,
            Expires = isRefreshToken ? _dateTime.Now.AddDays(31) : _dateTime.Now.AddDays(40),
            SameSite = SameSiteMode.Lax,
        };

        if (_cookieSetting != null)
        {
            cookieOption = new CookieOptions()
            {
                HttpOnly = cookieOption.HttpOnly,
                Expires = cookieOption.Expires,
                Secure = _cookieSetting.Secure,
                SameSite = _cookieSetting.SameSite
            };
        }

        Response.Cookies.Append(isRefreshToken ?  CookieSettings.REFRESH_COKIE_NAME : CookieSettings.COOKIE_NAME, token, cookieOption);
    }
    
    private void DeleteTokenCookie(bool isRefreshToken = false)
    {
        Response.Cookies.Delete(isRefreshToken ?  CookieSettings.REFRESH_COKIE_NAME : CookieSettings.COOKIE_NAME, new CookieOptions()
        {
            HttpOnly = true,
        });
    }
}