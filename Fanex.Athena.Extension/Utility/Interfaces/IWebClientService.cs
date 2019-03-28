namespace Fanex.Athena.Extension.Utility.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Threading.Tasks;

    public interface IWebClientService
    {
        string UploadString(Uri address, string parameters);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Sonar Code Smell", "S3994:UriParametersShouldNotBeStrings",
            Justification = "Reviewed. Suppression is OK here.")]
        string DownloadString(string url);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Sonar Code Smell", "S3994:UriParametersShouldNotBeStrings",
            Justification = "Reviewed. Suppression is OK here.")]
        byte[] DownloadData(string url);

        string UploadString(Uri address, string parameters, IDictionary<HttpRequestHeader, string> headers);

        Task<string> UploadStringAsync(Uri address, string parameters);
    }
}