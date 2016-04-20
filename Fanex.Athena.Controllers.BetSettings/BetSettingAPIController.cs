// --------------------------------------------------------------------------
// <copyright file="BetSettingAPIController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Controllers.BetSettings
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Models.DTO.BetSettings;
    using Fanex.Athena.Models.DTO.Common;
    using Fanex.Athena.Models.ViewModel;
    using Fanex.Athena.Service.BetSettings.Interfaces;
    using Fanex.Athena.Service.Core.Common;
    using Fanex.Athena.Service.Core.Common.Interfaces;
    using Fanex.Athena.Service.Customer;
    using Fanex.Athena.Service.Customer.Interfaces;
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

        private bool GetBooleanRequest(string name, bool defaultValue)
        {
            if (string.IsNullOrEmpty(HttpContext.Request[name]))
            {
                return defaultValue;
            }

            var value = HttpContext.Request[name].ToLower();

            return value == "1" || value == "on" || value == "true";
        }
    }
}