namespace Fanex.Athena.Common.Utility.Services
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using Fanex.Athena.Common.Utility.Serializer;
    using Interfaces;

    public class WebClientService : IWebClientService
    {
        private readonly ISerializer _serializer;

        public WebClientService(ISerializer serializer)
        {
            _serializer = serializer;
        }

        public string UploadString(Uri address, string parameters)
        {
            using (WebClient webClient = new WebClient())
            {
                webClient.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                return webClient.UploadString(address, parameters);
            }
        }

        public string DownloadString(string address)
        {
            using (WebClient webClient = new WebClient())
            {
                webClient.Headers[HttpRequestHeader.ContentType] = "application/json; charset=utf-8";
                return webClient.DownloadString(address);
            }
        }

        public byte[] DownloadData(string address)
        {
            using (WebClient webClient = new WebClient())
            {
                return webClient.DownloadData(address);
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

        public TOut UploadString<TIn, TOut>(string address, TIn requestData)
        {
            using (WebClient webClient = new WebClient())
            {
                webClient.Headers[HttpRequestHeader.ContentType] = "application/json";
                var serializedRequestData = _serializer.Serialize(requestData);
                var response = webClient.UploadString(address, "POST", serializedRequestData);

                return _serializer.Deserialize<TOut>(response);
            }
        }
    }
}