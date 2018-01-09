namespace Fanex.Athena.Service.Core.Utility.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Net;

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
    }
}