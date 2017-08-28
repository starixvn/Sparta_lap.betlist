namespace Fanex.Athena.DataAccess.Report
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Core;
    using Data;
    using Fanex.Athena.DataAccess.Report.Interfaces;
    using Model.Core.DTO.Configuration;
    using Model.Report.DTO;
    using Model.Report.MetaModel;

    public class SubsidiaryPerformanceDataAccess : BaseDataAccess, ISubsidiaryPerformanceDataAccess
    {
        public SubsidiaryPerformanceDataAccess(BalanceUp balanceUp) : base(balanceUp)
        {
        }

        public IEnumerable<SubsidiaryPerformance> GetData(SubsidiaryPerformanceMetaModel metaModel)
        {
            const string SpName = "Lrf_Rpt_SubsidiaryPerformance";
            var dataList = new List<SubsidiaryPerformance>();
            var baseCurrency = metaModel.ConvertAll ? metaModel.BaseCurrency : string.Empty;
            using (var objectDb = this.GetObjectDb(SpName))
            {
                AssignParameter(objectDb, metaModel);

                using (var reader = objectDb.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        dataList.Add(GetEntityData(reader, baseCurrency));
                    }
                }
            }

            return dataList;
        }

        private static void AssignParameter(IObjectDb objectDb, SubsidiaryPerformanceMetaModel metaModel)
        {
            objectDb.AddInParameter("FromDate", DbType.DateTime, metaModel.FromDate);
            objectDb.AddInParameter("ToDate", DbType.DateTime, metaModel.ToDate);
            objectDb.AddInParameter("IsExportExcel", DbType.Boolean, metaModel.IsExportExcel);
            objectDb.AddInParameter("LicenseeId", DbType.Int32, metaModel.LicenseeId);
            objectDb.AddInParameter("ProductCodeList", DbType.String, metaModel.ProductCodeList);
            objectDb.AddInParameter("SubsidiaryList", DbType.String, metaModel.SubsidiaryList);
            objectDb.AddInParameter("AgentListXml", DbType.String, metaModel.AgentListXml);
        }

        private static SubsidiaryPerformance GetEntityData(IDataReader reader, string baseCurrency)
        {
            return new SubsidiaryPerformance
            {
                Subsidiary = reader["Subsidiary"].ToString(),
                Currency = reader["Currency"].ToString(),
                Product = reader["Product"].ToString(),
                BetCount = Convert.ToInt32(reader["BetCount"]),
                Turnover = Convert.ToDecimal(reader["Turnover" + baseCurrency]),
                CustomerWinLoss = Convert.ToDecimal(reader["CustomerWinLoss" + baseCurrency]),
                SubsidiaryWinLoss = Convert.ToDecimal(reader["SubsidiaryWinLoss" + baseCurrency]),
                SubsidiaryComm = Convert.ToDecimal(reader["SubsidiaryComm" + baseCurrency])
            };
        }
    }
}