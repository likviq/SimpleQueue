using SimpleQueue.Domain.Interfaces;

namespace SimpleQueue.WebUI.Middlewares
{
    public class ExceptionHandlingException : IMiddleware
    {
        private readonly ILoggerManager _logger;
        public ExceptionHandlingException(ILoggerManager logger)
        {
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the action {ex}");
                context.Response.StatusCode = 500;
                await context.Response.WriteAsync(ex.Message);
            }
        }
    }
}
