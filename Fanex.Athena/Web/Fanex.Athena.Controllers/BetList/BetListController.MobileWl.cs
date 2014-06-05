// -----------------------------------------------------------------------
// <copyright file="BetListController.MobileWl.cs" company="Nexcel">
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
        /// <param name="metaModel">Meta model.</param>
        /// <returns>View main.</returns>
        [HttpGet]
        public ActionResult MobileWl(BetListMetaModel metaModel)
        {
            metaModel = CreateDefaultMetaModel(metaModel);

            if (metaModel.IsExportExcel)
            {
                var currencyList = SessionService.UserSession.CurrencyList;

                if (metaModel.CurrencyId == BaseCurrencyType.All || currencyList.Values.Contains(metaModel.CurrencyId))
                {
                    return this.MobileWlExcel(metaModel);
                }

                return File(ExcelBase.ExportEmptyFile(), "application/vnd.ms-excel", "Empty.xls");
            }

            return View("~/Views/BetList/MobileWl/Main.cshtml", metaModel);
        }

        /// <summary>
        /// Export excel for licensee win lost.
        /// </summary>
        /// <param name="metaModel">Meta model.</param>
        /// <returns>Workbook for excel.</returns>
        [NonAction]
        public ActionResult MobileWlExcel(BetListMetaModel metaModel)
        {
            const string UspKey = "Lrf_BetList_MobileReport_SelWinloss_Mem";
            var ticketResult = this._betListService.GetBetListData(metaModel, UspKey);
            var showingOption = new ShowingOption
            {
                CustomerCommVisible = SessionService.LicenseeSession.IsAllowedCustomerComm,
            };

            string viewinCurrency;
            var curencyId = metaModel.IsConvert 
                ? (metaModel.BaseCurrency.ToLower() == "eur" ? BaseCurrencyType.Eur : BaseCurrencyType.Usd) : metaModel.CurrencyId;
            switch (curencyId)
            {
                case BaseCurrencyType.All:
                    viewinCurrency = ViewResources.AllCurrencies;
                    break;

                case BaseCurrencyType.Eur:
                    viewinCurrency = string.Format(ViewResources.Viewin, FormatKey.Eur);
                    break;

                case BaseCurrencyType.Usd:
                    viewinCurrency = string.Format(ViewResources.Viewin, FormatKey.Usd);
                    break;

                default:
                    viewinCurrency = string.Format(ViewResources.Viewin, metaModel.CurrencyName);
                    break;
            }

            var siteList = new List<string> { string.Empty, ViewResources.Wap, ViewResources.SmartPhone, ViewResources.Others };
            var siteText = siteList[metaModel.SiteType];
            var title = ViewResources.Customer + " " + ViewResources.BetList
                + string.Format(
                " - {0} --> {1} - {2}({3}) - {4} - {5}",
                metaModel.FromDate.ToString(FormatKey.ShortFormatDate),
                metaModel.ToDate.ToString(FormatKey.ShortFormatDate),
                metaModel.ExternalId,
                metaModel.SystemId,
                ViewResources.Site + ": " + siteText,
                viewinCurrency);

            var colList = new List<int> { 1, 2 };

            var excel = new BetListExcel(showingOption, metaModel.LicenseeId == 1, colList);

            var workbook = excel.Export(ticketResult, title);

            return new ExcelResult(workbook, "MobileWinLossBetList");
        }
    }
}