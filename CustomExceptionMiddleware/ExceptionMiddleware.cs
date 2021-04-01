using System;
using monicaWebsocketServer;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using static monicaWebsocketServer.Clients;

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
                if(ex.Message != "he remote party closed the WebSocket connection without completing the close handshake.")
                    await Clients.RemoveClient(httpContext.Connection.RemoteIpAddress.ToString());
                else
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
}