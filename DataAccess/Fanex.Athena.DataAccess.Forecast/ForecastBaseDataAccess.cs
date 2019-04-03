// --------------------------------------------------------------------------
// <copyright file="ForecastDataAccess.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Forecast
{
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

        public void AssignTeamNames(ForecastBase item)
        {
            if (item != null)
            {
                item.HomeTeamName = _cacheRefAdapter.GetTeamName(item.HomeId) + (item.IsNeutral ? " (N)" : string.Empty);
                item.AwayTeamName = _cacheRefAdapter.GetTeamName(item.AwayId);
            }
        }
    }
}