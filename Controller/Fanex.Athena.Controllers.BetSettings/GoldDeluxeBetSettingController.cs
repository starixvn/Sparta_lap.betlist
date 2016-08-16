// --------------------------------------------------------------------------
// <copyright file="GoldDeluxeBetSettingController.cs" company="">
// Copyright (c) . All rights reserved.
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

    public class GoldDeluxeBetSettingController : BetSettingBaseController
    {
        private readonly IGoldDeluxeBetSettingService _betSettingService;

        public GoldDeluxeBetSettingController()
        {
            _betSettingService = new GoldDeluxeBetSettingService();
        }

        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult Index(BetSettingMetaModel metaModel)
        {
            var licenseeSession = SessionService.LicenseeSession;
            var viewModel = _betSettingService.GetViewModel(licenseeSession.LicenseeId, metaModel.CustId, licenseeSession.LicenseeName);
            AssignRequiredProperties(viewModel, metaModel);
            if (viewModel == null)
            {
                return this.Redirect("~/Utility/AccessDenied");
            }

            return View("~/Views/Customer/BetSetting/GoldDeluxe.cshtml", viewModel);
        }

        [HttpPost]
        [CustomValidateAntiForgery]
        public JsonResult UpdateStatus(int custId, string nickName, int status)
        {
            if (ProductAccess.IsAllowGD == false || !IsBetSettingEditable())
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
            if (ProductAccess.IsAllowGD == false || !IsBetSettingEditable())
            {
                return Json(new ResultInfo() { Code = -1, Message = ViewResources.AccessDeniedMsg });
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