// -----------------------------------------------------------------------
// <copyright file="SportsbookBetSettingController.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.BetSettings
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Extension.Utility;
    using Fanex.Athena.Model.BetSettings.MetaModel;
    using Fanex.Athena.Web.ActionFilter;
    using Model.Core.DTO.Common;
    using Resources;
    using Service.BetSettings;
    using Service.BetSettings.Interfaces;

    public class SportsbookBetSettingController : BetSettingBaseController
    {
        private readonly ISportsbookBetSettingService _betSettingService;

        public SportsbookBetSettingController()
        {
            this._betSettingService = new SportsbookBetSettingService();
        }

        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        [FunctionAuthorization(ParentUrl = CustomerListUrl)]
        public ActionResult Index(BetSettingMetaModel metaModel)
        {
            var viewModel = _betSettingService.GetViewModel(metaModel.CustId);

            AssignRequiredProperties(viewModel, metaModel);

            return View("~/Views/Customer/BetSetting/Main.cshtml", viewModel);
        }

        [HttpPost]
        [FunctionAuthorization(ParentUrl = CustomerListUrl)]
        [CustomValidateAntiForgery]
        public JsonResult UpdateBetSetting(string nickName)
        {
            if (ProductAccess.IsAllowSB == false)
            {
                return Json(new ResultInfo() { Code = -1, Message = ViewResources.AccessDeniedMsg });
            }

            var sportsBook = GetBetSettingCommonParam();
            var param = GetSportsBookParam();
            var result = _betSettingService.UpdateBetSetting(sportsBook, param);

            if (result.Code == 0)
            {
                result.Message = string.Format(ViewResources.UpdateSportsbookBetSettingSuccessMsg, nickName);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [FunctionAuthorization(ParentUrl = CustomerListUrl)]
        [CustomValidateAntiForgery]
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