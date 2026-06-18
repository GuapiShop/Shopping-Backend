using API_Shopping.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace API_Shopping.Middlewares
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            _logger.LogError(exception, "Unhandled exception on {Path}", httpContext.Request.Path);

            var (statusCode, title, detail) = exception switch
            {
                AppException appEx => (appEx.StatusCode, appEx.Title, appEx.Message),
                DbUpdateException => (StatusCodes.Status500InternalServerError, "Database update failed", "A database update error occurred."),
                SqlException => (StatusCodes.Status500InternalServerError, "Database error", "A database error occurred."),
                _ => (StatusCodes.Status500InternalServerError, "An unexpected error occurred", "Something went wrong. Please try again later.")
            };

            httpContext.Response.StatusCode = statusCode;

            await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Detail = detail,
                Instance = httpContext.Request.Path
            }, cancellationToken);

            return true;
        }
    }
}