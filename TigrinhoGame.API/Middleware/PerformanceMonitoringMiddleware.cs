using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace TigrinhoGame.API.Middleware
{
    public class PerformanceMonitoringMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<PerformanceMonitoringMiddleware> _logger;
        private const int WarningThresholdMs = 500;

        public PerformanceMonitoringMiddleware(RequestDelegate next, ILogger<PerformanceMonitoringMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();

            try
            {
                await _next(context);
            }
            finally
            {
                stopwatch.Stop();
                var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;

                // Add response header with timing information
                context.Response.Headers["X-Response-Time-Ms"] = elapsedMilliseconds.ToString();

                // Log warning for slow requests
                if (elapsedMilliseconds > WarningThresholdMs)
                {
                    _logger.LogWarning(
                        "Long running request: {Method} {Path} took {ElapsedMilliseconds}ms",
                        context.Request.Method,
                        context.Request.Path,
                        elapsedMilliseconds);
                }
                else
                {
                    _logger.LogInformation(
                        "Request completed: {Method} {Path} took {ElapsedMilliseconds}ms",
                        context.Request.Method,
                        context.Request.Path,
                        elapsedMilliseconds);
                }
            }
        }
    }

    public static class PerformanceMonitoringMiddlewareExtensions
    {
        public static IApplicationBuilder UsePerformanceMonitoring(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<PerformanceMonitoringMiddleware>();
        }
    }
} 