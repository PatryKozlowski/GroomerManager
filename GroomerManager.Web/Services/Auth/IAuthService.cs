using GroomerManager.Shared.DTOs.Request;
using GroomerManager.Shared.DTOs.Response;

namespace GroomerManager.Web.Services.Auth;

public interface IAuthService
{
    Task<LoginResponseDto> LoginUserAsync(LoginRequestDto request);
}