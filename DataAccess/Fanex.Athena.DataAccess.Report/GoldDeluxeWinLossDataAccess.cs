namespace Fanex.Athena.DataAccess.Report
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Fanex.Athena.DataAccess.Report.Interfaces;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Model.Core.DataDomain;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Common.Extensions;
    using Fanex.Data;
    using Model.Core.ResultModel;
    using Session.Interfaces;

    public class GoldDeluxeWinLossDataAccess : ThirdPartyWinLossDataAccess<GoldDeluxeWinLoss>, IGoldDeluxeWinLossDataAccess
    {
        public GoldDeluxeWinLossDataAccess(IAppSettings appSetting, ISessionStorage sessionStorage)
            : base(appSetting, sessionStorage)
        {
        }

        public ReportResultModel<GoldDeluxeWinLoss> GetSummaryWinloss(ThirdPartyWinLossMetaModel metaModel)
        {
            return GetSummaryWinloss(metaModel, "Lrf_Rpt_GoldDeluxeWinloss");
        }

        public ReportResultModel<GoldDeluxeWinLoss> GetCustomerWinlossDetail(ThirdPartyWinLossMetaModel metaModel)
        {
            return GetCustomerWinlossDetail(metaModel, "Lrf_Rpt_GoldDeluxeWinloss_Mem");
        }

        public ReportResultModel<GoldDeluxeWinLoss> GetAgentWinlossDetail(ThirdPartyWinLossMetaModel metaModel)
        {
            return GetAgentWinlossDetail(metaModel, "Lrf_Rpt_GoldDeluxeWinloss_Ag");
        }

        protected override void ReadSummaryTotal(IDataReader reader, IDictionary<string, GoldDeluxeWinLoss> summaryTotal)
        {
            foreach (var currency in SystemConstants.BaseCurrencyList)
            {
                var item = summaryTotal[currency];
                item.CustomerWinLoss += reader.ToDecimal("winlost" + currency);
                item.TotalTransaction += reader.ToInt("totaltrans");
                item.LicenseeWL += reader.ToDecimal("licenseewinlost" + currency);
                item.TurnOver += reader.ToDecimal("turnover" + currency);
                item.TipAmount += reader.ToDecimal("tipsamount" + currency);
                item.TipSharing += reader.ToDecimal("tipssharing" + currency);
                item.AmountDue += reader.ToDecimal("amountdue" + currency);
            }
        }

        [CLSCompliant(false)]
        protected override void UpdateUspParams(IObjectDb db, ThirdPartyWinLossMetaModel metaModel, bool drilldown)
        {
            base.UpdateUspParams(db, metaModel, drilldown);
            db.AddInParameter("licenseeId", DbType.Int32, metaModel.LicenseeId);
        }

        protected override void PostReadItem(IDataReader reader, GoldDeluxeWinLoss item, string baseCurrency)
        {
            item.TipAmount = reader.ToDecimal("tipsamount" + baseCurrency);
            item.TipSharing = reader.ToDecimal("tipssharing" + baseCurrency);
            item.AmountDue = reader.ToDecimal("amountdue" + baseCurrency);
        }

        protected override IEnumerable<GoldDeluxeWinLoss> CalculateTotal(IEnumerable<GoldDeluxeWinLoss> dataList)
        {
            if (dataList == null || !dataList.Any())
            {
                return new List<GoldDeluxeWinLoss>();
            }

            return dataList.GroupBy(d => d.CurrencyName).Select(g => new GoldDeluxeWinLoss
            {
                CurrencyName = g.Key,
                TurnOver = g.Sum(p => p.TurnOver),
                TotalTransaction = g.Sum(p => p.TotalTransaction),
                CustomerWinLoss = g.Sum(p => p.CustomerWinLoss),
                LicenseeWL = g.Sum(p => p.LicenseeWL),
                TipAmount = g.Sum(p => p.TipAmount),
                TipSharing = g.Sum(p => p.TipSharing),
                AmountDue = g.Sum(p => p.AmountDue)
            }).ToList();
        }
    }
}