// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MicroGamingWinLossService.cs" company="Nexcel">
// The ag casino win loss service.
// </copyright>
// <summary>
//   The third party win loss service.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Fanex.Athena.Service.Implement.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Athena.DataAccess.Implement.Report;
    using Fanex.Athena.DataAccess.Interface.Report;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Models.DTO.Report;
    using Fanex.Athena.Models.MetaModel.Report;
    using Fanex.Athena.Service.Core.Configuration;
    using Fanex.Athena.Service.Core.Configuration.Interfaces;
    using Fanex.Athena.Service.Interface.Report;

    /// <summary>
    /// The MicroGaming win loss service.
    /// </summary>
    public class MicroGamingWinLossService : IMicroGamingWinLossService
    {
        #region Constants and Fields

        private readonly IBalanceUpService _balanceService;

        private readonly IMicroGamingWinLossDataAccess _microGamingWinLossDataAccess;

        #endregion Constants and Fields

        #region Constructors and Destructors

        public MicroGamingWinLossService()
        {
            this._balanceService = new BalanceUpService();
            this._microGamingWinLossDataAccess = new MicroGamingWinLossDataAccess(this._balanceService.BalanceUpInfo);
        }

        #endregion Constructors and Destructors

        #region Public Methods and Operators

        public SortableList<MicroGamingWinLoss> GetSummaryWinloss(MicroGamingWinLossMetaModel metaModel, out IDictionary<string, MicroGamingWinLoss> summaryTotal)
        {
            summaryTotal = new Dictionary<string, MicroGamingWinLoss>();

            return metaModel.IsValid ?
                this._microGamingWinLossDataAccess.GetSummaryWinloss(metaModel, out summaryTotal) :
                new SortableList<MicroGamingWinLoss>();
        }

        #endregion Public Methods and Operators

        /// <summary>
        /// Calculate total win-loss.
        /// Group by currency.
        /// </summary>
        /// <param name="dataList">The data list.</param>
        /// <returns>The total list.</returns>
        public IList<MicroGamingWinLoss> TotalCalc(IList<MicroGamingWinLoss> dataList)
        {
            if (dataList == null || dataList.Count == 0)
            {
                return new SortableList<MicroGamingWinLoss>();
            }

            return dataList.GroupBy(d => d.CurrencyName).Select(g => new MicroGamingWinLoss
            {
                CurrencyName = g.Key,
                TurnOver = g.Sum(p => p.TurnOver),
                TotalTransaction = g.Sum(p => p.TotalTransaction),
                CustomerWinLoss = g.Sum(p => p.CustomerWinLoss),
                ProgressiveContribution = g.Sum(p => p.ProgressiveContribution),
                ProgressivePayout = g.Sum(p => p.ProgressivePayout),
                LicenseeWL = g.Sum(p => p.LicenseeWL),
                LicenseeNetWin = g.Sum(p => p.LicenseeNetWin),
                AmountDueToMGC = g.Sum(p => p.AmountDueToMGC),
                ProgresssiveFee = g.Sum(p => p.ProgresssiveFee)
            }).ToList();
        }
    }
}
