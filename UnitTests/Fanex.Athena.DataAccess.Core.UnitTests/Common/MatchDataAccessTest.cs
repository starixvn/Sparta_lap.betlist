namespace Fanex.Athena.DataAccess.Core.UnitTests.Common
{
    using System;
    using Core.Common;
    using Model.Core.DTO.Common;
    using NSubstitute;
    using NUnit.Framework;
    using TestHelpers;

    public class MatchDataAccessTest : BaseDataAccessHelperTest<MatchDataAccess>
    {
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(100)]
        public void GetForecastMatchListByLeague_WhenCall_ParametersPassedCorrectly(int leagueId)
        {
            // Arrange
            dynamic param = new { LeagueId = 0 };
            DataAccess.GetObjectDb(Arg.Is("Lrf_Forecast_MatchList")).Returns(ObjectDb);
            ObjectDb.When(x => x.ExecuteReader(Arg.Any<object>())).Do(x => param = x[0]);

            // Action
            DataAccess.GetForecastMatchListByLeague(leagueId);

            // Assert
            Assert.That(param.LeagueId, Is.EqualTo(leagueId));
        }

        [Test]
        public void GetForecastMatchListByLeague_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            const int LeagueId = 1;
            var expectedMatch = GetFakeMatch();

            DataAccess.GetObjectDb(Arg.Is("Lrf_Forecast_MatchList")).Returns(ObjectDb);
            MapReaderData(expectedMatch);
            Reader.Read().Returns(true, false);

            // Action
            var result = DataAccess.GetForecastMatchListByLeague(LeagueId);

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

        [Test]
        public void GetForecastMatchListByLeague_ReaderIsNull_ReturnEmptyResult()
        {
            // Arrange
            const int LeagueId = 1;

            DataAccess.GetObjectDb(Arg.Is("Lrf_Forecast_MatchList")).Returns(ObjectDb);
            ObjectDb.ExecuteReader(Arg.Any<object>()).Returns(x => { return null; });

            // Action
            var result = DataAccess.GetForecastMatchListByLeague(LeagueId);

            // Assert
            Assert.That(result, Is.Empty);
        }

        [Test]
        public void GetAllRunningMatch_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is("Lrf_LeagueMatchSelRunning")).Returns(ObjectDb);
            var sportId = 1;

            // Action
            DataAccess.GetAllRunningMatch(sportId);

            // Assert
            ObjectDb.Received().AddInParameter("sporttype", System.Data.DbType.Int32, sportId);
        }

        [Test]
        public void GetAllRunningMatch_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            const int SportId = 1;
            var expectedMatch = GetFakeMatch();

            DataAccess.GetObjectDb(Arg.Is("Lrf_LeagueMatchSelRunning")).Returns(ObjectDb);
            MapReaderData(expectedMatch);
            Reader.Read().Returns(true, false);

            // Action
            var result = DataAccess.GetAllRunningMatch(SportId);

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
        public void GetMatchesByLeague_WhenCall_ExecuteReaderMethodIsCalled()
        {
            // Arrange
            var leagueId = 1;
            var fromDate = new DateTime(2016, 8, 10);
            var toDate = new DateTime(2016, 8, 11);

            dynamic param = new { LeagueId = 0 };
            DataAccess.GetObjectDb(Arg.Is("Lrf_SelMatchByLeague")).Returns(ObjectDb);
            ObjectDb.When(x => x.ExecuteReader(Arg.Any<object>())).Do(x => param = x[0]);

            // Action
            DataAccess.GetMatchesByLeague(leagueId, fromDate, toDate);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(param.LeagueID, Is.EqualTo(leagueId));
                Assert.That(param.FromDate, Is.EqualTo(fromDate));
                Assert.That(param.ToDate, Is.EqualTo(toDate));
            });
        }

        [Test]
        public void GetMatchesByLeague_WhenCall_DataReturnedCorrectly()
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
            var result = DataAccess.GetMatchesByLeague(LeagueId, fromDate, toDate);

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