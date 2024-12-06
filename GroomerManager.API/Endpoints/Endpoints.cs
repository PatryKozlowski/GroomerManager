namespace GroomerManager.API.Endpoints;

public static class Endpoints
{
    public static WebApplication AddEndpoints(this WebApplication app)
    {
        app.MapUserEndpoints();
        return app;
    }
}