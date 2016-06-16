// --------------------------------------------------------------------------
// <copyright file="RacingBetSettingController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Controllers.BetSettings
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Fanex.Athena.Web.ActionFilter;
    using Model.BetSettings.DTO;
    using Model.BetSettings.MetaModel;
    using Model.Core.DTO.Common;
    using Resources;
    using Service.BetSettings;
    using Service.BetSettings.Interfaces;

    public class RacingBetSettingController : BetSettingBaseController
    {
        private readonly IRacingBetSettingService _betSettingService;

        public RacingBetSettingController()
        {
            _betSettingService = new RacingBetSettingService();
        }

        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        [FunctionAuthorization(PermissionKey = CustomerListUrl)]
        public ActionResult Index(BetSettingMetaModel metaModel)
        {
            var isAllowSaToteCurrency = ProductService.IsAllowedSaToteCurrency(metaModel.CustId);
            var viewModel = _betSettingService.GetViewModel(SessionService.LicenseeSession.LicenseeId, metaModel.CustId, isAllowSaToteCurrency);
            AssignRequiredProperties(viewModel, metaModel);

            return View("~/Views/Customer/BetSetting/Main.cshtml", viewModel);
        }

        [HttpPost]
        [FunctionAuthorization(PermissionKey = CustomerListUrl)]
        [CustomValidateAntiForgery]
        public JsonResult UpdateBetSetting(string nickName)
        {
            if (ProductAccess.IsAllowRB == false)
            {
                return Json(new ResultInfo() { Code = -1, Message = ViewResources.AccessDeniedMsg });
            }

            var racing = GetBetSettingCommonParam();
            var raceTypes = new string[] { "151_31", "151_41", "151_51", "152_41", "152_51", "153_51" };
            var betSettings = new Dictionary<string, RacingBetSetting>();
            foreach (var item in raceTypes)
            {
                betSettings.Add(item, GetParam(item, racing.CustId, racing));
            }

            var result = _betSettingService.UpdateBetSetting(SessionService.LicenseeSession.LicenseeId, betSettings);

            if (result["Code"].Length == 0)
            {
                result["Message"] = string.Format(ViewResources.UpdateRacingBetSettingSuccessMsg, nickName);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        private RacingBetSetting GetParam(string raceType, int custId, CommonBetSetting data)
        {
            var betSetting = new RacingBetSetting(custId, data)
            {
                IsDataChange = !string.IsNullOrEmpty(HttpContext.Request["minbet" + raceType])
                && !string.IsNullOrEmpty(HttpContext.Request["maxbet" + raceType]) && !string.IsNullOrEmpty(HttpContext.Request["maxpermatch" + raceType])
            };

            if (betSetting.IsDataChange)
            {
                betSetting.MinBet = GetDecimalRequest("minbet" + raceType, 0);
                betSetting.MaxBet = GetDecimalRequest("maxbet" + raceType, 0);
                betSetting.MaxPerMatch = GetDecimalRequest("maxpermatch" + raceType, 0);
            }

            return betSetting;
        }
    }
}