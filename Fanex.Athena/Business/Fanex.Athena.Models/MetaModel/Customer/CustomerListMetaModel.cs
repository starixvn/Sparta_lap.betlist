// -----------------------------------------------------------------------
// <copyright file="CustomerListMetaModel.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.MetaModel.Customer
{
    using System;
    using System.Collections.Generic;
    using Fanex.Athena.Models.DTO.Common;

    /// <summary>
    /// CustomerListMetaModel class.
    /// </summary>
    [Serializable]
    public class CustomerListMetaModel : BaseMetaModel
    {
        public CustomerListMetaModel()
        {
            LicenseeId = 1;
            LicenseeType = null;
            Username = string.Empty;
            PageSize = 1000;
            PageIndex = 1;
            BAFilter = 0;
            CustStatus = string.Empty;
            SpecialLevel = -1;
            GroupCommId = 0;
            AllowLicensees = new List<string>();
            OddsGroups = new List<string> { "A", "B", "C", "D" };
        }

        public string Username { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public int BAFilter { get; set; }

        public string CustStatus { get; set; }

        /// <summary>
        /// Gets or sets Special level.
        /// -1: All level.
        /// </summary>
        /// <value>The special level .</value>
        public int SpecialLevel { get; set; }

        public int GroupCommId { get; set; }

        public IList<string> AllowLicensees { get; set; }

        public IList<CheckedItem> GridColumns { get; set; }

        public IList<string> OddsGroups { get; set; }
    }
}