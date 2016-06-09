// --------------------------------------------------------------------------
// <copyright file="IForecastDataAccess.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Shadow.Nguyen" date="2012.12.19">Created</change>
// </history>

namespace Fanex.Athena.DataAccess.Forecast.Interfaces
{
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Model.Forecast.DTO.ForecastHdou;
    using Model.Forecast.DTO.Forecast1X2;
    using Model.Forecast.DTO.ForecastAH;
    using Model.Forecast.DTO.NewForecastHdpOu;
    using Model.Forecast.MetaModel;
    using Model.Forecast.ViewModel;

    /// <summary>
    /// Forecast Interface.
    /// </summary>
    public interface IForecastDataAccess
    {
        #region Forecast AH

        ForecastAH GetForecastTopOddsByMatch(int matchId, int betType);

        DataTable GetForecastHandicap(ForecastAHMetaModel metaModel, int userId);

        DataTable GetForecastOverUnder(ForecastAHMetaModel metaModel, int userId);

        #endregion Forecast AH

        #region Forecast 1 x 2

        IList<League> GetForecastRunningLeague(ForecastBaseMetaModel metaModel);

        IList<ForecastOneXTwo> GetForecastOneXTwoByMatch(ForecastOneXTwoMetaModel metaModel, int userId);

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