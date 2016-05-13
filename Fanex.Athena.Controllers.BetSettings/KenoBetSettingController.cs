// --------------------------------------------------------------------------
// <copyright file="KenoBetSettingController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Controllers.BetSettings
{
    using System;
    using System.Web.Mvc;
    using Fanex.Athena.Web.ActionFilter;
    using Model.BetSettings.DTO;
    using Model.BetSettings.MetaModel;
    using Model.Core.DTO.Common;
    using Resources;
    using Service.BetSettings;

    public class KenoBetSettingController : BetSettingBaseController
    {
        private readonly IKenoBetSettingService _betSettingService;

        public KenoBetSettingController()
        {
            _betSettingService = new KenoBetSettingService();
        }

        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        [FunctionAuthorization(ParentUrl = CustomerListUrl)]
        public ActionResult Index(BetSettingMetaModel metaModel)
        {
            var viewModel = _betSettingService.GetViewModel(metaModel.CustId);
            AssignRequiredProperties(viewModel, metaModel);
            return View("~/Views/Customer/BetSetting/Keno.cshtml", viewModel);
        }

        [HttpPost]
        [AjaxFunctionAuthorization(ParentUrl = CustomerListUrl)]
        [ValidateAntiForgeryToken]
        public JsonResult UpdateBetSetting(string nickName)
        {
            if (ProductAccess.IsAllowKN == false)
            {
                return Json(new ResultInfo() { Code = -1, Message = ViewResources.AccessDeniedMsg });
            }

            var id = Convert.ToInt32(Request["id"]);
            var commonParam = GetBetSettingCommonParam();

            if (id <= 0 || commonParam.UserId == 0 || commonParam.CustId == 0)
            {
                return Json(new ResultInfo { Code = 0, Message = ViewResources.InvalidParams }, JsonRequestBehavior.AllowGet);
            }

            var betSetting = GetBetSetting(id);

            var result = _betSettingService
                .UpdateBetSetting(
                commonParam.CustId,
                commonParam.UserId,
                commonParam.Language,
                commonParam.Ip,
                betSetting);

            if (result.Code == 0)
            {
                result.Message = string.Format(ViewResources.UpdateKenoBetSettingSuccessMsg, nickName);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        private KenoBetSetting GetBetSetting(int id)
        {
            return new KenoBetSetting
            {
                MaxBet = GetDecimalRequest("maxbet" + id, 0),
                MinBet = GetDecimalRequest("minbet" + id, 0),
                MaxPerMatch = GetDecimalRequest("maxpermatch" + id, 0),
                GameTypeID = id.ToString()
            };
        }
    }
}