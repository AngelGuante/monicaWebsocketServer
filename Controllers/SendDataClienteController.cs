using Microsoft.AspNetCore.Mvc;
using static monicaWebsocketServer.WSController;
using static monicaWebsocketServer.Clients;

namespace monicaWebsocketServer
{

    [ApiController]
    [Route("API/SendDataClient")]
    public class SendDataClientController : ControllerBase
    {
        [HttpGet]
        [Route("SendToClient")]
        public bool SendToClient(string IP, string data)
        {
            var websocket = GetClientByIP(IP);
            if (websocket != default)
            {
                Send(websocket, data);
                return true;
            }

            return false;
        }

        [HttpGet]
        [Route("GetClients")]
        public JsonResult GetClients() =>
         Clients.GetClients();
    }
}