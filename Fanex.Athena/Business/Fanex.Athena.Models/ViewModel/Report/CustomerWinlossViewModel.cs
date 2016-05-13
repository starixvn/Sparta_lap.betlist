// -----------------------------------------------------------------------
// <copyright file="CustomerWinLossViewModel.cs" company="Nexcel">
// warehouse winloss form parameters
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.ViewModel.Report
{
    using System.Collections.Generic;
    using Fanex.Athena.Models.DTO.Report;
    using Fanex.Athena.Models.MetaModel.Report;
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