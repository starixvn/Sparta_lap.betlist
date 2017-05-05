// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LicenseeWinLossService.cs" company="">
// The licensee win loss service.
// </copyright>
// <summary>
//   The licensee win loss service.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Fanex.Athena.Service.Report
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Athena.DataAccess.Report;
    using Fanex.Athena.DataAccess.Report.Interfaces;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Athena.Service.Core.Configuration;
    using Fanex.Athena.Service.Core.Configuration.Interfaces;
    using Fanex.Athena.Service.Report.Interfaces;

    public class LicenseeWinLossService : ILicenseeWinLossService
    {
        private readonly IBalanceUpService _balanceService;

        private readonly ILicenseeWinLossDataAccess _licenseeWinLossDataAccess;

        public LicenseeWinLossService()
        {
            this._balanceService = new BalanceUpService();
            this._licenseeWinLossDataAccess = new LicenseeWinLossDataAccess(this._balanceService.BalanceUpInfo);
        }

        public SortableList<LicenseeWinLoss> GetAgentWinlossDetail(
            LicenseeWinLossMetaModel metaModel,
            out IDictionary<string, LicenseeWinLoss> summaryTotal)
        {
            summaryTotal = new Dictionary<string, LicenseeWinLoss>();
            return metaModel.IsValid
                       ? this._licenseeWinLossDataAccess.GetAgentWinlossDetail(metaModel, out summaryTotal)
                       : new SortableList<LicenseeWinLoss>();
        }

        public SortableList<LicenseeWinLoss> GetCustomerWinlossDetail(
            LicenseeWinLossMetaModel metaModel,
            out IDictionary<string, LicenseeWinLoss> summaryTotal)
        {
            summaryTotal = new Dictionary<string, LicenseeWinLoss>();
            return metaModel.IsValid
                       ? this._licenseeWinLossDataAccess.GetCustomerWinlossDetail(metaModel, out summaryTotal)
                       : new SortableList<LicenseeWinLoss>();
        }

        public SortableList<LicenseeWinLoss> GetSummaryWinloss(LicenseeWinLossMetaModel metaModel, out IDictionary<string, LicenseeWinLoss> summaryTotal)
        {
            summaryTotal = new Dictionary<string, LicenseeWinLoss>();
            return metaModel.IsValid
                       ? this._licenseeWinLossDataAccess.GetSummaryWinloss(metaModel, out summaryTotal)
                       : new SortableList<LicenseeWinLoss>();
        }

        public IList<LicenseeWinLoss> CalculateTotal(IList<LicenseeWinLoss> dataList)
        {
            if (dataList == null || dataList.Count == 0)
            {
                return new SortableList<LicenseeWinLoss>();
            }

            return dataList.GroupBy(d => d.CurrencyName).Select(g => new LicenseeWinLoss
            {
                CurrencyName = g.Key,
                TurnOver = g.SelectMany(k => k.TurnOver).GroupBy(j => j.Key).ToDictionary(p => p.Key, p => p.Sum(j => j.Value)),
                ActualStake = g.Sum(p => p.ActualStake),
                BuyBackAmountSB = g.Sum(p => p.BuyBackAmountSB),
                BuyBackAmountBA = g.Sum(p => p.BuyBackAmountBA),
                TotalTransaction = g.Sum(p => p.TotalTransaction),
                CustomerWinLoss = g.Sum(p => p.CustomerWinLoss),
                CustomerComm = g.Sum(p => p.CustomerComm),
                LicenseeWL = g.SelectMany(k => k.LicenseeWL).GroupBy(j => j.Key).ToDictionary(p => p.Key, p => p.Sum(j => j.Value)),
                BetTradeCommission = g.Sum(p => p.BetTradeCommission),
                CashOutCommission = g.Sum(p => p.CashOutCommission),
                BACommission = g.Sum(p => p.BACommission),
                RacingRevenue = g.Sum(p => p.RacingRevenue),
                AmtDueToBetTrade = g.Sum(p => p.AmtDueToBetTrade),
                AmtDueToBA = g.Sum(p => p.AmtDueToBA),
                AmtDueToRacing = g.Sum(p => p.AmtDueToRacing),
            }).ToList();
        }
    }
}