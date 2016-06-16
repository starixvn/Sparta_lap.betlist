// -----------------------------------------------------------------------
// <copyright file="CancelledBetsBetListController.cs" company="Nexcel">
// Report controller
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.BetList
{
    #region Using

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

    [FunctionAuthorization(PermissionKeys.ReportCancelledBets)]
    public class CancelledBetsBetListController : BetListBaseController
    {
        [HttpGet]
        public ActionResult Index()
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

        [HttpPost]
        [CustomValidateAntiForgery]
        public ActionResult Index(BetListMetaModel metaModel)
        {
            if (metaModel.IsExportExcel)
            {
                return Excel(metaModel);
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

        private ActionResult Excel(BetListMetaModel metaModel)
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