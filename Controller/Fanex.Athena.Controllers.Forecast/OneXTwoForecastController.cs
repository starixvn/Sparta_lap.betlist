// --------------------------------------------------------------------------
// <copyright file="OneXTwoForecastController.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Controllers.Forecast
{
    using System.Web.Mvc;
    using Extension.Utility;
    using Fanex.Athena.Web.ActionFilter;
    using Model.Core.DataDomain;
    using Model.Forecast.MetaModel;
    using Model.Forecast.ViewModel;
    using Service.Core.Common.Interfaces;
    using Service.Core.Helpers;
    using Service.Core.Users.Interfaces;
    using Service.Forecast.Interfaces;

    [FunctionAuthorization(PermissionKeys.ForecastOneXTwo)]
    public class OneXTwoForecastController : ForecastBaseController
    {
        public OneXTwoForecastController()
        {
        }

        public OneXTwoForecastController(
            ILanguageService languageService,
            ISessionService sessionService,
            IAppSettings appSetting,
            IClientViewHelper clientViewHelper,
            IForecastService forecastService)
        : base(languageService, sessionService, appSetting, clientViewHelper, forecastService)
        {
        }

        [HttpGet]
        public ActionResult Index()
        {
            var metaModel = new ForecastOneXTwoMetaModel
            {
                LicenseeId = SessionService.LicenseeSession.LicenseeId,
            };

            SetConvetedUSDCurrencyName(metaModel);

            return ProcessAction(metaModel);
        }

        [HttpPost]
        [CustomValidateAntiForgery]
        public ActionResult Index(ForecastOneXTwoMetaModel metaModel)
        {
            metaModel.LicenseeId = SessionService.LicenseeSession.LicenseeId;

            return ProcessAction(metaModel);
        }

        public ActionResult Popup(ForecastOneXTwoMetaModel metaModel)
        {
            metaModel.LicenseeId = SessionService.LicenseeSession.LicenseeId;

            return ProcessAction(metaModel);
        }

        private ActionResult ProcessAction(ForecastOneXTwoMetaModel metaModel)
        {
            var model = GetViewModel(metaModel);

            return View("~/Views/Forecast/Forecast1X2/Main.cshtml", model);
        }

        private ForecastOneXTwoViewModel GetViewModel(ForecastOneXTwoMetaModel metaModel)
        {
            var model = new ForecastOneXTwoViewModel();

            metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId);

            var leagueList = ForecastService.GetForecastRunningLeague(metaModel);
            var currentLeagueId = metaModel.LeagueId;

            if (metaModel.MatchId != 0)
            {
                var forecastOneXTwoByMatch = ForecastService.GetForecastOneXTwoByMatch(metaModel);
                model.MainData = forecastOneXTwoByMatch;
            }
            else
            {
                metaModel.LeagueId = GetLeagueId(currentLeagueId, leagueList);

                var forecastcOneXTwoByLeague = ForecastService.GetForecastOneXTwoByLeague(metaModel);
                model.MainData = forecastcOneXTwoByLeague;
            }

            model.Param = metaModel;
            model.LeagueList = leagueList;
            model.CurrencyList = SessionService.UserSession.CurrencyList;

            return model;
        }
    }
}