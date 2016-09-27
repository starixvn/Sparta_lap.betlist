// -----------------------------------------------------------------------
// <copyright file="ColossusBetsWinLossDataAccess.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Report
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using Extension.Helper;
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

    public class ColossusBetsWinLossDataAccess : BaseDataAccess, IColossusBetsWinLossDataAccess
    {
        private readonly BalanceUp _balanceUp;

        public ColossusBetsWinLossDataAccess(BalanceUp balanceUp)
        {
            _balanceUp = balanceUp;
        }

        public SortableList<ColossusBetWinLoss> GetSummaryWinloss(ThirdPartyWinLossMetaModel metaModel, out IDictionary<string, ColossusBetWinLoss> summaryTotal)
        {
            var uspName = "Lrf_Rpt_ColossusBetsWinloss";
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
            catch (ObjectDbException sqlException)
            {
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, uspName, metaModel);
            }
        }

        public SortableList<ColossusBetWinLoss> GetWinlossDetail(
            ThirdPartyWinLossMetaModel metaModel,
            out IDictionary<string, ColossusBetWinLoss> summaryTotal)
        {
            var uspName = "Lrf_Rpt_ColossusBetsWinloss_detail";
            try
            {
                using (var db = this.GetObjectDb(uspName))
                {
                    // db.AddInParameter("currency", DbType.Int32, metaModel);
                    UpdateUspParams(db, metaModel, true);
                    using (var reader = db.ExecuteReader())
                    {
                        return GetData(reader, metaModel, out summaryTotal);
                    }
                }
            }
            catch (ObjectDbException sqlException)
            {
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, uspName, metaModel);
            }
        }

        #region Private

        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules", "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        private static SortableList<ColossusBetWinLoss> GetData(
            IDataReader reader,
            ThirdPartyWinLossMetaModel metaModel,
            out IDictionary<string, ColossusBetWinLoss> summaryTotal)
        {
            var result = new SortableList<ColossusBetWinLoss>();

            summaryTotal = new Dictionary<string, ColossusBetWinLoss>();
            foreach (var currency in SystemConstants.BaseCurrencyList)
            {
                summaryTotal.Add(currency, new ColossusBetWinLoss());
            }

            var turnoverColumn = "turnover" + metaModel.BaseCurrency;
            var winningPrizeColumn = "winningprize" + metaModel.BaseCurrency;
            var cashOutColumn = "cashoutamt" + metaModel.BaseCurrency;
            var cashInColumn = "cashinamt" + metaModel.BaseCurrency;
            var grantPrizeColumn = "grandamt" + metaModel.BaseCurrency;
            var commColumn = "commamt" + metaModel.BaseCurrency;

            var hasSystemId = reader.HasColumn("username");
            var hasExternalId = reader.HasColumn("nickname");
            var hasCustomerId = reader.HasColumn("custid");

            var index = 1;
            while (reader.Read())
            {
                var item = new ColossusBetWinLoss
                {
                    Index = index++,
                    SystemId = hasSystemId ? reader.ToString("username") : string.Empty,
                    ExternalId = hasExternalId ? reader.ToString("nickname") : string.Empty,
                    CustomerId = hasCustomerId ? reader.ToInt("custid") : 0,
                    CurrencyId = reader.ToInt("currencyid"),
                    CurrencyName = reader.ToString("currencyname"),
                    TurnOver = reader.ToDecimal(turnoverColumn),
                    TotalTransaction = reader.ToInt("totaltrans"),
                    WinningPrize = reader.ToDecimal(winningPrizeColumn),
                    CashOut = reader.ToDecimal(cashOutColumn),
                    CashIn = reader.ToDecimal(cashInColumn),
                    CommissionShare = reader.ToDecimal(commColumn),
                    GrandPrize = reader.ToDecimal(grantPrizeColumn),
                    Date = metaModel.DrillDownLevel > 0 ?
                        metaModel.DrillDownFromDate : 
                        ConvertHelper.ToDateTime(reader.ToString("reporttype"), metaModel.ReportBy == ReportByType.Monthly)
                };

                result.Add(item);

                var item2 = new ColossusBetWinLoss
                {
                    Index = index++,
                    SystemId = hasSystemId ? reader.ToString("username") : string.Empty,
                    ExternalId = hasExternalId ? reader.ToString("nickname") : string.Empty,
                    CustomerId = hasCustomerId ? reader.ToInt("custid") : 0,
                    CurrencyId = reader.ToInt("currencyid"),
                    CurrencyName = reader.ToString("currencyname"),
                    TurnOver = reader.ToDecimal(turnoverColumn),
                    TotalTransaction = reader.ToInt("totaltrans"),
                    WinningPrize = reader.ToDecimal(winningPrizeColumn),
                    CashOut = reader.ToDecimal(cashOutColumn),
                    CashIn = reader.ToDecimal(cashInColumn),
                    CommissionShare = reader.ToDecimal(commColumn),
                    GrandPrize = reader.ToDecimal(grantPrizeColumn),
                    Date = metaModel.DrillDownLevel > 0 ?
                        metaModel.DrillDownFromDate :
                        ConvertHelper.ToDateTime(reader.ToString("reporttype"), metaModel.ReportBy == ReportByType.Monthly)
                };

                result.Add(item2);

                var item3 = new ColossusBetWinLoss
                {
                    Index = index++,
                    SystemId = hasSystemId ? reader.ToString("username") : string.Empty,
                    ExternalId = hasExternalId ? reader.ToString("nickname") : string.Empty,
                    CustomerId = hasCustomerId ? reader.ToInt("custid") : 0,
                    CurrencyId = reader.ToInt("currencyid"),
                    CurrencyName = reader.ToString("currencyname"),
                    TurnOver = reader.ToDecimal(turnoverColumn),
                    TotalTransaction = reader.ToInt("totaltrans"),
                    WinningPrize = reader.ToDecimal(winningPrizeColumn),
                    CashOut = reader.ToDecimal(cashOutColumn),
                    CashIn = reader.ToDecimal(cashInColumn),
                    CommissionShare = reader.ToDecimal(commColumn),
                    GrandPrize = reader.ToDecimal(grantPrizeColumn),
                    Date = metaModel.DrillDownLevel > 0 ?
                        metaModel.DrillDownFromDate :
                        ConvertHelper.ToDateTime(reader.ToString("reporttype"), metaModel.ReportBy == ReportByType.Monthly)
                };

                item3.Date = item3.Date.AddDays(1);
                result.Add(item3);

                var item4 = new ColossusBetWinLoss
                {
                    Index = index++,
                    SystemId = hasSystemId ? reader.ToString("username") : string.Empty,
                    ExternalId = hasExternalId ? reader.ToString("nickname") : string.Empty,
                    CustomerId = hasCustomerId ? reader.ToInt("custid") : 0,
                    CurrencyId = reader.ToInt("currencyid"),
                    CurrencyName = reader.ToString("currencyname"),
                    TurnOver = reader.ToDecimal(turnoverColumn),
                    TotalTransaction = reader.ToInt("totaltrans"),
                    WinningPrize = reader.ToDecimal(winningPrizeColumn),
                    CashOut = reader.ToDecimal(cashOutColumn),
                    CashIn = reader.ToDecimal(cashInColumn),
                    CommissionShare = reader.ToDecimal(commColumn),
                    GrandPrize = reader.ToDecimal(grantPrizeColumn),
                    Date = metaModel.DrillDownLevel > 0 ?
                        metaModel.DrillDownFromDate :
                        ConvertHelper.ToDateTime(reader.ToString("reporttype"), metaModel.ReportBy == ReportByType.Monthly)
                };

                item4.Date = item4.Date.AddDays(1);
                result.Add(item4);

                foreach (var currency in SystemConstants.BaseCurrencyList)
                {
                    summaryTotal[currency].TotalTransaction += reader.ToInt("totaltrans");
                    summaryTotal[currency].TurnOver += reader.ToDecimal("turnover" + currency);
                    summaryTotal[currency].WinningPrize += reader.ToDecimal("winningprize" + currency);
                    summaryTotal[currency].CashOut += reader.ToDecimal("cashoutamt" + currency);
                    summaryTotal[currency].CashIn += reader.ToDecimal("cashinamt" + currency);
                    summaryTotal[currency].CommissionShare += reader.ToDecimal("commamt" + currency);
                    summaryTotal[currency].GrandPrize += reader.ToDecimal("grandamt" + currency);
                }
            }

            return result;
        }

        private void UpdateUspParams(IObjectDb db, ThirdPartyWinLossMetaModel metaModel, bool drilldown)
        {
            var currencyId = drilldown ? metaModel.DrillDownCurrencyId : metaModel.CurrencyId;
            currencyId = currencyId < 0 ? 0 : currencyId;

            db.AddInParameter("licenseeid", DbType.Int32, metaModel.LicenseeId);
            db.AddInParameter("currencyid", DbType.Int32, currencyId);
            db.AddInParameter("userid", DbType.Int32, metaModel.UserId);
            db.AddInParameter("fromdate", DbType.Date, drilldown ? metaModel.DrillDownFromDate : metaModel.FromDate.Date);
            db.AddInParameter("todate", DbType.Date, drilldown ? metaModel.DrillDownToDate : metaModel.ToDate.Date);
            db.AddInParameter("excludetest", DbType.Int32, metaModel.IsExcludedTestData);
            db.AddInParameter("movebetsday", DbType.DateTime, _balanceUp.MoveBetsDay);
            db.AddInParameter("ReportType", DbType.Int32, metaModel.ReportBy);
        }

        #endregion Private
    }
}