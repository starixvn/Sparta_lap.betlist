// -----------------------------------------------------------------------
// <copyright file="LicenseeModel.cs" company="">
// obj:licensee
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.Licensee
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [Serializable]
    public class LicenseeModel
    {
        /// <summary>
        /// Gets or sets LicenseeId.
        /// </summary>
        public int LicenseeID { get; set; }

        /// <summary>
        /// Gets or sets LicenseeName.
        /// </summary>
        public string LicenseeName { get; set; }
    }
}
