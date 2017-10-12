namespace Fanex.Athena.Service.Core.Utility.Interfaces
{
    using System;

    public interface IWebClientService
    {
        string UploadString(Uri address, string parameters);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Sonar Code Smell", "S3994:UriParametersShouldNotBeStrings",
            Justification = "Reviewed. Suppression is OK here.")]
        string DownloadString(string url);
    }
}