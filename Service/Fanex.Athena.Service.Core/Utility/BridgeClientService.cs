using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Fanex.Athena.Service.Core.Utility.Interfaces;
using Newtonsoft.Json;

namespace Fanex.Athena.Service.Core.Utility
{
    public class BridgeClientService : IBridgeClientService
    {
        public async Task<HttpResponseMessage> PostAsync<TParam>(Uri url, TParam value)
        {
            using (var client = new HttpClient())
            {
                var stringContent = new StringContent(JsonConvert.SerializeObject(value), Encoding.UTF8, "application/json");
                var response = await client.PostAsync(url, stringContent).ConfigureAwait(false);

                response.EnsureSuccessStatusCode();

                return response;
            }
        }
    }
}