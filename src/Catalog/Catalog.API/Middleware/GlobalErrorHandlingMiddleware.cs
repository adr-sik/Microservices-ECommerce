using System.Net;
using System.Security.Authentication;

namespace Catalog.API.Middleware
{
    public class GlobalErrorHandlingMiddleware
    {
        private readonly ILogger<GlobalErrorHandlingMiddleware> _logger;
        private readonly RequestDelegate _next;

        public GlobalErrorHandlingMiddleware(ILogger<GlobalErrorHandlingMiddleware> logger, RequestDelegate next)
        {
            _logger = logger;
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }
        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            HttpStatusCode status;
            string? stackTrace = string.Empty;
            string message;

            Type? exceptionType = exception.GetType();

            if (exceptionType == typeof(DirectoryNotFoundException) ||
                exceptionType == typeof(DllNotFoundException) ||
                exceptionType == typeof(EntryPointNotFoundException) ||
                exceptionType == typeof(FileNotFoundException) ||
                exceptionType == typeof(KeyNotFoundException))
            {
                message = exception.Message;
                status = HttpStatusCode.NotFound;
                stackTrace = exception.StackTrace;
            }
            else if (exceptionType == typeof(NotImplementedException))
            {
                status = HttpStatusCode.NotImplemented;
                message = exception.Message;
                stackTrace = exception.StackTrace;
            }
            else if (exceptionType == typeof(UnauthorizedAccessException) ||
                exceptionType == typeof(AuthenticationException))
            {
                status = HttpStatusCode.Unauthorized;
                message = exception.Message;
                stackTrace = exception.StackTrace;
            }
            else
            {
                status = HttpStatusCode.InternalServerError;
                message = exception.Message;
                stackTrace = exception.StackTrace;
            }

            string exceptionResult = System.Text.Json.JsonSerializer.Serialize(new { error = message, stackTrace });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)status;

            return context.Response.WriteAsync(exceptionResult);
        }
    }
}
