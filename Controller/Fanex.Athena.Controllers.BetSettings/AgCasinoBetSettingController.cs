// --------------------------------------------------------------------------
// <copyright file="AgCasinoBetSettingController.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Controllers.BetSettings
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Constant.DataDomain;
    using Extension.Utility;
    using Fanex.Athena.Web.ActionFilter;
    using Model.BetSettings.MetaModel;
    using Resources;
    using Service.BetSettings.Interfaces;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;

    public class AgCasinoBetSettingController : BetSettingBaseController
    {
        private readonly IAgCasinoBetSettingService _betSettingService;

        public AgCasinoBetSettingController(
            ILanguageService languageService,
            ISessionService sessionService,
            IAppSettings appSetting,
            ICoreBetSettingService coreBetSettingService,
            IProductService productService,
            IAgCasinoBetSettingService betSettingService)
        : base(languageService, sessionService, appSetting, coreBetSettingService, productService)
        {
            _betSettingService = betSettingService;
        }

        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public async Task<ActionResult> Index(BetSettingMetaModel metaModel)
        {
            var licenseeSession = SessionService.LicenseeSession;
            var viewModel = await _betSettingService.GetViewModelAsync(licenseeSession.LicenseeId, metaModel.CustId, licenseeSession.LicenseeName);
            if (viewModel == null)
            {
                return Redirect("~/Utility/AccessDenied");
            }

            await AssignRequiredProperties(viewModel, metaModel).ConfigureAwait(false);
            return View("~/Views/Desktop/Customer/BetSetting/AgCasino.cshtml", viewModel);
        }

        [HttpPost]
        [CustomValidateAntiForgery]
        public async Task<JsonResult> UpdateStatus(int custId, string nickName, int status)
        {
            var userSession = SessionService.UserSession;
            var licenseeId = SessionService.LicenseeSession.LicenseeId;

            if (!(await CheckPermission(userSession, ProductTypes.AgCasino).ConfigureAwait(false)))
            {
                return AccessDenied();
            }

            var result = await _betSettingService.UpdateStatusAsync(
                licenseeId,
                userSession.UserId,
                custId,
                userSession.Language,
                Request.UserHostAddress,
                status == 1).ConfigureAwait(false);

            if (result.Code == 0)
            {
                result.Message = string.Format(ViewResources.UpdateSuccess, nickName);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [CustomValidateAntiForgery]
        public async Task<JsonResult> UpdateBetSetting(string nickName, string group)
        {
            var userSession = SessionService.UserSession;
            var licenseeId = SessionService.LicenseeSession.LicenseeId;

            if (!(await CheckPermission(userSession, ProductTypes.AgCasino).ConfigureAwait(false)))
            {
                return AccessDenied();
            }

            var setting = GetBetSettingCommonParam(userSession.UserId, licenseeId, userSession.Language);
            var result = await _betSettingService.UpdateAgCasinoBetSettingAsync(setting, group).ConfigureAwait(false);

            if (result.Code == 0)
            {
                result.Message = string.Format(ViewResources.UpdateAgCasinoBetSettingSuccessMsg, nickName);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}