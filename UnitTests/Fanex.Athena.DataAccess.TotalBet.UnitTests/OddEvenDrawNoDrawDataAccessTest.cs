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
    public class OddEvenDrawNoDrawDataAccessTest
    {
        private IObjectDb _objectDb;
        private OddEvenDrawNoDrawDataAccess _dataAccess;
        private IDataReader _reader;

        [OneTimeSetUp]
        public void Setup()
        {
            _objectDb = Substitute.For<IObjectDb>();
            _dataAccess = Substitute.ForPartsOf<OddEvenDrawNoDrawDataAccess>();
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
        public void GetOddEventDrawNoDrawDataList_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_TotalBets_Sel25")).Returns(_objectDb);
            var metaModel = new OddEvenDrawNoDrawMetaModel();
            var totalData = new OddEvenDrawNoDraw();
            var expected = new OddEvenDrawNoDraw
            {
                ShowTime = "ShowTimeTest",
                LeagueId = 1,
                MatchId = 2,
                HomeId = 3,
                AwayId = 4,
                Bettype = 5,
                LiveHomeScore = "LiveHomeScoreTest",
                LiveAwayScore = "LiveAwayScoreTest",
                BetCountOddEven = 6,
                BetCountOddEven1StHalf = 7,
                OddEvenFt = 8m,
                OneXTwoFt = 6m,
                DrawNoDraw = 8m,
                OddEvenFirstHalf = 8m,
                OneXTwoFirstHalf = 6m,
            };
            ConfigureReaderForGetOddEventDrawNoDrawDataListMethod(expected);

            // Action
            var result = _dataAccess.GetOddEventDrawNoDrawDataList(metaModel, out totalData)[0];

            // Assert
            Assert.That(result.ShowTime, Is.EqualTo(expected.ShowTime));
            Assert.That(result.LeagueId, Is.EqualTo(expected.LeagueId));
            Assert.That(result.MatchId, Is.EqualTo(expected.MatchId));
            Assert.That(result.HomeId, Is.EqualTo(expected.HomeId));
            Assert.That(result.AwayId, Is.EqualTo(expected.AwayId));
            Assert.That(result.Bettype, Is.EqualTo(expected.Bettype));
            Assert.That(result.LiveHomeScore, Is.EqualTo(expected.LiveHomeScore));
            Assert.That(result.LiveAwayScore, Is.EqualTo(expected.LiveAwayScore));
            Assert.That(result.LiveIndicator, Is.EqualTo(expected.LiveIndicator));
            Assert.That(result.BetCountOddEven, Is.EqualTo(expected.BetCountOddEven));
            Assert.That(result.BetCountOneXTwo, Is.EqualTo(expected.BetCountOneXTwo));
            Assert.That(result.BetCountDrawNoDraw, Is.EqualTo(expected.BetCountDrawNoDraw));
            Assert.That(result.BetCountOddEven1StHalf, Is.EqualTo(expected.BetCountOddEven1StHalf));
            Assert.That(result.BetCountOneXTow1StHalf, Is.EqualTo(expected.BetCountOneXTow1StHalf));
            Assert.That(result.OddEvenFt, Is.EqualTo(expected.OddEvenFt));
            Assert.That(result.OneXTwoFt, Is.EqualTo(expected.OneXTwoFt));
            Assert.That(result.DrawNoDraw, Is.EqualTo(expected.DrawNoDraw));
            Assert.That(result.OddEvenFirstHalf, Is.EqualTo(expected.OddEvenFirstHalf));
            Assert.That(result.OneXTwoFirstHalf, Is.EqualTo(expected.OneXTwoFirstHalf));
        }

        private void ConfigureReaderForGetOddEventDrawNoDrawDataListMethod(OddEvenDrawNoDraw expected)
        {
            _reader.Read().Returns(true, false);

            _reader["showtime"].Returns(expected.ShowTime);
            _reader["leagueid"].Returns(expected.LeagueId);
            _reader["matchid"].Returns(expected.MatchId);
            _reader["homeID"].Returns(expected.HomeId);
            _reader["awayID"].Returns(expected.AwayId);
            _reader["bettype"].Returns(expected.Bettype);
            _reader["livehomescore"].Returns(expected.LiveHomeScore);
            _reader["liveawayscore"].Returns(expected.LiveAwayScore);
            _reader["liveindicator"].Returns(expected.LiveIndicator);

            _reader["betcount_oe"].Returns(expected.BetCountOddEven);
            _reader["betcount_1x2"].Returns(expected.BetCountOneXTwo);
            _reader["betcount_dnd"].Returns(expected.BetCountDrawNoDraw);
            _reader["betcount_oe_1st"].Returns(expected.BetCountOddEven1StHalf);
            _reader["betcount_1x2_1st"].Returns(expected.BetCountOneXTow1StHalf);

            _reader["home_odds_OE"].Returns(10);
            _reader["away_odds_OE"].Returns(2);
            _reader["odds_1"].Returns(1);
            _reader["odds_x"].Returns(2);
            _reader["odds_2"].Returns(3);
            _reader["home_odds_DND"].Returns(10);
            _reader["away_odds_DND"].Returns(2);
            _reader["home_odds_OE_1st"].Returns(10);
            _reader["away_odds_OE_1st"].Returns(2);
            _reader["odds_1_1st"].Returns(1);
            _reader["odds_x_1st"].Returns(2);
            _reader["odds_2_1st"].Returns(3);
        }
    }
}