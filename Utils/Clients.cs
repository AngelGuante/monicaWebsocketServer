using System.Linq;
using System.Net.WebSockets;
using Microsoft.AspNetCore.Mvc;
using static monicaWebsocketServer.ExceptionMiddleware;

namespace monicaWebsocketServer
{
    class Clients : WSController
    {
        public static WebSocket GetClientByIP(string IP)
        {
            clients.TryGetValue(IP, out WebSocket webSocket);
            return webSocket;
        }

        public static JsonResult GetClients() =>
            new JsonResult(new { IPs = clients.Keys });

        public static JsonResult Exceptions(bool clear = false)
        {
            if (!clear)
                return new JsonResult(new { ServerRunSinced, Exceptions = exceptions.ToList() });
            else
            {
                exceptions = new System.Collections.Generic.List<ExceptionDTO>();
                return new JsonResult(new { status = true });
            }
        }
    }
}