// -----------------------------------------------------------------------
// <copyright file="ActiveCustomerViewModel.cs" company="Microsoft">
// Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.ViewModel.Report
{
    using System.Collections.Generic;
    using Fanex.Athena.Models.DTO.Report;
    using Fanex.Athena.Models.MetaModel.Report;
    using Model.Core.ViewModel;

    /// <summary>
    /// ActiveCustomerViewModel class.
    /// </summary>
    public class ActiveCustomerViewModel : BaseViewModel
    {
        public IList<ActiveCustomer> MainResult { get; set; }

        public int TotalCustomer { get; set; }

        public ActiveCustomerMetaModel Param { get; set; }
    }
}