// -----------------------------------------------------------------------
// <copyright file="CustomerWinlossDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Report
{
    using System;
    using System.Data;
    using Core.Configuration;
    using Core.Configuration.Interfaces;
    using Data;
    using Fanex.Athena.DataAccess.Core;
    using Fanex.Athena.DataAccess.Report.Interfaces;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Common.Extensions;

    public class CustomerWinlossDataAccess : BaseDataAccess, ICustWinlossDataAccess
    {
        private const int PageSizeExcel = int.MaxValue - 1000;

        private readonly IBalanceUpDataAccess _balanceUpDataAccess;

        public CustomerWinlossDataAccess()
        {
            _balanceUpDataAccess = new BalanceUpDataAccess();
        }

        public CustomerWinlossDataAccess(IBalanceUpDataAccess balanceUpDataAccess)
        {
            _balanceUpDataAccess = balanceUpDataAccess;
        }

        public SortableList<CustomerWinLoss> GetDataList(CustomerWinLossMetaModel metaModel, out CustomerWinLoss totalData, out int totalRows)
        {
            using (var db = this.GetObjectDb("Lrf_Rpt_WinLoss"))
            {
                AddParameters(db, metaModel, _balanceUpDataAccess.GetBalanceUp().MoveBetsDay);

                using (var reader = db.ExecuteReader())
                {
                    var result = new SortableList<CustomerWinLoss>();
                    while (reader.Read())
                    {
                        result.Add(new CustomerWinLoss
                        {
                            CustId = reader.ToInt("CustID"),
                            ExternalId = reader.ToString("nickname"),
                            SystemId = reader.ToString("username"),
                            Turnover = reader.ToDecimal("turnover" + metaModel.BaseCurrency),
                            TotalTransaction = reader.ToInt("betcount"),
                            CustomerWL = reader.ToDecimal("memberwinlost" + metaModel.BaseCurrency),
                            BetEffective = reader.ToDecimal("beteffective" + metaModel.BaseCurrency),
                            PlayerComm = reader.ToDecimal("playercomm" + metaModel.BaseCurrency),
                            CustomerTotalWL = reader.ToDecimal("memberwinlost" + metaModel.BaseCurrency) + reader.ToDecimal("playercomm" + metaModel.BaseCurrency),
                            LicenseeWL = reader.ToDecimal("licenseewl" + metaModel.BaseCurrency),
                            BAComm = reader.ToDecimal("commba" + metaModel.BaseCurrency),
                            RacingRevenue = reader.ToDecimal("winlossrb" + metaModel.BaseCurrency)
                                + reader.ToDecimal("commrb" + metaModel.BaseCurrency),
                            AmtDueToBA = reader.ToDecimal("amtduetoba" + metaModel.BaseCurrency),
                            AmtDueToRacing = reader.ToDecimal("amtduetorb" + metaModel.BaseCurrency)
                        });
                    }

                    if (result.Count > 0 && reader.NextResult() && reader.Read())
                    {
                        totalData = ReadTotalData(reader, metaModel.BaseCurrency);
                        totalRows = reader.ToInt("TotalRows");
                    }
                    else
                    {
                        totalRows = 0;
                        totalData = new CustomerWinLoss();
                    }

                    return result;
                }
            }
        }

        private static void AddParameters(IObjectDb db, CustomerWinLossMetaModel metaModel, DateTime moveBetsDay)
        {
            db.AddInParameter("drawfilter", DbType.Int32, metaModel.DrawFilter);
            db.AddInParameter("fromdate", DbType.DateTime, metaModel.FromDate);
            db.AddInParameter("todate", DbType.DateTime, metaModel.ToDate);
            db.AddInParameter("pagesize", DbType.Int32, metaModel.IsExportExcel ? PageSizeExcel : metaModel.PageSize);
            db.AddInParameter("pageindex", DbType.Int32, metaModel.PageIndex);
            db.AddInParameter("movebetsday", DbType.DateTime, moveBetsDay);
            db.AddInParameter("isold", DbType.Boolean, metaModel.IsOld);
            db.AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
            db.AddInParameter("username", DbType.String, metaModel.Username);
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
                RacingRevenue = reader.ToDecimal("sumwinlossrb" + baseCurrency)
                + reader.ToDecimal("sumcommrb" + baseCurrency),
                AmtDueToBA = reader.ToDecimal("sumamtduetoba" + baseCurrency),
                AmtDueToRacing = reader.ToDecimal("sumamtduetorb" + baseCurrency),
                BetEffective = reader.ToDecimal("sumbeteffective" + baseCurrency),
                CustomerTotalWL = reader.ToDecimal("summemberwinlost" + baseCurrency) + reader.ToDecimal("sumplayercomm" + baseCurrency)
            };
        }
    }
}