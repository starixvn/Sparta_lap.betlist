// --------------------------------------------------------------------------
// <copyright file="AgCasinoBetSettingController.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Controllers.BetSettings
{
    using System.Web.Mvc;
    using Constant.DataDomain;
    using DataAccess.BetSettings.Interfaces;
    using Extension.Utility;
    using Fanex.Athena.Web.ActionFilter;
    using Model.BetSettings.MetaModel;
    using Model.Core.DTO.Common;
    using Resources;
    using Service.BetSettings;
    using Service.BetSettings.Interfaces;
    using Service.Core.Common.Interfaces;
    using Service.Core.Helpers;
    using Service.Core.Users.Interfaces;
    using Service.Customer.Interfaces;

    public class AgCasinoBetSettingController : BetSettingBaseController
    {
        private readonly IAgCasinoBetSettingService _betSettingService;
        
        public AgCasinoBetSettingController(
            ILanguageService languageService,
            ISessionService sessionService,
            IAppSettings appSetting,
            IClientViewHelper clientViewHelper,
            IAuthorizationService authorizationService,
            ICoreBetSettingDataAccess coreBetSettingDataAccess,
            ICustomerService customerService,
            IProductService productService,
            IAgCasinoBetSettingService betSettingService)
        : base(languageService, sessionService, appSetting, clientViewHelper, authorizationService, coreBetSettingDataAccess, customerService, productService)
        {
            _betSettingService = betSettingService;
        }

        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult Index(BetSettingMetaModel metaModel)
        {
            var licenseeSession = SessionService.LicenseeSession;
            var viewModel = _betSettingService.GetViewModel(licenseeSession.LicenseeId, metaModel.CustId, licenseeSession.LicenseeName);
            if (viewModel == null)
            {
                return Redirect("~/Utility/AccessDenied");
            }

            AssignRequiredProperties(viewModel, metaModel);
            return View("~/Views/Customer/BetSetting/AgCasino.cshtml", viewModel);
        }

        [HttpPost]
        [CustomValidateAntiForgery]
        public JsonResult UpdateStatus(int custId, string nickName, int status)
        {
            if (!ProductAccess.IsAllow(ProductTypes.AgCasino) || !IsBetSettingEditable())
            {
                return AccessDenied();
            }

            var userSession = SessionService.UserSession;
            var result = _betSettingService.UpdateStatus(
                SessionService.LicenseeSession.LicenseeId,
                userSession.UserId,
                custId,
                userSession.Language,
                Request.UserHostAddress,
                status == 1);

            if (result.Code == 0)
            {
                result.Message = string.Format(ViewResources.UpdateSuccess, nickName);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [CustomValidateAntiForgery]
        public JsonResult UpdateBetSetting(string nickName, string group)
        {
            if (!ProductAccess.IsAllow(ProductTypes.AgCasino) || !IsBetSettingEditable())
            {
                return AccessDenied();
            }

            var setting = GetBetSettingCommonParam();
            var result = _betSettingService.UpdateAgCasinoBetSetting(setting, group);

            if (result.Code == 0)
            {
                result.Message = string.Format(ViewResources.UpdateAgCasinoBetSettingSuccessMsg, nickName);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}