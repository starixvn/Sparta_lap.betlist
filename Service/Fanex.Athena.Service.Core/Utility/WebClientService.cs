namespace Fanex.Athena.Service.Core.Utility
{
    using System;
    using System.Net;
    using Interfaces;

    public class WebClientService : IWebClientService
    {
        public string UploadString(Uri address, string parameters)
        {
            using (WebClient webClient = new WebClient())
            {
                webClient.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                return webClient.UploadString(address, parameters);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Sonar Code Smell", "S3994:UriParametersShouldNotBeStrings",
            Justification = "Reviewed. Suppression is OK here.")]
        public string DownloadString(string url)
        {
            using (WebClient webClient = new WebClient())
            {
                webClient.Headers[HttpRequestHeader.ContentType] = "application/json; charset=utf-8";
                return webClient.DownloadString(url);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Sonar Code Smell", "S3994:UriParametersShouldNotBeStrings",
            Justification = "Reviewed. Suppression is OK here.")]
        public byte[] DownloadData(string url)
        {
            using (WebClient webClient = new WebClient())
            {
                return webClient.DownloadData(url);
            }
        }
    }
}