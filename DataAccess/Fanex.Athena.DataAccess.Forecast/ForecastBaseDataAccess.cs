// --------------------------------------------------------------------------
// <copyright file="ForecastDataAccess.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Forecast
{
    using System.Data;
    using Common.Extensions;
    using Core;
    using Model.Forecast.DTO;
    using VendorAdapter.Interfaces;

    public class ForecastBaseDataAccess : BaseDataAccess
    {
        private readonly ICacheRefAdapter _cacheRefAdapter;

        public ForecastBaseDataAccess(ICacheRefAdapter cacheRefAdapter)
        {
            _cacheRefAdapter = cacheRefAdapter;
        }

        protected void AssignTeamNames(ForecastBase item)
        {
            if (item != null)
            {
                item.HomeTeamName = _cacheRefAdapter.GetTeamName(item.HomeId) + (item.IsNeutral ? " (N)" : string.Empty);
                item.AwayTeamName = _cacheRefAdapter.GetTeamName(item.AwayId);
            }
        }

        protected void ReadCommonData(IDataReader reader, ForecastBase item)
        {
            item.MatchId = reader.ToInt("matchid");
            item.HomeId = reader.ToInt("homeid");
            item.AwayId = reader.ToInt("awayid");
            item.LiveIndicator = reader.ToBool("liveindicator");
            item.LiveHomeScore = reader.ToByte("livehomescore");
            item.LiveAwayScore = reader.ToByte("liveawayscore");
            item.ShowTime = reader.ToString("showtime");
            item.IsNeutral = reader.ToBool("isNeutral");
            AssignTeamNames(item);
        }
    }
}