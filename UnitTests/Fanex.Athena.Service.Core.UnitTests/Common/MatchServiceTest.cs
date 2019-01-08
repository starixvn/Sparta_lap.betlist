namespace Fanex.Athena.Service.Core.UnitTests.Common
{
    using System;
    using System.Collections.Generic;
    using Core.Common;
    using Core.Common.Interfaces;
    using DataAccess.Core.Common.Interfaces;
    using Extension.Utility.Interfaces;
    using Model.Core.DTO.Common;
    using NSubstitute;
    using NUnit.Framework;
    using UnitTest.Common.Helpers;

    [TestFixture]
    public class MatchServiceTest
    {
        private const string Language = "en-Us";
        private static readonly IList<Match> ExpectedResultDataList = new List<Match>
            {
                new Match { MatchId = 1 }
            };

        private IMemoryCacheService _cache;
        private IMatchDataAccess _matchDataAccess;
        private IMatchService _matchService;

        [SetUp]
        public void SetUp()
        {
            _cache = Substitute.For<IMemoryCacheService>();
            _matchDataAccess = Substitute.For<IMatchDataAccess>();
            _matchService = new MatchService(_matchDataAccess, _cache);
        }

        [TearDown]
        public void TearDown()
        {
            _matchDataAccess = null;
            _matchService = null;
        }

        [Test]
        public void GetAllRunningMatch_CacheContainsData_ReturnDataFromCache()
        {
            // Arrange
            var sportId = FakeHelper.Create<int>();
            _cache.Contains(Arg.Any<string>()).Returns(true);
            _cache.Get<IList<Match>>(Arg.Any<string>()).Returns(ExpectedResultDataList);

            // Action
            var result = _matchService.GetAllRunningMatch(sportId, Language);

            // Assert
            Assert.Multiple(() =>
            {
                _cache.Received().Get<IList<Match>>(Arg.Any<string>());
                Assert.That(result[0].MatchId, Is.EqualTo(ExpectedResultDataList[0].MatchId));
            });
        }

        [Test]
        public void GetAllRunningMatch_WhenCall_ReturnResultFromDataAccess()
        {
            // Arrange
            var sportId = FakeHelper.Create<int>();
            _matchDataAccess.GetAllRunningMatch(sportId).Returns(ExpectedResultDataList);

            // Action
            var result = _matchService.GetAllRunningMatch(sportId, Language);

            // Assert
            Assert.Multiple(() =>
            {
                _matchDataAccess.Received().GetAllRunningMatch(sportId);
                Assert.That(result[0].MatchId, Is.EqualTo(ExpectedResultDataList[0].MatchId));
            });
        }

        [Test]
        public void GetAllRunningMatch_GetDataFromCache_ReturnCachedData()
        {
            // Arrange
            var sportId = FakeHelper.Create<int>();
            _matchDataAccess.GetAllRunningMatch(sportId).Returns(ExpectedResultDataList);
            var firstCall = _matchService.GetAllRunningMatch(sportId, Language);

            // Action
            var result = _matchService.GetAllRunningMatch(sportId, Language);

            // Assert
            Assert.That(result, Is.EqualTo(ExpectedResultDataList));
        }

        [Test]
        public void GetForecastMatchListByLeague_WhenCall_ReturnResultFromDataAccess()
        {
            // Arrange
            var leagueId = FakeHelper.Create<int>();
            _matchDataAccess.GetForecastMatchListByLeague(leagueId).Returns(ExpectedResultDataList);

            // Action
            var result = _matchService.GetForecastMatchListByLeague(leagueId);

            // Assert
            Assert.That(result[0].MatchId, Is.EqualTo(ExpectedResultDataList[0].MatchId));
        }

        [Test]
        public void GetMatchesByLeague_WhenCall_ReturnResultFromDataAccess()
        {
            // Arrange
            var leagueId = FakeHelper.Create<int>();
            var fromDate = FakeHelper.Create<DateTime>();
            var toDate = FakeHelper.Create<DateTime>();
            _matchDataAccess.GetMatchesByLeague(leagueId, fromDate, toDate).Returns(ExpectedResultDataList);

            // Action
            var result = _matchService.GetMatchesByLeague(leagueId, fromDate, toDate);

            // Assert
            Assert.That(result[0].MatchId, Is.EqualTo(ExpectedResultDataList[0].MatchId));
        }
    }
}