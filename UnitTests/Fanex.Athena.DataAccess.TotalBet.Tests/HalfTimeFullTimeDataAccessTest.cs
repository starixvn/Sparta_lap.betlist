namespace Fanex.Athena.DataAccess.TotalBet.Tests
{
    using System.Data;
    using Data;
    using Model.TotalBet.DTO;
    using Model.TotalBet.MetaModel;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class HalfTimeFullTimeDataAccessTest
    {
        private IObjectDb _objectDb;
        private HalfTimeFullTimeDataAccess _dataAccess;
        private IDataReader _reader;

        [SetUp]
        public void Setup()
        {
            _objectDb = Substitute.For<IObjectDb>();
            _dataAccess = Substitute.ForPartsOf<HalfTimeFullTimeDataAccess>();
            _reader = Substitute.For<IDataReader>();
            _objectDb.ExecuteReader(Arg.Any<object>()).Returns(_reader);
        }

        [Test]
        public void GetHalfTimeFullTimeDataList_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_TotalBets_Sel16")).Returns(_objectDb);
            var metaModel = new HalfTimeFullTimeMetaModel();
            var totalData = new HalfTimeFullTime();
            var expected = new HalfTimeFullTime
            {
                MatchId = 1,
                HomeId = 2,
                AwayId = 3,
                IsNeutral = 0,
                HomeHome = 10m,
                DrawHome = 11m,
                AwayHome = 12m
            };
            ConfigureReaderForGetHalfTimeFullTimeDataListMethod(expected);

            // Action
            var result = _dataAccess.GetHalfTimeFullTimeDataList(metaModel, out totalData)[0];

            // Assert
            Assert.That(result.MatchId, Is.EqualTo(expected.MatchId));
            Assert.That(result.HomeId, Is.EqualTo(expected.HomeId));
            Assert.That(result.AwayId, Is.EqualTo(expected.AwayId));
            Assert.That(result.IsNeutral, Is.EqualTo(expected.IsNeutral));

            Assert.That(result.HomeHome, Is.EqualTo(expected.HomeHome));
            Assert.That(result.HomeDraw, Is.EqualTo(expected.HomeDraw));
            Assert.That(result.HomeAway, Is.EqualTo(expected.HomeAway));
            Assert.That(result.DrawHome, Is.EqualTo(expected.DrawHome));
            Assert.That(result.DrawDraw, Is.EqualTo(expected.DrawDraw));
            Assert.That(result.DrawAway, Is.EqualTo(expected.DrawAway));
            Assert.That(result.AwayHome, Is.EqualTo(expected.AwayHome));
            Assert.That(result.AwayDraw, Is.EqualTo(expected.AwayDraw));
            Assert.That(result.AwayAway, Is.EqualTo(expected.AwayAway));
        }

        private void ConfigureReaderForGetHalfTimeFullTimeDataListMethod(HalfTimeFullTime expected)
        {
            _reader.Read().Returns(true, false);

            _reader["matchid"].Returns(expected.MatchId);
            _reader["isNeutral"].Returns(expected.IsNeutral);
            _reader["homeID"].Returns(expected.HomeId);
            _reader["awayID"].Returns(expected.AwayId);

            _reader["totalHH"].Returns(expected.HomeHome);
            _reader["totalHD"].Returns(expected.HomeDraw);
            _reader["totalHA"].Returns(expected.HomeAway);
            _reader["totalDH"].Returns(expected.DrawHome);
            _reader["totalDD"].Returns(expected.DrawDraw);
            _reader["totalDA"].Returns(expected.DrawAway);
            _reader["totalAH"].Returns(expected.AwayHome);
            _reader["totalAD"].Returns(expected.AwayDraw);
            _reader["totalAA"].Returns(expected.AwayAway);
        }
    }
}