using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Fanex.Athena.Service.Core.Utility.Interfaces
{
    public interface IBridgeClientService
    {
        Task<HttpResponseMessage> PostAsync(Uri url, object value);

        Task<Stream> PostAsyncStream(Uri url, object value);

        Task<string> PostAsyncString(Uri url, object value);
    }
}