// -----------------------------------------------------------------------
// <copyright file="CustomerListMetaModel.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Customer.MetaModel
{
    using System;
    using System.Collections.Generic;
    using Core.MetaModel;
    using Fanex.Athena.Model.Core.DTO.Common;

    [Serializable]
    public class CustomerListMetaModel : BaseMetaModel
    {
        public CustomerListMetaModel()
        {
            LicenseeId = 0;
            Username = string.Empty;
            PageSize = 1000;
            PageIndex = 1;
            BAFilter = 0;
            CustStatus = string.Empty;
            SpecialLevel = -1;
            GroupCommId = 0;
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

        public bool IsShowStatusDropdown { get; set; }

        public IList<CheckedItem> GridColumns { get; set; }

        public IEnumerable<CheckedItem> OddsGroups { get; set; }
    }
}