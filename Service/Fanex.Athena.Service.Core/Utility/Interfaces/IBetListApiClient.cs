using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Fanex.Athena.Service.Core.Utility.Interfaces
{
    public interface IBetListApiClient
    {
        Task<HttpResponseMessage> PostAsync(string path, object value);

        Task<T> PostAsync<T>(string path, object value);

        Task<Stream> PostAsyncStream(string path, object value);

        Task<string> PostAsyncString(string path, object value);
    }
}