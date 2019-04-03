// --------------------------------------------------------------------------
// <copyright file="IForecastDataAccess.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Forecast.Interfaces
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.Forecast.DTO.ForecastHdou;
    using Model.Forecast.DTO.Forecast1X2;
    using Model.Forecast.DTO.NewForecastHdpOu;
    using Model.Forecast.MetaModel;
    using Model.Forecast.ViewModel;

    public interface IForecastDataAccess
    {
        #region Forecast 1 x 2

        IList<ForecastOneXTwo> GetForecastOneXTwoByMatch(ForecastOneXTwoMetaModel metaModel);

        IList<ForecastOneXTwo> GetForecastOneXTwoByLeague(ForecastOneXTwoMetaModel metaModel);

        #endregion Forecast 1 x 2

        #region Forecast HDP OU

        IList<ForecastHandicapOverUnder> GetForecastHandicapOverUnder(ForecastHandicapOverUnderMetaModel metaModel);

        #endregion Forecast HDP OU

        #region New Forecast

        NewForecastHdpOuData GetNewForecastData(NewForecastHandicapOverUnderMetaModel metaModel, NewForecastHdpOuMatchInfo matchInfo);

        #endregion New Forecast
    }
}