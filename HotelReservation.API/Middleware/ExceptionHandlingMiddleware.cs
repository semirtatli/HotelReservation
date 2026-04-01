using FluentValidation;
using HotelReservation.Domain.Exceptions;
using System.Text.Json;

namespace HotelReservation.API.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var statusCode = exception switch
            {
                NotFoundException => StatusCodes.Status404NotFound,
                ValidationException => StatusCodes.Status400BadRequest,
                ArgumentException => StatusCodes.Status400BadRequest,
                _ => StatusCodes.Status500InternalServerError
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            var message = exception is ValidationException validationException
                ? string.Join(", ", validationException.Errors.Select(e => e.ErrorMessage))
                : exception.Message;

            var response = new { statusCode, message };
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
