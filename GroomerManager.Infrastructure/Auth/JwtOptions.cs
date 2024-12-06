namespace GroomerManager.Infrastructure.Auth;

public abstract class JwtOptions
{
    public required string Secret { get; set; }
    public required string Issuer { get; set; }
    public required string Audience { get; set; }
    public int ExpirationMinutes { get; set; }
}
