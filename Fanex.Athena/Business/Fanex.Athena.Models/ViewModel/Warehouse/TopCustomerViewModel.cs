// -----------------------------------------------------------------------
// <copyright file="TopCustomerViewModel.cs" company="Nexcel">
// warehouse winloss form parameters
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.ViewModel.Warehouse
{
    using System.Collections.Generic;
    using Fanex.Athena.Models.DTO.Common;
    using Fanex.Athena.Models.DTO.Warehouse;
    using Fanex.Athena.Models.MetaModel.Warehouse;

    /// <summary>
    /// TopCustomerViewModel class.
    /// </summary>
    public class TopCustomerViewModel : BaseViewModel
    {
        public TopCustomerMetaModel Param { get; set; }

        public IList<TopCustomer> MainResult { get; set; }

        public IDictionary<string, decimal> TotalValues { get; set; }

        public IList<Sport> SportList { get; set; }
    }
}