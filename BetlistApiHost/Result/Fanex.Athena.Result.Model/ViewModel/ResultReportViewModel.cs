// -----------------------------------------------------------------------
// <copyright file="ResultReportViewModel.cs" company="">
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Result.Model.ViewModel
{
    using System.Collections.Generic;
    using MetaModel;

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