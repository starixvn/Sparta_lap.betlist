// -----------------------------------------------------------------------
// <copyright file="UserSession.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Core.DTO.Users
{
    using System;
    using System.Collections.Generic;
    using Fanex.Athena.Model.Core.DataDomain;

    /// <summary>
    /// UserSession class.
    /// </summary>
    [Serializable]
    public class UserSession
    {
        public string VendorID { get; set; }

        public string VendorIdentityID { get; set; }

        public int UserId { get; set; }

        public string UserName { get; set; }

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

        /// <summary>
        /// Gets or sets function List of current licensee.
        /// </summary>
        /// <value>The currency List.</value>
        public IList<int> FunctionIdList { get; set; }

        public IList<PortalSystemFunction> PortalFunctionList { get; set; }

        public IDictionary<string, int> CurrencyList { get; set; }

        public string DefaultReportUrl { get; set; }

        public ClientViewType ViewType { get; set; }

        public AppMode ApplicationMode { get; set; }
        public string HostDomain { get; set; }
    }
}