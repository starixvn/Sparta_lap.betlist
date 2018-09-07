// -----------------------------------------------------------------------
// <copyright file="UserSession.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Session.Model
{
    using System;
    using System.Collections.Generic;
    using Enum;

    [Serializable]
    public class UserSession
    {
        public string VendorID { get; set; }

        public string VendorIdentityID { get; set; }

        public int UserId { get; set; }

        public string UserName { get; set; }

        public byte UserLevel { get; set; }

        /// <summary>
        /// Gets or sets Language Code.
        /// Default: en-US.
        /// </summary>
        /// <value>The Language Code.</value>
        public string Language { get; set; }

        /// <summary>
        /// Gets or sets logged-in identity on OnUser database.
        /// Used for re-check in and kick out features.
        /// Return 0 if it is not available.
        /// </summary>
        /// <value>The userId.</value>
        public int OnUserId { get; set; }

        public bool IsInternal { get; set; }

        public IList<int> FunctionIdList { get; set; }

        public IList<string> PortalFunctionList { get; set; }

        public IDictionary<int, string> CurrencyList { get; set; }

        public IDictionary<int, IEnumerable<int>> CurrencyAgentList { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Sonar Code Smell", "S3996:UriPropertiesShouldNotBeStrings",
            Justification = "Reviewed. Suppression is OK here.")]
        public string DefaultReportUrl { get; set; }

        public ClientViewType ViewType { get; set; }

        public AppMode ApplicationMode { get; set; }
    }
}