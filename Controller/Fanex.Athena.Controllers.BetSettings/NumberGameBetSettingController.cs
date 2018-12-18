// --------------------------------------------------------------------------
// <copyright file="NumberGameBetSettingController.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Controllers.BetSettings
{
    using System.Collections;
    using System.Web.Mvc;
    using Extension.Utility;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Web.ActionFilter;
    using Model.BetSettings.MetaModel;
    using Resources;
    using Service.BetSettings.Interfaces;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;

    public class NumberGameBetSettingController : BetSettingBaseController
    {
        private readonly INumberGameBetSettingService _betSettingService;

        public NumberGameBetSettingController(
            ILanguageService languageService,
            ISessionService sessionService,
            IAppSettings appSetting,
            ICoreBetSettingService coreBetSettingService,
            IProductService productService,
            INumberGameBetSettingService betSettingService)
        : base(languageService, sessionService, appSetting, coreBetSettingService, productService)
        {
            _betSettingService = betSettingService;
        }

        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult Index(BetSettingMetaModel metaModel)
        {
            var viewModel = _betSettingService.GetViewModel(SessionService.LicenseeSession.LicenseeId, metaModel.CustId);
            AssignRequiredProperties(viewModel, metaModel);

            return View("~/Views/Desktop/Customer/BetSetting/NumberGame.cshtml", viewModel);
        }

        [HttpPost]
        [CustomValidateAntiForgery]
        public JsonResult UpdateBetSetting(string nickName)
        {
            if (!ProductAccess.IsAllowNG || !IsBetSettingEditable())
            {
                return AccessDenied();
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