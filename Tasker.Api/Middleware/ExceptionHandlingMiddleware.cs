using System.Text.Json;
using Tasker.Api.Exceptions;

namespace Tasker.Api.Middleware;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (NotFoundException ex)
        {
            logger.LogWarning("Resource not found: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unhandled exception occurred.");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var statusCode = exception switch
        {
            NotFoundException => StatusCodes.Status404NotFound,
            _ => StatusCodes.Status500InternalServerError
        };

        var message = exception switch
        {
            NotFoundException notFoundEx => notFoundEx.Message,
            _ => "An unexpected error occurred."
        };

        context.Response.StatusCode = statusCode;

        var result = JsonSerializer.Serialize(new { error = message });
        return context.Response.WriteAsync(result);
    }
}
