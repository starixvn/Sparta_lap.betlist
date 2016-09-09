namespace Fanex.Athena.Service.Core.Utility
{
    using System;
    using System.Net;
    using Interfaces;

    public class WebClientService : IWebClientService
    {
        public string SendRequest(Uri address, string parameters)
        {
            using (WebClient webClient = new WebClient())
            {
                webClient.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                return webClient.UploadString(address, parameters);
            }
        }
    }
}