using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace monicaWebsocketServer
{
    public class WSController
    {
        protected static Dictionary<string, WebSocket> _clients = new Dictionary<string, WebSocket>();

        public static async Task Listen(HttpContext context, WebSocket webSocket)
        {
            if (_clients.ContainsKey(context.Connection.RemoteIpAddress.ToString()))
                Respond(webSocket, "IP DUPLICADA.", ClientMessageStatus.DuplicatedIP);
            else
            {
                _clients.Add(context.Connection.RemoteIpAddress.ToString(), webSocket);
                Send(webSocket, $"WELCOME! {context.Connection.Id}");
                Send(webSocket, $"Your IP: {context.Connection.RemoteIpAddress}");
            }

            while (true)
            {
                WebSocketReceiveResult result;
                ArraySegment<byte> message = new ArraySegment<byte>(new byte[4096]);

                do
                {
                    result = await webSocket.ReceiveAsync(message, CancellationToken.None);
                    byte[] messageBytes = message.Skip(message.Offset).Take(result.Count).ToArray();
                    string serialisedMessage = Encoding.UTF8.GetString(messageBytes);

                    if (!webSocket.CloseStatus.HasValue)
                    {
                        _clients.TryGetValue(serialisedMessage, out WebSocket finded);

                        if (finded != null)
                            Respond(finded, "Ip encontrada", ClientMessageStatus.Correct);
                        else
                            Respond(webSocket, "Ip NO encontrada", ClientMessageStatus.Correct);
                    }
                    else
                        _clients.Remove(context.Connection.RemoteIpAddress.ToString());
                } while (!result.EndOfMessage);
            }
        }

        public static async void Send(WebSocket webSocket, string message) =>
            await _sendMessage(webSocket, message);

        public static async void Respond(WebSocket webSocket, string message, ClientMessageStatus status) =>
            await _sendMessage(webSocket, message, status);

        private static async Task _sendMessage(WebSocket webSocket, string message, ClientMessageStatus status = 0)
        {
            string serialisedMessage = message;

            if (status != 0)
                serialisedMessage = $"{(int)status}<{message}";

            var byteMessage = Encoding.UTF8.GetBytes(serialisedMessage);
            var segmnet = new ArraySegment<byte>(byteMessage, 0, byteMessage.Length);
            await webSocket.SendAsync(segmnet, WebSocketMessageType.Text, true, CancellationToken.None);
        }
    }
}