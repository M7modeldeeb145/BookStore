using System.Net;
using System.Text.Json;

namespace Task_Manager.API.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IWebHostEnvironment _env;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IWebHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception");
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                if (_env.IsDevelopment())
                {
                    var resultDev = JsonSerializer.Serialize(new { error = ex.Message, stackTrace = ex.StackTrace });
                    await context.Response.WriteAsync(resultDev);
                }
                else
                {
                    var result = JsonSerializer.Serialize(new { error = "An unexpected error occurred." });
                    await context.Response.WriteAsync(result);
                }
            }
        }
    }
}
