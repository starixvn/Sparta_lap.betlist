// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AgCasinoWinLossService.cs" company="Nexcel">
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
    using Fanex.Athena.DataAccess.Report;
    using Fanex.Athena.DataAccess.Report.Interfaces;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Athena.Service.Core.Configuration;
    using Fanex.Athena.Service.Core.Configuration.Interfaces;
    using Fanex.Athena.Service.Interface.Report;

    /// <summary>
    ///     The ag casino win loss service.
    /// </summary>
    public class AgCasinoWinLossService : IAgCasinoWinLossService
    {
        #region Constants and Fields

        /// <summary>
        ///     The _balance service.
        /// </summary>
        private readonly IBalanceUpService _balanceService;

        /// <summary>
        ///     The _licensee win loss data access.
        /// </summary>
        private readonly IAgCasinoWinLossDataAccess _agcasinoWinLossDataAccess;

        #endregion Constants and Fields

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="AgCasinoWinLossService" /> class.
        /// </summary>
        public AgCasinoWinLossService()
        {
            this._balanceService = new BalanceUpService();
            this._agcasinoWinLossDataAccess = new AgCasinoWinLossDataAccess(this._balanceService.BalanceUpInfo);
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
        public SortableList<AgCasinoWinLoss> GetAgentWinlossDetail(
            AgCasinoWinLossMetaModel metaModel,
            out IDictionary<string, AgCasinoWinLoss> summaryTotal)
        {
            summaryTotal = new Dictionary<string, AgCasinoWinLoss>();
            return metaModel.IsValid
                       ? this._agcasinoWinLossDataAccess.GetAgentWinlossDetail(metaModel, out summaryTotal)
                       : new SortableList<AgCasinoWinLoss>();
        }

        /// <summary>
        ///     The get customer win loss detail table.
        /// </summary>
        /// <param name="metaModel">
        ///     The metal model.
        /// </param>
        /// <param name="summaryTotal">The summary total.</param>
        /// <returns>The licensee win loss list.</returns>
        public SortableList<AgCasinoWinLoss> GetCustomerWinlossDetail(
            AgCasinoWinLossMetaModel metaModel,
            out IDictionary<string, AgCasinoWinLoss> summaryTotal)
        {
            summaryTotal = new Dictionary<string, AgCasinoWinLoss>();
            return metaModel.IsValid
                       ? this._agcasinoWinLossDataAccess.GetCustomerWinlossDetail(metaModel, out summaryTotal)
                       : new SortableList<AgCasinoWinLoss>();
        }

        /// <summary>
        ///     The get summary win loss table.
        /// </summary>
        /// <param name="metaModel">
        ///     The metaModel.
        /// </param>
        /// <param name="summaryTotal">The summary total.</param>
        /// <returns>The licensee win loss list.</returns>
        public SortableList<AgCasinoWinLoss> GetSummaryWinloss(AgCasinoWinLossMetaModel metaModel, out IDictionary<string, AgCasinoWinLoss> summaryTotal)
        {
            summaryTotal = new Dictionary<string, AgCasinoWinLoss>();
            var result = this._agcasinoWinLossDataAccess.GetSummaryWinloss(metaModel, out summaryTotal);
            var date = new System.DateTime();
            var index = 1;
            foreach (var item in result)
            {
                if (date != item.Date)
                {
                    date = item.Date ?? new System.DateTime();
                    item.RowSpan = result.Count(x => x.Date == item.Date);
                    item.Index = index++;
                }
                else
                {
                    item.RowSpan = -1;
                }
            }

            return result;
        }

        #endregion Public Methods and Operators

        /// <summary>
        /// Calculate total win-loss.
        /// Group by currency.
        /// </summary>
        /// <param name="dataList">The data list.</param>
        /// <returns>The total list.</returns>
        public IList<AgCasinoWinLoss> TotalCalc(IList<AgCasinoWinLoss> dataList)
        {
            if (dataList == null || dataList.Count == 0)
            {
                return new SortableList<AgCasinoWinLoss>();
            }

            return dataList.GroupBy(d => d.CurrencyName).Select(g => new AgCasinoWinLoss
            {
                CurrencyName = g.Key,
                TurnOver = g.Sum(p => p.TurnOver),
                TotalTransaction = g.Sum(p => p.TotalTransaction),
                CustomerWinLoss = g.Sum(p => p.CustomerWinLoss),
                LicenseeWL = g.Sum(p => p.LicenseeWL)
            }).ToList();
        }
    }
}