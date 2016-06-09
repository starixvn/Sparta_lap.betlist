// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LicenseeWinLossService.cs" company="Nexcel">
// The licensee win loss service.
// </copyright>
// <summary>
//   The licensee win loss service.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Fanex.Athena.Service.Implement.Report
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
    using Fanex.Athena.Service.Interface.Report;

    /// <summary>
    ///     The licensee win loss service.
    /// </summary>
    public class LicenseeWinLossService : ILicenseeWinLossService
    {
        #region Constants and Fields

        /// <summary>
        ///     The _balance service.
        /// </summary>
        private readonly IBalanceUpService _balanceService;

        /// <summary>
        ///     The _licensee win loss data access.
        /// </summary>
        private readonly ILicenseeWinLossDataAccess _licenseeWinLossDataAccess;

        #endregion Constants and Fields

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="LicenseeWinLossService" /> class.
        /// </summary>
        public LicenseeWinLossService()
        {
            this._balanceService = new BalanceUpService();
            this._licenseeWinLossDataAccess = new LicenseeWinLossDataAccess(this._balanceService.BalanceUpInfo);
        }

        #endregion Constructors and Destructors

        #region Public Methods and Operators

        /// <summary>
        ///     The get agent win loss detail table.
        /// </summary>
        /// <param name="metaModel">
        ///     The metal model.
        /// </param>
        /// <param name="summaryTotal">The summary total.</param>
        /// <returns>The licensee win loss list.</returns>
        public SortableList<LicenseeWinLoss> GetAgentWinlossDetail(
            LicenseeWinLossMetaModel metaModel,
            out IDictionary<string, LicenseeWinLoss> summaryTotal)
        {
            summaryTotal = new Dictionary<string, LicenseeWinLoss>();
            return metaModel.IsValid
                       ? this._licenseeWinLossDataAccess.GetAgentWinlossDetail(metaModel, out summaryTotal)
                       : new SortableList<LicenseeWinLoss>();
        }

        /// <summary>
        ///     The get customer win loss detail table.
        /// </summary>
        /// <param name="metaModel">
        ///     The metal model.
        /// </param>
        /// <param name="summaryTotal">The summary total.</param>
        /// <returns>The licensee win loss list.</returns>
        public SortableList<LicenseeWinLoss> GetCustomerWinlossDetail(
            LicenseeWinLossMetaModel metaModel,
            out IDictionary<string, LicenseeWinLoss> summaryTotal)
        {
            summaryTotal = new Dictionary<string, LicenseeWinLoss>();
            return metaModel.IsValid
                       ? this._licenseeWinLossDataAccess.GetCustomerWinlossDetail(metaModel, out summaryTotal)
                       : new SortableList<LicenseeWinLoss>();
        }

        /// <summary>
        ///     The get summary win loss table.
        /// </summary>
        /// <param name="metaModel">
        ///     The metaModel.
        /// </param>
        /// <param name="summaryTotal">The summary total.</param>
        /// <returns>The licensee win loss list.</returns>
        public SortableList<LicenseeWinLoss> GetSummaryWinloss(LicenseeWinLossMetaModel metaModel, out IDictionary<string, LicenseeWinLoss> summaryTotal)
        {
            summaryTotal = new Dictionary<string, LicenseeWinLoss>();
            return metaModel.IsValid
                       ? this._licenseeWinLossDataAccess.GetSummaryWinloss(metaModel, out summaryTotal)
                       : new SortableList<LicenseeWinLoss>();
        }

        #endregion Public Methods and Operators

        /// <summary>
        /// Calculate total win-loss.
        /// Group by currency.
        /// </summary>
        /// <param name="dataList">The data list.</param>
        /// <returns>The total list.</returns>
        public IList<LicenseeWinLoss> TotalCalc(IList<LicenseeWinLoss> dataList)
        {
            if (dataList == null || dataList.Count == 0)
            {
                return new SortableList<LicenseeWinLoss>();
            }

            return dataList.GroupBy(d => d.CurrencyName).Select(g => new LicenseeWinLoss
            {
                CurrencyName = g.Key,
                TurnOver = g.SelectMany(k => k.TurnOver)
                .GroupBy(j => j.Key)
                .ToDictionary(p => p.Key, p => p.Sum(j => j.Value)),
                ActualStake = g.Sum(p => p.ActualStake),
                BuyBackAmountSB = g.Sum(p => p.BuyBackAmountSB),
                BuyBackAmountBA = g.Sum(p => p.BuyBackAmountBA),
                TotalTransaction = g.Sum(p => p.TotalTransaction),
                CustomerWinLoss = g.Sum(p => p.CustomerWinLoss),
                CustomerComm = g.Sum(p => p.CustomerComm),
                LicenseeWL = g.SelectMany(k => k.LicenseeWL)
                .GroupBy(j => j.Key)
                .ToDictionary(p => p.Key, p => p.Sum(j => j.Value)),
                BetTradeCommission = g.Sum(p => p.BetTradeCommission),
                BACommission = g.Sum(p => p.BACommission),
                RacingRevenue = g.Sum(p => p.RacingRevenue),
                AmtDueToBetTrade = g.Sum(p => p.AmtDueToBetTrade),
                AmtDueToBA = g.Sum(p => p.AmtDueToBA),
                AmtDueToRacing = g.Sum(p => p.AmtDueToRacing),
            }).ToList();
        }
    }
}