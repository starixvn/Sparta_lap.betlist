using System.Collections.Generic;
using Fanex.Athena.Model.Core.DTO.Common;
using Fanex.Athena.Model.Report.DTO;

namespace Fanex.Athena.Model.Report.ResultModel
{
    public class WinLossByProductResultModel
    {
        public IEnumerable<WinLossByProduct> DataList { get; set; }

        public IEnumerable<WinLossByProduct> TotalData { get; set; }

        public IDictionary<string, IEnumerable<WinLossByProduct>> SummaryTotal { get; set; }

        public IResultInfo ResultInfo { get; set; }
    }
}