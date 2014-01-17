// -----------------------------------------------------------------------
// <copyright file="CustomerPendingFundsViewModel.cs" company="Nexcel">
// warehouse winloss form parameters
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.ViewModel.Report
{
    using System.Collections.Generic;
    using Fanex.Athena.Models.DTO.Report;
    using Fanex.Athena.Models.MetaModel.Report;

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