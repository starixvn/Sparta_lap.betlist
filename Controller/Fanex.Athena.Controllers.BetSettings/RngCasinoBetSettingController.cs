// --------------------------------------------------------------------------
// <copyright file="RngCasinoBetSettingController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Controllers.BetSettings
{
    using System.Data;
    using System.Web.Mvc;
    using Fanex.Athena.Web.ActionFilter;
    using Model.BetSettings.MetaModel;
    using Model.Casino.MetaModel;
    using Model.Core.DTO.Common;
    using Resources;
    using Service.BetSettings;

    public class RngCasinoBetSettingController : BetSettingBaseController
    {
        private readonly IRngCasinoBetSettingService _betSettingService;

        public RngCasinoBetSettingController()
        {
            _betSettingService = new RngCasinoBetSettingService();
        }

        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        [FunctionAuthorization(ParentUrl = CustomerListUrl)]
        public ActionResult Index(BetSettingMetaModel metaModel)
        {
            var customer = CustomerService.GetCustomerInfo(metaModel.CustId);
            var viewModel = _betSettingService.GetViewModel(metaModel.CustId, customer, SessionService.UserSession.Language, SessionService.LicenseeSession.LicenseeName);
            AssignRequiredProperties(viewModel, metaModel);

            return View("~/Views/Customer/BetSetting/RngCasino.cshtml", viewModel);
        }

        [HttpPost]
        [FunctionAuthorization(ParentUrl = CustomerListUrl)]
        [ValidateAntiForgeryToken]
        public JsonResult UpdateBetSetting(int custId, string gameTypeChangedList)
        {
            if (ProductAccess.IsAllowCS == false || CustomerService.GetCustomerInfo(custId).SyncCasino == false)
            {
                return Json(new ResultInfo() { Code = -1, Message = ViewResources.AccessDeniedMsg });
            }

            try
            {
                var licenseeSession = SessionService.LicenseeSession;
                var userSession = SessionService.UserSession;

                var metaModel = new RngCasinoBetSettingMetaModel
                {
                    LicenseeId = licenseeSession.LicenseeId,
                    LicenseeName = licenseeSession.LicenseeName,
                    CustId = custId,
                    ModifyById = userSession.UserId,
                    ModifyByUsername = userSession.UserName,
                    Ip = Request.UserHostAddress
                };

                var tableParam = GetRngCasinoParam(metaModel, gameTypeChangedList);

                metaModel.TableParam = tableParam;

                var customer = CustomerService.GetCustomerInfo(custId);

                var result = _betSettingService.UpdateSetting(metaModel, customer.SyncCasino, customer.DisableCasino);

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (System.Exception e)
            {
                if (IsDebuggingEnabled)
                {
                    throw;
                }

                return Json(new ResultInfo { Code = 1, Message = e.Message });
            }
        }

        [HttpPost]
        [FunctionAuthorization(ParentUrl = CustomerListUrl)]
        [ValidateAntiForgeryToken]
        public JsonResult UpdateStatus(int custId, string nickName, int status)
        {
            if (ProductAccess.IsAllowCS == false || CustomerService.GetCustomerInfo(custId).SyncCasino == false)
            {
                return Json(new ResultInfo() { Code = -1, Message = ViewResources.AccessDeniedMsg });
            }

            var customer = CustomerService.GetCustomerInfo(custId);
            var userSession = SessionService.UserSession;
            var result = _betSettingService.UpdateStatus(
                userSession.UserId,
                custId,
                status,
                userSession.Language,
                Request.UserHostAddress,
                userSession.UserName,
                customer.SyncCasino);

            if (result.Code == 0)
            {
                result.Message = string.Format(ViewResources.UpdateSuccess, nickName);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        private DataTable GetRngCasinoParam(RngCasinoBetSettingMetaModel metaModel, string gameTypeChangedList)
        {
            var table = new DataTable();
            table.Columns.Add("IBCCustID", typeof(int));
            table.Columns.Add("GameTypeID", typeof(int));
            table.Columns.Add("StakeLevelList", typeof(string));
            table.Columns.Add("RoleId", typeof(int));
            table.Columns.Add("FromIP", typeof(string));
            table.Columns.Add("ModifyBy", typeof(int));
            table.Columns.Add("ModifyUserName", typeof(string));
            table.Columns.Add("FromSystem", typeof(int));

            var gameTypeList = gameTypeChangedList.Split(',');

            foreach (var gameTypeId in gameTypeList)
            {
                var stakeLevel = this.HttpContext.Request["stake" + gameTypeId];
                table.Rows.Add(metaModel.CustId, gameTypeId, stakeLevel, 1, metaModel.Ip, metaModel.ModifyById, metaModel.ModifyByUsername, metaModel.SystemId);
            }

            return table;
        }
    }
}