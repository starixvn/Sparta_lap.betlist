// --------------------------------------------------------------------------
// <copyright file="IForecastDataAccess.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Shadow.Nguyen" date="2012.12.19">Created</change>
// </history>

namespace Fanex.Athena.DataAccess.Interface.Forecast
{
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Athena.Models.DTO.Common;
    using Fanex.Athena.Models.DTO.Forecast.ForecastHDOU;
    using Fanex.Athena.Models.DTO.Forecast.NewForecastHdpOu;
    using Models.DTO.Forecast.Forecast1X2;
    using Models.DTO.Forecast.ForecastAH;
    using Models.MetaModel.Forecast;
    using Models.ViewModel.Forecast;

    /// <summary>
    /// Forecast Interface.
    /// </summary>
    public interface IForecastDataAccess
    {
        #region Forecast AH

        ForecastAh GetFcTopOddsByMatch(int matchId, int betType);

        DataTable GetForecastHandicap(ForecastAhMetaModel metaModel, int userId);

        DataTable GetForecastOverUnder(ForecastAhMetaModel metaModel, int userId);

        #endregion Forecast AH

        #region Forecast 1 x 2

        IList<League> GetFcRunningLeague(ForecastBaseMetaModel metaModel);

        IList<ForecastOneXTwo> GetFcOneXTwoByMatch(ForecastOneXTwoMetaModel metaModel, int userId);

        IList<ForecastOneXTwo> GetFcOneXTwoByLeague(ForecastOneXTwoMetaModel metaModel);

        #endregion Forecast 1 x 2

        #region Forecast HDP OU

        IList<ForecastHandicapOverUnder> GetFcHandicapOverUnder(ForecastHdpOuMetaModel metaModel);

        #endregion Forecast HDP OU

        #region New Forecast

        NewForecastHdpOuData GetNewForecastData(NewForecastHdpOuMetaModel metaModel, NewForecastHdpOuMatchInfo headerInfo);

        #endregion New Forecast
    }
}