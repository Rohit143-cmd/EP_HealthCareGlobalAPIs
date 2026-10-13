using Microsoft.AspNetCore.Diagnostics;

namespace DbDemoProject.Helper
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(
            ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            _logger.LogError(
                exception,
                "An unhandled exception occurred. TraceId: {TraceId}",
                httpContext.TraceIdentifier);

            if (httpContext.Response.HasStarted)
            {
                return false;
            }

            var errorResponse = new
            {
                StatusCode = 500,
                Message = "An unexpected error occurred.",
                TraceId = httpContext.TraceIdentifier
            };

            httpContext.Response.StatusCode = 500;
            httpContext.Response.ContentType = "application/json";

            await httpContext.Response.WriteAsJsonAsync(
                errorResponse,
                cancellationToken);

            return true;
        }
    }
}
