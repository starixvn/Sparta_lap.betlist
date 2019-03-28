// --------------------------------------------------------------------------
// <copyright file="RngCasinoBetSettingController.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Controllers.BetSettings
{
    using System.Data;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Extension.Utility;
    using Fanex.Athena.Web.ActionFilter;
    using Model.BetSettings.MetaModel;
    using Model.Core.DTO.Common;
    using Resources;
    using Service.BetSettings.Interfaces;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;

    public class RngCasinoBetSettingController : BetSettingBaseController
    {
        private readonly IRngCasinoBetSettingService _betSettingService;

        public RngCasinoBetSettingController(
            ILanguageService languageService,
            ISessionService sessionService,
            IAppSettings appSetting,
            ICoreBetSettingService coreBetSettingService,
            IProductService productService,
            IRngCasinoBetSettingService betSettingService)
        : base(languageService, sessionService, appSetting, coreBetSettingService, productService)
        {
            _betSettingService = betSettingService;
        }

        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public async Task<ActionResult> Index(BetSettingMetaModel metaModel)
        {
            var customer = GetCustomerInfo(metaModel.CustId);
            var viewModel = _betSettingService.GetViewModel(metaModel.CustId, customer, SessionService.UserSession.Language);
            await AssignRequiredProperties(viewModel, metaModel).ConfigureAwait(false);

            return View("~/Views/Desktop/Customer/BetSetting/RngCasino.cshtml", viewModel);
        }

        [HttpPost]
        [CustomValidateAntiForgery]
        public async Task<JsonResult> UpdateBetSetting(int custId, string gameTypeChangedList)
        {
            var productAccess = await GetProductAccess().ConfigureAwait(false);
            var customer = productAccess.IsAllowCS ? GetCustomerInfo(custId) : null;
            if (customer == null || !customer.SyncCasino || !(await IsBetSettingEditable().ConfigureAwait(false)))
            {
                return AccessDenied();
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
        public async Task<JsonResult> UpdateStatus(int custId, string nickName, int status)
        {
            var productAccess = await GetProductAccess().ConfigureAwait(false);
            if (!productAccess.IsAllowCS ||
                !GetCustomerInfo(custId).SyncCasino ||
                !(await IsBetSettingEditable().ConfigureAwait(false)))
            {
                return AccessDenied();
            }

            var licenseeId = SessionService.LicenseeSession.LicenseeId;
            var customer = GetCustomerInfo(custId);
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
            const int SystemID = 4;
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
                table.Rows.Add(metaModel.CustId, gameTypeId, stakeLevel, 1, metaModel.Ip, metaModel.ModifyById, metaModel.ModifyByUsername, SystemID);
            }

            return table;
        }
    }
}