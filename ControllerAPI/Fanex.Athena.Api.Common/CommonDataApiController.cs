namespace Fanex.Athena.Api.Base
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;
    using DataAccess.Core.Common;
    using DataAccess.Core.Common.Interfaces;
    using Model.Core.DataDomain;
    using Model.Core.MetaModel.Common;
    using ONEbook.UM.Filters;
    using Service.Core.Common;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users;
    using Service.Core.Users.Interfaces;
    using Web.ActionFilter;

    [CLSCompliant(false)]
    [CheckUMFilter]
    [XFrameOptionApi]
    [FunctionAuthorization(PermissionKeys.BettingTicketCenter)]
    public class CommonDataApiController : ApiController
    {
        private readonly ISportTypeDataAccess _sportTypeDataAccess;

        private readonly ISessionService _sessionService;

        private readonly ILeagueDataAccess _leagueDataAccess;

        private readonly IMatchDataAccess _matchDataAccess;

        private readonly IBetTypeService _betTypeService;

        public CommonDataApiController()
        {
            _sportTypeDataAccess = new SportTypeDataAccess();
            _sessionService = new SessionService();
            _leagueDataAccess = new LeagueDataAccess();
            _matchDataAccess = new MatchDataAccess();
            _betTypeService = new BetTypeService();
        }

        public CommonDataApiController(
            ISportTypeDataAccess sportTypeDataAccess,
            ISessionService sessionService,
            ILeagueDataAccess leagueDataAccess,
            IMatchDataAccess matchDataAccess,
            IBetTypeService betTypeService)
        {
            _sportTypeDataAccess = sportTypeDataAccess;
            _sessionService = sessionService;
            _leagueDataAccess = leagueDataAccess;
            _matchDataAccess = matchDataAccess;
            _betTypeService = betTypeService;
        }

        [HttpGet]
        public IEnumerable<object> GetSports(string productCode)
        {
            var sports = _sportTypeDataAccess.GetSportsAndProducts(
                 _sessionService.LicenseeSession.LicenseeId,
                 _sessionService.UserSession.Language,
                 productCode);

            return sports.Where(x => x.ParentId > -1).Select(x => new { value = x.ID, label = x.Name });
        }

        [HttpGet]
        public IEnumerable<object> GetBetTypes(string productCode)
        {
            var bettypes = _betTypeService.GetBettypesByProduct(
                _sessionService.LicenseeSession.LicenseeId, 
                productCode, 
                _sessionService.UserSession.Language);

            return bettypes.Select(x => new { value = x.ID, label = x.Name });
        }

        /// <summary>
        /// Get Leagues of SB only
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="sportId"></param>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<object> GetLeagues(DateTime fromDate, DateTime toDate, int sportId)
        {
            var metaModel = new LeagueMetaModel
            {
                FromDate = fromDate,
                ToDate = toDate,
                SportId = sportId,
                LicenseeId = _sessionService.LicenseeSession.LicenseeId,
                UserId = _sessionService.UserSession.UserId,
                AgentListXml = _sessionService.GenerateAgentListXml()
            };

            var leagues = _leagueDataAccess.GetLeaguesBySport(metaModel);

            return leagues.Select(x => new { value = x.LeagueId, label = x.LeagueName });
        }

        [HttpGet]
        public IEnumerable<object> GetMatches(DateTime fromDate, DateTime toDate, int leagueid)
        {
            var matches = _matchDataAccess.GetMatchesByLeague(leagueid, fromDate, toDate);

            return matches.Select(x => new { value = x.MatchId, label = x.MatchName });
        }
    }
}