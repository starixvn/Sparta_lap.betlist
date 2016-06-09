// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ThirdPartyWinLossService.cs" company="Nexcel">
// The ag casino win loss service.
// </copyright>
// <summary>
//   The third party win loss service.
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
    /// The third party win loss service.
    /// </summary>
    public class ThirdPartyWinLossService : IThirdPartyWinLossService
    {
        #region Constants and Fields

        /// <summary>
        ///     The _balance service.
        /// </summary>
        private readonly IBalanceUpService _balanceService;

        /// <summary>
        ///     The gold deluxe win loss data access.
        /// </summary>
        private readonly IThirdPartyWinLossDataAccess _thirdPartyWinLossDataAccess;

        #endregion Constants and Fields

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="ThirdPartyWinLossService" /> class.
        /// </summary>
        public ThirdPartyWinLossService()
        {
            this._balanceService = new BalanceUpService();
            this._thirdPartyWinLossDataAccess = new ThirdPartyWinLossDataAccess(this._balanceService.BalanceUpInfo);
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
        public SortableList<ThirdPartyWinLoss> GetAgentWinlossDetail(
            ThirdPartyWinLossMetaModel metaModel,
            out IDictionary<string, ThirdPartyWinLoss> summaryTotal)
        {
            summaryTotal = new Dictionary<string, ThirdPartyWinLoss>();
            return metaModel.IsValid
                       ? this._thirdPartyWinLossDataAccess.GetAgentWinlossDetail(metaModel, out summaryTotal)
                       : new SortableList<ThirdPartyWinLoss>();
        }

        /// <summary>
        ///     The get customer win loss detail table.
        /// </summary>
        /// <param name="metaModel">
        ///     The metal model.
        /// </param>
        /// <param name="summaryTotal">The summary total.</param>
        /// <returns>The licensee win loss list.</returns>
        public SortableList<ThirdPartyWinLoss> GetCustomerWinlossDetail(
            ThirdPartyWinLossMetaModel metaModel,
            out IDictionary<string, ThirdPartyWinLoss> summaryTotal)
        {
            summaryTotal = new Dictionary<string, ThirdPartyWinLoss>();
            return metaModel.IsValid
                       ? this._thirdPartyWinLossDataAccess.GetCustomerWinlossDetail(metaModel, out summaryTotal)
                       : new SortableList<ThirdPartyWinLoss>();
        }

        /// <summary>
        ///     The get summary win loss table.
        /// </summary>
        /// <param name="metaModel">
        ///     The metaModel.
        /// </param>
        /// <param name="summaryTotal">The summary total.</param>
        /// <returns>The licensee win loss list.</returns>
        public SortableList<ThirdPartyWinLoss> GetSummaryWinloss(ThirdPartyWinLossMetaModel metaModel, out IDictionary<string, ThirdPartyWinLoss> summaryTotal)
        {
            summaryTotal = new Dictionary<string, ThirdPartyWinLoss>();
            var result = this._thirdPartyWinLossDataAccess.GetSummaryWinloss(metaModel, out summaryTotal);
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
        public IList<ThirdPartyWinLoss> TotalCalc(IList<ThirdPartyWinLoss> dataList)
        {
            if (dataList == null || dataList.Count == 0)
            {
                return new SortableList<ThirdPartyWinLoss>();
            }

            return dataList.GroupBy(d => d.CurrencyName).Select(g => new ThirdPartyWinLoss
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
