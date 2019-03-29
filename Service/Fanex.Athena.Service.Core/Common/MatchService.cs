// -----------------------------------------------------------------------
// <copyright file="MatchService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Core.Common
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Extension.Utility.Interfaces;
    using Fanex.Athena.DataAccess.Core.Common.Interfaces;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Service.Core.Common.Interfaces;

    public class MatchService : IMatchService
    {
        private const string RunningMatchCacheKey = "RunningMatchCacheKey_";

        private readonly IMatchDataAccess _matchDataAccess;

        private readonly IMemoryCacheService _cache;

        public MatchService(IMatchDataAccess matchDataAccess, IMemoryCacheService cache)
        {
            _matchDataAccess = matchDataAccess;
            _cache = cache;
        }

        public async Task<IList<Match>> GetAllRunningMatch(int sportId, string languageCode)
        {
            var cacheKey = RunningMatchCacheKey + sportId + languageCode;
            var isInCache = _cache.Contains(cacheKey);
            if (isInCache)
            {
                return _cache.Get<IList<Match>>(cacheKey);
            }
            else
            {
                var matchList = await _matchDataAccess.GetAllRunningMatch(sportId).ConfigureAwait(false);
                _cache.Set(cacheKey, matchList);

                return matchList;
            }
        }

        public Task<IList<Match>> GetForecastMatchListByLeague(int leagueId)
        {
            return _matchDataAccess.GetForecastMatchListByLeague(leagueId);
        }

        public Task<IList<Match>> GetMatchesByLeague(int leagueId, DateTime fromDate, DateTime toDate)
        {
            return _matchDataAccess.GetMatchesByLeague(leagueId, fromDate, toDate);
        }
    }
}