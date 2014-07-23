// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MobileWinLossService.cs" company="Nexcel">
// The mobile win loss service.
// </copyright>
// <summary>
//   The mobile win loss service.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Fanex.Athena.Service.Implement.Report
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Athena.DataAccess.Implement.Report;
    using Fanex.Athena.Models.DTO.Report;
    using Fanex.Athena.Models.MetaModel.Report;
    using Fanex.Athena.Service.Exception;
    using Fanex.Athena.Service.Implement.Configuration;
    using Fanex.Athena.Service.Interface.Configuration;
    using Fanex.Athena.Service.Interface.Report;

    /// <summary>
    ///     The mobile win loss service.
    /// </summary>
    public class MobileWinLossService : IMobileWinLossService
    {
        #region Constants and Fields

        /// <summary>
        ///     The _balance up service.
        /// </summary>
        private readonly IBalanceUpService _balanceUpService;

        /// <summary>
        ///     The _mobile win loss data access.
        /// </summary>
        private readonly MobileWinLossDataAccess _mobileWinLossDataAccess;

        #endregion Constants and Fields

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="MobileWinLossService" /> class.
        /// </summary>
        public MobileWinLossService()
        {
            this._balanceUpService = new BalanceUpService();
            this._mobileWinLossDataAccess = new MobileWinLossDataAccess(this._balanceUpService.BalanceUpInfo);
        }

        #endregion Constructors and Destructors

        #region Public Methods and Operators

        /// <summary>
        ///     The get data list.
        /// </summary>
        /// <param name="metaModel"> The metal model. </param>
        /// <param name="summaryTotal"> The total in EUR. </param>
        /// <returns>The mobile win loss list.</returns>
        /// <exception cref="ServiceException">The service exception.</exception>
        public IList<MobileWinLoss> GetMasterData(MobileWinLossMetaModel metaModel, out Dictionary<string, MobileWinLoss> summaryTotal)
        {
            summaryTotal = new Dictionary<string, MobileWinLoss>();
            return metaModel.IsValid
                       ? this._mobileWinLossDataAccess.GetMasterData(metaModel, out summaryTotal)
                       : new List<MobileWinLoss>();
        }

        /// <summary>
        /// Re-structure data list.
        /// </summary>
        /// <param name="dataList">The original data list.</param>
        /// <returns>The mobile win-loss data list.</returns>
        public IDictionary<string, IList<MobileWinLoss>> ReStructuredData(IList<MobileWinLoss> dataList)
        {
            IDictionary<string, IList<MobileWinLoss>> result = new Dictionary<string, IList<MobileWinLoss>>();

            var keyList = dataList.Select(p => p.Date).Distinct();
            foreach (var item in keyList)
            {
                result.Add(item.ToString("MM/dd/yyyy"), dataList.Where(p => p.Date.Equals(item)).ToList());
            }

            return result;
        }

        /// <summary>
        ///     The total Mobile win loss.
        /// </summary>
        /// <param name="dataList"> The data list. </param>
        /// <returns> The mobile win loss list.</returns>
        public IList<MobileWinLoss> TotalCalc(IList<MobileWinLoss> dataList)
        {
            return dataList.GroupBy(d => d.CurrencyName).Select(g => new MobileWinLoss
                {
                    CurrencyName = g.Key,
                    Turnover = g.Sum(p => p.Turnover),
                    TotalTransactions = g.Sum(p => p.TotalTransactions),
                    CustomerWinLoss = g.Sum(p => p.CustomerWinLoss),
                    CustomerComm = g.Sum(p => p.CustomerComm),
                    LicenseeWL = g.Sum(p => p.LicenseeWL),
                    BACommission = g.Sum(p => p.BACommission),
                    RacingRevenue = g.Sum(p => p.RacingRevenue),
                    AmtDueToBA = g.Sum(p => p.AmtDueToBA),
                    AmtDueToRacing = g.Sum(p => p.AmtDueToRacing)
                })
                           .ToList();
        }

        #endregion Public Methods and Operators
    }
}