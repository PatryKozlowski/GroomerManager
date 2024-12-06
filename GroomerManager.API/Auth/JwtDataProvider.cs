using GroomerManager.Application.Interfaces;
using GroomerManager.Infrastructure.Auth;

namespace GroomerManager.API.Auth;

public class JwtDataProvider(
    JwtManager jwtManager,
    IHttpContextAccessor httpContextAccessor
    ) : IAuthenticationDataProvider
{
    private readonly JwtManager _jwtManager = jwtManager;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    public int? GetUserId()
    {
        var userIdString = GetClaimValue(JwtManager.USER_ID_CLAIM);

        if (int.TryParse(userIdString, out var result))
        {
            return result;
        }

        return null;
    }

    private string? GetTokenFromCookie()
    {
        return _httpContextAccessor.HttpContext?.Request.Cookies[CookieSettings.COOKIE_NAME];
    }

    private string? GetTokenFromHeader()
    {
        var authorizationHeader = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].FirstOrDefault();

        if (string.IsNullOrEmpty(authorizationHeader))
        {
            return null;
        }

        var splited = authorizationHeader.Split(' ');

        if (splited.Length > 1 && splited[0] == "Bearer")
        {
            return splited[1];
        }

        return null;
    }

    private string? GetClaimValue(string claimType)
    {
        var token = GetTokenFromHeader();

        if (string.IsNullOrEmpty(token))
        {
            token = GetTokenFromCookie();
        }
        
        if (!string.IsNullOrWhiteSpace(token) && _jwtManager.ValidateToken(token))
        {
            return _jwtManager.GetClaim(token, claimType);
        }

        return null;
    }
}