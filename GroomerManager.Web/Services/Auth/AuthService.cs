using GroomerManager.Shared.DTOs.Request;
using GroomerManager.Shared.DTOs.Response;
using GroomerManager.Web.Utils;

namespace GroomerManager.Web.Services.Auth;

public class AuthService(ApiClient apiClient) : IAuthService
{
    private const string LOGIN_API = "/api/Auth/Login";

    public async Task<LoginResponseDto> LoginUserAsync(LoginRequestDto request)
    {
        var response = await apiClient.PostAsync<LoginResponseDto, LoginRequestDto>(LOGIN_API, request);

        return response;
    }
}