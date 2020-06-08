using System.Net.Http;
using System.Threading.Tasks;

namespace monicaWebsocketServer.Utils
{
    public class RequestsHTTP
    {
        public static async Task POST(string path, StringContent content)
        {
            using (var client = new HttpClient())
            using (var response = await client.PostAsync(path, content)) { }
        }
    }
}