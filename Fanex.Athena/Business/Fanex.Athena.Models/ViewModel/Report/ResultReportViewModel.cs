// -----------------------------------------------------------------------
// <copyright file="ResultReportViewModel.cs" company="Nexcel">
// warehouse winloss form parameters
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.ViewModel.Report
{
    using System.Collections.Generic;

    using Fanex.Athena.Models.MetaModel.Report;

    /// <summary>
    /// ResultReportViewModel class.
    /// </summary>
    public class ResultReportViewModel
    {
        public string Data { get; set; }

        public ResultMetalModel Param { get; set; }

        public Dictionary<int, string> Sports { get; set; }

        public Dictionary<int, string> Leagues { get; set; }
    }
}