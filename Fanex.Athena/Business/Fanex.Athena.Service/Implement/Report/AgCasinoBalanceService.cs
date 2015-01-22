// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AgCasinoBalanceService.cs" company="Nexcel">
// The ag casino win loss service.
// </copyright>
// <summary>
//   The ag casino win loss service.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Fanex.Athena.Service.Implement.Report
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Athena.DataAccess.Implement.Report;
    using Fanex.Athena.DataAccess.Interface.Report;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Models.DTO.Report;
    using Fanex.Athena.Models.MetaModel.Report;
    using Fanex.Athena.Service.Implement.Configuration;
    using Fanex.Athena.Service.Interface.Configuration;
    using Fanex.Athena.Service.Interface.Report;

    /// <summary>
    ///     The licensee win loss service.
    /// </summary>
    public class AgCasinoBalanceService : IAgCasinoBalanceService
    {
        #region Constants and Fields

        /// <summary>
        ///     The _balance service.
        /// </summary>
        private readonly IBalanceUpService _balanceService;

        /// <summary>
        ///     The _licensee win loss data access.
        /// </summary>
        private readonly IAgCasinoBalanceDataAccess _licenseeBalanceDataAccess;

        #endregion Constants and Fields

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="AgCasinoBalanceService" /> class.
        /// </summary>
        public AgCasinoBalanceService()
        {
            this._balanceService = new BalanceUpService();
            this._licenseeBalanceDataAccess = new AgCasinoBalanceDataAccess(this._balanceService.BalanceUpInfo);
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
        public SortableList<AgCasinoBalance> GetAgentBalanceDetail(
            AgCasinoBalanceMetaModel metaModel,
            out IDictionary<string, AgCasinoBalance> summaryTotal)
        {
            summaryTotal = new Dictionary<string, AgCasinoBalance>();
            return metaModel.IsValid
                       ? this._licenseeBalanceDataAccess.GetAgentWinlossDetail(metaModel, out summaryTotal)
                       : new SortableList<AgCasinoBalance>();
        }

        /// <summary>
        ///     The get customer win loss detail table.
        /// </summary>
        /// <param name="metaModel">
        ///     The metal model.
        /// </param>
        /// <param name="summaryTotal">The summary total.</param>
        /// <returns>The licensee win loss list.</returns>
        public SortableList<AgCasinoBalance> GetCustomerBalanceDetail(
            AgCasinoBalanceMetaModel metaModel,
            out IDictionary<string, AgCasinoBalance> summaryTotal)
        {
            summaryTotal = new Dictionary<string, AgCasinoBalance>();
            return metaModel.IsValid
                       ? this._licenseeBalanceDataAccess.GetCustomerWinlossDetail(metaModel, out summaryTotal)
                       : new SortableList<AgCasinoBalance>();
        }

        /// <summary>
        ///     The get summary win loss table.
        /// </summary>
        /// <param name="metaModel">
        ///     The metaModel.
        /// </param>
        /// <param name="summaryTotal">The summary total.</param>
        /// <returns>The licensee win loss list.</returns>
        public SortableList<AgCasinoBalance> GetSummaryBalance(AgCasinoBalanceMetaModel metaModel, out IDictionary<string, AgCasinoBalance> summaryTotal)
        {
            summaryTotal = new Dictionary<string, AgCasinoBalance>();
            return metaModel.IsValid
                       ? this._licenseeBalanceDataAccess.GetSummaryWinloss(metaModel, out summaryTotal)
                       : new SortableList<AgCasinoBalance>();
        }

        #endregion Public Methods and Operators

        /// <summary>
        /// Calculate total win-loss.
        /// Group by currency.
        /// </summary>
        /// <param name="dataList">The data list.</param>
        /// <returns>The total list.</returns>
        public IList<AgCasinoBalance> TotalCalc(IList<AgCasinoBalance> dataList)
        {
            if (dataList == null || dataList.Count == 0)
            {
                return new SortableList<AgCasinoBalance>();
            }

            return dataList.GroupBy(d => d.CurrencyName).Select(g => new AgCasinoBalance
            {
                CurrencyName = g.Key,
                TurnOver = g.Sum(p => p.TurnOver),
                TotalTransaction = g.Sum(p => p.TotalTransaction),
                CustomerWinLoss = g.Sum(p => p.CustomerWinLoss),
                LicenseeWL = g.Sum(p => p.LicenseeWL),
                OpeningBalance = g.Sum(p => p.OpeningBalance),
                TransferIn = g.Sum(p => p.TransferIn),
                TransferOut = g.Sum(p => p.TransferOut),
                ClosingBalance = g.Sum(p => p.ClosingBalance),
            }).ToList();
        }
    }
}