// -----------------------------------------------------------------------
// <copyright file="BetListController.CancelBetList.cs" company="Nexcel">
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
    using Fanex.Athena.Excel.Entities;
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
            metaModel.BaseCurrency = metaModel.BaseUsdLicensees.Contains(metaModel.LicenseeId.ToString()) ? "usd" : "eur";

            var licenseeSession = this.SessionService.LicenseeSession;

            var custList = this._betListService.GetCancelBetCustList(metaModel);

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
                CurrencyList = SessionService.UserSession.CurrencyList,
                LicenseeTypeList = licenseeSession.LicenseeType,
                ProductAccess = this._productService.GetProductAccess(metaModel.LicenseeId, SessionService.UserSession.Language),
                IsAllowedCustomerCommission = licenseeSession.IsAllowedCustomerComm,
                CustomerList = custList
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

            metaModel = CreateDefaultMetaModel(metaModel);

            var custList = this._betListService.GetCancelBetCustList(metaModel);

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
                CurrencyList = SessionService.UserSession.CurrencyList,
                LicenseeTypeList = licenseeSession.LicenseeType,
                ProductAccess = this._productService.GetProductAccess(metaModel.LicenseeId, SessionService.UserSession.Language),
                IsAllowedCustomerCommission = licenseeSession.IsAllowedCustomerComm,
                CustomerList = custList
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
            var currencyList = SessionService.UserSession.CurrencyList;

            if (metaModel.CurrencyId == 0 || currencyList.Values.Contains(metaModel.CurrencyId))
            {
                const string UspKey = "Lrf_BetList_SelDeleted";

                metaModel = CreateDefaultMetaModel(metaModel);

                var ticketResult = this._betListService.GetBetListData(metaModel, UspKey);

                var showingOption = new ShowingOption
                   {
                       CustomerCommVisible = SessionService.LicenseeSession.IsAllowedCustomerComm
                   };
                var currencyTitle = metaModel.IsConvert ? ViewResources.Viewin + " " + metaModel.BaseCurrency.ToUpper()
                        : ViewResources.Viewin + " " + metaModel.CurrencyName;
                var title = ViewResources.CancelledBets
                    + string.Format(" - {0}", currencyTitle);

                var hiddenColumnIdx = new List<int> { 7, 8 }; // 7: winloss, 8: licenseewl

                var excel = new Excel.BetList.BetListExcel(
                    showingOption, metaModel.LicenseeId == 1, hiddenColumnIdx);

                var workbook = excel.Export(ticketResult, title);

                return new ExcelResult(workbook, "CancelledBetList");
            }

            return File(ExcelBase.ExportEmptyFile(), "application/vnd.ms-excel", "Empty.xls");
        }
    }
}