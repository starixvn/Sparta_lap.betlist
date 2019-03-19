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
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using DataAccess.Core.Configuration.Interfaces;
    using Extension.Utility;
    using Fanex.Athena.Model.Core.DataDomain;
    using Fanex.Athena.Service.StatelessCore.Interfaces;
    using Fanex.Athena.Web.ActionFilter;
    using Model.BetList.DTO;
    using Model.BetList.MetaModel;
    using Model.BetList.ViewModel;
    using Resources;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;
    using Service.Core.Utility.Interfaces;
    using VendorAdapter.Interfaces;

    #endregion Using

    [FunctionAuthorization(PermissionKeys.ReportCancelledBets)]
    public class CancelledBetsBetListController : BetListBaseController
    {
        private readonly IAuthorizationService _authorizationService;

#pragma warning disable S107 // Methods should not have too many parameters

        public CancelledBetsBetListController(
            ILanguageService languageService,
            ISessionService sessionService,
            IAppSettings appSetting,
            IProductService productService,
            IBalanceUpDataAccess balanceUpDataAccess,
            ICacheRefAdapter cacheRefAdapter,
            IBetListApiClient betListApiClient,
            IAuthorizationService authorizationService)
#pragma warning restore S107 // Methods should not have too many parameters
        : base(languageService, sessionService, appSetting, productService, balanceUpDataAccess, cacheRefAdapter, betListApiClient)
        {
            _authorizationService = authorizationService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var viewParam = CreateDefaultMetaModel();

            SetConvetedUSDCurrencyName(viewParam);

            var model = CreateViewModel(viewParam);

            return View("~/Views/Desktop/BetList/CancelledBetsBetList/Main.cshtml", model);
        }

        [HttpPost]
        [CustomValidateAntiForgery]
        public async Task<ActionResult> Index(BetListExcelMetaModel metaModel)
        {
            if (metaModel.IsExportExcel)
            {
                return await Excel(metaModel).ConfigureAwait(false);
            }

            var viewParam = CreateDefaultMetaModel(metaModel);

            var model = CreateViewModel(viewParam);

            return View("~/Views/Desktop/BetList/CancelledBetsBetList/Main.cshtml", model);
        }

        private CancelledBetsBetListViewModel CreateViewModel(BetListMetaModel metaModel)
        {
            metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId);
            var customers = GetData(metaModel).Result;

            if (customers.Any())
            {
                metaModel.CustId = customers.First().CustId;
            }

            var userSession = SessionService.UserSession;
            return new CancelledBetsBetListViewModel
            {
                ShowEmptyContent = true,
                Param = metaModel,
                CurrencyList = userSession.CurrencyList,
                ProductAccess = ProductService.GetProductAccess(metaModel.LicenseeId, userSession.Language),
                IsAllowedCustomerCommission = SessionService.LicenseeSession.IsAllowedCustomerComm,
                CustomerList = customers
            };
        }

        private async Task<ActionResult> Excel(BetListExcelMetaModel metaModel)
        {
            if (metaModel.CurrencyId == BaseCurrencyType.All ||
                SessionService.UserSession.CurrencyList.ContainsKey(metaModel.CurrencyId))
            {
                const string UspKey = "Lrf_BetList_SelDeleted";
                CreateDefaultMetaModel(metaModel);
                metaModel.UspKey = UspKey;

                var currencyTitle = GetViewInCurrency(metaModel);
                metaModel.ShowingOption = new BetListShowingOption
                {
                    CustomerCommVisible = metaModel.HasPlayerCom
                };
                metaModel.HiddenColumns = HiddenColumnIndexes;
                metaModel.Title = $"{ViewResources.CancelledBets} - {currencyTitle}";
                return await ProcessExportExcel(metaModel, "CancelledBetList").ConfigureAwait(false);
            }

            return EmptyExcel();
        }

        private async Task<IEnumerable<CustomerCancelBetItem>> GetData(BetListMetaModel metaModel)
        {
            if (!HasPermissionOnData(metaModel))
            {
                return new List<CustomerCancelBetItem>();
            }

            return await BetListApiClient.PostAsync<IEnumerable<CustomerCancelBetItem>>(
                "/CancelledBetsBetList/GetCustomerList",
                new
                {
                    AgentListXml = metaModel.AgentListXml
                }).ConfigureAwait(false);
        }

        private bool HasPermissionOnData(BetListMetaModel metaModel)
        {
            return _authorizationService.HasPermissionOnData(metaModel.LicenseeId, metaModel.CustId, metaModel.TransId);
        }
    }
}