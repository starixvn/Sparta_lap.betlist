namespace Fanex.Athena.DataAccess.Report
{
    using System.Collections.Generic;
    using System.Data;
    using Core;
    using Fanex.Athena.DataAccess.Report.Interfaces;
    using Model.Core.DTO.Configuration;
    using Model.Report.DTO;
    using Model.Report.MetaModel;

    public class SubLicenseePerformanceDataAccess : BaseDataAccess, ISubLicenseePerformanceDataAccess
    {
        public SubLicenseePerformanceDataAccess(BalanceUp balanceUp) : base(balanceUp)
        {
        }

        public IEnumerable<SubLicenseePerformance> GetData(SubLicenseePerformanceMetaModel metaModel)
        {
            const string SpName = "Lrf_Rpt_SubLicenseePerformance";
            using (var obejctDb = this.GetObjectDb(SpName))
            {
                obejctDb.AddInParameter("BaseCurrency", DbType.String, metaModel.BaseCurrency);
                obejctDb.AddInParameter("ConvertAll", DbType.Boolean, metaModel.ConvertAll);
                obejctDb.AddInParameter("FromDate", DbType.DateTime, metaModel.FromDate);
                obejctDb.AddInParameter("ToDate", DbType.DateTime, metaModel.ToDate);
                obejctDb.AddInParameter("IsExportExcel", DbType.Boolean, metaModel.IsExportExcel);
                obejctDb.AddInParameter("LicenseeId", DbType.Int32, metaModel.LicenseeId);
                obejctDb.AddInParameter("ProductCodeList", DbType.String, metaModel.ProductCodeList);
                obejctDb.AddInParameter("SubLicenseeList", DbType.String, metaModel.SubLicenseeList);
                obejctDb.AddInParameter("AgentListXml", DbType.String, metaModel.AgentListXml);
            }

            return new List<SubLicenseePerformance>();
        }
    }
}