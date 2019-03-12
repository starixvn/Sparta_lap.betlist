namespace Fanex.Athena.Model.Report.ViewModel.Result
{
    using System.Collections.Generic;
    using MetaModel.Result;

    public class ResultReportViewModel
    {
        public string Data { get; set; }

        public ResultMetaModel Param { get; set; }

        public Dictionary<int, string> Sports { get; set; }

        public Dictionary<int, string> Leagues { get; set; }
    }
}