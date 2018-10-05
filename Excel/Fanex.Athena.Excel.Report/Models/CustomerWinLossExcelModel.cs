namespace Fanex.Athena.Excel.Report.Models
{
    using System.Collections.Generic;
    using Base.Entities;
    using Model.Report.DTO;
    using Model.Report.MetaModel;

    public class CustomerWinLossExcelModel
    {
        public ConvertingOption ConvertingOption { get; set; }

        public CustomerWinLossMetaModel MetaModel { get; set; }

        public IEnumerable<CustomerWinLoss> DataList { get; set; }

        public CustomerWinLoss TotalData { get; set; }

        public bool CustomerCommVisible { get; set; }

        public bool BAVisible { get; set; }

        public bool SystemIdVisible { get; set; }
    }
}