using System.Net;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using SnowBuddies.Domain.Entities.Errors;
using SnowBuddies.Domain.Exceptions;

namespace SnowBuddies.Api.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (AppException ex)
            {
                _logger.LogInformation("Request {Method} {Path} failed with {StatusCode}: {Message}",
                    context.Request.Method, context.Request.Path, ex.StatusCode, ex.Message);
                await WriteErrorAsync(context, ex.StatusCode, ex.Message);
            }
            catch (DbUpdateException ex) when (ex.InnerException is PostgresException { SqlState: PostgresErrorCodes.UniqueViolation })
            {
                // Two requests raced past the service-level duplicate check; the unique index caught it.
                _logger.LogInformation("Unique constraint violation on {Method} {Path}", context.Request.Method, context.Request.Path);
                await WriteErrorAsync(context, (int)HttpStatusCode.Conflict, "Resource already exists");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception processing {Method} {Path}", context.Request.Method, context.Request.Path);
                await WriteErrorAsync(context, (int)HttpStatusCode.InternalServerError, "Internal Server Error");
            }
        }

        private static async Task WriteErrorAsync(HttpContext context, int statusCode, string message)
        {
            if (context.Response.HasStarted)
            {
                return;
            }

            context.Response.Clear();
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;
            await context.Response.WriteAsJsonAsync(new Error
            {
                StatusCode = statusCode,
                Message = message
            });
        }
    }
}
