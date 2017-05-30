// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WinLossByProductService.cs" company="">
// The  win loss by product service.
// </copyright>
// <summary>
//   The  win loss by product service.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Fanex.Athena.Service.Report
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Athena.DataAccess.Report;
    using Fanex.Athena.DataAccess.Report.Interfaces;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Athena.Service.Core.Configuration;
    using Fanex.Athena.Service.Core.Configuration.Interfaces;
    using Fanex.Athena.Service.Report.Interfaces;

    public class WinLossByProductService : IWinLossByProductService
    {
        private readonly IBalanceUpService _balanceService;

        private readonly IWinLossByProductDataAccess _winLossByProductDataAccess;

        public WinLossByProductService()
        {
            this._balanceService = new BalanceUpService();
            this._winLossByProductDataAccess = new WinLossByProductDataAccess(this._balanceService.BalanceUpInfo);
        }

        #region IWinLossByProductService Members

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

        public IList<WinLossByProduct> TotalCalc(IList<WinLossByProduct> dataList)
        {
            var temp = from item in dataList
                       group item by new { item.CurrencyName, item.ProductCode } into g
                       select new WinLossByProduct
                       {
                           ProductCode = g.Key.ProductCode,
                           ProductName = g.First().ProductName,
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