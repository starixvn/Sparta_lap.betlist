// -----------------------------------------------------------------------
// <copyright file="ExchangeComm.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.BetSettings
{
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Extension.Utility;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Web.ActionFilter;
    using Model.BetSettings.MetaModel;
    using Model.BetSettings.ViewModel;
    using Resources;
    using Service.BetSettings.Interfaces;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;

    public class ExchangeDcsBetSettingController : BetSettingBaseController
    {
        private static readonly double[] ValidComms = new[] { 2, 2.5, 3, 3.5, 4, 4.5, 5, 5.5, 6, 6.5, 7 };

        private readonly IDcsBetSettingService _betSettingService;

        private readonly IExchangeCommApiService _exchangeCommApiService;

        public ExchangeDcsBetSettingController(
            ILanguageService languageService,
            ISessionService sessionService,
            IAppSettings appSetting,
            ICoreBetSettingService coreBetSettingService,
            IProductService productService,
            IDcsBetSettingService betSettingService,
            IExchangeCommApiService exchangeCommApiService)
        : base(languageService, sessionService, appSetting, coreBetSettingService, productService)
        {
            _betSettingService = betSettingService;
            _exchangeCommApiService = exchangeCommApiService;
        }

        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public async Task<ActionResult> Index(BetSettingMetaModel metaModel)
        {
            var viewModel = new ExchangeBetSettingViewModel
            {
                CommissionList = ValidComms
            };
            await AssignRequiredProperties(viewModel, metaModel).ConfigureAwait(false);
            const string ViewPath = "~/Views/Desktop/Customer/BetSetting/Exchange.cshtml";
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