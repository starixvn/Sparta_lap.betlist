// --------------------------------------------------------------------------
// <copyright file="FirstGoalLastGoalTotalBetController.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Controllers.TotalBets
{
    using System.Web.Mvc;
    using DataAccess.TotalBet;
    using DataAccess.TotalBet.Interfaces;
    using Extension.Utility;
    using Extension.Utility.Interfaces;
    using Fanex.Athena.Model.TotalBet.DTO;
    using Fanex.Athena.Web.ActionFilter;
    using Model.Core.DataDomain;
    using Model.TotalBet.MetaModel;
    using Model.TotalBet.ViewModel;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;
    using Service.Core.Helpers;

    [FunctionAuthorization(PermissionKeys.TotalBetFirstGoalLastGoal)]
    public class FirstGoalLastGoalTotalBetController : TotalBetBaseController
    {
        private readonly IFirstGoalLastGoalDataAccess _firstGoalLastGoalDataAccess;

        public FirstGoalLastGoalTotalBetController()
        {
            _firstGoalLastGoalDataAccess = new FirstGoalLastGoalDataAccess();
        }

        public FirstGoalLastGoalTotalBetController(
            ILanguageService languageService,
            ISessionService sessionService,
            IAppSettings appSetting,
            IFirstGoalLastGoalDataAccess firstGoalLastGoalDataAccess,
           IClientViewHelper clientViewHelper
            )
            : base(languageService, sessionService, appSetting, clientViewHelper)
        {
            _firstGoalLastGoalDataAccess = firstGoalLastGoalDataAccess;
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
            var currencies = SessionService.UserSession.CurrencyList;

            if (metaModel.CurrencyId == 0 || currencies.ContainsKey(metaModel.CurrencyId))
            {
                metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId);
                FirstGoalLastGoal totalData;
                var dataList = _firstGoalLastGoalDataAccess.GetFirstGoalLastGoalDataList(metaModel, out totalData);

                var model = new FirstGoalLastGoalViewModel
                {
                    Param = metaModel,
                    MainData = dataList,
                    CurrencyList = currencies,
                    TotalData = totalData
                };

                return View("~/Views/TotalBets/FirstGoalLastGoal/Main.cshtml", model);
            }

            return Redirect("~/Utility/AccessDenied");
        }
    }
}