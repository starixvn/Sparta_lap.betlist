// -----------------------------------------------------------------------
// <copyright file="ColossusBetsWinLossDataAccess.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Report
{
    using System.Collections.Generic;
    using System.Data;
    using Core;
    using Core.Configuration.Interfaces;
    using Extension.Helper;
    using Fanex.Athena.DataAccess.Core.Exception;
    using Fanex.Athena.DataAccess.Core.Extensions;
    using Fanex.Athena.DataAccess.Report.Interfaces;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Model.Core.DataDomain;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Common.Extensions;
    using Fanex.Data;
    using Model.Core.ResultModel;
    using Session.Interfaces;

    public class ColossusBetsWinLossDataAccess : BaseDataAccess, IColossusBetsWinLossDataAccess
    {
        private readonly IBalanceUpDataAccess _balanceUpDataAccess;

        public ColossusBetsWinLossDataAccess(
            IAppSettings appSetting,
            ISessionStorage sessionStorage,
            IBalanceUpDataAccess balanceUpDataAccess)
            : base(appSetting, sessionStorage)
        {
            _balanceUpDataAccess = balanceUpDataAccess;
        }

        public SortableReportResultModel<ColossusBetWinLoss> GetSummaryWinloss(ColossusBetsWinLossMetaModel metaModel)
        {
            return GetData("Lrf_Rpt_ColossusBetsWinloss", metaModel);
        }

        public SortableReportResultModel<ColossusBetWinLoss> GetWinlossDetail(ColossusBetsWinLossMetaModel metaModel)
        {
            return GetData("Lrf_Rpt_ColossusBetsWinloss_detail", metaModel);
        }

        private static void ReadSummaryTotal(IDataReader reader, IDictionary<string, ColossusBetWinLoss> summaryTotal)
        {
            foreach (var currency in SystemConstants.BaseCurrencyList)
            {
                summaryTotal[currency].TotalTransaction += reader.ToInt("totaltrans");
                summaryTotal[currency].TurnOver += reader.ToDecimal("turnover" + currency);
                summaryTotal[currency].MemberWinning += reader.ToDecimal("winningprize" + currency);
                summaryTotal[currency].CashOut += reader.ToDecimal("cashoutamt" + currency);
                summaryTotal[currency].CashOutShare += reader.ToDecimal("cashinamt" + currency);
                summaryTotal[currency].CommissionShare += reader.ToDecimal("commamt" + currency);
                summaryTotal[currency].GrandPrize += reader.ToDecimal("grandamt" + currency);
            }
        }

        private SortableReportResultModel<ColossusBetWinLoss> GetData(string spname, ColossusBetsWinLossMetaModel metaModel)
        {
            try
            {
                using (var db = this.GetObjectDb(spname))
                {
                    UpdateUspParams(db, metaModel, metaModel.DrillDownLevel > 0);
                    using (var reader = db.ExecuteReader())
                    {
                        return ReadData(reader, metaModel);
                    }
                }
            }
            catch (ObjectDbException sqlException)
            {
                throw new DataAccessException(sqlException, spname, metaModel);
            }
        }

        private void UpdateUspParams(IObjectDb db, ColossusBetsWinLossMetaModel metaModel, bool drilldown)
        {
            db.AddInParameter("fromdate", DbType.Date, drilldown ? metaModel.DrillDownFromDate : metaModel.FromDate.Date);
            db.AddInParameter("todate", DbType.Date, drilldown ? metaModel.DrillDownToDate : metaModel.ToDate.Date);
            db.AddInParameter("movebetsday", DbType.DateTime, _balanceUpDataAccess.GetBalanceUp().MoveBetsDay);
            db.AddInParameter("ReportType", DbType.Int32, metaModel.ReportBy);
            db.AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
            db.AddInParameter("TicketType", DbType.Int32, metaModel.TicketType);
        }

        private SortableReportResultModel<ColossusBetWinLoss> ReadData(
            IDataReader reader,
            ColossusBetsWinLossMetaModel metaModel)
        {
            var turnoverColumn = "turnover" + metaModel.BaseCurrency;
            var memberWinningColumn = "winningprize" + metaModel.BaseCurrency;
            var cashOutColumn = "cashoutamt" + metaModel.BaseCurrency;
            var cashOutShareColumn = "cashinamt" + metaModel.BaseCurrency;
            var grantPrizeColumn = "grandamt" + metaModel.BaseCurrency;
            var commColumn = "commamt" + metaModel.BaseCurrency;
            var hasSystemId = reader.HasColumn("systemid");
            var hasExternalId = reader.HasColumn("externalid");
            var hasCustomerId = reader.HasColumn("custid");
            var index = 1;
            var isFirstLevel = metaModel.DrillDownLevel == 0;
            var isMonthly = metaModel.ReportBy == ReportByType.Monthly;
            var result = new SortableList<ColossusBetWinLoss>();
            var summaryTotal = new Dictionary<string, ColossusBetWinLoss>();
            foreach (var currency in SystemConstants.BaseCurrencyList)
            {
                summaryTotal.Add(currency, new ColossusBetWinLoss());
            }

            while (reader.Read())
            {
                var currencyId = reader.ToInt("currencyid");
                result.Add(new ColossusBetWinLoss
                {
                    Index = index++,
                    SystemId = hasSystemId ? reader.ToString("systemid") : string.Empty,
                    ExternalId = hasExternalId ? reader.ToString("externalid") : string.Empty,
                    CustomerId = hasCustomerId ? reader.ToInt("custid") : 0,
                    CurrencyId = currencyId,
                    CurrencyName = GetCurrencyName(currencyId),
                    TurnOver = reader.ToDecimal(turnoverColumn),
                    TotalTransaction = reader.ToInt("totaltrans"),
                    MemberWinning = reader.ToDecimal(memberWinningColumn),
                    CashOut = reader.ToDecimal(cashOutColumn),
                    CashOutShare = reader.ToDecimal(cashOutShareColumn),
                    CommissionShare = reader.ToDecimal(commColumn),
                    GrandPrize = reader.ToDecimal(grantPrizeColumn),
                    Date = isFirstLevel ? ConvertHelper.ToDateTime(reader.ToString("reporttype"), isMonthly) : metaModel.DrillDownFromDate
                });
                ReadSummaryTotal(reader, summaryTotal);
            }

            return new SortableReportResultModel<ColossusBetWinLoss>
            {
                DataList = result,
                SummaryTotal = summaryTotal
            };
        }
    }
}