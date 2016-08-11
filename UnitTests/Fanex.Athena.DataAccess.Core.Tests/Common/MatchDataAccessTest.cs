namespace Fanex.Athena.DataAccess.Core.Tests.Common
{
    using System;
    using Core.Common;
    using Data;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class MatchDataAccessTest
    {
        private IObjectDb _objectDb;
        private MatchDataAccess _dataAccess;

        [SetUp]
        public void Setup()
        {
            _objectDb = Substitute.For<IObjectDb>();
            _dataAccess = Substitute.ForPartsOf<MatchDataAccess>();
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
    }
}