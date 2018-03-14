// -----------------------------------------------------------------------
// <copyright file="CancelledBetsBetListController.cs" company="">
// Report controller
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.BetList
{
    #region Using

    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using DataAccess.BetList;
    using DataAccess.BetList.Interfaces;
    using DataAccess.Core.Configuration.Interfaces;
    using Extension.Utility;
    using Fanex.Athena.Excel.Base.Entities;
    using Fanex.Athena.Model.BetList.MetaModel;
    using Fanex.Athena.Model.BetList.ViewModel;
    using Fanex.Athena.Model.Core.DataDomain;
    using Fanex.Athena.Web.ActionFilter;
    using Model.BetList.DTO;
    using Resources;
    using Service.BetList.Interfaces;
    using Service.Core.Common.Interfaces;
    using Service.Core.Helpers;
    using Service.Core.Users;
    using Service.Core.Users.Interfaces;
    using VendorAdapter.Interfaces;

    #endregion Using

    [FunctionAuthorization(PermissionKeys.ReportCancelledBets)]
    public class CancelledBetsBetListController : BetListBaseController
    {
        private readonly IAuthorizationService _authorizationService;

        private readonly ICancelledBetListDataAccess _cancelledBetListDataAccess;

        public CancelledBetsBetListController()
        {
            _authorizationService = new AuthorizationService();
            _cancelledBetListDataAccess = new CancelledBetListDataAccess();
        }

        public CancelledBetsBetListController(
            ILanguageService languageService,
            ISessionService sessionService,
            IAppSettings appSetting,
            IClientViewHelper clientViewHelper,
            IProductService productService,
            IBetListService betListService,
            IBalanceUpDataAccess balanceUpDataAccess,
            ICacheRefAdapter cacheRefAdapter,
            IAuthorizationService authorizationService,
            ICancelledBetListDataAccess cancelledBetListDataAccess)
        : base(languageService, sessionService, appSetting, clientViewHelper, productService, betListService, balanceUpDataAccess, cacheRefAdapter)
        {
            _authorizationService = authorizationService;
            _cancelledBetListDataAccess = cancelledBetListDataAccess;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var viewParam = CreateDefaultMetaModel();

            SetConvetedUSDCurrencyName(viewParam);

            var model = CreateViewModel(viewParam);

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

            var viewParam = CreateDefaultMetaModel(metaModel);

            var model = CreateViewModel(viewParam);

            return View("~/Views/BetList/CancelledBetsBetList/Main.cshtml", model);
        }

        private CancelledBetsBetListViewModel CreateViewModel(BetListMetaModel viewParam)
        {
            viewParam.AgentListXml = SessionService.GenerateAgentListXml(viewParam.CurrencyId);
            var customers = GetData(viewParam);

            if (customers.Any())
            {
                viewParam.CustId = customers.First().CustId;
            }

            var userSession = SessionService.UserSession;
            return new CancelledBetsBetListViewModel
            {
                ShowEmptyContent = true,
                Param = viewParam,
                CurrencyList = userSession.CurrencyList,
                ProductAccess = ProductService.GetProductAccess(viewParam.LicenseeId, userSession.Language),
                IsAllowedCustomerCommission = SessionService.LicenseeSession.IsAllowedCustomerComm,
                CustomerList = customers
            };
        }

        private ActionResult Excel(BetListMetaModel metaModel)
        {
            if (metaModel.CurrencyId == BaseCurrencyType.All ||
                SessionService.UserSession.CurrencyList.ContainsKey(metaModel.CurrencyId))
            {
                const string UspKey = "Lrf_BetList_SelDeleted";
                var excelModel = CreateDefaultMetaModel(metaModel);
                excelModel.AgentListXml = SessionService.GenerateAgentListXml(excelModel.CurrencyId);

                var ticketResult = BetListService.GetBetListData(excelModel, UspKey);

                var showingOption = new ShowingOption
                {
                    CustomerCommVisible = SessionService.LicenseeSession.IsAllowedCustomerComm
                };
                var currencyTitle = GetViewInCurrency(excelModel);
                var title = $"{ViewResources.CancelledBets} - {currencyTitle}";

                return ProcessExportExcel(showingOption, excelModel, ticketResult, HiddenColumnIndexes, title, "CancelledBetList");
            }

            return EmptyExcel();
        }

        private IEnumerable<CustomerCancelBetItem> GetData(BetListMetaModel metaModel)
        {
            return HasPermissionOnData(metaModel) ?
                _cancelledBetListDataAccess.GetCancelBetCustList(metaModel) :
                new List<CustomerCancelBetItem>();
        }

        private bool HasPermissionOnData(BetListMetaModel metaModel)
        {
            return _authorizationService.HasPermissionOnData(metaModel.LicenseeId, metaModel.CustId, metaModel.TransId);
        }
    }
}