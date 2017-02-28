// -----------------------------------------------------------------------
// <copyright file="BaseViewModel.cs" company="">
// warehouse winloss form parameters
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Core.ViewModel
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.Core.DTO.Common;

    public class BaseViewModel
    {
        public bool IsAllowedCustomerCommission { get; set; }

        public ProductAccess ProductAccess { get; set; }

        public IDictionary<int, string> CurrencyList { get; set; }

        public bool ShowEmptyContent { get; set; }
    }
}