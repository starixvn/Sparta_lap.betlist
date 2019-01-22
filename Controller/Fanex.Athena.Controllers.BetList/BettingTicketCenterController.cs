namespace Fanex.Athena.Controllers.BetList
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Constant.DataDomain;
    using DataAccess.Core.Configuration.Interfaces;
    using Extension.Utility;
    using Model.BetList.ViewModel;
    using Model.Core.DataDomain;
    using Model.Core.MetaModel.Product;
    using Resources;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;
    using Web.ActionFilter;

    [FunctionAuthorization(PermissionKeys.BettingTicketCenter)]
    public class BettingTicketCenterController : BaseController
    {
        private readonly IProductService _productService;

        private readonly IBalanceUpDataAccess _balanceUpDataAccess;

        public BettingTicketCenterController(
            ILanguageService languageService,
            ISessionService sessionService,
            IAppSettings appSetting,
            IProductService productService,
            IBalanceUpDataAccess balanceUpDataAccess)
        : base(languageService, sessionService, appSetting)
        {
            _productService = productService;
            _balanceUpDataAccess = balanceUpDataAccess;
        }

        public ActionResult Index()
        {
            return MultipleCustomers();
        }

        public ActionResult MultipleCustomers()
        {
            var viewModel = new BettingTicketCenterViewModel
            {
                CurrencyList = GetCurrencyList(),
                LiveStatusList = GetLiveStatusList(),
                StatusList = GetStatusList(),
                ProductOptionList = GetProductOptionList(true),
                MinDate = _balanceUpDataAccess.GetBalanceUp().BalanceUpDay,
                IsAllowedCustomerComm = SessionService.LicenseeSession.IsAllowedCustomerComm
            };
            return View("~/Views/Desktop/BetList/BettingTicketCenter/MultiCustomer.cshtml", viewModel);
        }

        public ActionResult SingleCustomer()
        {
            var viewModel = new BettingTicketCenterViewModel
            {
                LiveStatusList = GetLiveStatusList(),
                StatusList = GetStatusList(),
                PlatformList = GetPlatformList(),
                ProductOptionList = GetProductOptionList(),
                IsAllowedCustomerComm = SessionService.LicenseeSession.IsAllowedCustomerComm
            };
            return View("~/Views/Desktop/BetList/BettingTicketCenter/SingleCustomer.cshtml", viewModel);
        }

        public ActionResult RefNo()
        {
            return View(
                "~/Views/Desktop/BetList/BettingTicketCenter/RefNo.cshtml",
                SessionService.LicenseeSession.IsAllowedCustomerComm);
        }

        public ActionResult Running()
        {
            var viewModel = new BettingTicketCenterViewModel
            {
                CurrencyList = GetCurrencyList(),
                ProductOptionList = GetProductOptionList(),
                IsAllowedCustomerComm = SessionService.LicenseeSession.IsAllowedCustomerComm
            };

            return View("~/Views/Desktop/BetList/BettingTicketCenter/Running.cshtml", viewModel);
        }

        private static IDictionary<int, string> GetStatusList()
        {
            return new Dictionary<int, string>
            {
                { StatusTypes.Won, ViewResources.Won },
                { StatusTypes.Lose, ViewResources.Lose },
                { StatusTypes.Draw, ViewResources.Draw },
                { StatusTypes.Void, ViewResources.Void },
                { StatusTypes.Reject, ViewResources.Reject },
                { StatusTypes.Refund, ViewResources.Refund },
            };
        }

        private static IDictionary<int, string> GetPlatformList()
        {
            return new Dictionary<int, string>
            {
                { 0, ViewResources.All },
                { PlatformType.Asia, ViewResources.Asia },
                { PlatformType.China, ViewResources.China },
                { PlatformType.Euro, ViewResources.Euro },
                { PlatformType.Wap, ViewResources.Wap },
                { PlatformType.Mobile, ViewResources.Mobile },
                { PlatformType.Tablet, ViewResources.Tablet },
                { PlatformType.NewAsia, ViewResources.NewAsia },
                { PlatformType.MobileApp, ViewResources.MobileApp },
                { PlatformType.MobileLiteVersion, ViewResources.MobileLiteVersion }
            };
        }

        private static IDictionary<int, string> GetLiveStatusList()
        {
            return new Dictionary<int, string>
            {
                { LiveStatusTypes.All, ViewResources.All },
                { LiveStatusTypes.NonLive, ViewResources.NoneLive },
                { LiveStatusTypes.Live, ViewResources.Live },
                { LiveStatusTypes.Mix, ViewResources.MixLiveAndNoneLive }
            };
        }

        private IDictionary<int, string> GetCurrencyList()
        {
            return SessionService.UserSession.CurrencyList;
        }

        private IEnumerable<ProductOption> GetProductOptionList(bool isMultiCustomersTab = false)
        {
            var productAccess = _productService.GetSingleWalletProductAccess(
                SessionService.LicenseeSession.LicenseeId,
                SessionService.UserSession.Language,
                isMultiCustomersTab ? FunctionNames.BettingTicketCenterMultiCustomers: FunctionNames.BettingTicketCenter);

            return productAccess.ProductList.Select(i => new ProductOption
            {
                Code = i.ProductCode,
                Name = i.ProductName,
                Status = true
            }).ToList();
        }
    }
}