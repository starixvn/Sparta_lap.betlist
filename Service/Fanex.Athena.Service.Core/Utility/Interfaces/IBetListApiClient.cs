using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Fanex.Athena.Service.Core.Utility.Interfaces
{
    public interface IBetListApiClient
    {
        Task<HttpResponseMessage> PostAsync(string path, object value, string language);

        Task<T> PostAsync<T>(string path, object value, string language);

        Task<Stream> PostAsyncStream(string path, object value, string language);

        Task<string> PostAsyncString(string path, object value, string language);
    }
}