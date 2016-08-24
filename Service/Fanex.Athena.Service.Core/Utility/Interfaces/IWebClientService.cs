namespace Fanex.Athena.Service.Core.Utility.Interfaces
{
    using System;

    public interface IWebClientService
    {
        string SendRequest(Uri address, string parameters);
    }
}