// --------------------------------------------------------------------------
// <copyright file="AgCasinoBetSettingController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Controllers.BetSettings
{
    using System.Web.Mvc;
    using Fanex.Athena.Web.ActionFilter;
    using Model.BetSettings.MetaModel;
    using Model.Core.DTO.Common;
    using Resources;
    using Service.BetSettings;
    using Service.BetSettings.Interfaces;

    public class AgCasinoBetSettingController : BetSettingBaseController
    {
        private readonly IAgCasinoBetSettingService _betSettingService;

        public AgCasinoBetSettingController()
        {
            _betSettingService = new AgCasinoBetSettingService();
        }

        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        [FunctionAuthorization(PermissionKey = CustomerListUrl)]
        public ActionResult Index(BetSettingMetaModel metaModel)
        {
            var licenseeSession = SessionService.LicenseeSession;
            var viewModel = _betSettingService.GetViewModel(licenseeSession.LicenseeId, metaModel.CustId, licenseeSession.LicenseeName);
            AssignRequiredProperties(viewModel, metaModel);

            if (viewModel == null)
            {
                return this.Redirect("~/Utility/AccessDenied");
            }

            return View("~/Views/Customer/BetSetting/AgCasino.cshtml", viewModel);
        }

        [HttpPost]
        [FunctionAuthorization(PermissionKey = CustomerListUrl)]
        [CustomValidateAntiForgery]
        public JsonResult UpdateStatus(int custId, string nickName, int status)
        {
            if (ProductAccess.IsAllowAG == false)
            {
                return Json(new ResultInfo() { Code = -1, Message = ViewResources.AccessDeniedMsg });
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
        [FunctionAuthorization(PermissionKey = CustomerListUrl)]
        [CustomValidateAntiForgery]
        public JsonResult UpdateBetSetting(string nickName, string group)
        {
            if (ProductAccess.IsAllowAG == false)
            {
                return Json(new ResultInfo() { Code = -1, Message = ViewResources.AccessDeniedMsg });
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