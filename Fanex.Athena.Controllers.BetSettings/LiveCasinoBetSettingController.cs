// --------------------------------------------------------------------------
// <copyright file="LiveCasinoBetSettingController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Controllers.BetSettings
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Fanex.Athena.Web.ActionFilter;
    using Model.BetSettings.DTO;
    using Model.BetSettings.MetaModel;
    using Model.Core.DTO.Common;
    using Model.Customer.DTO;
    using Resources;
    using Service.BetSettings;

    public class LiveCasinoBetSettingController : BetSettingBaseController
    {
        private readonly ILiveCasinoBetSettingService _betSettingService;

        public LiveCasinoBetSettingController()
        {
            _betSettingService = new LiveCasinoBetSettingService();
        }

        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        [FunctionAuthorization(ParentUrl = CustomerListUrl)]
        public ActionResult Index(BetSettingMetaModel metaModel)
        {
            var customer = CustomerService.GetCustomerInfo(metaModel.CustId);
            var viewModel = _betSettingService.GetViewModel(metaModel.CustId, SessionService.UserSession.Language);
            viewModel.IsEnabled = IsLiveCasinoEnabled(customer);

            AssignRequiredProperties(viewModel, metaModel);

            return View("~/Views/Customer/BetSetting/LiveCasino.cshtml", viewModel);
        }

        [HttpPost]
        [FunctionAuthorization(ParentUrl = CustomerListUrl)]
        [ValidateAntiForgeryToken]
        public JsonResult UpdateStatus(int custId, string nickName, int status)
        {
            var customer = CustomerService.GetCustomerInfo(custId);
            if (ProductAccess.IsAllowLC == false || customer.SyncLiveCasino == false)
            {
                return Json(new ResultInfo() { Code = -1, Message = ViewResources.AccessDeniedMsg });
            }

            var userSession = SessionService.UserSession;
            var result = _betSettingService.UpdateStatus(
                userSession.UserId,
                custId,
                status,
                userSession.Language,
                Request.UserHostAddress,
                customer.SyncLiveCasino);

            if (result.Code == 0)
            {
                result.Message = string.Format(ViewResources.UpdateSuccess, nickName);
            }

            return Json(result);
        }

        [HttpPost]
        [FunctionAuthorization(ParentUrl = CustomerListUrl)]
        [ValidateAntiForgeryToken]
        public JsonResult UpdateBetSetting(string nickName)
        {
            var custId = int.Parse(HttpContext.Request["custid"]);

            if (ProductAccess.IsAllowLC == false || CustomerService.GetCustomerInfo(custId).SyncLiveCasino == false)
            {
                return Json(new ResultInfo() { Code = -1, Message = ViewResources.AccessDeniedMsg });
            }

            var userSession = SessionService.UserSession;
            var ip = Request.UserHostAddress;

            var gameTypeCount = int.Parse(HttpContext.Request["gameTypeCount"]);

            var liveCasinoBetSetting = new List<LiveCasinoBetSetting>();

            for (var i = 1; i <= gameTypeCount; i++)
            {
                var gameTypeId = int.Parse(HttpContext.Request["gameTypeId" + i]);
                var stakeLevel = int.Parse(HttpContext.Request["stakeId" + i]);
                var stakeLevelName = HttpContext.Request["stakeLevelName" + i];

                var betTypeId = int.Parse(HttpContext.Request["betTypeId" + i]);

                var liveCasino = new LiveCasinoBetSetting
                {
                    GameTypeId = gameTypeId,
                    StakeLevelId = stakeLevel,
                    StakeLevelName = stakeLevelName,
                    BetTypeId = betTypeId
                };

                liveCasinoBetSetting.Add(liveCasino);
            }

            var customer = CustomerService.GetCustomerInfo(custId);
            var result = _betSettingService.UpdateBetSetting(
                userSession.UserId,
                custId,
                liveCasinoBetSetting,
                ip,
                userSession.Language,
                customer.DisableLiveCasino);

            if (result.Code == 0)
            {
                result.Message = string.Format(ViewResources.UpdateLiveCasinoBetSettingSuccessMsg, nickName);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        private static bool? IsLiveCasinoEnabled(CustomerInfo customer)
        {
            bool? resultValue = false;
            if (customer.DisableLiveCasino == false && customer.SyncLiveCasino)
            {
                resultValue = true;
            }
            else if (customer.SyncLiveCasino == false)
            {
                resultValue = null;
            }

            return resultValue;
        }
    }
}