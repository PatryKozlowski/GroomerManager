using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GroomerManager.Application.Common.Interfaces;
using GroomerManager.Domain.DTOs;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace GroomerManager.Infrastructure.Auth;

public class JwtManager(IOptions<JwtOptions> jwtOptions) : IJwtManager
{
    public const string USER_ID_CLAIM = "UserId";
    public const string USER_EMAIL_CLAIM = "UserEmail";
    public const string USER_ROLE = "UserRole";
    private readonly JwtOptions _jwtOptions = jwtOptions.Value;

    private SymmetricSecurityKey GetSecurityKey(bool isRefreshToken)
    {
        var secret = isRefreshToken ? _jwtOptions.RefreshTokenSecret : _jwtOptions.Secret;

        if (string.IsNullOrWhiteSpace(secret))
        {
            throw new ArgumentException("[JWT] Secret is empty");
        }

        return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret));
    }

    private string GenerateTokenWithClaims(IEnumerable<Claim> claims, bool isRefreshToken)
    {
        if (string.IsNullOrWhiteSpace(_jwtOptions.Issuer) || string.IsNullOrWhiteSpace(_jwtOptions.Audience))
        {
            throw new ArgumentException("[JWT] Issuer or Audience is empty");
        }

        if (_jwtOptions.Expires <= 0)
        {
            throw new ArgumentException("[JWT] Expiration is empty or has wrong value");
        }

        var secretKey = GetSecurityKey(isRefreshToken);

        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescription = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(isRefreshToken ? 24*60*35 : 24*60*30),
            Issuer = _jwtOptions.Issuer,
            Audience = _jwtOptions.Audience,
            SigningCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescription);
        return tokenHandler.WriteToken(token);
    }

    public bool ValidateToken(string token, bool isRefreshToken)
    {
        if (string.IsNullOrEmpty(token))
        {
            return false;
        }

        var securityKey = GetSecurityKey(isRefreshToken);

        var tokenHandler = new JwtSecurityTokenHandler();
        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidIssuer = _jwtOptions.Issuer,
                ValidAudience = _jwtOptions.Audience,
                IssuerSigningKey = securityKey
            }, out var validatedToken);
        }
        catch
        {
            return false;
        }

        return true;
    }

    public string GenerateUserToken(UserDto user, bool isRefreshToken)
    {
        var claims = new List<Claim>()
        {
            new(USER_ID_CLAIM, user.Id.ToString()),
            new (USER_EMAIL_CLAIM, user.Email)
        };
        claims.AddRange(user.Roles.Select(n => new Claim(USER_ROLE, n.Role.Name)));

        return GenerateTokenWithClaims(claims, isRefreshToken);
    }

    public string? GetClaim(string token, string claimType)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var securityToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

        var stringClaimValue = securityToken?.Claims.FirstOrDefault(claim => claim.Type == claimType)?.Value;
        return stringClaimValue;
    }
}
