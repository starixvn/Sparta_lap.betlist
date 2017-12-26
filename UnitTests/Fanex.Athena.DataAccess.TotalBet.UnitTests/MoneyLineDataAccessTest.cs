namespace Fanex.Athena.DataAccess.TotalBet.UnitTests
{
    using System.Data;
    using System.Linq;
    using Data;
    using Model.TotalBet.DTO;
    using Model.TotalBet.MetaModel;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class MoneyLineDataAccessTest
    {
        private IObjectDb _objectDb;
        private MoneyLineDataAccess _dataAccess;
        private IDataReader _reader;

        [OneTimeSetUp]
        public void Setup()
        {
            _objectDb = Substitute.For<IObjectDb>();
            _dataAccess = Substitute.ForPartsOf<MoneyLineDataAccess>();
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
        public void GetMoneyLineDataList_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is("Lrf_TotalBets_Sel2021")).Returns(_objectDb);
            var metaModel = new MoneyLineMetaModel();
            var totalData = new MoneyLine();
            var expected = new MoneyLine
            {
                ShowTime = "ShowTimeTest",
                LiveHomeScore = "LiveHomeScoreTest",
                LiveAwayScore = "LiveAwayScoreTest",
                LeagueId = 1,
                MatchId = 2,
                HomeId = 3,
                AwayId = 4,

                TotalHomeFt = 10m,
                ForecastAwayFt = 11m,
                TotalHomeFirstHalf = 12m,
                ForecastHomeFirstHalf = 13m
            };
            ConfigureReaderForGetHomeDrawAwayNoBetDataListMethod(expected);

            // Action
            var result = _dataAccess.GetMoneyLineDataList(metaModel);
            var firstItem = result.DataList.First();

            // Assert
            Assert.That(firstItem.ShowTime, Is.EqualTo(expected.ShowTime));
            Assert.That(firstItem.LeagueId, Is.EqualTo(expected.LeagueId));
            Assert.That(firstItem.MatchId, Is.EqualTo(expected.MatchId));
            Assert.That(firstItem.HomeId, Is.EqualTo(expected.HomeId));
            Assert.That(firstItem.AwayId, Is.EqualTo(expected.AwayId));

            Assert.That(firstItem.TotalHomeFt, Is.EqualTo(expected.TotalHomeFt));
            Assert.That(firstItem.TotalAwayFt, Is.EqualTo(expected.TotalAwayFt));
            Assert.That(firstItem.ForecastHomeFt, Is.EqualTo(expected.ForecastHomeFt));
            Assert.That(firstItem.ForecastAwayFt, Is.EqualTo(expected.ForecastAwayFt));

            Assert.That(firstItem.TotalHomeFirstHalf, Is.EqualTo(expected.TotalHomeFirstHalf));
            Assert.That(firstItem.TotalAwayFirstHalf, Is.EqualTo(expected.TotalAwayFirstHalf));
            Assert.That(firstItem.ForecastHomeFirstHalf, Is.EqualTo(expected.ForecastHomeFirstHalf));
            Assert.That(firstItem.ForecastAwayFirstHalf, Is.EqualTo(expected.ForecastAwayFirstHalf));

            Assert.That(firstItem.LiveHomeScore, Is.EqualTo(expected.LiveHomeScore));
            Assert.That(firstItem.LiveAwayScore, Is.EqualTo(expected.LiveAwayScore));
            Assert.That(firstItem.LiveIndicator, Is.EqualTo(expected.LiveIndicator));
        }

        private void ConfigureReaderForGetHomeDrawAwayNoBetDataListMethod(MoneyLine expected)
        {
            _reader.Read().Returns(true, false);

            _reader["showtime"].Returns(expected.ShowTime);
            _reader["leagueid"].Returns(expected.LeagueId);
            _reader["matchid"].Returns(expected.MatchId);
            _reader["homeID"].Returns(expected.HomeId);
            _reader["awayID"].Returns(expected.AwayId);

            _reader["home_odds"].Returns(expected.TotalHomeFt);
            _reader["away_odds"].Returns(expected.TotalAwayFt);
            _reader["winlost_home"].Returns(expected.ForecastHomeFt);
            _reader["winlost_away"].Returns(expected.ForecastAwayFt);

            _reader["home_odds_HT"].Returns(expected.TotalHomeFirstHalf);
            _reader["away_odds_HT"].Returns(expected.TotalAwayFirstHalf);
            _reader["winlost_home_HT"].Returns(expected.ForecastHomeFirstHalf);
            _reader["winlost_away_HT"].Returns(expected.ForecastAwayFirstHalf);

            _reader["livehomescore"].Returns(expected.LiveHomeScore);
            _reader["liveawayscore"].Returns(expected.LiveAwayScore);
            _reader["liveindicator"].Returns(expected.LiveIndicator);
        }
    }
}