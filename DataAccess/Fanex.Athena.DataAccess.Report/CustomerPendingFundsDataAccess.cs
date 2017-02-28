// -----------------------------------------------------------------------
// <copyright file="CustomerPendingFundsDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Report
{
    using System.Data;
    using Fanex.Athena.DataAccess.Core;
    using Fanex.Athena.DataAccess.Report.Interfaces;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Common.Extensions;

    public class CustomerPendingFundsDataAccess : BaseDataAccess, ICustPendingFundsDataAccess
    {
        public SortableList<CustPendingFunds> GetDataList(CustomerPendingFundsMetaModel metaModel, ref CustPendingFunds totalData, ref int totalRows)
        {
            SortableList<CustPendingFunds> result = new SortableList<CustPendingFunds>();
            using (var db = this.GetObjectDb("Lrf_Rpt_Outstanding"))
            {
                db.AddInParameter("pagesize", DbType.Int32, metaModel.PageSize);
                db.AddInParameter("pageindex", DbType.Int32, metaModel.PageIndex);
                db.AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
                foreach (var item in metaModel.ProductOptions)
                {
                    db.AddInParameter("show" + item.Code, DbType.Boolean, item.Status);
                }

                using (var reader = db.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(new CustPendingFunds
                        {
                            CustId = reader.ToInt("CustID"),
                            ExternalId = reader.ToString("nickname"),
                            SystemId = reader.ToString("username"),
                            BetCounts = reader.ToInt("BetCounts"),
                            PendingFunds = reader.ToDecimal("Outstanding" + metaModel.BaseCurrency)
                        });
                    }

                    if (result.Count > 0 && reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            totalData = new CustPendingFunds
                            {
                                BetCounts = reader.ToInt("SumBetCounts"),
                                PendingFunds = reader.ToDecimal("SumOutstanding" + metaModel.BaseCurrency)
                            };
                            totalRows = reader.ToInt("TotalRows");
                        }
                    }
                }
            }

            return result;
        }
    }
}