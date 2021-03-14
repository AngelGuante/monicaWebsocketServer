using System;
using monicaWebsocketServer;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace monicaWebsocketServer
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        public static List<ExceptionDTO> exceptions = new List<ExceptionDTO>();

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                exceptions.Add(new ExceptionDTO
                {
                    Ip = httpContext.Connection.RemoteIpAddress.ToString(),
                    Message = ex.ToString()
                });
            }
        }
    }
}