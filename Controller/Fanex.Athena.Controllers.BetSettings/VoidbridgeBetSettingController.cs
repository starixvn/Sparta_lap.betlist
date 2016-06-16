// --------------------------------------------------------------------------
// <copyright file="VoidbridgeController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Controllers.BetSettings
{
    using System.Web.Mvc;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Web.ActionFilter;
    using Model.BetSettings.MetaModel;
    using Model.BetSettings.ViewModel;
    using Resources;
    using Service.BetSettings;
    using Service.BetSettings.Interfaces;

    public class VoidbridgeBetSettingController : BetSettingBaseController
    {
        private readonly IVoidbridgeBetSettingService _betSettingService;

        public VoidbridgeBetSettingController()
        {
            _betSettingService = new VoidbridgeBetSettingService();
        }

        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult Index(BetSettingMetaModel metaModel)
        {
            var viewModel = new BetSettingBaseViewModel()
            {
                IsEnabled = _betSettingService.GetStatus(SessionService.LicenseeSession.LicenseeId, metaModel.CustId),
            };
            AssignRequiredProperties(viewModel, metaModel);

            return View("~/Views/Customer/BetSetting/Voidbridge.cshtml", viewModel);
        }

        [HttpPost]
        [CustomValidateAntiForgery]
        public JsonResult UpdateStatus(int custId, string nickName, int status)
        {
            if (ProductAccess.IsAllowVB == false)
            {
                return Json(new ResultInfo() { Code = -1, Message = ViewResources.AccessDeniedMsg });
            }

            var userSession = SessionService.UserSession;
            var result = _betSettingService.UpdateStatus(
                SessionService.LicenseeSession.LicenseeId,
                userSession.UserId, 
                custId, 
                Request.UserHostAddress, 
                status == 1);

            if (result.Code == 0)
            {
                result.Message = string.Format(ViewResources.UpdateSuccess, nickName);
            }

            return Json(result);
        }
    }
}