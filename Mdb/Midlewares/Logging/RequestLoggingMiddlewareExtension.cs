using System;
using Microsoft.AspNetCore.Builder;

namespace Mdb.Midlewares.Logging
{
    /// <summary>
    /// Extension for connection to ASP.Net Core app.
    /// </summary>
    public static class RequestLoggingMiddlewareExtension
    {
        public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder app)
        {
            if (app == null)
                throw new ArgumentNullException(nameof(app));
            return app.UseMiddleware<RequestLoggingMiddleware>();
        }
    }
}