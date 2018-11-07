// --------------------------------------------------------------------------
// <copyright file="GoldDeluxeBetSettingController.cs" company="">
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
    using Service.BetSettings.Interfaces;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;
    using Service.Customer.Interfaces;

    public class GoldDeluxeBetSettingController : BetSettingBaseController
    {
        private readonly IGoldDeluxeBetSettingService _betSettingService;

#pragma warning disable S107 // Methods should not have too many parameters
        public GoldDeluxeBetSettingController(
            ILanguageService languageService,
            ISessionService sessionService,
            IAppSettings appSetting,
            IAuthorizationService authorizationService,
            ICoreBetSettingDataAccess coreBetSettingDataAccess,
            ICustomerService customerService,
            IProductService productService,
            IGoldDeluxeBetSettingService betSettingService)
#pragma warning restore S107 // Methods should not have too many parameters
        : base(languageService, sessionService, appSetting, authorizationService, coreBetSettingDataAccess, customerService, productService)
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
                return this.Redirect("~/Utility/AccessDenied");
            }

            AssignRequiredProperties(viewModel, metaModel);
            return View("~/Views/Customer/BetSetting/GoldDeluxe.cshtml", viewModel);
        }

        [HttpPost]
        [CustomValidateAntiForgery]
        public JsonResult UpdateStatus(int custId, string nickName, int status)
        {
            if (!ProductAccess.IsAllow(ProductTypes.GoldDeluxe) || !IsBetSettingEditable())
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

            if (result.Code == 0 && string.IsNullOrWhiteSpace(result.Message))
            {
                result.Message = string.Format(ViewResources.UpdateSuccess, nickName);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [CustomValidateAntiForgery]
        public JsonResult UpdateBetSetting(int custId, string nickName, string group)
        {
            if (!ProductAccess.IsAllow(ProductTypes.GoldDeluxe) || !IsBetSettingEditable())
            {
                return AccessDenied();
            }

            var licenseeSession = SessionService.LicenseeSession;
            var result = _betSettingService.UpdateBetSetting(licenseeSession.LicenseeId, licenseeSession.LicenseeName, custId, group);

            if (result.Code == 0)
            {
                result.Message = string.Format(ViewResources.UpdateGoldDeluxeBetSettingSuccessMsg, nickName);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}