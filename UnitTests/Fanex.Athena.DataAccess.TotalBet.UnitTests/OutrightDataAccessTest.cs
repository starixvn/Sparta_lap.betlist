namespace Fanex.Athena.DataAccess.TotalBet.UnitTests
{
    using System.Collections.Generic;
    using System.Data;
    using Data;
    using Model.TotalBet.DTO;
    using Model.TotalBet.MetaModel;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class OutrightDataAccessTest
    {
        private IObjectDb _objectDb;
        private OutrightDataAccess _dataAccess;
        private IDataReader _reader;

        [OneTimeSetUp]
        public void Setup()
        {
            _objectDb = Substitute.For<IObjectDb>();
            _dataAccess = Substitute.ForPartsOf<OutrightDataAccess>();
            _reader = Substitute.For<IDataReader>();
            _objectDb.ExecuteReader(Arg.Any<object>()).Returns(_reader);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _objectDb = null;
            _dataAccess = null;
            _reader = null;
        }

        [Test]
        public void GetOutrightDataList_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_TotalBets_Sel10")).Returns(_objectDb);
            var metaModel = new OutrightMetaModel();
            var expected = new Outright
            {
                LeagueId = 1,
                TeamId = 2,
                MatchId = 3,
                Forecast = 10m,
                Balance = 10.5m
            };
            ConfigureReaderForGetOutrightDataListMethod(expected);

            // Action
            var result = _dataAccess.GetOutrightDataList(metaModel)[0];

            // Assert
            Assert.That(result.LeagueId, Is.EqualTo(expected.LeagueId));
            Assert.That(result.TeamId, Is.EqualTo(expected.TeamId));
            Assert.That(result.MatchId, Is.EqualTo(expected.MatchId));
            Assert.That(result.Forecast, Is.EqualTo(expected.Forecast));
            Assert.That(result.Balance, Is.EqualTo(expected.Balance));
        }

        private void ConfigureReaderForGetOutrightDataListMethod(Outright expected)
        {
            _reader.Read().Returns(true, false);

            _reader["leagueid"].Returns(expected.LeagueId);
            _reader["teamid"].Returns(expected.TeamId);
            _reader["orid"].Returns(expected.MatchId);
            _reader["totalwinlost"].Returns(expected.Forecast);
            _reader["balance"].Returns(expected.Balance);
        }
    }
}