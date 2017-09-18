// -----------------------------------------------------------------------
// <copyright file="MatchService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Core.Common
{
    using System;
    using System.Collections.Generic;
    using Extension.Utility;
    using Extension.Utility.Interfaces;
    using Fanex.Athena.DataAccess.Core.Common;
    using Fanex.Athena.DataAccess.Core.Common.Interfaces;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Service.Core.Common.Interfaces;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MatchService : IMatchService
    {
        private const string RunningMatchCacheKey = "RunningMatchCacheKey_";

        private readonly IMatchDataAccess _matchDataAccess;

        private readonly IMemoryCacheService _cache;

        public MatchService()
        {
            _matchDataAccess = new MatchDataAccess();
            _cache = new MemoryCacheService();
        }

        public MatchService(IMatchDataAccess matchDataAccess)
        {
            _matchDataAccess = matchDataAccess;
        }

        public IList<Match> GetAllRunningMatch(int sportId)
        {
            var cacheKey = RunningMatchCacheKey + sportId;
            var isInCache = _cache.Contains(cacheKey);
            if (isInCache)
            {
                return _cache.Get<IList<Match>>(cacheKey);
            }
            else
            {
                var matchList = _matchDataAccess.GetAllRunningMatch(sportId);
                _cache.Set(cacheKey, matchList);

                return matchList;
            }
        }

        public IList<Match> GetForecastMatchListByLeague(int leagueId)
        {
            return _matchDataAccess.GetForecastMatchListByLeague(leagueId);
        }

        public IList<Match> GetMatchesByLeague(int leagueId, DateTime fromDate, DateTime toDate)
        {
            return _matchDataAccess.GetMatchesByLeague(leagueId, fromDate, toDate);
        }
    }
}