using System.Text.Json;
using Jobify.Application.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Jobify.WebAPI.Middlewares;

public class CustomExceptionHandlerMiddleware(
    RequestDelegate next,
    ILogger<CustomExceptionHandlerMiddleware> logger,
    IWebHostEnvironment env)
{
    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await next(httpContext);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
    {
        logger.LogError(exception, "Unhandled exception occurred. TraceId: {TraceId}", httpContext.TraceIdentifier);

        var (detail, title, statusCode) = exception switch
        {
            BadRequestException => (
                exception.Message,
                "Bad Request",
                StatusCodes.Status400BadRequest
            ),
            DomainException => (
                exception.Message,
                "Business Rule Violation",
                StatusCodes.Status400BadRequest
            ),
            UnauthorizedException => (
                exception.Message,
                "Unauthorized",
                StatusCodes.Status401Unauthorized
            ),
            ForbiddenAccessException => (
                exception.Message,
                "Forbidden",
                StatusCodes.Status403Forbidden
            ),
            NotFoundException => (
                exception.Message,
                "Not Found",
                StatusCodes.Status404NotFound
            ),
            NullDataException => (
                exception.Message,
                "Data Not Found",
                StatusCodes.Status404NotFound
            ),
            _ => (
                env.IsDevelopment() ? exception.ToString() : "An unexpected error occurred.",
                "Internal Server Error",
                StatusCodes.Status500InternalServerError
            )
        };

        ProblemDetails problemDetails = new()
        {
            Title = title, Detail = detail, Status = statusCode, Instance = httpContext.Request.Path
        };

        problemDetails.Extensions["traceId"] = httpContext.TraceIdentifier;

        var response = env.IsDevelopment()
            ? JsonSerializer.Serialize(problemDetails, new JsonSerializerOptions { WriteIndented = true })
            : JsonSerializer.Serialize(new
            {
                problemDetails.Status, problemDetails.Title, TraceId = httpContext.TraceIdentifier
            });

        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = statusCode;

        await httpContext.Response.WriteAsync(response);
    }
}
