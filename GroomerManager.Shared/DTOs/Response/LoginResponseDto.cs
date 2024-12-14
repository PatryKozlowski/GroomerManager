using System.Text.Json.Serialization;

namespace GroomerManager.Shared.DTOs.Response;

public class LoginResponseDto
{
    [property: JsonPropertyName("token")]
    public required string Token { get; set; }
    [property: JsonPropertyName("tokenExpired")]
    public required int TokenExpired { get; set; }
    [property: JsonPropertyName("refreshToken")]
    public required string RefreshToken { get; set; }
}