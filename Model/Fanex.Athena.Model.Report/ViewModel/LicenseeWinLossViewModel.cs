// -----------------------------------------------------------------------
// <copyright file="LicenseeWinLossViewModel.cs" company="Microsoft">
// Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Report.ViewModel
{
    using System;
    using System.Collections.Generic;
    using Core.DTO.Configuration;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Model.Core.ViewModel;

    /// <summary>
    /// LicenseeWinLossViewModel class.
    /// </summary>
    public class LicenseeWinLossViewModel : BaseViewModel
    {
        public IList<LicenseeWinLoss> MainResult { get; set; }

        public IDictionary<string, LicenseeWinLoss> SummaryTotal { get; set; }

        public LicenseeWinLossMetaModel Param { get; set; }

        public ReportSetting ReportSetting { get; set; }
    }
}