// --------------------------------------------------------------------------
// <copyright file="NumberGameBetSettingController.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Controllers.BetSettings
{
    using System.Collections;
    using System.Threading.Tasks;
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
        public async Task<ActionResult> Index(BetSettingMetaModel metaModel)
        {
            var viewModel = await _betSettingService.GetViewModelAsync(SessionService.LicenseeSession.LicenseeId, metaModel.CustId).ConfigureAwait(false);
            await AssignRequiredProperties(viewModel, metaModel).ConfigureAwait(false);

            return View("~/Views/Desktop/Customer/BetSetting/NumberGame.cshtml", viewModel);
        }

        [HttpPost]
        [CustomValidateAntiForgery]
        public async Task<JsonResult> UpdateBetSetting(string nickName)
        {
            var productAccess = await GetProductAccess().ConfigureAwait(false);
            if (!productAccess.IsAllowNG || !(await IsBetSettingEditable().ConfigureAwait(false)))
            {
                return AccessDenied();
            }

            var numberGame = GetBetSettingCommonParam();
            var hashData = GetNumberGameParam();
            var result = await _betSettingService.UpdateBetSettingAsync(numberGame, hashData).ConfigureAwait(false);

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