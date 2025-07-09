using Microsoft.AspNetCore.Mvc;

namespace APIs
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
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
                _logger.LogError(ex, "Unhandled exception occurred");

                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/problem+json";

                var problem = new ProblemDetails
                {
                    Status = 500,
                    Title = "An unexpected error occurred",
                    Detail = _env.IsDevelopment() ? ex.ToString() : "Please contact support.",
                    Instance = context.Request.Path
                };

                await context.Response.WriteAsJsonAsync(problem);
            }
        }
    }
}
