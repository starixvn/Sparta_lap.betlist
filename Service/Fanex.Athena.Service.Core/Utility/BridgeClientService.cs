using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Fanex.Athena.Service.Core.Utility.Interfaces;
using Newtonsoft.Json;

namespace Fanex.Athena.Service.Core.Utility
{
    public class BridgeClientService : IBridgeClientService
    {
        public async Task<HttpResponseMessage> PostAsync(Uri url, object value)
        {
            using (var client = new HttpClient())
            {
                var stringContent = new StringContent(JsonConvert.SerializeObject(value), Encoding.UTF8, "application/json");
                var response = await client.PostAsync(url, stringContent).ConfigureAwait(false);

              //  response.EnsureSuccessStatusCode();

                return response;
            }
        }

        public async Task<T> PostAsync<T>(Uri url, object value)
        {
            using (var client = new HttpClient())
            {
                var response = await client.PostAsJsonAsync(url, value).ConfigureAwait(false);

                // response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                return JsonConvert.DeserializeObject<T>(json);
            }
        }

        public async Task<Stream> PostAsyncStream(Uri url, object value)
        {
            using (var client = new HttpClient())
            {
                var response = await client.PostAsJsonAsync(url, value).ConfigureAwait(false);
           //     response.EnsureSuccessStatusCode();

                return await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
            }
        }

        public async Task<string> PostAsyncString(Uri url, object value)
        {
            using (var client = new HttpClient())
            {
                var stringContent = new StringContent(JsonConvert.SerializeObject(value), Encoding.UTF8, "application/json");
                var response = await client.PostAsync(url, stringContent).ConfigureAwait(false);

                //  response.EnsureSuccessStatusCode();

                return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            }
        }
    }
}