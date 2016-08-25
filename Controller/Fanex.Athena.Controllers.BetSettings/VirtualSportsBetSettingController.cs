// --------------------------------------------------------------------------
// <copyright file="VirtualSportsBetSettingController.cs" company="">
// Copyright (c) . All rights reserved.
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

    public class VirtualSportsBetSettingController : BetSettingBaseController
    {
        private readonly IVirtualSportsBetSettingService _betSettingService;

        public VirtualSportsBetSettingController()
        {
            _betSettingService = new VirtualSportsBetSettingService();
        }

        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult Index(BetSettingMetaModel metaModel)
        {
            var viewModel = _betSettingService.GetViewModel(SessionService.LicenseeSession.LicenseeId, metaModel.CustId);
            AssignRequiredProperties(viewModel, metaModel);
            return View("~/Views/Customer/BetSetting/Main.cshtml", viewModel);
        }

        [HttpPost]
        [CustomValidateAntiForgery]
        public JsonResult UpdateBetSetting(string nickName)
        {
            if (ProductAccess.IsAllowVS == false || !IsBetSettingEditable())
            {
                return Json(new ResultInfo() { Code = -1, Message = ViewResources.AccessDeniedMsg });
            }

            var commonParam = GetBetSettingCommonParam();
            var parameters = GetParameters();

            if (parameters.Count == 0 || commonParam.UserId == 0 || commonParam.CustId == 0)
            {
                return Json(new ResultInfo { Code = 0, Message = ViewResources.InvalidParams }, JsonRequestBehavior.AllowGet);
            }

            var result = _betSettingService.UpdateBetSetting(
                commonParam.LicenseeId,
                commonParam.CustId,
                commonParam.UserId,
                commonParam.Language,
                commonParam.Ip,
                parameters);

            if (result.Code == 0)
            {
                result.Message = string.Format(ViewResources.UpdateVirtualSportsBetSettingSuccessMsg, nickName);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        private IList<VSBetSetting> GetParameters()
        {
            string[] sports = { "180", "181", "182", "183", "184", "185", "186" };

            var settings = new List<VSBetSetting>();

            foreach (var sport in sports)
            {
                settings.Add(new VSBetSetting
                {
                    MaxBet = GetDecimalRequest("maxbet" + sport, 0),
                    MinBet = GetDecimalRequest("minbet" + sport, 0),
                    MaxPerMatch = GetDecimalRequest("maxpermatch" + sport, 0),
                    SportId = sport
                });
            }

            return settings;
        }
    }
}