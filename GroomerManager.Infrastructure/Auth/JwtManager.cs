using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace GroomerManager.Infrastructure.Auth;

public class JwtManager(IOptions<JwtOptions> jwtOptions)
{
    public const string USER_ID_CLAIM = "UserId";
    private readonly JwtOptions _jwtOptions = jwtOptions.Value;

    private SymmetricSecurityKey GetSecurityKey()
    {
        var secret = _jwtOptions.Secret;
        
        if (string.IsNullOrWhiteSpace(secret))
        {
            throw new ArgumentException("[JWT] Secret is empty");
        }

        return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret));
    }

    private string GenerateTokenWithClaims(IEnumerable<Claim> claims)
    {
        if (string.IsNullOrWhiteSpace(_jwtOptions.Issuer) || string.IsNullOrWhiteSpace(_jwtOptions.Audience))
        {
            throw new ArgumentException("[JWT] Issuer or Audience is empty");
        }

        if (_jwtOptions.ExpirationMinutes <= 0)
        {
            throw new ArgumentException("[JWT] Expiration is empty or has wrong value");
        }
        
        var secretKey = GetSecurityKey();

        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescription = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(_jwtOptions.ExpirationMinutes),
            Issuer = _jwtOptions.Issuer,
            Audience = _jwtOptions.Audience,
            SigningCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescription);
        return tokenHandler.WriteToken(token);
    }

    public bool ValidateToken(string token)
    {
        if (string.IsNullOrEmpty(token))
        {
            return false;
        }

        var securityKey = GetSecurityKey();

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
            }, out SecurityToken validatedToken);
        }
        catch
        {
            return false;
        }

        return true;
    }
    
    public string GenerateUserToken(int userId)
    {
        var claims = new Claim[]
        {
            new Claim(USER_ID_CLAIM, userId.ToString()),
        };

        return GenerateTokenWithClaims(claims);
    }
    
    public string? GetClaim(string token, string claimType)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var securityToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

        var stringClaimValue = securityToken?.Claims.FirstOrDefault(claim => claim.Type == claimType)?.Value;
        return stringClaimValue;
    }
}