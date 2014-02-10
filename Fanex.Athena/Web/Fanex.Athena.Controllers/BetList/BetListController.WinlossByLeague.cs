// -----------------------------------------------------------------------
// <copyright file="BetListController.WinlossByLeague.cs" company="Nexcel">
// Report controller
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.BetList
{
    #region Using

    using System.Collections.Generic;
    using System.Web.Mvc;
    using Fanex.Athena.Excel.Base;
    using Fanex.Athena.Excel.BetList;
    using Fanex.Athena.Excel.Entities;
    using Fanex.Athena.Models.DataDomain;
    using Fanex.Athena.Models.MetaModel.BetList;
    using Fanex.Athena.Models.ViewModel;
    using Fanex.Athena.Web.ResultExtension;

    #endregion Using

    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    public partial class BetListController
    {
        /// <summary>
        /// Action Get for licensee win lost.
        /// </summary>
        /// <param name="metaModel">The meta model.</param>
        /// <returns>Action result.</returns>        
        public ActionResult WinlossByLeague(BetListMetaModel metaModel)
        {
            metaModel = CreateDefaultMetaModel(metaModel);

            if (!metaModel.IsExportExcel)
            {
                return this.View("~/Views/BetList/WinlossByLeague/Main.cshtml", metaModel);
            }

            var currencyList = this.SessionService.UserSession.CurrencyList;

            if (metaModel.CurrencyId == 0 || currencyList.Values.Contains(metaModel.CurrencyId))
            {
                return this.WinlossByLeagueExcel(metaModel);
            }

            return this.File(ExcelBase.ExportEmptyFile(), "application/vnd.ms-excel", "Empty.xls");
        }

        /// <summary>
        /// Export excel for licensee win lost.
        /// </summary>
        /// <param name="metaModel">The meta model.</param>
        /// <returns>Workbook for excel.</returns>
        [NonAction]
        public ActionResult WinlossByLeagueExcel(BetListMetaModel metaModel)
        {
            const string UspKey = "Lrf_BetList_WinlostByLeague_Mem";
            var ticketResult = this._betListService.GetBetListData(metaModel, UspKey);           

            // determine currency viewed in
            string viewinCurrency;
            var curencyId = metaModel.IsConvert ? (metaModel.BaseCurrency.ToLower() == "eur" ? -1 : -2) : metaModel.CurrencyId;
            switch ((BaseCurrencyType)curencyId)
            {
                case BaseCurrencyType.All:
                    viewinCurrency = ViewResources.AllCurrencies;
                    break;

                case BaseCurrencyType.Eur:
                    viewinCurrency = ViewResources.Viewin + ": " + FormatKey.Eur;
                    break;

                case BaseCurrencyType.Usd:
                    viewinCurrency = ViewResources.Viewin + ": " + FormatKey.Usd;
                    break;

                default:
                    viewinCurrency = ViewResources.Viewin + ": " + metaModel.CurrencyName.ToUpper();
                    break;
            }
            
            var title = string.Format(
                "{0} - {1} → {2} - {3}({4}) - {5}",
                ViewResources.CustomerBetList,
                metaModel.FromDate.ToString(FormatKey.ShortFormatDate),
                metaModel.ToDate.ToString(FormatKey.ShortFormatDate),   
                metaModel.ExternalId,
                metaModel.SystemId,                             
                viewinCurrency);

            var colList = new List<int> { 1, 2 };

            var excel = new BetListExcel(new ShowingOption(), metaModel.LicenseeId == 1, colList);

            var workbook = excel.Export(ticketResult, title);

            return new ExcelResult(workbook, "WinLossByLeagueBetList");
        }
    }
}