using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Security.Claims;
using GroomerManager.Shared.DTOs.Response;

namespace GroomerManager.Web.Utils;

public class CustomAuthStateProvider(ProtectedLocalStorage localStorage) : AuthenticationStateProvider
{
    public const string LOCAL_STORAGE_TOKEN_NAME = "auth_token";
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var sessionModel = (await localStorage.GetAsync<LoginResponseDto>(LOCAL_STORAGE_TOKEN_NAME)).Value;
        var identity = sessionModel == null ? new ClaimsIdentity() : GetClaimsIdentity(sessionModel.Token);
        var user = new ClaimsPrincipal(identity);
        return new AuthenticationState(user);
    }

    public async Task MarkUserAsAuthenticated(LoginResponseDto response)
    {
        await localStorage.SetAsync(LOCAL_STORAGE_TOKEN_NAME, response);

        var identity = GetClaimsIdentity(response.Token);
        var user = new ClaimsPrincipal(identity);
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
    }

    private ClaimsIdentity GetClaimsIdentity(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);
        var claims = jwtToken.Claims;
        return new ClaimsIdentity(claims, "jwt_auth");
    }

    public async Task MarkUserAsLoggedOut()
    {
        await localStorage.DeleteAsync(LOCAL_STORAGE_TOKEN_NAME);
        var identity = new ClaimsIdentity();
        var user = new ClaimsPrincipal(identity);
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
    }
}