// -----------------------------------------------------------------------
// <copyright file="CancelledBetsBetListController.cs" company="">
// Report controller
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.BetList
{
    using System.Collections.Generic;

    #region Using

    using System.Linq;
    using System.Web.Mvc;
    using DataAccess.BetList;
    using DataAccess.BetList.Interfaces;
    using Fanex.Athena.Excel.Base.Entities;
    using Fanex.Athena.Excel.BetList;
    using Fanex.Athena.Model.BetList.MetaModel;
    using Fanex.Athena.Model.BetList.ViewModel;
    using Fanex.Athena.Model.Core.DataDomain;
    using Fanex.Athena.Web.ActionFilter;
    using Fanex.Athena.Web.ResultExtension;
    using Model.BetList.DTO;
    using Resources;

    #endregion Using

    [FunctionAuthorization(PermissionKeys.ReportCancelledBets)]
    public class CancelledBetsBetListController : BetListBaseController
    {
        private readonly ICancelledBetListDataAccess _cancelledBetListDataAccess;

        public CancelledBetsBetListController()
        {
            _cancelledBetListDataAccess = new CancelledBetListDataAccess();
        }

        [HttpGet]
        public ActionResult Index()
        {
            var metaModel = CreateDefaultMetaModel();

            SetConvetedUSDCurrencyName(metaModel);

            var licenseeSession = this.SessionService.LicenseeSession;
            var userSession = this.SessionService.UserSession;
            metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId);
            var customers = GetData(metaModel);

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
                CurrencyList = userSession.CurrencyList,
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

            var viewParam = CreateDefaultMetaModel(metaModel);
            viewParam.AgentListXml = SessionService.GenerateAgentListXml(viewParam.CurrencyId);
            var custList = GetData(viewParam);

            if (custList.Any())
            {
                var customerCancelBetItem = custList.FirstOrDefault();
                if (customerCancelBetItem != null)
                {
                    viewParam.CustId = customerCancelBetItem.CustId;
                }
            }

            var model = new CancelledBetsBetListViewModel
            {
                ShowEmptyContent = true,
                Param = viewParam,
                CurrencyList = userSession.CurrencyList,
                ProductAccess = ProductService.GetProductAccess(viewParam.LicenseeId, userSession.Language),
                IsAllowedCustomerCommission = licenseeSession.IsAllowedCustomerComm,
                CustomerList = custList,
            };

            return View("~/Views/BetList/CancelledBetsBetList/Main.cshtml", model);
        }

        private ActionResult Excel(BetListMetaModel metaModel)
        {
            if (metaModel.CurrencyId == BaseCurrencyType.All ||
                SessionService.UserSession.CurrencyList.ContainsKey(metaModel.CurrencyId))
            {
                const string UspKey = "Lrf_BetList_SelDeleted";
                var excelModel = CreateDefaultMetaModel(metaModel);
                excelModel.AgentListXml = SessionService.GenerateAgentListXml(excelModel.CurrencyId);

                var ticketResult = this.BetListService.GetBetListData(excelModel, UspKey);

                var showingOption = new ShowingOption
                {
                    CustomerCommVisible = SessionService.LicenseeSession.IsAllowedCustomerComm
                };
                var currencyTitle = string.Format(
                    ViewResources.Viewin,
                    excelModel.IsConvert ? excelModel.BaseCurrency.ToUpperInvariant() : excelModel.CurrencyName);
                var title = ViewResources.CancelledBets
                    + string.Format(" - {0}", currencyTitle);

                var excel = new BetListExcel(
                    showingOption, excelModel.LicenseeId, HiddenColumnIndexes);

                var workbook = excel.Export(ticketResult, title);

                return new ExcelResult(workbook, "CancelledBetList");
            }

            return EmptyExcel();
        }

        private IEnumerable<CustomerCancelBetItem> GetData(BetListMetaModel metaModel)
        {
            return HasPermissionOnData(metaModel) ?
                _cancelledBetListDataAccess.GetCancelBetCustList(metaModel) :
                new List<CustomerCancelBetItem>();
        }
    }
}