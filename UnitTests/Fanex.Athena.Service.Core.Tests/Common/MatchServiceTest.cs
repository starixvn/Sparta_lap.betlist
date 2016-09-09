namespace Fanex.Athena.Service.Core.Tests.Common
{
    using System;
    using System.Collections.Generic;
    using Core.Common;
    using Core.Common.Interfaces;
    using DataAccess.Core.Common.Interfaces;
    using Model.Core.DTO.Common;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class MatchServiceTest
    {
        private IMatchDataAccess _matchDataAccess;
        private IMatchService _matchService;

        [SetUp]
        public void SetUp()
        {
            _matchDataAccess = Substitute.For<IMatchDataAccess>();
            _matchService = new MatchService(_matchDataAccess);
        }

        [Test]
        public void GetAllRunningMatch_WhenCall_ReturnResultFromDataAccess()
        {
            // Arrange
            var expectedResult = new List<Match>()
            {
                new Match() { MatchId = 1 }
            };
            var sportId = 1;
            _matchDataAccess.GetAllRunningMatch(sportId).Returns(expectedResult);

            // Action
            var result = _matchService.GetAllRunningMatch(sportId);

            // Assert
            Assert.That(result[0].MatchId, Is.EqualTo(expectedResult[0].MatchId));
        }

        [Test]
        public void GetForecastMatchListByLeague_WhenCall_ReturnResultFromDataAccess()
        {
            // Arrange
            var expectedResult = new List<Match>()
            {
                new Match() { MatchId = 1 }
            };
            var leagueId = 1;
            _matchDataAccess.GetForecastMatchListByLeague(leagueId).Returns(expectedResult);

            // Action
            var result = _matchService.GetForecastMatchListByLeague(leagueId);

            // Assert
            Assert.That(result[0].MatchId, Is.EqualTo(expectedResult[0].MatchId));
        }

        [Test]
        public void GetMatchesByLeague_WhenCall_ReturnResultFromDataAccess()
        {
            // Arrange
            var expectedResult = new List<Match>()
            {
                new Match() { MatchId = 1 }
            };
            var leagueId = 1;
            var fromDate = new DateTime(2016, 1, 1);
            var toDate = new DateTime(2016, 2, 2);
            _matchDataAccess.GetMatchesByLeague(leagueId, fromDate, toDate).Returns(expectedResult);

            // Action
            var result = _matchService.GetMatchesByLeague(leagueId, fromDate, toDate);

            // Assert
            Assert.That(result[0].MatchId, Is.EqualTo(expectedResult[0].MatchId));
        }
    }
}