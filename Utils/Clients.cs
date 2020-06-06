using System.Net.WebSockets;
using Microsoft.AspNetCore.Mvc;

namespace monicaWebsocketServer
{
    class Clients : WSController
    {
        public static WebSocket GetClientByIP(string IP)
        {
            _clients.TryGetValue(IP, out WebSocket webSocket);
            return webSocket;
        }

        public static JsonResult GetClients() =>
         new JsonResult(new { IPs = _clients.Keys });
    }
}