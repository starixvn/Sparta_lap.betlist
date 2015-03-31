// -----------------------------------------------------------------------
// <copyright file="BetListController.DailyBetList.cs" company="Nexcel">
// Report controller
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.BetList
{
    #region Using

    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.Athena.Excel.Base;
    using Fanex.Athena.Excel.BetList;
    using Fanex.Athena.Excel.Entities;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Models.DataDomain;
    using Fanex.Athena.Models.MetaModel.BetList;
    using Fanex.Athena.Models.ViewModel;
    using Fanex.Athena.Models.ViewModel.BetList;
    using Fanex.Athena.Web.ActionFilter;
    using Fanex.Athena.Web.ResultExtension;

    #endregion Using

    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    public partial class BetListController
    {
        private const string BetListDailyBetListName = "BetList_DailyBetList";

        /// <summary>
        /// Action Get for daily bet list.
        /// </summary>
        /// <param name="metaModel">The metal model.</param>
        /// <returns>The action result.</returns>
        [HttpGet]
        [FunctionAuthorization]
        public ActionResult DailyBetList(BetListMetaModel metaModel)
        {
            var licenseeSession = SessionService.LicenseeSession;
            var userSession = SessionService.UserSession;
            metaModel.BalanceUpDay = this._balanceUpService.BalanceUpInfo.BalanceUpDay;
            metaModel.LicenseeId = licenseeSession.LicenseeId;
            metaModel.UserId = userSession.UserId;

            if (metaModel.IsExportExcel)
            {
                var currencyList = SessionService.UserSession.CurrencyList;

                if (metaModel.CurrencyId == BaseCurrencyType.All || currencyList.Values.Contains(metaModel.CurrencyId))
                {
                    return DailyBetListExcel(metaModel);
                }

                return File(ExcelBase.ExportEmptyFile(), "application/vnd.ms-excel", "Empty.xls");
            }

            SetConvetedUSDCurrencyName(metaModel);
            var model = new BetListBaseViewModel
            {
                ShowEmptyContent = true,
                Param = metaModel,
                CurrencyList = userSession.CurrencyList,
                LicenseeTypeList = licenseeSession.LicenseeType,
                ProductAccess = this._productService.GetProductAccess(licenseeSession.LicenseeId, userSession.Language, Util.GetAllowedProducts(BetListDailyBetListName)),
                IsAllowedCustomerCommission = licenseeSession.IsAllowedCustomerComm
            };

            return View("~/Views/BetList/DailyBetList/Main.cshtml", model);
        }

        /// <summary>
        /// Export excel for daily bet list.
        /// Export all data .
        /// </summary>
        /// <param name="metaModel">The metal model.</param>
        /// <returns>The action result.</returns>
        [NonAction]
        [FunctionAuthorization]
        public ActionResult DailyBetListExcel(BetListMetaModel metaModel)
        {
            const string UspKey = "Lrf_BetList_SelWinlossDaily";

            metaModel.IsServerPaging = true;
            metaModel.PageSize = MaxPageSizeDailyBetlist;
            metaModel.PageIndex = 1;
            var ticketResult = this._betListService.GetBetListData(metaModel, UspKey);

            // Sum value by page
            ticketResult.SumWinLoss = ticketResult.MainTicketList.Sum(x => x.Winlost);
            ticketResult.SumLicenseeWL = ticketResult.MainTicketList.Sum(x => x.LicenseeWL);
            ticketResult.SumComm = ticketResult.MainTicketList.Sum(x => x.Comm);
            ticketResult.SumPlayerComm = ticketResult.MainTicketList.Sum(x => x.PlayerComm);

            var showingOption = new ShowingOption
               {
                   CustomerCommVisible = SessionService.LicenseeSession.IsAllowedCustomerComm,
               };
            var currencyTitle = string.Format(
                ViewResources.Viewin,
                metaModel.IsConvert ? metaModel.BaseCurrency.ToUpper() : metaModel.CurrencyName);
            var title = ViewResources.DailyBetList + " " + ViewResources.On
                + string.Format(" {0} - {1}", metaModel.Date.ToString("MM/dd/yyyy"), currencyTitle);

            var dailyBetListExcel = new BetListExcel(showingOption, metaModel.LicenseeId == 1, new List<int> { BetEffectiveColumn });
            var workbook = dailyBetListExcel.Export(ticketResult, title);

            return new ExcelResult(workbook, "DailyBetList");
        }
    }
}