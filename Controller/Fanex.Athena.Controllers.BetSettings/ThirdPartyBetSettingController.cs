// --------------------------------------------------------------------------
// <copyright file="ThirdPartyBetSettingController.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Controllers.BetSettings
{
    using System;
    using System.Web.Mvc;
    using Constant.DataDomain;
    using DataAccess.BetSettings.Interfaces;
    using Extension.Utility;
    using Fanex.Athena.Web.ActionFilter;
    using Model.BetSettings.MetaModel;
    using Model.Core.DTO.Common;
    using Resources;
    using Service.BetSettings;
    using Service.BetSettings.Interfaces;
    using Service.Core.Common.Interfaces;
    using Service.Core.Helpers;
    using Service.Core.Users.Interfaces;
    using Service.Customer.Interfaces;

    public class ThirdPartyBetSettingController : BetSettingBaseController
    {
        private readonly IThirdPartyBetSettingService _betSettingService;
        
        public ThirdPartyBetSettingController(
            ILanguageService languageService,
            ISessionService sessionService,
            IAppSettings appSetting,
            IClientViewHelper clientViewHelper,
            IAuthorizationService authorizationService,
            ICoreBetSettingDataAccess coreBetSettingDataAccess,
            ICustomerService customerService,
            IProductService productService,
            IThirdPartyBetSettingService betSettingService)
        : base(languageService, sessionService, appSetting, clientViewHelper, authorizationService, coreBetSettingDataAccess, customerService, productService)
        {
            _betSettingService = betSettingService;
        }

        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult Index(BetSettingMetaModel metaModel)
        {
            var viewModel = _betSettingService.GetViewModel(SessionService.LicenseeSession.LicenseeId, metaModel.CustId, metaModel.ProductId);
            if (viewModel == null)
            {
                return this.Redirect("~/Utility/AccessDenied");
            }

            AssignRequiredProperties(viewModel, metaModel);

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

                case ProductTypes.CashOut:
                    return View("~/Views/Customer/BetSetting/CashOut.cshtml", viewModel);

                default:
                    throw new ArgumentException("Not a thirdparty product.");
            }
        }

        [HttpPost]
        [CustomValidateAntiForgery]
        public JsonResult UpdateMacauStatus(int custId, string nickName, int status)
        {
            return UpdateThirdPartyProductStatus(ProductTypes.Macau, custId, nickName, status);
        }

        [HttpPost]
        [CustomValidateAntiForgery]
        public JsonResult UpdateMicroGamingLiveCasinoStatus(int custId, string nickName, int status)
        {
            return UpdateThirdPartyProductStatus(ProductTypes.MicroGamingLiveCasino, custId, nickName, status);
        }

        [HttpPost]
        [CustomValidateAntiForgery]
        public JsonResult UpdateMicroGamingRNGCasinoStatus(int custId, string nickName, int status)
        {
            return UpdateThirdPartyProductStatus(ProductTypes.MicrogamingRNGCasino, custId, nickName, status);
        }

        [HttpPost]
        [CustomValidateAntiForgery]
        public JsonResult UpdateBetTradeStatus(int custId, string nickName, int status)
        {
            return UpdateThirdPartyProductStatus(ProductTypes.BetTrade, custId, nickName, status);
        }

        [HttpPost]
        [CustomValidateAntiForgery]
        public JsonResult UpdateColossusBetsStatus(int custId, string nickName, int status)
        {
            return UpdateThirdPartyProductStatus(ProductTypes.ColossusBets, custId, nickName, status);
        }

        [HttpPost]
        [CustomValidateAntiForgery]
        public JsonResult UpdateCashOutStatus(int custId, string nickName, int status)
        {
            return UpdateThirdPartyProductStatus(ProductTypes.CashOut, custId, nickName, status);
        }

        private JsonResult UpdateThirdPartyProductStatus(int productId, int custId, string nickName, int status)
        {
            if (!ProductAccess.IsAllow(productId) || !IsBetSettingEditable())
            {
                return AccessDenied();
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