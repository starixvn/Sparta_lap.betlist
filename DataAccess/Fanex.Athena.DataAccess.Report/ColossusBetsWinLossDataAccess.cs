// -----------------------------------------------------------------------
// <copyright file="ColossusBetsWinLossDataAccess.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Report
{
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
            return GetData("Lrf_Rpt_ColossusBetsWinloss", metaModel, out summaryTotal);
        }

        public SortableList<ColossusBetWinLoss> GetWinlossDetail(ThirdPartyWinLossMetaModel metaModel, out IDictionary<string, ColossusBetWinLoss> summaryTotal)
        {
            return GetData("Lrf_Rpt_ColossusBetsWinloss_detail", metaModel, out summaryTotal);
        }

        #region Private

        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules", "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        private static SortableList<ColossusBetWinLoss> ReadData(
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
            var memberWinningColumn = "winningprize" + metaModel.BaseCurrency;
            var cashOutColumn = "cashoutamt" + metaModel.BaseCurrency;
            var cashOutShareColumn = "cashinamt" + metaModel.BaseCurrency;
            var grantPrizeColumn = "grandamt" + metaModel.BaseCurrency;
            var commColumn = "commamt" + metaModel.BaseCurrency;

            var hasSystemId = reader.HasColumn("systemid");
            var hasExternalId = reader.HasColumn("externalid");
            var hasCustomerId = reader.HasColumn("custid");

            var index = 1;
            while (reader.Read())
            {
                var item = new ColossusBetWinLoss
                {
                    Index = index++,
                    SystemId = hasSystemId ? reader.ToString("systemid") : string.Empty,
                    ExternalId = hasExternalId ? reader.ToString("externalid") : string.Empty,
                    CustomerId = hasCustomerId ? reader.ToInt("custid") : 0,
                    CurrencyId = reader.ToInt("currencyid"),
                    CurrencyName = reader.ToString("currencyname"),
                    TurnOver = reader.ToDecimal(turnoverColumn),
                    TotalTransaction = reader.ToInt("totaltrans"),
                    MemberWinning = reader.ToDecimal(memberWinningColumn),
                    CashOut = reader.ToDecimal(cashOutColumn),
                    CashOutShare = reader.ToDecimal(cashOutShareColumn),
                    CommissionShare = reader.ToDecimal(commColumn),
                    GrandPrize = reader.ToDecimal(grantPrizeColumn),
                    Date = metaModel.DrillDownLevel > 0 ?
                        metaModel.DrillDownFromDate :
                        ConvertHelper.ToDateTime(reader.ToString("reporttype"), metaModel.ReportBy == ReportByType.Monthly)
                };

                result.Add(item);
                foreach (var currency in SystemConstants.BaseCurrencyList)
                {
                    summaryTotal[currency].TotalTransaction += item.TotalTransaction;
                    summaryTotal[currency].TurnOver += reader.ToDecimal("turnover" + currency);
                    summaryTotal[currency].MemberWinning += reader.ToDecimal("winningprize" + currency);
                    summaryTotal[currency].CashOut += reader.ToDecimal("cashoutamt" + currency);
                    summaryTotal[currency].CashOutShare += reader.ToDecimal("cashinamt" + currency);
                    summaryTotal[currency].CommissionShare += reader.ToDecimal("commamt" + currency);
                    summaryTotal[currency].GrandPrize += reader.ToDecimal("grandamt" + currency);
                }
            }

            return result;
        }

        private SortableList<ColossusBetWinLoss> GetData(string spname, ThirdPartyWinLossMetaModel metaModel, out IDictionary<string, ColossusBetWinLoss> summaryTotal)
        {
            try
            {
                using (var db = this.GetObjectDb(spname))
                {
                    UpdateUspParams(db, metaModel, metaModel.DrillDownLevel > 0);
                    using (var reader = db.ExecuteReader())
                    {
                        return ReadData(reader, metaModel, out summaryTotal);
                    }
                }
            }
            catch (ObjectDbException sqlException)
            {
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, spname, metaModel);
            }
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