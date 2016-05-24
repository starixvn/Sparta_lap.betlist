// -----------------------------------------------------------------------
// <copyright file="TopCustomerViewModel.cs" company="Nexcel">
// warehouse winloss form parameters
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Warehouse.ViewModel
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Model.Warehouse.DTO;
    using Fanex.Athena.Model.Warehouse.MetaModel;
    using Model.Core.ViewModel;

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