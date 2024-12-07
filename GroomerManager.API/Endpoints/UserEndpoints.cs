using GroomerManager.API.Auth;
using GroomerManager.Application.Interfaces;
using GroomerManager.Domain.DTOs;
using GroomerManager.Infrastructure.Auth;
using Microsoft.Extensions.Options;

namespace GroomerManager.API.Endpoints;

public static class UserEndpoints
{
    public static WebApplication MapUserEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("user").WithParameterValidation();

        group.MapGet("/account", async (ICurrentAccountProvider currentAccountProvider) =>
        {
            var result = await currentAccountProvider.GetAuthenticatedAccount();

            return Results.Ok(new { name = result.Name });
        });
        
        group.MapPost("/create", async (CreateUserDto request, IUserService userService) =>
        {
            var result = await userService.CreateUserWithAccount(request);
            
            var location = $"/user/{result}"; 

            return Results.Created(location, new { userId = result });
        });

        group.MapPost("/login", async (
            LoginUserDto request, 
            IUserService userService,
            JwtManager jwtManager,
            HttpContext context,
            IOptions<CookieSettings> cookieSettings
            ) =>
        {
            var userId = await userService.LoginUser(request);
            var token = jwtManager.GenerateUserToken(userId);
            SetTokenCookie(context, token, cookieSettings.Value);
            return Results.Ok(new { AccessToken = token });
        });

        group.MapPost("/logout", (HttpContext context) =>
        {
            DeleteTokenCookie(context);
            return Results.Ok();
        });
        
        return app;
    }

    private static void SetTokenCookie(HttpContext context, string token, CookieSettings? cookieSettings)
    {
        var cookieOption = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            Expires = DateTime.UtcNow.AddMinutes(60),
            SameSite = SameSiteMode.Lax
        };
        
        if (cookieSettings != null)
        {
            cookieOption = new CookieOptions()
            {
                HttpOnly = cookieOption.HttpOnly,
                Expires = DateTime.UtcNow.AddMinutes(cookieSettings.Expires),
                Secure = cookieSettings.Secure,
                SameSite = cookieSettings.SameSite,
            };
        }

        context.Response.Cookies.Append(CookieSettings.COOKIE_NAME, token, cookieOption);
    }
    
    private static void DeleteTokenCookie(HttpContext context)
    {
        context.Response.Cookies.Delete(CookieSettings.COOKIE_NAME, new CookieOptions()
        {
            HttpOnly = true,
        });
    }
}