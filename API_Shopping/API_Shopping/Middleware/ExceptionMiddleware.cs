using System.Net;
using System.Text.Json;
using API_Shopping.Exceptions;
namespace API_Shopping.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ProductNotFoundException ex)
            {
                await HandleException(context, HttpStatusCode.NotFound, "PRODUCT_NOT_FOUND", ex.Message);
            }
            catch (OutOfStockException ex)
            {
                await HandleException(context, HttpStatusCode.Conflict, "OUT_OF_STOCK", ex.Message);
            }
            catch (Exception ex)
            {
                await HandleException(context, HttpStatusCode.InternalServerError, "INTERNAL_ERROR", "Unexpected error");
            }
        }

        private static async Task HandleException(
            HttpContext context,
            HttpStatusCode status,
            string code,
            string message)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)status;

            var response = new
            {
                code,
                message
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
