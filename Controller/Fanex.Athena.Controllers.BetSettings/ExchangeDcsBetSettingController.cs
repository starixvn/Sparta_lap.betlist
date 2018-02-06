// -----------------------------------------------------------------------
// <copyright file="ExchangeComm.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.BetSettings
{
    using System.Linq;
    using System.Web.Mvc;
    using DataAccess.BetSettings.Interfaces;
    using Extension.Utility;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Web.ActionFilter;
    using Model.BetSettings.MetaModel;
    using Model.BetSettings.ViewModel;
    using Resources;
    using Service.BetSettings;
    using Service.BetSettings.Interfaces;
    using Service.Core.Common.Interfaces;
    using Service.Core.Helpers;
    using Service.Core.Users.Interfaces;
    using Service.Customer.Interfaces;

    public class ExchangeDcsBetSettingController : BetSettingBaseController
    {
        private static readonly double[] ValidComms = new[] { 2, 2.5, 3, 3.5, 4, 4.5, 5, 5.5, 6, 6.5, 7 };

        private readonly IDcsBetSettingService _betSettingService;

        private readonly IExchangeCommApiService _exchangeCommApiService;

        public ExchangeDcsBetSettingController()
        : this(null, null, null, null, null, null, null, null, null, null)
        {
        }

        public ExchangeDcsBetSettingController(
            ILanguageService languageService,
            ISessionService sessionService,
            IAppSettings appSetting,
            IClientViewHelper clientViewHelper,
            IAuthorizationService authorizationService,
            ICoreBetSettingDataAccess coreBetSettingDataAccess,
            ICustomerService customerService,
            IProductService productService,
            IDcsBetSettingService betSettingService,
            IExchangeCommApiService exchangeCommApiService)
        : base(languageService, sessionService, appSetting, clientViewHelper, authorizationService, coreBetSettingDataAccess, customerService, productService)
        {
            _betSettingService = betSettingService ?? new DcsBetSettingService();
            _exchangeCommApiService = exchangeCommApiService ?? new ExchangeCommApiService();
        }

        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult Index(BetSettingMetaModel metaModel)
        {
            var viewModel = new ExchangeBetSettingViewModel
            {
                CommissionList = ValidComms
            };
            AssignRequiredProperties(viewModel, metaModel);
            const string ViewPath = "~/Views/Customer/BetSetting/Exchange.cshtml";
            var licenseeId = SessionService.LicenseeSession.LicenseeId;
            viewModel.IsEnabled = _betSettingService.GetStatus(licenseeId, metaModel.CustId, metaModel.ProductId);
            var apiResult = _exchangeCommApiService.GetCommission(licenseeId, metaModel.CustId);
            if (apiResult.IsSuccessful)
            {
                viewModel.Commission = apiResult.ExtendedData;
            }
            else
            {
                viewModel.IsNoInfo = true;
            }

            return View(ViewPath, viewModel);
        }

        [HttpPost]
        [AjaxRequest]
        [CustomValidateAntiForgery]
        public ActionResult UpdateComm(int custId, double oldCommission, double newCommission)
        {
            if (!ValidComms.Contains(newCommission))
            {
                return Json(new ResultInfo { Code = 100, Message = ViewResources.InvalidChargeCommission });
            }

            var result = _exchangeCommApiService.UpdateCommission(
                SessionService.LicenseeSession.LicenseeId,
                SessionService.UserSession.UserId,
                custId,
                oldCommission,
                newCommission);

            return Json(result);
        }
    }
}