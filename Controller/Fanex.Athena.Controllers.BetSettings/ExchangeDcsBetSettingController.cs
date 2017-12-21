// -----------------------------------------------------------------------
// <copyright file="ExchangeComm.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.BetSettings
{
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Web.ActionFilter;
    using Model.BetSettings.MetaModel;
    using Model.BetSettings.ViewModel;
    using Resources;
    using Service.BetSettings;
    using Service.BetSettings.Interfaces;

    public class ExchangeDcsBetSettingController : BetSettingBaseController
    {
        private static readonly decimal[] ValidComms = new[] { 2, 2.5m, 3, 3.5m, 4, 4.5m, 5, 5.5m, 6, 6.5m, 7 };

        private readonly IDcsBetSettingService _betSettingService;

        private readonly IExchangeCommApiService _exchangeCommApiService;

        public ExchangeDcsBetSettingController() : this(null, null)
        {
        }

        public ExchangeDcsBetSettingController(
            IDcsBetSettingService betSettingService,
            IExchangeCommApiService exchangeCommApiService)
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
            var apiResult = _exchangeCommApiService.GetCommission(licenseeId, metaModel.CustId, metaModel.CurrencyId);
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
        public ActionResult UpdateComm(int custId, decimal commission)
        {
            if (!ValidComms.Contains(commission))
            {
                return Json(new ResultInfo { Code = 100, Message = ViewResources.InvalidChargeCommission });
            }

            var result = _exchangeCommApiService.UpdateCommission(
                SessionService.LicenseeSession.LicenseeId,
                SessionService.UserSession.UserId,
                custId,
                commission);
            
            return Json(result);
        }
    }
}