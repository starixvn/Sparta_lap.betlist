// -----------------------------------------------------------------------
// <copyright file="CustomerPendingFundsViewModel.cs" company="Nexcel">
// warehouse winloss form parameters
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Report.ViewModel
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Model.Core.ViewModel;

    /// <summary>
    /// CustomerPendingFundsViewModel class.
    /// </summary>
    public class CustomerPendingFundsViewModel : BaseViewModel
    {
        public IList<CustPendingFunds> DataList { get; set; }

        public CustPendingFunds TotalData { get; set; }

        public CustomerPendingFundsMetaModel Param { get; set; }

        public int TotalRows { get; set; }
    }
}