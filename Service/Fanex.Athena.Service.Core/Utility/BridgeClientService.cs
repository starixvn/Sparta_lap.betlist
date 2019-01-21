using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Fanex.Athena.Extension.Utility;
using Fanex.Athena.Service.Core.Utility.Interfaces;
using Newtonsoft.Json;

namespace Fanex.Athena.Service.Core.Utility
{
    public class BridgeClientService : IBridgeClientService
    {
        public async Task<TResult> PostAsJsonAsync<TParam, TResult>(Uri url, TParam value)
        {
            using (var client = new HttpClient())
            {
                var response = await client.PostAsJsonAsync(url, value).ConfigureAwait(false);

                response.EnsureSuccessStatusCode();

                return await response.Content.ReadAsAsync<TResult>().ConfigureAwait(false);
            }
        }
    }
}
