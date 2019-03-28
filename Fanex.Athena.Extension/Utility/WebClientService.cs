namespace Fanex.Athena.Extension.Utility
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Threading.Tasks;
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

        public string UploadString(Uri address, string parameters, IDictionary<HttpRequestHeader, string> headers)
        {
            using (WebClient webClient = new WebClient())
            {
                if (headers != null)
                {
                    foreach (var header in headers)
                    {
                        webClient.Headers[header.Key] = header.Value;
                    }
                }
              
                return webClient.UploadString(address, parameters);
            }
        }

        public async Task<string> UploadStringAsync(Uri address, string parameters)
        {
            using (WebClient webClient = new WebClient())
            {
                webClient.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                return await webClient.UploadStringTaskAsync(address, parameters);
            }
        }
    }
}