// --------------------------------------------------------------------------
// <copyright file="ForecastService.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Service.Forecast
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using DataAccess.Core.Common.Interfaces;
    using DataAccess.Forecast.Interfaces;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Model.Forecast.DTO.ForecastHdou;
    using Interfaces;
    using Model.Forecast.DTO.Forecast1X2;
    using Model.Forecast.DTO.NewForecastHdpOu;
    using Model.Forecast.MetaModel;
    using Model.Forecast.ViewModel;

    public class ForecastService : IForecastService
    {
        private readonly ILeagueDataAccess _leagueDataAccess;

        private readonly IForecastDataAccess _forecastDataAccess;

        public ForecastService(ILeagueDataAccess leagueDataAccess, IForecastDataAccess forecastDataAccess)
        {
            _leagueDataAccess = leagueDataAccess;
            _forecastDataAccess = forecastDataAccess;
        }

        #region Forecast 1 x 2

        public Task<IEnumerable<League>> GetForecastRunningLeague(ForecastBaseMetaModel metaModel)
        {
            return _leagueDataAccess.GetRunningLeagues(metaModel.AgentListXml, metaModel.BetTypeString);
        }

        public IList<ForecastOneXTwo> GetForecastOneXTwoByMatch(ForecastOneXTwoMetaModel metaModel)
        {
            return _forecastDataAccess.GetForecastOneXTwoByMatch(metaModel);
        }

        public IList<ForecastOneXTwo> GetForecastOneXTwoByLeague(ForecastOneXTwoMetaModel metaModel)
        {
            return _forecastDataAccess.GetForecastOneXTwoByLeague(metaModel);
        }

        #endregion Forecast 1 x 2

        #region Forecast Handicap Over/Under

        public IList<ForecastHandicapOverUnder> GetForecastHandicapOverUnder(ForecastHandicapOverUnderMetaModel metaModel)
        {
            return _forecastDataAccess.GetForecastHandicapOverUnder(metaModel);
        }

        #endregion Forecast Handicap Over/Under

        #region New Forecast

        public NewForecastHdpOuData GetNewForecastData(NewForecastHandicapOverUnderMetaModel metaModel, NewForecastHdpOuMatchInfo headerInfo)
        {
            return metaModel.IsValid
                ? _forecastDataAccess.GetNewForecastData(metaModel, headerInfo)
                : new NewForecastHdpOuData();
        }

        #endregion New Forecast
    }
}