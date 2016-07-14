// -----------------------------------------------------------------------
// <copyright file="ActiveCustomerViewModel.cs" company="Microsoft">
// Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Report.ViewModel
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
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