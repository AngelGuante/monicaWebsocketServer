using Microsoft.AspNetCore.Mvc;
using static monicaWebsocketServer.WSController;
using static monicaWebsocketServer.Clients;

namespace monicaWebsocketServer
{
    [ApiController]
    [Route("API/SendDataClient")]
    public class SendDataClientController : ControllerBase
    {
        [HttpPost]
        [Route("SendToClient")]
        [Route("SendToClient/{IP}")]
        public bool SendToClient(WebSocketDTO data, string IP)
        {
            var websocket = GetClientByIP(IP);
            if (websocket != default)
            {
                Respond(websocket, data.data);
                return true;
            }

            return false;
        }

        [HttpGet]
        [Route("GetClients")]
        public JsonResult GetClients() =>
            Clients.GetClients();

        [HttpGet]
        [Route("Exceptions")]
        public JsonResult Exceptions(bool clear = false) =>
            Clients.Exceptions(clear);
        
        [HttpGet]
        [Route("SetException")]
        public JsonResult SetException(){
            throw new System.Exception("Exception throwed");
        }
    }
}