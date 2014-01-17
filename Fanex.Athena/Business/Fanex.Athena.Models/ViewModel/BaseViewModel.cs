// -----------------------------------------------------------------------
// <copyright file="BaseViewModel.cs" company="Nexcel">
// warehouse winloss form parameters
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.ViewModel
{
    using System.Collections.Generic;
    using Fanex.Athena.Models.DTO.Common;

    /// <summary>
    /// BaseViewModel class.
    /// </summary>
    public class BaseViewModel
    {
        public IList<string> LicenseeTypeList { get; set; }

        public bool IsAllowedCustomerCommission { get; set; }

        public ProductAccess ProductAccess { get; set; }

        public IDictionary<string, int> CurrencyList { get; set; }

        public bool ShowEmptyContent { get; set; }
    }
}