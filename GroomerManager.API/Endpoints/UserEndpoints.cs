using GroomerManager.Application.Interfaces;
using GroomerManager.Domain.DTOs;

namespace GroomerManager.API.Endpoints;

public static class UserEndpoints
{
    public static WebApplication MapUserEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("user").WithParameterValidation();
        
        group.MapPost("/create", async (CreateUserDto request, IUserService userService) =>
        {
            var result = await userService.CreateUserWithAccount(request);
            
            var location = $"/user/{result}"; 

            return Results.Created(location, new { userId = result });
        });
        
        return app;
    }
}