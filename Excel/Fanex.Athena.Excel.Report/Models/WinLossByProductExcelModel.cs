namespace Fanex.Athena.Excel.Report.Models
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;

    public class WinLossByProductExcelModel
    {
        public WinLossByProductMetaModel MetaModel { get; set; }

        public string SystemId { get; set; }

        public string ErrorMessage { get; set; }

        public IEnumerable<WinLossByProduct> DataList { get; set; }

        public IEnumerable<WinLossByProduct> TotalData { get; set; }

        public IDictionary<string, IEnumerable<WinLossByProduct>> SummaryTotal { get; set; }

        public bool CustomerCommVisible { get; set; }

        public bool BAVisible { get; set; }

        public bool SystemIdVisible { get; set; }

        public bool HasAmountDueToColumn { get; set; }
    }
}