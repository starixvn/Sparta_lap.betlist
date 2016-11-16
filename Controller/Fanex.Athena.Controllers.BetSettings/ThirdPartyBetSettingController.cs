// --------------------------------------------------------------------------
// <copyright file="ThirdPartyBetSettingController.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Controllers.BetSettings
{
    using System;
    using System.Web.Mvc;
    using Fanex.Athena.Web.ActionFilter;
    using Model.BetSettings.MetaModel;
    using Model.Core.DataDomain;
    using Model.Core.DTO.Common;
    using Resources;
    using Service.BetSettings;
    using Service.BetSettings.Interfaces;

    public class ThirdPartyBetSettingController : BetSettingBaseController
    {
        private readonly IThirdPartyBetSettingService _betSettingService;

        public ThirdPartyBetSettingController()
        {
            _betSettingService = new ThirdPartyBetSettingService();
        }

        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult Index(BetSettingMetaModel metaModel)
        {
            var viewModel = _betSettingService.GetViewModel(SessionService.LicenseeSession.LicenseeId, metaModel.CustId, metaModel.ProductId);
            AssignRequiredProperties(viewModel, metaModel);
            if (viewModel == null)
            {
                return this.Redirect("~/Utility/AccessDenied");
            }

            switch (metaModel.ProductId)
            {
                case ProductTypes.Macau:
                    return View("~/Views/Customer/BetSetting/Macau.cshtml", viewModel);

                case ProductTypes.MicroGamingLiveCasino:
                    return View("~/Views/Customer/BetSetting/MicroGamingLiveCasino.cshtml", viewModel);

                case ProductTypes.MicrogamingRNGCasino:
                    return View("~/Views/Customer/BetSetting/MicroGamingRNGCasino.cshtml", viewModel);

                case ProductTypes.BetTrade:
                    return View("~/Views/Customer/BetSetting/BetTrade.cshtml", viewModel);

                case ProductTypes.ColossusBets:
                    return View("~/Views/Customer/BetSetting/ColossusBets.cshtml", viewModel);

                case ProductTypes.FishingWorld:
                    return View("~/Views/Customer/BetSetting/FishingWorld.cshtml", viewModel);

                default:
                    throw new Exception("Not a thirdparty product.");
            }
        }

        [HttpPost]
        [CustomValidateAntiForgery]
        public JsonResult UpdateMacauStatus(int custId, string nickName, int status)
        {
            return UpdateThirdPartyProductStatus((int)ProductTypes.Macau, custId, nickName, status);
        }

        [HttpPost]
        [CustomValidateAntiForgery]
        public JsonResult UpdateMicroGamingLiveCasinoStatus(int custId, string nickName, int status)
        {
            return UpdateThirdPartyProductStatus((int)ProductTypes.MicroGamingLiveCasino, custId, nickName, status);
        }

        [HttpPost]
        [CustomValidateAntiForgery]
        public JsonResult UpdateMicroGamingRNGCasinoStatus(int custId, string nickName, int status)
        {
            return UpdateThirdPartyProductStatus((int)ProductTypes.MicrogamingRNGCasino, custId, nickName, status);
        }

        [HttpPost]
        [CustomValidateAntiForgery]
        public JsonResult UpdateBetTradeStatus(int custId, string nickName, int status)
        {
            return UpdateThirdPartyProductStatus((int)ProductTypes.BetTrade, custId, nickName, status);
        }

        [HttpPost]
        [CustomValidateAntiForgery]
        public JsonResult UpdateColossusBetsStatus(int custId, string nickName, int status)
        {
            return UpdateThirdPartyProductStatus((int)ProductTypes.ColossusBets, custId, nickName, status);
        }

        [HttpPost]
        [CustomValidateAntiForgery]
        public JsonResult UpdateFishingWorldStatus(int custId, string nickName, int status)
        {
            return UpdateThirdPartyProductStatus(ProductTypes.FishingWorld, custId, nickName, status);
        }

        private JsonResult UpdateThirdPartyProductStatus(int productId, int custId, string nickName, int status)
        {
            if (ProductAccess.IsAllow(productId) == false || !IsBetSettingEditable())
            {
                return Json(new ResultInfo() { Code = -1, Message = ViewResources.AccessDeniedMsg });
            }

            var userSession = SessionService.UserSession;
            var isEnabled = status == 1;
            var result = _betSettingService.UpdateStatus(
                SessionService.LicenseeSession.LicenseeId,
                productId,
                userSession.UserId,
                custId,
                userSession.Language,
                Request.UserHostAddress,
                isEnabled);

            if (result.Code == 0 && string.IsNullOrWhiteSpace(result.Message))
            {
                result.Message = string.Format(ViewResources.UpdateSuccess, nickName);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}