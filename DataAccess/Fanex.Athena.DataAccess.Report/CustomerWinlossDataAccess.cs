// -----------------------------------------------------------------------
// <copyright file="CustomerWinlossDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Report
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Core.Configuration.Interfaces;
    using Core.Exception;
    using Data;
    using Fanex.Athena.DataAccess.Core;
    using Fanex.Athena.DataAccess.Report.Interfaces;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Common.Extensions;
    using Model.Core.ResultModel;

    public class CustomerWinlossDataAccess : BaseDataAccess, ICustWinlossDataAccess
    {
        private const int PageSizeExcel = int.MaxValue - 1000;

        private readonly IBalanceUpDataAccess _balanceUpDataAccess;

        public CustomerWinlossDataAccess(
            IBalanceUpDataAccess balanceUpDataAccess)
        {
            _balanceUpDataAccess = balanceUpDataAccess;
        }

        public ReportResultModel<CustomerWinLoss> GetDataList(CustomerWinLossMetaModel metaModel)
        {
            const string UspName = "Lrf_Rpt_WinLoss";
            try
            {
                using (var db = GetObjectDb(UspName))
                {
                    AddParameters(db, metaModel, _balanceUpDataAccess.GetBalanceUp().MoveBetsDay);

                    using (var reader = db.ExecuteReader())
                    {
                        var dataList = new List<CustomerWinLoss>();
                        var index = ((metaModel.PageIndex - 1) * metaModel.PageSize) + 1;
                        while (reader.Read())
                        {
                            dataList.Add(ReadDataItem(reader, metaModel.BaseCurrency, index++));
                        }

                        var result = new ReportResultModel<CustomerWinLoss>
                        {
                            DataList = dataList
                        };

                        if (dataList.Any() && reader.NextResult() && reader.Read())
                        {
                            result.TotalData = ReadTotalData(reader, metaModel.BaseCurrency);
                            result.TotalRows = reader.ToInt("TotalRows");
                            result.AdditionalInfo = reader.ToString("SumPromotionPrize");
                        }
                        else
                        {
                            result.TotalData = new CustomerWinLoss();
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

        private static CustomerWinLoss ReadDataItem(IDataReader reader, string baseCurrency, int index)
        {
            return new CustomerWinLoss
            {
                Index = index,
                CustId = reader.ToInt("CustID"),
                ExternalId = reader.ToString("nickname"),
                SystemId = reader.ToString("username"),
                Turnover = reader.ToDecimal("turnover" + baseCurrency),
                TotalTransaction = reader.ToInt("betcount"),
                CustomerWL = reader.ToDecimal("memberwinlost" + baseCurrency),
                BetEffective = reader.ToDecimal("beteffective" + baseCurrency),
                PlayerComm = reader.ToDecimal("playercomm" + baseCurrency),
                CustomerTotalWL = reader.ToDecimal("customertotalwl" + baseCurrency),
                LicenseeWL = reader.ToDecimal("licenseewl" + baseCurrency),
                BAComm = reader.ToDecimal("commba" + baseCurrency),
                AmtDueToBA = reader.ToDecimal("amtduetoba" + baseCurrency)
            };
        }

        private static void AddParameters(IObjectDb db, CustomerWinLossMetaModel metaModel, DateTime moveBetsDay)
        {
            db.AddInParameter("fromdate", DbType.DateTime, metaModel.FromDate);
            db.AddInParameter("todate", DbType.DateTime, metaModel.ToDate);
            db.AddInParameter("pagesize", DbType.Int32, metaModel.IsExportExcel ? PageSizeExcel : metaModel.PageSize);
            db.AddInParameter("pageindex", DbType.Int32, metaModel.PageIndex);
            db.AddInParameter("movebetsday", DbType.DateTime, moveBetsDay);
            db.AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
            db.AddInParameter("username", DbType.String, metaModel.Username);
            db.AddInParameter("OrderItem", DbType.Int16, metaModel.OrderItem);
            db.AddInParameter("IsAscSort", DbType.Boolean, metaModel.IsAscSort);
            db.AddInParameter("BaseCurrency", DbType.String, metaModel.BaseCurrency.ToUpperInvariant());
            foreach (var item in metaModel.ProductOptions)
            {
                db.AddInParameter("show" + item.Code, DbType.Boolean, item.Status);
            }
        }

        private static CustomerWinLoss ReadTotalData(IDataReader reader, string baseCurrency)
        {
            return new CustomerWinLoss
            {
                Turnover = reader.ToDecimal("sumturnover" + baseCurrency),
                TotalTransaction = reader.ToInt("sumbetcount"),
                CustomerWL = reader.ToDecimal("summemberwinlost" + baseCurrency),
                PlayerComm = reader.ToDecimal("sumplayercomm" + baseCurrency),
                LicenseeWL = reader.ToDecimal("sumlicenseewl" + baseCurrency),
                BAComm = reader.ToDecimal("sumcommba" + baseCurrency),
                AmtDueToBA = reader.ToDecimal("sumamtduetoba" + baseCurrency),
                BetEffective = reader.ToDecimal("sumbeteffective" + baseCurrency),
                CustomerTotalWL = reader.ToDecimal("sumcustomertotalwl" + baseCurrency)
            };
        }
    }
}