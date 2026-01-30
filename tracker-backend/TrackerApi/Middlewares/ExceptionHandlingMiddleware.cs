namespace TrackerApi.Middlewares;

using System.Net;
using System.Text.Json;
using TrackerApi.DTOs;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger = logger;

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception");

            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(
        HttpContext context,
        Exception exception)
    {
        var (statusCode, message) = exception switch
        {
            ArgumentException or
            ArgumentOutOfRangeException or
            InvalidOperationException =>
                (HttpStatusCode.BadRequest, exception.Message),

            FileNotFoundException =>
                (HttpStatusCode.NotFound, exception.Message),

            _ =>
                (HttpStatusCode.InternalServerError,
                 "An unexpected error occurred.")
        };

        var response = new ApiErrorResponseDTO(
            (int)statusCode,
            message);

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var json = JsonSerializer.Serialize(response);

        return context.Response.WriteAsync(json);
    }
}
