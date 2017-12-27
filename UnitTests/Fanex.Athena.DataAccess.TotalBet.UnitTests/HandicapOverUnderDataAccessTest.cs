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
    public class HandicapOverUnderDataAccessTest
    {
        private IObjectDb _objectDb;
        private HandicapOverUnderDataAccess _dataAccess;
        private IDataReader _reader;

        [OneTimeSetUp]
        public void Setup()
        {
            _objectDb = Substitute.For<IObjectDb>();
            _dataAccess = Substitute.ForPartsOf<HandicapOverUnderDataAccess>();
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
        public void GetHandicapOverUnderDataList_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_TotalBets_Sel1378")).Returns(_objectDb);
            var metaModel = new HandicapOverUnderMetaModel();
            var totalData = new HandicapOverUnder();
            var expected = new HandicapOverUnder
            {
                LeagueId = 1,
                MatchId = 2,
                HomeId = 3,
                AwayId = 4,
                ShowTime = "ShowTimeTest",
                BetCountHandicap = 5,
                BetCountOverUnder1StHalf = 6,
                LiveHomeScore = "LiveHomeScoreTest",
                LiveAwayScore = "LiveAwayScoreTest",
                FullTimeHandicap = 8m,
                FullTimeOverUnder = 8m,
                FirstTimeHandicap = 8m,
                FirstTimeOverUnder = 8m
            };
            ConfigureReaderForGetHandicapOverUnderDataListMethod(expected);

            // Action
            var result = _dataAccess.GetHandicapOverUnderDataList(metaModel);
            var firstItem = result.DataList.First();

            // Assert
            Assert.That(firstItem.ShowTime, Is.EqualTo(expected.ShowTime));
            Assert.That(firstItem.LeagueId, Is.EqualTo(expected.LeagueId));
            Assert.That(firstItem.MatchId, Is.EqualTo(expected.MatchId));
            Assert.That(firstItem.HomeId, Is.EqualTo(expected.HomeId));
            Assert.That(firstItem.AwayId, Is.EqualTo(expected.AwayId));
            Assert.That(firstItem.LiveHomeScore, Is.EqualTo(expected.LiveHomeScore));
            Assert.That(firstItem.LiveAwayScore, Is.EqualTo(expected.LiveAwayScore));
            Assert.That(firstItem.LiveIndicator, Is.EqualTo(expected.LiveIndicator));

            Assert.That(firstItem.BetCountHandicap, Is.EqualTo(expected.BetCountHandicap));
            Assert.That(firstItem.BetCountOverUnder, Is.EqualTo(expected.BetCountOverUnder));
            Assert.That(firstItem.BetCountHandicap1StHalf, Is.EqualTo(expected.BetCountHandicap1StHalf));
            Assert.That(firstItem.BetCountOverUnder1StHalf, Is.EqualTo(expected.BetCountOverUnder1StHalf));

            Assert.That(firstItem.FullTimeHandicap, Is.EqualTo(expected.FullTimeHandicap));
            Assert.That(firstItem.FullTimeOverUnder, Is.EqualTo(expected.FullTimeOverUnder));
            Assert.That(firstItem.FirstTimeHandicap, Is.EqualTo(expected.FirstTimeHandicap));
            Assert.That(firstItem.FirstTimeOverUnder, Is.EqualTo(expected.FirstTimeOverUnder));
        }

        private void ConfigureReaderForGetHandicapOverUnderDataListMethod(HandicapOverUnder expected)
        {
            _reader.Read().Returns(true, false);

            _reader["showtime"].Returns(expected.ShowTime);
            _reader["leagueid"].Returns(expected.LeagueId);
            _reader["matchid"].Returns(expected.MatchId);
            _reader["homeID"].Returns(expected.HomeId);
            _reader["awayID"].Returns(expected.AwayId);
            _reader["livehomescore"].Returns(expected.LiveHomeScore);
            _reader["liveawayscore"].Returns(expected.LiveAwayScore);
            _reader["liveindicator"].Returns(expected.LiveIndicator);

            _reader["betcount_hdp"].Returns(expected.BetCountHandicap);
            _reader["betcount_ou"].Returns(expected.BetCountOverUnder);
            _reader["BetCount_HTHDP"].Returns(expected.BetCountHandicap1StHalf);
            _reader["BetCount_HTOU"].Returns(expected.BetCountOverUnder1StHalf);

            _reader["home_odds_HDP"].Returns(10);
            _reader["away_odds_HDP"].Returns(2);
            _reader["home_odds_OU"].Returns(10);
            _reader["away_odds_OU"].Returns(2);

            _reader["home_odds_HTHDP"].Returns(10);
            _reader["away_odds_HTHDP"].Returns(2);
            _reader["home_odds_HTOU"].Returns(10);
            _reader["away_odds_HTOU"].Returns(2);
        }
    }
}