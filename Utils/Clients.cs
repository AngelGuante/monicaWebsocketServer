using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
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

        public async static Task<JsonResult> RemoveClient(string IP)
        {
            clients.TryGetValue(IP, out WebSocket ws);
            if (ws != default && ws.State == WebSocketState.Open)
                await ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "", new CancellationToken());
            clients.Remove(IP);
            return new JsonResult(new { });
        }

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