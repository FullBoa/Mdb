using System;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using log4net;
using Microsoft.AspNetCore.Http;

namespace Mdb.Midlewares.Logging
{
    /// <summary>
    /// Middleware for logging request, result code and measuring request processing time.
    /// </summary>
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestLoggingMiddleware(RequestDelegate next)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public async Task InvokeAsync(HttpContext httpContext, ILog logger)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            try
            {
                await _next.Invoke(httpContext);
            }
            catch (Exception e)
            {
                logger.Error($"Error: {e}");
                httpContext.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
            }

            stopwatch.Stop();
            var builder = new StringBuilder();

            builder.Append($"Request: {httpContext.Request.Method}");
            builder.Append($" {httpContext.Request.Path}{httpContext.Request.QueryString}");
            builder.Append($" {httpContext.Response.StatusCode}");
            builder.Append($" in {stopwatch.Elapsed}");

            logger.Info(builder.ToString());
        }
    }
}