using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using static monicaWebsocketServer.Utils.RequestsHTTP;

namespace monicaWebsocketServer
{
    public class WSController
    {
        protected static Dictionary<string, WebSocket> _clients = new Dictionary<string, WebSocket>();
        private static Dictionary<string, string> _clientDataTMP = new Dictionary<string, string>();

        public static async Task Listen(HttpContext context, WebSocket webSocket)
        {
            try
            {
                if (!_clients.ContainsKey(context.Connection.RemoteIpAddress.ToString()))
                    _clients.Add(context.Connection.RemoteIpAddress.ToString(), webSocket);
                while (true)
                {
                    WebSocketReceiveResult result = null;
                    ArraySegment<byte> message = new ArraySegment<byte>(new byte[8192]);

                    do
                    {
                        if (!webSocket.CloseStatus.HasValue)
                        {
                            result = await webSocket.ReceiveAsync(message, CancellationToken.None);
                            byte[] messageBytes = message.Skip(message.Offset).Take(result.Count).ToArray();
                            string serialisedMessage = Encoding.UTF8.GetString(messageBytes);

                            if (_clientDataTMP.ContainsKey(context.Connection.Id))
                                _clientDataTMP[context.Connection.Id] += serialisedMessage;
                            else
                                _clientDataTMP.Add(context.Connection.Id, serialisedMessage);
                        }
                        else
                        {
                            _clients.Remove(context.Connection.RemoteIpAddress.ToString());
                            break;
                        }
                    } while (result != null && !result.EndOfMessage);

                    if (webSocket.State.ToString() == "CloseReceived"){
                        _clients.Remove(context.Connection.RemoteIpAddress.ToString());
                        break;
                    }

                    if (!webSocket.CloseStatus.HasValue)
                    {
                        await SendToServer(context, _clientDataTMP[context.Connection.Id]);
                        _clientDataTMP.Remove(context.Connection.Id);
                    }
                }
            }
            catch (Exception e)
            {
                await SendToServer(context, $"Error: {e.Message}");
            }
        }

        public static async void Respond(WebSocket webSocket, string message)
        {
            var byteMessage = Encoding.UTF8.GetBytes(message);
            var segmnet = new ArraySegment<byte>(byteMessage, 0, byteMessage.Length);
            await webSocket.SendAsync(segmnet, WebSocketMessageType.Text, true, CancellationToken.None);
        }

        public static async Task SendToServer(HttpContext context, string data)
        {
            var obj = new WebSocketDTO
            {
                data = data
            };
            var content = new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");

            // await POST($"https://localhost:44392/API/ReportesLocales/ReceiveDataFromWebSocketServer?IP={context.Connection.RemoteIpAddress}", content);
            await POST($"https://moniextra.com/API/ReportesLocales/ReceiveDataFromWebSocketServer?IP={context.Connection.RemoteIpAddress}", content);
        }
    }
}