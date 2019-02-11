// -----------------------------------------------------------------------
namespace Fanex.Athena.Result.Model.MetaModel.Keno
{
    using System.Diagnostics.CodeAnalysis;

    public class KenoResultMetaModel
    {
        [SuppressMessage("Sonar Code Smell", "S3996:UriPropertiesShouldNotBeStrings", Justification = "Reviewed. Suppression is OK here.")]
        public string ResultUrl { get; set; }

        public string SiteID { get; set; }

        public string Vendor { get; set; }

        public string VenderKey { get; set; }

        [SuppressMessage("Sonar Code Smell", "S3996:UriPropertiesShouldNotBeStrings", Justification = "Reviewed. Suppression is OK here.")]
        public string AuthorizedUrl { get; set; }

        [SuppressMessage("Sonar Code Smell", "S3996:UriPropertiesShouldNotBeStrings", Justification = "Reviewed. Suppression is OK here.")]
        public string ExpireTokenUrl { get; set; }

        [SuppressMessage("Sonar Code Smell", "S3996:UriPropertiesShouldNotBeStrings", Justification = "Reviewed. Suppression is OK here.")]
        public string ExtendTokenUrl { get; set; }

        [SuppressMessage("Sonar Code Smell", "S3996:UriPropertiesShouldNotBeStrings", Justification = "Reviewed. Suppression is OK here.")]
        public string KickOutUrl { get; set; }

        public string Language { get; set; }

        public int VendorProduct { get; set; }
    }
}
