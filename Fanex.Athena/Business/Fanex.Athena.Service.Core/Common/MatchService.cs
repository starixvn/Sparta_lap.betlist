// -----------------------------------------------------------------------
// <copyright file="MatchService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Core.Common
{
    using System;
    using System.Collections.Generic;
    using Fanex.Athena.DataAccess.Core.Common;
    using Fanex.Athena.DataAccess.Core.Common.Interfaces;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Service.Core.Common.Interfaces;
    using Fanex.Caching;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MatchService : IMatchService
    {
        private const string RunningMatchCacheKey = "RunningMatchCacheKey_";
        private IMatchDataAccess _matchDataAccess = new MatchDataAccess();

        private ICache _cacheInstance = Fanex.Caching.CacheService.CreateInstance(CacheType.MemoryCache);

        #region IMatchService Members

        /// <summary>
        /// Get all running matches.
        /// </summary>
        /// <param name="sportId">The sport identity.</param>
        /// <returns>The match list.</returns>
        public IList<Match> GetAllRunningMatch(int sportId)
        {
            var cacheKey = RunningMatchCacheKey + sportId;
            var isInCache = _cacheInstance.Contains(cacheKey);
            if (isInCache)
            {
                return _cacheInstance.Get<IList<Match>>(cacheKey);
            }
            else
            {
                var matchList = _matchDataAccess.GetAllRunningMatch(sportId);
                _cacheInstance.Set(cacheKey, matchList);

                return matchList;
            }
        }

        /// <summary>
        /// Get all running matches.
        /// </summary>
        /// <param name="leagueId">The league identity.</param>
        /// <returns>The match list.</returns>
        public IList<Match> GetForecastMatchListByLeague(int leagueId)
        {
            return _matchDataAccess.GetForecastMatchListByLeague(leagueId);
        }

        public IList<Match> GetMatchesByLeague(int leagueId, DateTime fromDate, DateTime toDate)
        {            
            return _matchDataAccess.GetMatchesByLeague(leagueId, fromDate, toDate);            
        }

        #endregion IMatchService Members
    }
}