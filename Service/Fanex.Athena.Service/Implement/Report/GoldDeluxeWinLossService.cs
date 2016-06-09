// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GoldDeluxeWinLossService.cs" company="Nexcel">
// The ag casino win loss service.
// </copyright>
// <summary>
//   The gold deluxe win loss service.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Fanex.Athena.Service.Implement.Report
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Athena.DataAccess.Implement.Report;
    using Fanex.Athena.DataAccess.Interface.Report;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Athena.Service.Core.Configuration;
    using Fanex.Athena.Service.Core.Configuration.Interfaces;
    using Fanex.Athena.Service.Interface.Report;

    /// <summary>
    ///     The gold deluxe win loss service.
    /// </summary>
    public class GoldDeluxeWinLossService : IGoldDeluxeWinLossService
    {
        #region Constants and Fields

        /// <summary>
        ///     The _balance service.
        /// </summary>
        private readonly IBalanceUpService _balanceService;

        /// <summary>
        ///     The gold deluxe win loss data access.
        /// </summary>
        private readonly IGoldDeluxeWinLossDataAccess _goldDeluxeWinLossDataAccess;

        #endregion Constants and Fields

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="GoldDeluxeWinLossService" /> class.
        /// </summary>
        public GoldDeluxeWinLossService()
        {
            this._balanceService = new BalanceUpService();
            this._goldDeluxeWinLossDataAccess = new GoldDeluxeWinLossDataAccess(this._balanceService.BalanceUpInfo);
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
        public SortableList<GoldDeluxeWinLoss> GetAgentWinlossDetail(
            GoldDeluxeWinLossMetaModel metaModel,
            out IDictionary<string, GoldDeluxeWinLoss> summaryTotal)
        {
            summaryTotal = new Dictionary<string, GoldDeluxeWinLoss>();
            return metaModel.IsValid
                       ? this._goldDeluxeWinLossDataAccess.GetAgentWinlossDetail(metaModel, out summaryTotal)
                       : new SortableList<GoldDeluxeWinLoss>();
        }

        /// <summary>
        ///     The get customer win loss detail table.
        /// </summary>
        /// <param name="metaModel">
        ///     The metal model.
        /// </param>
        /// <param name="summaryTotal">The summary total.</param>
        /// <returns>The licensee win loss list.</returns>
        public SortableList<GoldDeluxeWinLoss> GetCustomerWinlossDetail(
            GoldDeluxeWinLossMetaModel metaModel,
            out IDictionary<string, GoldDeluxeWinLoss> summaryTotal)
        {
            summaryTotal = new Dictionary<string, GoldDeluxeWinLoss>();
            return metaModel.IsValid
                       ? this._goldDeluxeWinLossDataAccess.GetCustomerWinlossDetail(metaModel, out summaryTotal)
                       : new SortableList<GoldDeluxeWinLoss>();
        }

        /// <summary>
        ///     The get summary win loss table.
        /// </summary>
        /// <param name="metaModel">
        ///     The metaModel.
        /// </param>
        /// <param name="summaryTotal">The summary total.</param>
        /// <returns>The licensee win loss list.</returns>
        public SortableList<GoldDeluxeWinLoss> GetSummaryWinloss(GoldDeluxeWinLossMetaModel metaModel, out IDictionary<string, GoldDeluxeWinLoss> summaryTotal)
        {
            summaryTotal = new Dictionary<string, GoldDeluxeWinLoss>();
            var result = this._goldDeluxeWinLossDataAccess.GetSummaryWinloss(metaModel, out summaryTotal);
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
        public IList<GoldDeluxeWinLoss> TotalCalc(IList<GoldDeluxeWinLoss> dataList)
        {
            if (dataList == null || dataList.Count == 0)
            {
                return new SortableList<GoldDeluxeWinLoss>();
            }

            return dataList.GroupBy(d => d.CurrencyName).Select(g => new GoldDeluxeWinLoss
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