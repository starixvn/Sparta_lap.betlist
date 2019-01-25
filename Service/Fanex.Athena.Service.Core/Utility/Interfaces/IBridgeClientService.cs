using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Fanex.Athena.Service.Core.Utility.Interfaces
{
    public interface IBridgeClientService
    {
        Task<HttpResponseMessage> PostAsync(Uri url, object value);
    }
}