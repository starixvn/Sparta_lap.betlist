namespace Fanex.Athena.DataAccess.Core.UnitTests.Common
{
    using System;
    using System.Threading.Tasks;
    using Core.Common;
    using Extension.Utility;
    using Model.Core.DTO.Common;
    using NSubstitute;
    using NUnit.Framework;
    using TestHelpers;
    using VendorAdapter.Interfaces;

    public class MatchDataAccessTest : BaseDataAccessHelperTest<MatchDataAccess>
    {
        private IAppSettings _appSettings;

        protected override void SetUpDataAccess()
        {
            _appSettings = Substitute.For<IAppSettings>();
            DataAccess = Substitute.ForPartsOf<MatchDataAccess>(Substitute.For<ICacheRefAdapter>(), _appSettings);
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(100)]
        public async Task GetForecastMatchListByLeague_WhenCall_ParametersPassedCorrectly(int leagueId)
        {
            // Arrange
            dynamic param = new { LeagueId = 0 };
            DataAccess.GetObjectDb(Arg.Is("Lrf_Forecast_MatchList")).Returns(ObjectDb);
            ObjectDb.When(x => x.ExecuteReaderAsync(Arg.Any<object>())).Do(x => param = x[0]);

            // Action
            await DataAccess.GetForecastMatchListByLeague(leagueId);

            // Assert
            Assert.That(param.LeagueId, Is.EqualTo(leagueId));
        }

        [Test]
        public async Task GetForecastMatchListByLeague_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            const int LeagueId = 1;
            var expectedMatch = GetFakeMatch();

            DataAccess.GetObjectDb(Arg.Is("Lrf_Forecast_MatchList")).Returns(ObjectDb);
            MapReaderData(expectedMatch);
            Reader.Read().Returns(true, false);

            // Action
            var result = await DataAccess.GetForecastMatchListByLeague(LeagueId);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result[0].MatchId, Is.EqualTo(expectedMatch.MatchId));
                Assert.That(result[0].LeagueId, Is.EqualTo(LeagueId));
                Assert.That(result[0].IsOutRight, Is.EqualTo(expectedMatch.IsNeutral));
                Assert.That(result[0].HomeId, Is.EqualTo(expectedMatch.HomeId));
                Assert.That(result[0].AwayId, Is.EqualTo(expectedMatch.AwayId));
            });
        }

        [TestCase(false)]
        [TestCase(true)]
        public async Task GetAllRunningMatch_WhenCall_ParametersPassedCorrectly(bool isProduction)
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is("Lrf_LeagueMatchSelRunning")).Returns(ObjectDb);
            const int SportId = 1;
            _appSettings.IsProduction.Returns(isProduction);

            // Action
            await DataAccess.GetAllRunningMatch(SportId);

            // Assert
            ObjectDb.Received(1).SetParameterValue("sporttype", SportId);
            ObjectDb.Received(1).SetParameterValue("IsIncludeTestLeague", !isProduction);
        }

        [Test]
        public async Task GetAllRunningMatch_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            const int SportId = 1;
            var expectedMatch = GetFakeMatch();

            DataAccess.GetObjectDb(Arg.Is("Lrf_LeagueMatchSelRunning")).Returns(ObjectDb);
            MapReaderData(expectedMatch);
            Reader.Read().Returns(true, false);

            // Action
            var result = await DataAccess.GetAllRunningMatch(SportId);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result[0].MatchId, Is.EqualTo(expectedMatch.MatchId));
                Assert.That(result[0].LeagueId, Is.EqualTo(expectedMatch.LeagueId));
                Assert.That(result[0].IsOutRight, Is.EqualTo(expectedMatch.IsOutRight));
                Assert.That(result[0].HomeId, Is.EqualTo(expectedMatch.HomeId));
                Assert.That(result[0].AwayId, Is.EqualTo(expectedMatch.AwayId));
            });
        }

        [Test]
        public async Task GetMatchesByLeague_WhenCall_ExecuteReaderMethodIsCalled()
        {
            // Arrange
            var leagueId = 1;
            var fromDate = new DateTime(2016, 8, 10);
            var toDate = new DateTime(2016, 8, 11);

            dynamic param = new { LeagueId = 0 };
            DataAccess.GetObjectDb(Arg.Is("Lrf_SelMatchByLeague")).Returns(ObjectDb);
            ObjectDb.When(x => x.ExecuteReaderAsync(Arg.Any<object>())).Do(x => param = x[0]);

            // Action
            await DataAccess.GetMatchesByLeague(leagueId, fromDate, toDate);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(param.LeagueID, Is.EqualTo(leagueId));
                Assert.That(param.FromDate, Is.EqualTo(fromDate));
                Assert.That(param.ToDate, Is.EqualTo(toDate));
            });
        }

        [Test]
        public async Task GetMatchesByLeague_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            const int LeagueId = 1;
            var fromDate = new DateTime(2016, 8, 10);
            var toDate = new DateTime(2016, 8, 11);
            var expectedMatch = GetFakeMatch();

            DataAccess.GetObjectDb(Arg.Is("Lrf_SelMatchByLeague")).Returns(ObjectDb);
            MapReaderData(expectedMatch);
            Reader.Read().Returns(true, false);

            // Action
            var result = await DataAccess.GetMatchesByLeague(LeagueId, fromDate, toDate);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result[0].MatchId, Is.EqualTo(expectedMatch.MatchId));
                Assert.That(result[0].HomeId, Is.EqualTo(expectedMatch.HomeId));
                Assert.That(result[0].AwayId, Is.EqualTo(expectedMatch.AwayId));
            });
        }

        private Match GetFakeMatch()
        {
            return new Match
            {
                MatchId = 1,
                LeagueId = 2,
                IsOutRight = true,
                HomeId = 3,
                AwayId = 0,
                IsNeutral = false
            };
        }
    }
}