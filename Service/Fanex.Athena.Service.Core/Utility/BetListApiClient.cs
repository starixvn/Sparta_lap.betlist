using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Fanex.Athena.Extension.Utility;
using Fanex.Athena.Service.Core.Utility.Interfaces;
using Newtonsoft.Json;

namespace Fanex.Athena.Service.Core.Utility
{
    public class BetListApiClient : IBetListApiClient
    {
        private const string KEYNAME = "BETLIST_TOKEN_KEY";

        private readonly IAppSettings _appSetting;

        public BetListApiClient(IAppSettings appSettings)
        {
            _appSetting = appSettings;
        }

        public async Task<HttpResponseMessage> PostAsync(string path, object value)
        {
            using (var client = new HttpClient())
            {
                AddHeaderToken(client);
                var stringContent = new StringContent(JsonConvert.SerializeObject(value), Encoding.UTF8, "application/json");
                var response = await client.PostAsync(GetFullUri(path), stringContent).ConfigureAwait(false);

                ////  response.EnsureSuccessStatusCode();

                return response;
            }
        }

        public async Task<T> PostAsync<T>(string path, object value)
        {
            using (var client = new HttpClient())
            {
                AddHeaderToken(client);
                var stringContent = new StringContent(JsonConvert.SerializeObject(value), Encoding.UTF8, "application/json");
                var response = await client.PostAsync(GetFullUri(path), stringContent).ConfigureAwait(false);

                ////   response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                return JsonConvert.DeserializeObject<T>(json);
            }
        }

        public async Task<Stream> PostAsyncStream(string path, object value)
        {
            using (var client = new HttpClient())
            {
                AddHeaderToken(client);
                var stringContent = new StringContent(JsonConvert.SerializeObject(value), Encoding.UTF8, "application/json");
                var response = await client.PostAsync(GetFullUri(path), stringContent).ConfigureAwait(false);
                ////   response.EnsureSuccessStatusCode();

                return await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
            }
        }

        public async Task<string> PostAsyncString(string path, object value)
        {
            using (var client = new HttpClient())
            {
                AddHeaderToken(client);
                var stringContent = new StringContent(JsonConvert.SerializeObject(value), Encoding.UTF8, "application/json");
                var response = await client.PostAsync(GetFullUri(path), stringContent).ConfigureAwait(false);

                ////     response.EnsureSuccessStatusCode();

                return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            }
        }

        private Uri GetFullUri(string path)
        {
            return new Uri(_appSetting.BetListApiHostUrl + path);
        }

        private void AddHeaderToken(HttpClient client)
        {
            client.DefaultRequestHeaders.Add(KEYNAME, _appSetting.GetValue("BetListApiTokenValue"));
        }
    }
}