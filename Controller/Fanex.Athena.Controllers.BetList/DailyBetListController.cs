// -----------------------------------------------------------------------
// <copyright file="DailyBetListController.cs" company="">
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
    using Fanex.Athena.Excel.Base.Entities;
    using Fanex.Athena.Excel.BetList;
    using Fanex.Athena.Model.BetList.MetaModel;
    using Fanex.Athena.Model.BetList.ViewModel;
    using Fanex.Athena.Model.Core.DataDomain;
    using Fanex.Athena.Web.ActionFilter;
    using Fanex.Athena.Web.ResultExtension;
    using Resources;

    #endregion Using

    [FunctionAuthorization(PermissionKeys.ReportDailyBetList)]
    public class DailyBetListController : BetListBaseController
    {
        private const string BetListDailyBetListName = "BetList_DailyBetList";

        [HttpGet]
        public ActionResult Index(BetListMetaModel metaModel)
        {
            var licenseeSession = SessionService.LicenseeSession;
            var userSession = SessionService.UserSession;
            metaModel.BalanceUpDay = this.BalanceUpService.BalanceUpInfo.BalanceUpDay;
            metaModel.LicenseeId = licenseeSession.LicenseeId;
            metaModel.UserId = userSession.UserId;
            var currencies = userSession.CurrencyList;

            if (metaModel.IsExportExcel)
            {
                if (metaModel.CurrencyId == BaseCurrencyType.All || currencies.ContainsKey(metaModel.CurrencyId))
                {
                    return Excel(metaModel);
                }

                return File(ExcelBase.ExportEmptyFile(), "application/vnd.ms-excel", "Empty.xls");
            }

            SetConvetedUSDCurrencyName(metaModel);
            var model = new BetListBaseViewModel
            {
                ShowEmptyContent = true,
                Param = metaModel,
                CurrencyList = currencies,
                LicenseeTypeList = licenseeSession.LicenseeType,
                ProductAccess = this.ProductService.GetSingleWalletProductAccess(licenseeSession.LicenseeId, userSession.Language, BetListDailyBetListName),
                IsAllowedCustomerCommission = licenseeSession.IsAllowedCustomerComm
            };

            return View("~/Views/BetList/DailyBetList/Main.cshtml", model);
        }

        private ActionResult Excel(BetListMetaModel metaModel)
        {
            const string UspKey = "Lrf_BetList_SelWinlossDaily";

            metaModel.IsServerPaging = true;
            metaModel.PageSize = MaxPageSizeDailyBetlist;
            metaModel.PageIndex = 1;
            var ticketResult = this.BetListService.GetBetListData(metaModel, UspKey);

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