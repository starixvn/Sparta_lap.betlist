namespace Fanex.Athena.Excel.Report.Models
{
    using System.Collections.Generic;
    using Base.Entities;
    using Model.Report.DTO;

    public class CustomerPendingFundsExcelModel
    {
        public ConvertingOption ConvertingOption { get; set; }

        public IEnumerable<CustPendingFunds> DataList { get; set; }

        public CustPendingFunds TotalData { get; set; }

        public bool SystemIdVisible { get; set; }
    }
}