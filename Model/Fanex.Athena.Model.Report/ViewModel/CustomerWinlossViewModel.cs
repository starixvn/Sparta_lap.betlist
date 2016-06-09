// -----------------------------------------------------------------------
// <copyright file="CustomerWinLossViewModel.cs" company="Nexcel">
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
    /// CustomerWinLossViewModel class.
    /// </summary>
    public class CustomerWinLossViewModel : BaseViewModel
    {
        public IList<CustomerWinLoss> DataList { get; set; }

        public CustomerWinLoss TotalData { get; set; }

        public CustomerWinLossMetaModel Param { get; set; }

        public CustomerWinLossHistoryMetaModel HistoryParam { get; set; }

        public int TotalRows { get; set; }

        public bool IsHistory { get; set; }
    }
}