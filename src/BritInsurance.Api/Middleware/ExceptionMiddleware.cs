namespace BritInsurance.Api.Middleware
{
    public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            logger.LogError(exception, "An unexpected error occurred.");

            ExceptionMiddlewareResponse response = exception switch
            {
                UnauthorizedAccessException => new ExceptionMiddlewareResponse("Unauthorized access", 401),
                _ => new ExceptionMiddlewareResponse("An unexpected error occurred", 500)
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = response.StatusCode;
            await context.Response.WriteAsJsonAsync(response);
        }
    }

    public record ExceptionMiddlewareResponse
    {
        public string Message { get; init; }
        public int StatusCode { get; init; }

        public ExceptionMiddlewareResponse(string message, int statusCode)
        {
            Message = message;
            StatusCode = statusCode;
        }
    }
}