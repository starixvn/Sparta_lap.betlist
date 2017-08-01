// -----------------------------------------------------------------------
// <copyright file="KenoResultMetalModel.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Report.MetaModel.Keno
{
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class KenoResultMetalModel
    {
        [SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings", Justification = "Reviewed. Suppression is OK here.")]
        public string ResultUrl { get; set; }

        public string SiteID { get; set; }

        public string Vendor { get; set; }

        public string VenderKey { get; set; }

        [SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings", Justification = "Reviewed. Suppression is OK here.")]
        public string AuthorizedUrl { get; set; }

        [SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings", Justification = "Reviewed. Suppression is OK here.")]
        public string ExpireTokenUrl { get; set; }

        [SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings", Justification = "Reviewed. Suppression is OK here.")]
        public string ExtendTokenUrl { get; set; }

        [SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings", Justification = "Reviewed. Suppression is OK here.")]
        public string KickOutUrl { get; set; }

        public string Language { get; set; }

        public int VendorProduct { get; set; }
    }
}
