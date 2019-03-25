// -----------------------------------------------------------------------
// <copyright file="CustomerPendingFundsDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Report
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Core.Exception;
    using Data;
    using Fanex.Athena.DataAccess.Core;
    using Fanex.Athena.DataAccess.Report.Interfaces;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Common.Extensions;
    using Model.Core.ResultModel;

    public class CustomerPendingFundsDataAccess : BaseDataAccess, ICustPendingFundsDataAccess
    {
        public ReportResultModel<CustPendingFunds> GetDataList(CustomerPendingFundsMetaModel metaModel)
        {
            const string UspName = "Lrf_Rpt_Outstanding";
            try
            {
                using (var db = GetObjectDb(UspName))
                {
                    AddParameters(db, metaModel);

                    using (var reader = db.ExecuteReader())
                    {
                        var dataList = new List<CustPendingFunds>();
                        var index = ((metaModel.PageIndex - 1) * metaModel.PageSize) + 1;
                        while (reader.Read())
                        {
                            dataList.Add(ReadData(reader, metaModel.BaseCurrency, index++));
                        }

                        var result = new ReportResultModel<CustPendingFunds>
                        {
                            DataList = dataList
                        };

                        if (dataList.Any() && reader.NextResult() && reader.Read())
                        {
                            result.TotalData = new CustPendingFunds
                            {
                                BetCounts = reader.ToInt("SumBetCounts"),
                                PendingFunds = reader.ToDecimal("SumOutstanding" + metaModel.BaseCurrency)
                            };
                            result.TotalRows = reader.ToInt("TotalRows");
                        }
                        else
                        {
                            result.TotalData = new CustPendingFunds();
                        }

                        return result;
                    }
                }
            }
            catch (ObjectDbException sqlException)
            {
                throw new DataAccessException(sqlException, UspName, metaModel);
            }
        }

        private static void AddParameters(IObjectDb db, CustomerPendingFundsMetaModel metaModel)
        {
            db.SetParameterValue("pagesize", metaModel.PageSize);
            db.SetParameterValue("pageindex", metaModel.PageIndex);
            db.SetParameterValue("OrderItem", metaModel.OrderItem);
            db.SetParameterValue("IsAscSort", metaModel.IsAscSort);
            db.SetParameterValue("BaseCurrency", metaModel.BaseCurrency.ToUpperInvariant());
            db.SetParameterValue("AgentList", metaModel.AgentListXml);
            foreach (var item in metaModel.ProductOptions)
            {
                db.SetParameterValue("show" + item.Code, item.Status);
            }
        }

        private static CustPendingFunds ReadData(IDataReader reader, string baseCurrency, int index)
        {
            return new CustPendingFunds
            {
                Index = index,
                CustId = reader.ToInt("CustID"),
                ExternalId = reader.ToString("nickname"),
                SystemId = reader.ToString("username"),
                BetCounts = reader.ToInt("BetCounts"),
                PendingFunds = reader.ToDecimal("Outstanding" + baseCurrency)
            };
        }
    }
}