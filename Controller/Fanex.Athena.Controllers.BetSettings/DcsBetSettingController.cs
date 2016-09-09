// --------------------------------------------------------------------------
// <copyright file="DcsController.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Controllers.BetSettings
{
    using System;
    using System.Web.Mvc;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Web.ActionFilter;
    using Model.BetSettings.MetaModel;
    using Model.BetSettings.ViewModel;
    using Model.Core.DataDomain;
    using Resources;
    using Service.BetSettings;
    using Service.BetSettings.Interfaces;

    public class DcsBetSettingController : BetSettingBaseController
    {
        private readonly IDcsBetSettingService _betSettingService;

        public DcsBetSettingController()
        {
            _betSettingService = new DcsBetSettingService();
        }

        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult Index(BetSettingMetaModel metaModel)
        {
            var viewModel = new BetSettingBaseViewModel()
            {
                IsEnabled = _betSettingService.GetStatus(SessionService.LicenseeSession.LicenseeId, metaModel.CustId, metaModel.ProductId),
            };
            AssignRequiredProperties(viewModel, metaModel);

            switch (metaModel.ProductId)
            {
                case ProductTypes.AllBet:
                    return View("~/Views/Customer/BetSetting/AllBet.cshtml", viewModel);

                case ProductTypes.Voidbridge:
                    return View("~/Views/Customer/BetSetting/Voidbridge.cshtml", viewModel);

                default:
                    throw new ArgumentException("Invalid product id.");
            }
        }

        [HttpPost]
        [CustomValidateAntiForgery]
        public JsonResult UpdateStatus(int custId, string nickName, int status, int productId)
        {
            if (ProductAccess.IsAllow(productId) == false || !IsBetSettingEditable())
            {
                return Json(new ResultInfo() { Code = -1, Message = ViewResources.AccessDeniedMsg });
            }

            var userSession = SessionService.UserSession;
            var result = _betSettingService.UpdateStatus(
                SessionService.LicenseeSession.LicenseeId,
                userSession.UserId,
                custId,
                Request.UserHostAddress,
                status == 1,
                productId);

            if (result.Code == 0)
            {
                result.Message = string.Format(ViewResources.UpdateSuccess, nickName);
            }

            return Json(result);
        }
    }
}