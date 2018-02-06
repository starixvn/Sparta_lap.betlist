// --------------------------------------------------------------------------
// <copyright file="RngCasinoBetSettingController.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Controllers.BetSettings
{
    using System.Data;
    using System.Web.Mvc;
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

    public class RngCasinoBetSettingController : BetSettingBaseController
    {
        private readonly IRngCasinoBetSettingService _betSettingService;

        public RngCasinoBetSettingController()
        {
            _betSettingService = new RngCasinoBetSettingService();
        }

        public RngCasinoBetSettingController(
            ILanguageService languageService,
            ISessionService sessionService,
            IAppSettings appSetting,
            IClientViewHelper clientViewHelper,
            IAuthorizationService authorizationService,
            ICoreBetSettingDataAccess coreBetSettingDataAccess,
            ICustomerService customerService,
            IProductService productService,
            IRngCasinoBetSettingService betSettingService)
        : base(languageService, sessionService, appSetting, clientViewHelper, authorizationService, coreBetSettingDataAccess, customerService, productService)
        {
            _betSettingService = betSettingService;
        }

        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult Index(BetSettingMetaModel metaModel)
        {
            var customer = GetCustomerInfo(SessionService.LicenseeSession.LicenseeId, metaModel.CustId);
            var viewModel = _betSettingService.GetViewModel(metaModel.CustId, customer, SessionService.UserSession.Language);
            AssignRequiredProperties(viewModel, metaModel);

            return View("~/Views/Customer/BetSetting/RngCasino.cshtml", viewModel);
        }

        [HttpPost]
        [CustomValidateAntiForgery]
        public JsonResult UpdateBetSetting(int custId, string gameTypeChangedList)
        {
            var licenseeSession = SessionService.LicenseeSession;
            var customer = ProductAccess.IsAllowCS ? GetCustomerInfo(licenseeSession.LicenseeId, custId) : null;
            if (customer == null || !customer.SyncCasino || !IsBetSettingEditable())
            {
                return AccessDenied();
            }

            try
            {
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

                var result = _betSettingService.UpdateSetting(metaModel, customer.SyncCasino, customer.DisableCasino);

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (System.Exception e)
            {
                return Json(new ResultInfo { Code = 1, Message = e.Message });
            }
        }

        [HttpPost]
        [CustomValidateAntiForgery]
        public JsonResult UpdateStatus(int custId, string nickName, int status)
        {
            var licenseeId = SessionService.LicenseeSession.LicenseeId;
            if (!ProductAccess.IsAllowCS ||
                !GetCustomerInfo(licenseeId, custId).SyncCasino ||
                !IsBetSettingEditable())
            {
                return AccessDenied();
            }

            var customer = GetCustomerInfo(licenseeId, custId);
            var userSession = SessionService.UserSession;
            var metaModel = new BetSettingCasinoStatusMetaModel
            {
                LicenseeId = licenseeId,
                UserId = userSession.UserId,
                CustId = custId,
                Status = status,
                Language = userSession.Language,
                Ip = Request.UserHostAddress,
                UserName = userSession.UserName,
                IsSyncCasino = customer.SyncCasino
            };
            var result = _betSettingService.UpdateStatus(metaModel);

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
                table.Rows.Add(metaModel.CustId, gameTypeId, stakeLevel, 1, metaModel.Ip, metaModel.ModifyById, metaModel.ModifyByUsername);
            }

            return table;
        }
    }
}