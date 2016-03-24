// -----------------------------------------------------------------------
// <copyright file="BetListController.CancelBetList.cs" company="Nexcel">
// Report controller
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.BetList
{
    #region Using

    using System.Linq;
    using System.Web.Mvc;
    using Fanex.Athena.Excel.Base;
    using Fanex.Athena.Excel.BetList;
    using Fanex.Athena.Excel.Entities;
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
        /// <summary>
        /// Cancelled bet list.
        /// </summary>
        /// <returns>Action result.</returns>
        [HttpGet]
        [FunctionAuthorization]
        public ActionResult CancelledBetsBetList()
        {
            var metaModel = CreateDefaultMetaModel();

            SetConvetedUSDCurrencyName(metaModel);

            var licenseeSession = this.SessionService.LicenseeSession;
            var userSession = this.SessionService.UserSession;
            var customers = this.BetListService.GetCancelBetCustList(metaModel);

            if (customers.Any())
            {
                var customerCancelBetItem = customers.FirstOrDefault();
                if (customerCancelBetItem != null)
                {
                    metaModel.CustId = customerCancelBetItem.CustId;
                }
            }

            var model = new CancelledBetsBetListViewModel
            {
                ShowEmptyContent = true,
                Param = metaModel,
                CurrencyList = GetUserCurrencies(),
                LicenseeTypeList = licenseeSession.LicenseeType,
                ProductAccess = this.ProductService.GetProductAccess(metaModel.LicenseeId, userSession.Language),
                IsAllowedCustomerCommission = licenseeSession.IsAllowedCustomerComm,
                CustomerList = customers
            };

            return View("~/Views/BetList/CancelledBetsBetList/Main.cshtml", model);
        }

        /// <summary>
        /// Cancelled bet list.
        /// </summary>
        /// <param name="metaModel">The bet list metal model.</param>
        /// <returns>The action result.</returns>
        [HttpPost]
        public ActionResult CancelledBetsBetList(BetListMetaModel metaModel)
        {
            if (metaModel.IsExportExcel)
            {
                return CancelledBetsBetListExcel(metaModel);
            }

            var licenseeSession = this.SessionService.LicenseeSession;
            var userSession = this.SessionService.UserSession;

            metaModel = CreateDefaultMetaModel(metaModel);

            var custList = this.BetListService.GetCancelBetCustList(metaModel);

            if (custList.Any())
            {
                var customerCancelBetItem = custList.FirstOrDefault();
                if (customerCancelBetItem != null)
                {
                    metaModel.CustId = customerCancelBetItem.CustId;
                }
            }

            var model = new CancelledBetsBetListViewModel
            {
                ShowEmptyContent = true,
                Param = metaModel,
                CurrencyList = GetUserCurrencies(),
                LicenseeTypeList = licenseeSession.LicenseeType,
                ProductAccess = this.ProductService.GetProductAccess(metaModel.LicenseeId, userSession.Language),
                IsAllowedCustomerCommission = licenseeSession.IsAllowedCustomerComm,
                CustomerList = custList,
            };

            return View("~/Views/BetList/CancelledBetsBetList/Main.cshtml", model);
        }

        /// <summary>
        /// Action for cancelled bets export excel.
        /// </summary>
        /// <param name="metaModel">The bet list metal model.</param>
        /// <returns>The action result.</returns>
        [NonAction]
        public ActionResult CancelledBetsBetListExcel(BetListMetaModel metaModel)
        {
            var currencies = GetUserCurrencies();

            if (metaModel.CurrencyId == BaseCurrencyType.All || currencies.Values.Contains(metaModel.CurrencyId))
            {
                const string UspKey = "Lrf_BetList_SelDeleted";

                metaModel = CreateDefaultMetaModel(metaModel);

                var ticketResult = this.BetListService.GetBetListData(metaModel, UspKey);

                var showingOption = new ShowingOption
                   {
                       CustomerCommVisible = SessionService.LicenseeSession.IsAllowedCustomerComm
                   };
                var currencyTitle = string.Format(
                    ViewResources.Viewin,
                    metaModel.IsConvert ? metaModel.BaseCurrency.ToUpper() : metaModel.CurrencyName);
                var title = ViewResources.CancelledBets
                    + string.Format(" - {0}", currencyTitle);

                var excel = new BetListExcel(
                    showingOption, metaModel.LicenseeId == 1, HiddenColumnIndexes);

                var workbook = excel.Export(ticketResult, title);

                return new ExcelResult(workbook, "CancelledBetList");
            }

            return File(ExcelBase.ExportEmptyFile(), "application/vnd.ms-excel", "Empty.xls");
        }
    }
}