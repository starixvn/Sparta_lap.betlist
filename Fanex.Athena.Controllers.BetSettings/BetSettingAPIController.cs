// --------------------------------------------------------------------------
// <copyright file="BetSettingAPIController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Controllers.BetSettings
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Web.Mvc;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Models.DTO.BetSettings;
    using Fanex.Athena.Models.DTO.Common;
    using Fanex.Athena.Models.MetaModel.Casino;
    using Fanex.Athena.Models.ViewModel;
    using Fanex.Athena.Service.BetSettings.Interfaces;
    using Fanex.Athena.Service.Core.Common;
    using Fanex.Athena.Service.Core.Common.Interfaces;
    using Fanex.Athena.Service.Implement.Customer;
    using Fanex.Athena.Service.Interface.Customer;
    using Fanex.Athena.Web.ActionFilter;
    using Service.BetSettings;

    public class BetSettingApiController : ReadonlyBaseController
    {
        private const string CustomerListUrl = "Customer/CustomerList";

        private readonly IBetSettingService _betSettingService;
        private readonly IProductService _productService;
        private readonly ICustomerService _customerService;

        private ProductAccess _productAccess;

        #region Constructor

        public BetSettingApiController()
        {
            _betSettingService = new BetSettingService();
            _productService = new ProductService();
            _customerService = new CustomerService();

            _productAccess = _productService.GetProductAccess(
                    SessionService.LicenseeSession.LicenseeId,
                    SessionService.UserSession.Language);
        }

        #endregion Constructor

        #region Bet setting API

        /// <summary>
        /// Update Live Casino status.
        /// </summary>
        /// <param name="custId">Customer ID value.</param>
        /// <param name="nickName">Nick name.</param>
        /// <param name="status">Status value: 0 - enable, 1 - disable.</param>
        /// <returns>Result value.</returns>
        [HttpPost]
        [AjaxFunctionAuthorization(ParentUrl = CustomerListUrl)]
        [ValidateAntiForgeryToken]
        public JsonResult UpdateLiveCasinoStatus(int custId, string nickName, int status)
        {
            if (_productAccess.IsAllowLC == false || _customerService.GetCustomerInfo(custId).SyncLiveCasino == false)
            {
                return Json(new ResultInfo() { Code = -1, Message = ViewResources.AccessDeniedMsg });
            }

            var userSession = SessionService.UserSession;
            var result = _betSettingService.UpdateLiveCasinoStatus(userSession.UserId, custId, status, userSession.Language, Request.UserHostAddress);

            if (result.Code == 0)
            {
                result.Message = string.Format(ViewResources.UpdateSuccess, nickName);
            }

            return Json(result);
        }

        /// <summary>
        /// Update RNG Casino status.
        /// </summary>
        /// <param name="custId">Customer ID value.</param>
        /// <param name="nickName">Nick name.</param>
        /// <param name="status">Status value: 0 - enable, 1 - disable.</param>
        /// <returns>Result value.</returns>
        [HttpPost]
        [AjaxFunctionAuthorization(ParentUrl = CustomerListUrl)]
        [ValidateAntiForgeryToken]
        public JsonResult UpdateRngCasinoStatus(int custId, string nickName, int status)
        {
            if (_productAccess.IsAllowCS == false || _customerService.GetCustomerInfo(custId).SyncCasino == false)
            {
                return Json(new ResultInfo() { Code = -1, Message = ViewResources.AccessDeniedMsg });
            }

            var userSession = SessionService.UserSession;
            var result = _betSettingService.UpdateRngCasinoStatus(
                userSession.UserId,
                custId,
                status,
                userSession.Language,
                Request.UserHostAddress,
                userSession.UserName);

            if (result.Code == 0)
            {
                result.Message = string.Format(ViewResources.UpdateSuccess, nickName);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Update sport-book settings.
        /// </summary>
        /// <param name="nickName">The user name.</param>
        /// <returns>The JSON result.</returns>
        [HttpPost]
        [AjaxFunctionAuthorization(ParentUrl = CustomerListUrl)]
        [ValidateAntiForgeryToken]
        public JsonResult UpdateSportBookBetSetting(string nickName)
        {
            if (_productAccess.IsAllowSB == false)
            {
                return Json(new ResultInfo() { Code = -1, Message = ViewResources.AccessDeniedMsg });
            }

            var sportsBook = GetBetSettingCommonParam();
            var param = GetSportsBookParam();
            var result = _betSettingService.UpdateSportsBookBetSetting(sportsBook, param);

            if (result.Code == 0)
            {
                result.Message = string.Format(ViewResources.UpdateSportsbookBetSettingSuccessMsg, nickName);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Update sport-book settings.
        /// </summary>
        /// <param name="isDisabled">Is disabled.</param>
        /// <returns>The JSON result.</returns>
        [HttpPost]
        [AjaxFunctionAuthorization(ParentUrl = CustomerListUrl)]
        [ValidateAntiForgeryToken]
        public JsonResult UpdateEsportStatus(bool isDisabled)
        {
            if (AppSettings.GetValue("EnableEsportSettingStatus") == "false")
            {
                var resultInfo = new ResultInfo() { Code = -1, Message = "Function is under maintenance." };
                return Json(resultInfo, JsonRequestBehavior.AllowGet);
            }

            var sportsBook = GetBetSettingCommonParam();
            var result = _betSettingService.UpdateEsportStatus(sportsBook, isDisabled);

            if (result.Code == 0)
            {
                var enableText = isDisabled ? "disabled" : "enable";
                result.Message = string.Format(ViewResources.UpdateMemberEsportsSuccessMsg, enableText);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Update Keno settings.
        /// </summary>
        /// <param name="nickName">The user name.</param>
        /// <returns>The JSON result.</returns>
        [HttpPost]
        [AjaxFunctionAuthorization(ParentUrl = CustomerListUrl)]
        [ValidateAntiForgeryToken]
        public JsonResult UpdateKenoBetSetting(string nickName)
        {
            if (_productAccess.IsAllowKN == false)
            {
                return Json(new ResultInfo() { Code = -1, Message = ViewResources.AccessDeniedMsg });
            }

            var id = Convert.ToInt32(Request["id"]);
            var commonParam = GetBetSettingCommonParam();

            if (id <= 0 || commonParam.UserId == 0 || commonParam.CustId == 0)
            {
                return Json(new ResultInfo { Code = 0, Message = ViewResources.InvalidParams }, JsonRequestBehavior.AllowGet);
            }

            var betSetting = GetKenoBetSetting(id);

            var result = _betSettingService
                .UpdateKenoBetSetting(
                commonParam.CustId,
                commonParam.UserId,
                commonParam.Language,
                commonParam.Ip,
                betSetting);

            if (result.Code == 0)
            {
                result.Message = string.Format(ViewResources.UpdateKenoBetSettingSuccessMsg, nickName);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Update virtual sport settings.
        /// </summary>
        /// <param name="nickName">The user name.</param>
        /// <returns>The JSON result.</returns>
        [HttpPost]
        [AjaxFunctionAuthorization(ParentUrl = CustomerListUrl)]
        [ValidateAntiForgeryToken]
        public JsonResult UpdateVirtualSportsBetSetting(string nickName)
        {
            if (_productAccess.IsAllowVS == false)
            {
                return Json(new ResultInfo() { Code = -1, Message = ViewResources.AccessDeniedMsg });
            }

            var commonParam = GetBetSettingCommonParam();
            var dataParams = GetVirtualSportsParam();
            /*Validate parameter*/
            if (dataParams == null || dataParams.Count == 0 || commonParam.UserId == 0 || commonParam.CustId == 0)
            {
                return Json(new ResultInfo { Code = 0, Message = ViewResources.InvalidParams }, JsonRequestBehavior.AllowGet);
            }

            var result = _betSettingService
                .UpdateVirtualSportsBetSetting(
                commonParam.CustId,
                commonParam.UserId,
                commonParam.Language,
                commonParam.Ip,
                dataParams);

            if (result.Code == 0)
            {
                result.Message = string.Format(ViewResources.UpdateVirtualSportsBetSettingSuccessMsg, nickName);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Update live casino settings.
        /// </summary>
        /// <param name="nickName">The user name.</param>
        /// <returns>The JSON result.</returns>
        [HttpPost]
        [AjaxFunctionAuthorization(ParentUrl = CustomerListUrl)]
        [ValidateAntiForgeryToken]
        public JsonResult UpdateLiveCasinoBetSetting(string nickName)
        {
            var custId = int.Parse(HttpContext.Request["custid"]);

            if (_productAccess.IsAllowLC == false || _customerService.GetCustomerInfo(custId).SyncLiveCasino == false)
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

            var result = _betSettingService.UpdateLiveCasinoBetSetting(userSession.UserId, custId, liveCasinoBetSetting, ip, userSession.Language);

            if (result.Code == 0)
            {
                result.Message = string.Format(ViewResources.UpdateLiveCasinoBetSettingSuccessMsg, nickName);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Update RNG CASINO settings.
        /// </summary>
        /// <param name="custId">The customer id.</param>
        /// <param name="gameTypeChangedList">The game types.</param>
        /// <returns>The JSON result.</returns>
        [HttpPost]
        [AjaxFunctionAuthorization(ParentUrl = CustomerListUrl)]
        [ValidateAntiForgeryToken]
        public JsonResult UpdateRngCasinoBetSetting(int custId, string gameTypeChangedList)
        {
            if (_productAccess.IsAllowCS == false || _customerService.GetCustomerInfo(custId).SyncCasino == false)
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

                var tableParam = GetRnGCasinoParam(metaModel, gameTypeChangedList);

                metaModel.TableParam = tableParam;

                var result = _betSettingService.UpdateRngCasinoSetting(metaModel);

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

        #endregion Bet setting API

        #region Get Betsetting params

        /// <summary>
        /// Get common parameters.
        /// </summary>
        /// <returns>The common bet setting.</returns>
        private CommonBetSetting GetBetSettingCommonParam()
        {
            var userSession = SessionService.UserSession;
            var commonItem = new CommonBetSetting
            {
                LicenseeId = SessionService.LicenseeSession.LicenseeId,
                Ip = Request.UserHostAddress,
                Language = userSession.Language,
                UserId = userSession.UserId,
                CustId = int.Parse(Request["custid"])
            };

            return commonItem;
        }

        /// <summary>
        /// Get requested parameter by sport type.
        /// </summary>
        /// <param name="sportType">The sport id.</param>
        /// <param name="sportsBookParam">The sport-book parameters.</param>
        private void GetParams(string sportType, IDictionary<string, object> sportsBookParam)
        {
            var minBet = GetDecimalRequest("minbet" + sportType, 0);
            var maxBet = GetDecimalRequest("maxbet" + sportType, 0);
            var maxPerMatch = GetDecimalRequest("maxpermatch" + sportType, 0);
            var update = GetBooleanRequest("update" + sportType, false);

            var paramSuffix = sportType == "1" ? string.Empty : "v" + sportType;
            sportsBookParam.Add("minbet" + paramSuffix, minBet);
            sportsBookParam.Add("maxbet" + paramSuffix, maxBet);
            sportsBookParam.Add("maxpermatch" + paramSuffix, maxPerMatch);
            sportsBookParam.Add("updateminmaxbet" + paramSuffix, update);
        }

        /// <summary>
        /// Get sport-book parameters.
        /// </summary>
        /// <returns>The hash table parameters.</returns>
        private IDictionary<string, object> GetSportsBookParam()
        {
            var sportsBookParam = new Dictionary<string, object>();

            GetParams("1", sportsBookParam);
            GetParams("2", sportsBookParam);
            GetParams("3", sportsBookParam);
            GetParams("5", sportsBookParam);
            GetParams("8", sportsBookParam);
            GetParams("10", sportsBookParam);
            GetParams("11", sportsBookParam);
            GetParams("44", sportsBookParam);
            GetParams("99", sportsBookParam);
            GetParams("99mp", sportsBookParam);
            GetParams("154", sportsBookParam);

            return sportsBookParam;
        }

        /// <summary>
        /// Get Keno requested parameters.
        /// </summary>
        /// <param name="id">Game type id.</param>
        /// <returns>Keno bet setting.</returns>
        private KenoBetSetting GetKenoBetSetting(int id)
        {
            return new KenoBetSetting
            {
                MaxBet = GetDecimalRequest("maxbet" + id, 0),
                MinBet = GetDecimalRequest("minbet" + id, 0),
                MaxPerMatch = GetDecimalRequest("maxpermatch" + id, 0),
                GameTypeID = id.ToString()
            };
        }

        /// <summary>
        /// Get virtual sport requested parameters.
        /// </summary>
        /// <returns>The parameters.</returns>
        private IList<VSBetSetting> GetVirtualSportsParam()
        {
            string[] sports = { "180", "181", "182", "183", "184", "185", "186" };

            var settings = new List<VSBetSetting>();

            foreach (var sport in sports)
            {
                settings.Add(new VSBetSetting
                {
                    MaxBet = GetDecimalRequest("maxbet" + sport, 0),
                    MinBet = GetDecimalRequest("minbet" + sport, 0),
                    MaxPerMatch = GetDecimalRequest("maxpermatch" + sport, 0),
                    SportId = sport
                });
            }

            return settings;
        }

        /// <summary>
        /// Get RNG CASINO parameters.
        /// </summary>
        /// <param name="metaModel">The metal model.</param>
        /// <param name="gameTypeChangedList">The game types.</param>
        /// <returns>The data table.</returns>
        private DataTable GetRnGCasinoParam(RngCasinoBetSettingMetaModel metaModel, string gameTypeChangedList)
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

        /// <summary>
        /// Get decimal parameters.
        /// </summary>
        /// <param name="name">The parameter name.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The decimal value.</returns>
        private decimal GetDecimalRequest(string name, decimal defaultValue)
        {
            if (string.IsNullOrEmpty(HttpContext.Request[name]))
            {
                return defaultValue;
            }

            decimal value;
            return !decimal.TryParse(HttpContext.Request[name], out value)
                       ? defaultValue
                       : value;
        }

        /// <summary>
        /// Get boolean parameters.
        /// </summary>
        /// <param name="name">The parameter name.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The boolean value.</returns>
        private bool GetBooleanRequest(string name, bool defaultValue)
        {
            if (string.IsNullOrEmpty(HttpContext.Request[name]))
            {
                return defaultValue;
            }

            var value = HttpContext.Request[name].ToLower();

            return value == "1" || value == "on" || value == "true";
        }

        #endregion Get Betsetting params
    }
}