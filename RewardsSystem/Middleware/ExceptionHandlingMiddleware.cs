using System.Net;
using System.Text.Json;
using RewardsSystem.Dtos;
using RewardsSystem.Exceptions;

namespace RewardsSystem.Middleware;

/// <summary>
/// Catches every exception thrown further down the pipeline and turns it into
/// a clear, consistent JSON error response instead of letting the app crash
/// or leak a raw stack trace to the client.
/// </summary>
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (BusinessRuleException ex)
        {
            // Expected, validated business errors -> 400 Bad Request with a clear message.
            _logger.LogWarning(ex, "Business rule violation.");
            await WriteErrorAsync(context, HttpStatusCode.BadRequest, ex.GetType().Name, ex.Message);
        }
        catch (Exception ex)
        {
            // Anything unexpected -> 500, but still a clean JSON response, never a crash.
            _logger.LogError(ex, "Unhandled exception.");
            await WriteErrorAsync(context, HttpStatusCode.InternalServerError, "InternalServerError",
                "An unexpected error occurred while processing the request.");
        }
    }

    private static async Task WriteErrorAsync(HttpContext context, HttpStatusCode statusCode, string error, string message)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var payload = new ErrorResponse
        {
            Error = error,
            Message = message
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(payload));
    }
}
