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
    using Fanex.Athena.Service.Implement.Common;
    using Fanex.Athena.Service.Interface.Common;
    using Fanex.Athena.Web.ActionFilter;
    using Models.MetaModel.Customer.BetSetting;
    using Models.ViewModel.Customer.BetSetting;
    using Service.Implement.BetSettings;

    public class VoidbridgeBetSettingController : BetSettingBaseController
    {
        private readonly IVoidbridgeBetSettingService _voidbridgeBetSettingService;
        private readonly IProductService _productService;

        private ProductAccess _productAccess;

        public VoidbridgeBetSettingController()
        {
            _voidbridgeBetSettingService = new VoidbridgeBetSettingService();
            _productService = new ProductService();

            _productAccess = _productService.GetProductAccess(
                    SessionService.LicenseeSession.LicenseeId,
                    SessionService.UserSession.Language);
        }

        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        [FunctionAuthorization(ParentUrl = CustomerListUrl)]
        public ActionResult Index(BetSettingMetaModel metaModel)
        {
            var viewModel = GetViewModel(metaModel);

            if (viewModel == null)
            {
                return this.Redirect("~/Utility/AccessDenied");
            }

            return View("~/Views/Customer/BetSetting/Voidbridge.cshtml", viewModel);
        }

        [HttpGet]
        [AjaxFunctionAuthorization(ParentUrl = CustomerListUrl)]
        [ValidateAntiForgeryToken]
        public JsonResult GetStatus(int custId)
        {
            if (_productAccess.IsAllowVB == false)
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
            if (_productAccess.IsAllowVB == false)
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
            metaModel.Editable = IsBetSettingEditable(SessionService.UserSession.UserId, SessionService.LicenseeSession.LicenseeId);

            metaModel.BetSettingProducts = GetBetSettingProducts(metaModel.CustId);
            return new BetSettingViewModel()
            {
                IsEnabled = _voidbridgeBetSettingService.GetStatus(metaModel.CustId),
                Param = metaModel
            };
        }
    }
}