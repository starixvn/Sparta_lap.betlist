// --------------------------------------------------------------------------
// <copyright file="NumberGameBetSettingController.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Controllers.BetSettings
{
    using System.Collections;
    using System.Web.Mvc;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Web.ActionFilter;
    using Model.BetSettings.MetaModel;
    using Resources;
    using Service.BetSettings;
    using Service.BetSettings.Interfaces;

    public class NumberGameBetSettingController : BetSettingBaseController
    {
        private readonly INumberGameBetSettingService _betSettingService;

        public NumberGameBetSettingController()
        {
            _betSettingService = new NumberGameBetSettingService();
        }

        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult Index(BetSettingMetaModel metaModel)
        {
            var viewModel = _betSettingService.GetViewModel(SessionService.LicenseeSession.LicenseeId, metaModel.CustId);
            AssignRequiredProperties(viewModel, metaModel);

            return View("~/Views/Customer/BetSetting/NumberGame.cshtml", viewModel);
        }

        [HttpPost]
        [CustomValidateAntiForgery]
        public JsonResult UpdateBetSetting(string nickName)
        {
            if (ProductAccess.IsAllowNG == false || !IsBetSettingEditable())
            {
                return Json(new ResultInfo() { Code = -1, Message = ViewResources.AccessDeniedMsg });
            }

            var numberGame = GetBetSettingCommonParam();
            var hashData = GetNumberGameParam();
            var result = _betSettingService.UpdateBetSetting(numberGame, hashData);

            if (result.Code == 0)
            {
                result.Message = string.Format(ViewResources.UpdateNumberGameBetSettingSuccessMsg, nickName);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        private Hashtable GetNumberGameParam()
        {
            var hashData = new Hashtable
                               {
                                   { "minbet161", decimal.Parse(HttpContext.Request["minbet161"]) },
                                   { "maxbet161", decimal.Parse(HttpContext.Request["maxbet161"]) },
                                   { "maxpermatch161", decimal.Parse(HttpContext.Request["maxpermatch161"]) },
                                   { "maxperball", GetDecimalRequest("maxperball", 0) }
                               };

            return hashData;
        }
    }
}