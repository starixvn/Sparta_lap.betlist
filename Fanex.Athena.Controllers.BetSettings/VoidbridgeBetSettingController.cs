// --------------------------------------------------------------------------
// <copyright file="VoidbridgeController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Controllers.BetSettings
{
    using System.Web.Mvc;
    using BetSettings;
    using Fanex.Athena.Models.DTO.Common;
    using Fanex.Athena.Models.ViewModel;
    using Fanex.Athena.Web.ActionFilter;
    using Models.MetaModel.BetSettings;
    using Models.ViewModel.BetSettings;
    using Service.Implement.BetSettings;
    using Service.Interface.BetSettings;

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

        private BetSettingBaseViewModel GetViewModel(BetSettingMetaModel metaModel)
        {
            var viewModel = new BetSettingBaseViewModel()
            {
                IsEnabled = _voidbridgeBetSettingService.GetStatus(metaModel.CustId),
                Param = metaModel
            };
            AssignRequiredProperties(viewModel, metaModel);
            return viewModel;
        }
    }
}