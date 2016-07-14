// -----------------------------------------------------------------------
// <copyright file="ResultReportViewModel.cs" company="">
// warehouse winloss form parameters
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Report.ViewModel
{
    using System.Collections.Generic;

    using Fanex.Athena.Model.Report.MetaModel;

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