// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WinLossByProductService.cs" company="Nexcel">
// The  win loss by product service.
// </copyright>
// <summary>
//   The  win loss by product service.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Fanex.Athena.Service.Implement.Report
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Athena.DataAccess.Implement.Report;
    using Fanex.Athena.DataAccess.Interface.Report;
    using Fanex.Athena.Models.DTO.Common;
    using Fanex.Athena.Models.DTO.Report;
    using Fanex.Athena.Models.MetaModel.Report;
    using Fanex.Athena.Service.Implement.Configuration;
    using Fanex.Athena.Service.Interface.Configuration;
    using Fanex.Athena.Service.Interface.Report;

    /// <summary>
    ///     The licensee win loss service.
    /// </summary>
    public class WinLossByProductService : IWinLossByProductService
    {
        #region Constants and Fields

        /// <summary>
        ///     The _balance service.
        /// </summary>
        private readonly IBalanceUpService _balanceService;

        /// <summary>
        ///     The _licensee win loss data access.
        /// </summary>
        private readonly IWinLossByProductDataAccess _winLossByProductDataAccess;

        #endregion Constants and Fields

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="WinLossByProductService" /> class.
        /// </summary>
        public WinLossByProductService()
        {
            this._balanceService = new BalanceUpService();
            this._winLossByProductDataAccess = new WinLossByProductDataAccess(this._balanceService.BalanceUpInfo);
        }

        #endregion Constructors and Destructors

        #region IWinLossByProductService Members

        /// <summary>
        /// Get win loss by products.
        /// </summary>
        /// <param name="metaModel">Meta model data.</param>
        /// <param name="summaryTotal">Summary total.</param>
        /// <param name="resultInfo">Result information.</param>
        /// <returns>List of Win loss.</returns>
        public IList<WinLossByProduct> GetWinlossByProducts(
            WinLossByProductMetaModel metaModel,
            out IDictionary<string, IList<WinLossByProduct>> summaryTotal,
            out IResultInfo resultInfo)
        {            
            if (metaModel.IsValid)
            {
                return this._winLossByProductDataAccess.GetWinlossByProducts(metaModel, out summaryTotal, out resultInfo);
            }
            else
            {
                resultInfo = new ResultInfo();
                summaryTotal = new Dictionary<string, IList<WinLossByProduct>>();
                return new List<WinLossByProduct>();
            }
        }

        public IList<WinLossByProduct> GetWinlossByProductAgents(
            WinLossByProductMetaModel metaModel,
            out IDictionary<string, IList<WinLossByProduct>> summaryTotal)
        {
            summaryTotal = new Dictionary<string, IList<WinLossByProduct>>();

            return metaModel.IsValid
                       ? this._winLossByProductDataAccess.GetWinlossByProductAgents(metaModel, out summaryTotal)
                       : new List<WinLossByProduct>();
        }

        public IList<WinLossByProduct> GetWinlossByProductCustomers(
            WinLossByProductMetaModel metaModel,
            out IDictionary<string, IList<WinLossByProduct>> summaryTotal)
        {
            summaryTotal = new Dictionary<string, IList<WinLossByProduct>>();

            return metaModel.IsValid
                       ? this._winLossByProductDataAccess.GetWinlossByProductCustomers(metaModel, out summaryTotal)
                       : new List<WinLossByProduct>();
        }

        #endregion IWinLossByProductService Members

        /// <summary>
        /// Get Total win loss.
        /// </summary>
        /// <param name="dataList">The data list.</param>
        /// <returns>The win loss list.</returns>
        public IList<WinLossByProduct> TotalCalc(IList<WinLossByProduct> dataList)
        {
            var temp = from item in dataList
                       group item by new { item.CurrencyName, item.ProductCode } into g
                       select new WinLossByProduct
                       {
                           ProductCode = g.Key.ProductCode,
                           CurrencyName = g.Key.CurrencyName,
                           AmountDue = g.Sum(i => i.AmountDue),
                           CustomerComm = g.Sum(i => i.CustomerComm),
                           CustomerWinLoss = g.Sum(i => i.CustomerWinLoss),
                           LicenseeWL = g.Sum(i => i.LicenseeWL),
                           TurnOver = g.Sum(i => i.TurnOver),
                           TotalTransaction = g.Sum(i => i.TotalTransaction),
                           LicenseeWLComm = g.Sum(i => i.LicenseeWLComm),
                       };
            return temp.OrderBy(i => i.CurrencyName).ToList();
        }
    }
}