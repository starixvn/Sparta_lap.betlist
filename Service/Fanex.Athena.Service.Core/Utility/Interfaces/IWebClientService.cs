namespace Fanex.Athena.Service.Core.Utility.Interfaces
{
    using System;

    public interface IWebClientService
    {
        string UploadString(Uri address, string parameters);
    }
}