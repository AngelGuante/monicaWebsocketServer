using monicaWebsocketServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using System.Net;

namespace monicaWebsocketServer
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureCustomExceptionMiddleware(this IApplicationBuilder app) =>
            app.UseMiddleware<ExceptionMiddleware>();
    }
}