namespace GroomerManager.Domain.DTOs;
public class LoginResponseDto
{
    public required string Token { get; set; }
    public required long TokenExpired { get; set; }
    public required string RefreshToken { get; set; }
}