namespace Fanex.Athena.DataAccess.Core.UnitTests.Common
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Core.Common;
    using Data;
    using Model.Core.DTO.Common;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class MatchDataAccessTest
    {
        private IObjectDb _objectDb;
        private MatchDataAccess _dataAccess;
        private IDataReader _reader;

        [OneTimeSetUp]
        public void Setup()
        {
            _objectDb = Substitute.For<IObjectDb>();
            _dataAccess = Substitute.ForPartsOf<MatchDataAccess>();
            _reader = Substitute.For<IDataReader>();
            _objectDb.ExecuteReader().Returns(_reader);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _objectDb = null;
            _dataAccess = null;
            _reader = null;
        }

        [Test]
        public void GetForecastMatchListByLeague_WhenCall_ExecuteReaderMethodIsCalled()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_Forecast_MatchList")).Returns(_objectDb);
            var leagueId = 1;

            // Action
            _dataAccess.GetForecastMatchListByLeague(leagueId);

            // Assert
            _objectDb.ReceivedWithAnyArgs().ExecuteReader(null);
        }

        [Test]
        public void GetAllRunningMatch_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_LeagueMatchSelRunning")).Returns(_objectDb);
            var sportId = 1;

            // Action
            _dataAccess.GetAllRunningMatch(sportId);

            // Assert
            _objectDb.Received().AddInParameter("sporttype", System.Data.DbType.Int32, sportId);
        }

        [Test]
        public void GetAllRunningMatch_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_LeagueMatchSelRunning")).Returns(_objectDb);
            ConfigureReader();
            var sportId = 1;
            var expected = GetFakeMatchList();

            // Action
            var result = _dataAccess.GetAllRunningMatch(sportId);

            // Assert
            Assert.That(result[0].MatchId, Is.EqualTo(expected[0].MatchId));
            Assert.That(result[0].LeagueId, Is.EqualTo(expected[0].LeagueId));
            Assert.That(result[0].IsOutRight, Is.EqualTo(expected[0].IsOutRight));
            Assert.That(result[0].HomeId, Is.EqualTo(expected[0].HomeId));
            Assert.That(result[0].AwayId, Is.EqualTo(expected[0].AwayId));
        }

        [Test]
        public void GetMatchesByLeague_WhenCall_ExecuteReaderMethodIsCalled()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_SelMatchByLeague")).Returns(_objectDb);
            var leagueId = 1;
            var fromDate = new DateTime(2016, 8, 10);
            var toDate = new DateTime(2016, 8, 11);

            // Action
            _dataAccess.GetMatchesByLeague(leagueId, fromDate, toDate);

            // Assert
            _objectDb.ReceivedWithAnyArgs().ExecuteReader(null);
        }

        [Test]
        public void GetMatchesByLeague_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_SelMatchByLeague")).Returns(_objectDb);
            _objectDb.ExecuteReader(Arg.Any<object>()).Returns(_reader);
            ConfigureReader();
            var leagueId = 1;
            var fromDate = new DateTime(2016, 8, 10);
            var toDate = new DateTime(2016, 8, 11);
            var expected = GetFakeMatchList();

            // Action
            var result = _dataAccess.GetMatchesByLeague(leagueId, fromDate, toDate);

            // Assert
            Assert.That(result[0].MatchId, Is.EqualTo(expected[0].MatchId));
            Assert.That(result[0].HomeId, Is.EqualTo(expected[0].HomeId));
            Assert.That(result[0].AwayId, Is.EqualTo(expected[0].AwayId));
        }

        private void ConfigureReader()
        {
            _reader.Read().Returns(true, false);
            _reader["matchid"].Returns(1);
            _reader["leagueid"].Returns(2);
            _reader["awayid"].Returns(0);
            _reader["homeid"].Returns(3);
        }

        private IList<Match> GetFakeMatchList()
        {
            return new List<Match>
            {
                new Match
                {
                    MatchId = 1,
                    LeagueId = 2,
                    IsOutRight = true,
                    HomeId = 3,
                    AwayId = 0
                }
            };
        }
    }
}