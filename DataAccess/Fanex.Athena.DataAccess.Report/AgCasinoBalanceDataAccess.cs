// -----------------------------------------------------------------------
// <copyright file="AgCasinoBalanceDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Report
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Fanex.Athena.DataAccess.Core;
    using Fanex.Athena.DataAccess.Core.Exception;
    using Fanex.Athena.DataAccess.Core.Extensions;
    using Fanex.Athena.DataAccess.Report.Interfaces;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Model.Core.DataDomain;
    using Fanex.Athena.Model.Core.DTO.Configuration;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Common.Extensions;
    using Fanex.Data;
    using Fanex.Logging;

    public class AgCasinoBalanceDataAccess : BaseDataAccess, IAgCasinoBalanceDataAccess
    {
        public AgCasinoBalanceDataAccess(BalanceUp balanceUp)
            : base(balanceUp)
        {
        }

        public SortableList<AgCasinoBalance> GetSummaryWinloss(AgCasinoBalanceMetaModel metaModel, out IDictionary<string, AgCasinoBalance> summaryTotal)
        {
            var uspName = "Lrf_Rpt_AGCasinoBalance";
            try
            {
                using (var db = this.GetObjectDb(uspName))
                {
                    UpdateUspParams(db, metaModel, false);

                    using (var reader = db.ExecuteReader())
                    {
                        return GetData(reader, metaModel, out summaryTotal);
                    }
                }
            }
            catch (SqlException sqlException)
            {
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, uspName, metaModel);
            }
        }

        public SortableList<AgCasinoBalance> GetCustomerWinlossDetail(
            AgCasinoBalanceMetaModel metaModel,
            out IDictionary<string, AgCasinoBalance> summaryTotal,
            out int totalRows)
        {
            var uspName = "LRF_Rpt_AGCasinoBalance_Mem";
            try
            {
                using (var db = this.GetObjectDb(uspName))
                {
                    db.AddInParameter("custid", DbType.Int32, metaModel.DrillDownCustomerId);
                    db.AddInParameter("pagesize", DbType.Int32, metaModel.IsExportExcel ? metaModel.PageSizeExcel : metaModel.PageSize);
                    db.AddInParameter("pageindex", DbType.Int32, metaModel.IsExportExcel ? 1 : metaModel.PageIndex);
                    db.AddOutParameter("TotalRows", DbType.Int32, 10);

                    UpdateUspParams(db, metaModel, true);
                    SortableList<AgCasinoBalance> data;
                    using (var reader = db.ExecuteReader())
                    {
                        data = GetData(reader, metaModel, out summaryTotal);
                    }

                    totalRows = (int)db.GetParameterValue("TotalRows");
                    return data;
                }
            }
            catch (SqlException sqlException)
            {
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, uspName, metaModel);
            }
        }

        public SortableList<AgCasinoBalance> GetAgentWinlossDetail(
            AgCasinoBalanceMetaModel metaModel,
            out IDictionary<string, AgCasinoBalance> summaryTotal)
        {
            var uspName = "LRF_Rpt_AGCasinoBalance_Ag";
            try
            {
                using (var db = this.GetObjectDb(uspName))
                {
                    UpdateUspParams(db, metaModel, true);
                    using (var reader = db.ExecuteReader())
                    {
                        return GetData(reader, metaModel, out summaryTotal);
                    }
                }
            }
            catch (SqlException sqlException)
            {
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, uspName, metaModel);
            }
        }

        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules", "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        private static SortableList<AgCasinoBalance> GetData(
            IDataReader reader,
            AgCasinoBalanceMetaModel metaModel,
            out IDictionary<string, AgCasinoBalance> summaryTotal)
        {
            var result = new SortableList<AgCasinoBalance>();

            summaryTotal = SystemConstants.BaseCurrencyList.ToDictionary(x => x, x => new AgCasinoBalance());

            var winlostColumn = "winlost" + metaModel.BaseCurrency;
            var licenseewlColumn = "licenseewinlost" + metaModel.BaseCurrency;
            var turnoverColumn = "turnover" + metaModel.BaseCurrency;
            var openingBalanceColumn = "OpeningBalance" + metaModel.BaseCurrency;
            var transferInColumn = "TransferIn" + metaModel.BaseCurrency;
            var transferOutColumn = "TransferOut" + metaModel.BaseCurrency;
            var closingBalanceColumn = "ClosingBalance" + metaModel.BaseCurrency;

            var hasSystemIdColumn = reader.HasColumn("username");
            var hasExternalIdColumn = reader.HasColumn("nickname");
            var hasCustomerIdColumn = reader.HasColumn("custid");
            var hasWinlostDateColumn = reader.HasColumn("date");
            var index = metaModel.DrillDownLevel == 2 ? ((metaModel.PageIndex - 1) * metaModel.PageSize) + 1 : 1;
            while (reader.Read())
            {
                var item = new AgCasinoBalance
                {
                    Index = index++,
                    SystemId = hasSystemIdColumn ? reader.ToString("username") : string.Empty,
                    ExternalId = hasExternalIdColumn ? reader.ToString("nickname") : string.Empty,
                    CustomerId = hasCustomerIdColumn ? reader.ToInt("custid") : 0,
                    CurrencyId = reader.ToInt("currency"),
                    CurrencyName = reader.ToString("currencyname"),
                    CustomerWinLoss = reader.ToDecimal(winlostColumn),
                    LicenseeWL = reader.ToDecimal(licenseewlColumn),
                    TurnOver = reader.ToDecimal(turnoverColumn),
                    TotalTransaction = reader.ToInt("totaltrans"),
                    Date = hasWinlostDateColumn ? (DateTime?)reader.ToDateTime("date") : metaModel.DrillDownFromDate,
                    OpeningBalance = reader.ToDecimal(openingBalanceColumn),
                    TransferIn = reader.ToDecimal(transferInColumn),
                    TransferOut = reader.ToDecimal(transferOutColumn),
                    ClosingBalance = reader.ToDecimal(closingBalanceColumn),
                };

                result.Add(item);

                // Read Summary
                foreach (var currencySuffix in SystemConstants.BaseCurrencyList)
                {
                    summaryTotal[currencySuffix].CustomerWinLoss += reader.ToDecimal("winlost" + currencySuffix);
                    summaryTotal[currencySuffix].TotalTransaction += reader.ToInt("totaltrans");
                    summaryTotal[currencySuffix].LicenseeWL += reader.ToDecimal("licenseewinlost" + currencySuffix);
                    summaryTotal[currencySuffix].TurnOver += reader.ToDecimal("turnover" + currencySuffix);
                    summaryTotal[currencySuffix].OpeningBalance += reader.ToDecimal("OpeningBalance" + currencySuffix);
                    summaryTotal[currencySuffix].TransferIn += reader.ToDecimal("TransferIn" + currencySuffix);
                    summaryTotal[currencySuffix].TransferOut += reader.ToDecimal("TransferOut" + currencySuffix);
                    summaryTotal[currencySuffix].ClosingBalance += reader.ToDecimal("ClosingBalance" + currencySuffix);
                }
            }

            return result;
        }

        private void UpdateUspParams(IObjectDb db, AgCasinoBalanceMetaModel metaModel, bool drilldown)
        {
            db.AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
            db.AddInParameter("fromdate", DbType.Date, drilldown ? metaModel.DrillDownFromDate : metaModel.FromDate.Date);
            db.AddInParameter("todate", DbType.Date, drilldown ? metaModel.DrillDownToDate : metaModel.ToDate.Date);
        }
    }
}