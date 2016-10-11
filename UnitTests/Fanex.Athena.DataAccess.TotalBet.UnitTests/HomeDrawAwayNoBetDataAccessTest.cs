namespace Fanex.Athena.DataAccess.TotalBet.UnitTests
{
    using System.Data;
    using Data;
    using Model.TotalBet.DTO;
    using Model.TotalBet.MetaModel;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class HomeDrawAwayNoBetDataAccessTest
    {
        private IObjectDb _objectDb;
        private HomeDrawAwayNoBetDataAccess _dataAccess;
        private IDataReader _reader;

        [SetUp]
        public void Setup()
        {
            _objectDb = Substitute.For<IObjectDb>();
            _dataAccess = Substitute.ForPartsOf<HomeDrawAwayNoBetDataAccess>();
            _reader = Substitute.For<IDataReader>();
            _objectDb.ExecuteReader(Arg.Any<object>()).Returns(_reader);
        }

        [Test]
        public void GetHomeDrawAwayNoBetDataList_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_TotalBets_Sel121")).Returns(_objectDb);
            var metaModel = new HomeDrawAwayNoBetMetaModel();
            var expected = new HomeDrawAwayNoBet
            {
                ShowTime = "ShowTimeTest",
                LiveHomeScore = "LiveHomeScoreTest",
                LiveAwayScore = "LiveAwayScoreTest",
                LeagueId = 1,
                MatchId = 2,
                HomeId = 3,
                AwayId = 4,
                BetCountAwayNoBet = 5,
                BetCountDrawNoBet = 6,
                HomeOddsHomeNoBet = 10m,
                AwayOddsDrawNoBet = 11m,
                AwayOddsAwayNoBet = 12m
            };
            ConfigureReaderForGetHomeDrawAwayNoBetDataList(expected);

            // Action
            var result = _dataAccess.GetHomeDrawAwayNoBetDataList(metaModel)[0];

            // Assert
            Assert.That(result.LiveIndicator, Is.EqualTo(expected.LiveIndicator));
            Assert.That(result.LiveHomeScore, Is.EqualTo(expected.LiveHomeScore));
            Assert.That(result.LiveAwayScore, Is.EqualTo(expected.LiveAwayScore));
            Assert.That(result.ShowTime, Is.EqualTo(expected.ShowTime));
            Assert.That(result.LeagueId, Is.EqualTo(expected.LeagueId));
            Assert.That(result.MatchId, Is.EqualTo(expected.MatchId));
            Assert.That(result.HomeId, Is.EqualTo(expected.HomeId));
            Assert.That(result.AwayId, Is.EqualTo(expected.AwayId));
            Assert.That(result.IsNeutral, Is.EqualTo(expected.IsNeutral));
            Assert.That(result.BetCountHomeNoBet, Is.EqualTo(expected.BetCountHomeNoBet));
            Assert.That(result.BetCountDrawNoBet, Is.EqualTo(expected.BetCountDrawNoBet));
            Assert.That(result.BetCountAwayNoBet, Is.EqualTo(expected.BetCountAwayNoBet));
            Assert.That(result.HomeOddsHomeNoBet, Is.EqualTo(expected.HomeOddsHomeNoBet));
            Assert.That(result.AwayOddsHomeNoBet, Is.EqualTo(expected.AwayOddsHomeNoBet));
            Assert.That(result.HomeOddsDrawNoBet, Is.EqualTo(expected.HomeOddsDrawNoBet));
            Assert.That(result.AwayOddsDrawNoBet, Is.EqualTo(expected.AwayOddsDrawNoBet));
            Assert.That(result.HomeOddsAwayNoBet, Is.EqualTo(expected.HomeOddsAwayNoBet));
            Assert.That(result.AwayOddsAwayNoBet, Is.EqualTo(expected.AwayOddsAwayNoBet));
        }

        private void ConfigureReaderForGetHomeDrawAwayNoBetDataList(HomeDrawAwayNoBet expected)
        {
            _reader.Read().Returns(true, false);

            _reader["liveindicator"].Returns(expected.LiveIndicator);
            _reader["livehomescore"].Returns(expected.LiveHomeScore);
            _reader["liveawayscore"].Returns(expected.LiveAwayScore);
            _reader["showtime"].Returns(expected.ShowTime);
            _reader["leagueid"].Returns(expected.LeagueId);
            _reader["matchid"].Returns(expected.MatchId);
            _reader["homeid"].Returns(expected.HomeId);
            _reader["awayid"].Returns(expected.AwayId);
            _reader["isNeutral"].Returns(expected.IsNeutral);
            _reader["betcount_hnb"].Returns(expected.BetCountHomeNoBet);
            _reader["betcount_dnb"].Returns(expected.BetCountDrawNoBet);
            _reader["betcount_anb"].Returns(expected.BetCountAwayNoBet);
            _reader["home_odds_hnb"].Returns(expected.HomeOddsHomeNoBet);
            _reader["away_odds_hnb"].Returns(expected.AwayOddsHomeNoBet);
            _reader["home_odds_dnb"].Returns(expected.HomeOddsDrawNoBet);
            _reader["away_odds_dnb"].Returns(expected.AwayOddsDrawNoBet);
            _reader["home_odds_anb"].Returns(expected.HomeOddsAwayNoBet);
            _reader["away_odds_anb"].Returns(expected.AwayOddsAwayNoBet);
        }
    }
}