// --------------------------------------------------------------------------
// <copyright file="AhForecastController.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bravo.Tran" date="2013.08.13">Created</change>
// </history>

namespace Fanex.Athena.Controllers.Forecast
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Web.Mvc;
    using Fanex.Athena.Model.Core.DataDomain;
    using Fanex.Athena.Model.Forecast.DTO.ForecastAH;
    using Model.Forecast.MetaModel;
    using Model.Forecast.ViewModel;
    using Resources;
    using Web.ActionFilter;

    [FunctionAuthorization(PermissionKeys.ForecastHandicapOverUnderLive)]
    public class AhForecastController : ForecastBaseController
    {        
        public ActionResult Index(ForecastAHMetaModel metaModel)
        {
            metaModel.LicenseeId = SessionService.LicenseeSession.LicenseeId;
            metaModel.UserId = SessionService.UserSession.UserId;

            var model = this.GetViewModel(metaModel);

            return View("~/Views/Forecast/ForecastAH/Main.cshtml", model);
        }

        private static List<string> RenderForecastLink(int betType, int currentScore)
        {
            var forecastLink = new List<string>();
            var endIndex = 0;
            if (currentScore < 0 && IsOverUnder(betType))
            {
                currentScore = 0;
            }

            var startIndex = currentScore + 5;
            endIndex = currentScore - 5;

            if (endIndex < 0 && IsOverUnder(betType))
            {
                endIndex = 0;
            }

            for (var i = startIndex; i >= endIndex; i--)
            {
                forecastLink.Add(i.ToString(CultureInfo.InvariantCulture));
            }

            return forecastLink;
        }

        private static IList<int> RenderForecastHeader(int betType, int currentScore)
        {
            var forecastHeader = new List<int>();
            var startIndex = 0;
            if (betType == (int)BetType.Handicap || betType == (int)BetType.FirstHalfHandicap)
            {
                startIndex = currentScore + 2;
            }
            else
            {
                if (currentScore < 0 && IsOverUnder(betType))
                {
                    currentScore = 0;
                }

                startIndex = currentScore + 4;
            }

            var endIndex = startIndex - 4;

            for (int i = endIndex; i <= startIndex; i++)
            {
                forecastHeader.Add(i);
            }

            return forecastHeader;
        }

        private static bool IsOverUnder(int betType)
        {
            return betType == (int)BetType.OverUnder || betType == (int)BetType.FirstHalfOverUnder;
        }

        private ForecastAHViewModel GetViewModel(ForecastAHMetaModel metaModel)
        {
            var forecastHeaderParam = ForecastService.GetForecastTopOddsByMatch(metaModel.MatchId, metaModel.BetType);
            ForecastValue forecastValue = null;
            var userSession = SessionService.UserSession;
            if (forecastHeaderParam != null)
            {
                /* Set default currentScore*/
                if (Request.QueryString["CurrentScore"] == null)
                {
                    if (metaModel.BetType == (int)BetType.OverUnder || metaModel.BetType == (int)BetType.FirstHalfOverUnder)
                    {
                        // Overunder
                        metaModel.CurrentScore = forecastHeaderParam.LiveHomeScore + forecastHeaderParam.LiveAwayScore;
                    }
                    else
                    {
                        metaModel.CurrentScore = forecastHeaderParam.LiveHomeScore - forecastHeaderParam.LiveAwayScore;
                    }
                }

                /*Handicap or 1st Handicap*/
                if (metaModel.BetType == (int)BetType.Handicap || metaModel.BetType == (int)BetType.FirstHalfHandicap)
                {
                    forecastHeaderParam.ForecastName = metaModel.BetType == (int)BetType.Handicap ? ViewResources.Handicap : ViewResources.lbl1stHandicap;
                    forecastValue = ForecastService.GetForecastHandicap(metaModel, userSession.UserId);
                }
                else
                {
                    /* Over/Under or 1st Over/Under*/
                    forecastHeaderParam.ForecastName = metaModel.BetType == (int)BetType.OverUnder ? ViewResources.OverUnder : ViewResources.lbl1stOver_Under;
                    forecastValue = ForecastService.GetForecastOverUnder(metaModel, userSession.UserId);
                }

                forecastValue.ForeCastLink = RenderForecastLink(metaModel.BetType, metaModel.CurrentScore);
                forecastValue.ForecastHeader = RenderForecastHeader(metaModel.BetType, metaModel.CurrentScore);
            }

            return new ForecastAHViewModel
            {
                Param = metaModel,
                ForecastHeaderParam = forecastHeaderParam,
                ForecastValue = forecastValue
            };
        }
    }
}