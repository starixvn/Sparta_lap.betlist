namespace Fanex.Athena.Service.Core.Utility
{
    using System;
    using System.IO;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Service.Core.Utility.Interfaces;
    using Fanex.Athena.Session.Interfaces;
    using Newtonsoft.Json;

    public class BetListApiClient : IBetListApiClient
    {
        private const string KEYNAME = "BETLIST_TOKEN_KEY";

        private const string LANGUAGEKEY = "Language";

        private readonly IAppSettings _appSetting;

        private readonly ISessionStorage _sessionStorage;

        public BetListApiClient(IAppSettings appSettings, ISessionStorage sessionStorage)
        {
            _appSetting = appSettings;
            _sessionStorage = sessionStorage;
        }

        public async Task<HttpResponseMessage> PostAsync(string path, object value)
        {
            using (var client = CreateClient())
            {
                var stringContent = new StringContent(JsonConvert.SerializeObject(value), Encoding.UTF8, "application/json");
                var response = await client.PostAsync(GetFullUri(path), stringContent).ConfigureAwait(false);

                await EnsureSuccessStatusCodeAsync(response).ConfigureAwait(false);

                return response;
            }
        }

        public async Task<T> PostAsync<T>(string path, object value)
        {
            using (var client = CreateClient())
            {
                var stringContent = new StringContent(JsonConvert.SerializeObject(value), Encoding.UTF8, "application/json");
                var response = await client.PostAsync(GetFullUri(path), stringContent).ConfigureAwait(false);

                await EnsureSuccessStatusCodeAsync(response).ConfigureAwait(false);

                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                return JsonConvert.DeserializeObject<T>(json);
            }
        }

        public async Task<Stream> PostAsyncStream(string path, object value)
        {
            using (var client = CreateClient())
            {
                var stringContent = new StringContent(JsonConvert.SerializeObject(value), Encoding.UTF8, "application/json");
                var response = await client.PostAsync(GetFullUri(path), stringContent).ConfigureAwait(false);

                await EnsureSuccessStatusCodeAsync(response).ConfigureAwait(false);

                return await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
            }
        }

        public async Task<string> PostAsyncString(string path, object value)
        {
            using (var client = CreateClient())
            {
                var stringContent = new StringContent(JsonConvert.SerializeObject(value), Encoding.UTF8, "application/json");
                var response = await client.PostAsync(GetFullUri(path), stringContent).ConfigureAwait(false);

                await EnsureSuccessStatusCodeAsync(response).ConfigureAwait(false);

                return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            }
        }

        private static async Task EnsureSuccessStatusCodeAsync(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                return;
            }

            var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (response.Content != null)
            {
                response.Content.Dispose();
            }

            throw new HttpException((int)response.StatusCode, content);
        }

        private Uri GetFullUri(string path)
        {
            return new Uri(_appSetting.BetListApiHostUrl + path);
        }

        private HttpClient CreateClient()
        {
            var client = new HttpClient();
            client.Timeout = TimeSpan.FromMinutes(1);
            client.DefaultRequestHeaders.Add(KEYNAME, _appSetting.GetValue("BetListApiTokenValue"));
            client.DefaultRequestHeaders.Add(LANGUAGEKEY, _sessionStorage.UserSession.Language);

            return client;
        }
    }
}