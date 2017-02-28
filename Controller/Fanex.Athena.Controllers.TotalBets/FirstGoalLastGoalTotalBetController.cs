// --------------------------------------------------------------------------
// <copyright file="FirstGoalLastGoalTotalBetController.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Controllers.TotalBets
{
    using System.Web.Mvc;
    using Extension.Utility;
    using Fanex.Athena.Model.TotalBet.DTO;
    using Fanex.Athena.Web.ActionFilter;
    using Model.Core.DataDomain;
    using Model.TotalBet.MetaModel;
    using Model.TotalBet.ViewModel;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;
    using Service.TotalBet;
    using Service.TotalBet.Interfaces;

    [FunctionAuthorization(PermissionKeys.TotalBetFirstGoalLastGoal)]
    public class FirstGoalLastGoalTotalBetController : TotalBetBaseController
    {
        private readonly IFirstGoalLastGoalService _firstGoalLastGoalService;

        public FirstGoalLastGoalTotalBetController()
        {
            _firstGoalLastGoalService = new FirstGoalLastGoalService();
        }

        public FirstGoalLastGoalTotalBetController(
            ILanguageService languageService,
            ISessionService sessionService,
            IAppSettings appSetting,
            IFirstGoalLastGoalService firstGoalLastGoalService)
            : base(languageService, sessionService, appSetting)
        {
            _firstGoalLastGoalService = firstGoalLastGoalService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var param = new FirstGoalLastGoalMetaModel
            {
                LicenseeId = SessionService.LicenseeSession.LicenseeId,
            };

            SetConvetedUSDCurrencyName(param);
            return this.GetView(param);
        }

        [HttpPost]
        [CustomValidateAntiForgery]
        public ActionResult Index(FirstGoalLastGoalMetaModel metaModel)
        {
            metaModel.LicenseeId = SessionService.LicenseeSession.LicenseeId;
            return this.GetView(metaModel);
        }

        private ActionResult GetView(FirstGoalLastGoalMetaModel metaModel)
        {
            var userSession = SessionService.UserSession;
            var licenseeSession = SessionService.LicenseeSession;
            metaModel.UserId = userSession.UserId;
            var currencies = userSession.CurrencyList;

            if (metaModel.CurrencyId == 0 || currencies.ContainsKey(metaModel.CurrencyId))
            {
                FirstGoalLastGoal totalData;
                var dataList = _firstGoalLastGoalService.GetFirstGoalLastGoalDataList(metaModel, out totalData);

                var model = new FirstGoalLastGoalViewModel
                {
                    Param = metaModel,
                    MainData = dataList,
                    LicenseeTypeList = licenseeSession.LicenseeType,
                    CurrencyList = currencies,
                    TotalData = totalData
                };

                return View("~/Views/TotalBets/FirstGoalLastGoal/Main.cshtml", model);
            }

            return Redirect("~/Utility/AccessDenied");
        }
    }
}