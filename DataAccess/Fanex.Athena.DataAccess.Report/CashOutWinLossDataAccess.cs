// -----------------------------------------------------------------------
// <copyright file="CashOutWinLossDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Report
{
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using Core.Configuration;
    using Core.Configuration.Interfaces;
    using Extension.Helper;
    using Fanex.Athena.DataAccess.Core;
    using Fanex.Athena.DataAccess.Core.Exception;
    using Fanex.Athena.DataAccess.Core.Extensions;
    using Fanex.Athena.DataAccess.Report.Interfaces;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Model.Core.DataDomain;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Common.Extensions;
    using Fanex.Data;
    using Fanex.Logging;

    public class CashOutWinLossDataAccess : BaseDataAccess, ICashOutWinLossDataAccess
    {
        private readonly IBalanceUpDataAccess _balanceUpDataAccess;

        public CashOutWinLossDataAccess()
        {
            _balanceUpDataAccess = new BalanceUpDataAccess();
        }

        public CashOutWinLossDataAccess(IBalanceUpDataAccess balanceUpDataAccess)
        {
            _balanceUpDataAccess = balanceUpDataAccess;
        }

        public SortableList<CashOutWinLoss> GetSummary(CashOutWinLossMetaModel metaModel, out IDictionary<string, CashOutWinLoss> summaryTotal)
        {
            var uspName = "Lrf_Rpt_CashoutWinloss";
            try
            {
                using (var db = this.GetObjectDb(uspName))
                {
                    UpdateUspParams(db, metaModel, false);
                    db.AddInParameter("ReportType", DbType.Int32, metaModel.ReportBy);

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

        public SortableList<CashOutWinLoss> GetDetail(
            CashOutWinLossMetaModel metaModel,
            out IDictionary<string, CashOutWinLoss> summaryTotal)
        {
            var uspName = "Lrf_Rpt_CashoutWinloss_Detail";
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
            catch (ObjectDbException sqlException)
            {
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, uspName, metaModel);
            }
        }

        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules", "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        private static SortableList<CashOutWinLoss> GetData(
            IDataReader reader,
            CashOutWinLossMetaModel metaModel,
            out IDictionary<string, CashOutWinLoss> summaryTotal)
        {
            var result = new SortableList<CashOutWinLoss>();

            summaryTotal = new Dictionary<string, CashOutWinLoss>();
            foreach (var currency in SystemConstants.BaseCurrencyList)
            {
                summaryTotal.Add(currency, new CashOutWinLoss());
            }

            var cashOutValueColumn = "cashoutvalue" + metaModel.BaseCurrency;

            var buybackAmountSBColumn = "buybackamountSB" + metaModel.BaseCurrency;
            var buybackAmountBAColumn = "buybackamountBA" + metaModel.BaseCurrency;

            var ticketPayoutColumn = "ticketpayout" + metaModel.BaseCurrency;
            var cashOutCommColumn = "cashoutcomm" + metaModel.BaseCurrency;

            var settleTicketPayoutColumn = "AmountDue_TicketPayout" + metaModel.BaseCurrency;
            var settleCommColumn = "AmountDue_Comm" + metaModel.BaseCurrency;
            var settleTotalColumn = "Amount_Total" + metaModel.BaseCurrency;

            var hasSystemId = reader.HasColumn("systemid");
            var hasExternalId = reader.HasColumn("externalid");
            var hasCustomerId = reader.HasColumn("custid");
            var isDrilldown = metaModel.DrillDownLevel > 0;
            var index = 1;
            while (reader.Read())
            {
                var item = new CashOutWinLoss
                {
                    Index = index++,
                    SystemId = hasSystemId ? reader.ToString("systemid") : string.Empty,
                    ExternalId = hasExternalId ? reader.ToString("externalid") : string.Empty,
                    CustomerId = hasCustomerId ? reader.ToInt("custid") : 0,
                    CurrencyId = reader.ToInt("currencyid"),
                    CurrencyName = reader.ToString("currencyname"),
                    CashOutTimes = reader.ToInt("cashouttime"),
                    CashOutValue = reader.ToDecimal(cashOutValueColumn),
                    CashOutTickets = reader.ToInt("cashoutticket"),
                    BuyBackAmountSB = reader.ToDecimal(buybackAmountSBColumn),
                    BuyBackAmountBA = reader.ToDecimal(buybackAmountBAColumn),
                    TicketPayout = reader.ToDecimal(ticketPayoutColumn),
                    CashOutCommission = reader.ToDecimal(cashOutCommColumn),
                    SettleAmountTicketPayout = reader.ToDecimal(settleTicketPayoutColumn),
                    SettleAmountBuyBackComm = reader.ToDecimal(settleCommColumn),
                    SettleAmountTotal = reader.ToDecimal(settleTotalColumn),
                    Date = isDrilldown
                        ? metaModel.DrillDownFromDate
                        : ConvertHelper.ToDateTime(reader.ToString("reporttype"), metaModel.ReportBy == ReportByType.Monthly)
                };

                result.Add(item);

                foreach (var currency in SystemConstants.BaseCurrencyList)
                {
                    summaryTotal[currency].CashOutValue += reader.ToDecimal("cashoutvalue" + currency);
                    summaryTotal[currency].CashOutTickets += reader.ToInt("cashoutticket");
                    summaryTotal[currency].CashOutTimes += reader.ToInt("cashouttime");
                    summaryTotal[currency].BuyBackAmountSB += reader.ToDecimal("buybackamountSB" + currency);
                    summaryTotal[currency].BuyBackAmountBA += reader.ToDecimal("buybackamountBA" + currency);
                    summaryTotal[currency].TicketPayout += reader.ToDecimal("ticketpayout" + currency);
                    summaryTotal[currency].CashOutCommission += reader.ToDecimal("cashoutcomm" + currency);
                    summaryTotal[currency].SettleAmountTicketPayout += reader.ToDecimal("AmountDue_TicketPayout" + currency);
                    summaryTotal[currency].SettleAmountBuyBackComm += reader.ToDecimal("AmountDue_Comm" + currency);
                    summaryTotal[currency].SettleAmountTotal += reader.ToDecimal("Amount_Total" + currency);
                }
            }

            return result;
        }

        private void UpdateUspParams(IObjectDb db, CashOutWinLossMetaModel metaModel, bool drilldown)
        {
            db.AddInParameter("fromdate", DbType.Date, drilldown ? metaModel.DrillDownFromDate : metaModel.FromDate.Date);
            db.AddInParameter("todate", DbType.Date, drilldown ? metaModel.DrillDownToDate : metaModel.ToDate.Date);
            db.AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
            db.AddInParameter("MoveBetsDay", DbType.DateTime, _balanceUpDataAccess.GetBalanceUpDay().MoveBetsDay);
        }
    }
}