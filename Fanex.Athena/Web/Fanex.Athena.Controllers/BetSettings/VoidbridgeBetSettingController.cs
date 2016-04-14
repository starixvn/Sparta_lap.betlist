// --------------------------------------------------------------------------
// <copyright file="VoidbridgeController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Controllers.Customer
{
    using System.Web.Mvc;
    using BetSettings;
    using Fanex.Athena.Models.DTO.Common;
    using Fanex.Athena.Models.ViewModel;
    using Fanex.Athena.Web.ActionFilter;
    using Models.MetaModel.BetSettings;
    using Models.ViewModel.BetSettings;
    using Service.Implement.BetSettings;

    public class VoidbridgeBetSettingController : BetSettingBaseController
    {
        private readonly IVoidbridgeBetSettingService _voidbridgeBetSettingService;

        public VoidbridgeBetSettingController()
        {
            _voidbridgeBetSettingService = new VoidbridgeBetSettingService();
        }

        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        [FunctionAuthorization(ParentUrl = CustomerListUrl)]
        public ActionResult Index(BetSettingMetaModel metaModel)
        {
            var viewModel = GetViewModel(metaModel);

            return View("~/Views/Customer/BetSetting/Voidbridge.cshtml", viewModel);
        }

        [HttpGet]
        [AjaxFunctionAuthorization(ParentUrl = CustomerListUrl)]
        [ValidateAntiForgeryToken]
        public JsonResult GetStatus(int custId)
        {
            if (ProductAccess.IsAllowVB == false)
            {
                return Json(new ResultInfo() { Code = -1, Message = ViewResources.AccessDeniedMsg });
            }

            var data = _voidbridgeBetSettingService.GetStatus(custId);

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AjaxFunctionAuthorization(ParentUrl = CustomerListUrl)]
        [ValidateAntiForgeryToken]
        public JsonResult UpdateStatus(int custId, string nickName, int status)
        {
            if (ProductAccess.IsAllowVB == false)
            {
                return Json(new ResultInfo() { Code = -1, Message = ViewResources.AccessDeniedMsg });
            }

            var userSession = SessionService.UserSession;
            var result = _voidbridgeBetSettingService.UpdateStatus(userSession.UserId, custId, Request.UserHostAddress, status == 1);

            if (result.Code == 0)
            {
                result.Message = string.Format(ViewResources.UpdateSuccess, nickName);
            }

            return Json(result);
        }

        private BetSettingViewModel GetViewModel(BetSettingMetaModel metaModel)
        {
            AssignRequiredProperties(metaModel);
            return new BetSettingViewModel()
            {
                IsEnabled = _voidbridgeBetSettingService.GetStatus(metaModel.CustId),
                Param = metaModel
            };
        }
    }
}