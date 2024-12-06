using System.Diagnostics;
using Microsoft.AspNetCore.Diagnostics;

namespace GroomerManager.API.Exception;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger = logger;

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        System.Exception exception,
        CancellationToken cancellationToken)
    {
        var traceId = Activity.Current?.Id ?? httpContext.TraceIdentifier;

        _logger.LogError(
            exception,
            "Could not process a request on machine {MachineName}, TraceId: {TraceId}",
            Environment.MachineName,
            traceId);

        var (statusCode, title) = MapException(exception);

        await Results.Problem(
            title: title,
            statusCode: statusCode,
            extensions: new Dictionary<string, object?>
            {
                { "traceId", traceId }
            }
        ).ExecuteAsync(httpContext);

        return true;
    }
    private static (int StatusCode, string Title) MapException(System.Exception exception)
    {
        return exception switch
        {
            InvalidOperationException => (StatusCodes.Status400BadRequest, exception.Message),
            _ => (StatusCodes.Status500InternalServerError, "We are working on it !")
        };
    }
}