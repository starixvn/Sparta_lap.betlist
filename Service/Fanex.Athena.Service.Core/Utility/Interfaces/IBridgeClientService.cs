using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Fanex.Athena.Service.Core.Utility.Interfaces
{
    public interface IBridgeClientService
    {
        Task<TResult> PostAsJsonAsync<TParam, TResult>(Uri url, TParam value);
    }
}