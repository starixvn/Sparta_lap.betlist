namespace Fanex.Athena.DataAccess.TotalBet.UnitTests
{
    using System.Linq;
    using Core.UnitTests.TestHelpers;
    using Model.TotalBet.DTO;
    using Model.TotalBet.MetaModel;
    using NSubstitute;
    using NUnit.Framework;

    public class OddEvenDrawNoDrawDataAccessTest : BaseDataAccessHelperTest<OddEvenDrawNoDrawDataAccess>
    {
        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();

            DataAccess.GetObjectDb(Arg.Is("Lrf_TotalBets_Sel25")).Returns(ObjectDb);
        }

        [Test]
        public void GetOddEventDrawNoDrawDataList_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            var metaModel = new OddEvenDrawNoDrawMetaModel();
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
            var expectedTotal = new OddEvenDrawNoDraw
            {
                OddEvenFt = 12m,
                OneXTwoFt = 6m,
                DrawNoDraw = 12m,
                OddEvenFirstHalf = 12m,
                OneXTwoFirstHalf = 6m,
            };
            ConfigureReaderForGetOddEventDrawNoDrawDataListMethod(expected);
            Reader.Read().Returns(true, false, true, false);
            Reader.NextResult().Returns(true);

            // Action
            var result = DataAccess.GetOddEvenDrawNoDrawDataList(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                AssertMainData(result.DataList.ElementAt(0), expected);
                AssertScoreData(result.TotalData, expectedTotal);
            });
        }

        [Test]
        public void GetOddEventDrawNoDrawDataList_NoDataReturned_ReturnEmptyData()
        {
            // Arrange
            var metaModel = new OddEvenDrawNoDrawMetaModel();
            var expectedTotal = new OddEvenDrawNoDraw();

            Reader.Read().Returns(false);
            Reader.NextResult().Returns(false);

            // Action
            var result = DataAccess.GetOddEvenDrawNoDrawDataList(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.DataList, Is.Empty);
                AssertScoreData(result.TotalData, expectedTotal);
            });
        }

        private static void AssertMainData(OddEvenDrawNoDraw actual, OddEvenDrawNoDraw expected)
        {
            Assert.That(actual.ShowTime, Is.EqualTo(expected.ShowTime));
            Assert.That(actual.LeagueId, Is.EqualTo(expected.LeagueId));
            Assert.That(actual.MatchId, Is.EqualTo(expected.MatchId));
            Assert.That(actual.HomeId, Is.EqualTo(expected.HomeId));
            Assert.That(actual.AwayId, Is.EqualTo(expected.AwayId));
            Assert.That(actual.Bettype, Is.EqualTo(expected.Bettype));
            Assert.That(actual.LiveHomeScore, Is.EqualTo(expected.LiveHomeScore));
            Assert.That(actual.LiveAwayScore, Is.EqualTo(expected.LiveAwayScore));
            Assert.That(actual.LiveIndicator, Is.EqualTo(expected.LiveIndicator));
            Assert.That(actual.BetCountOddEven, Is.EqualTo(expected.BetCountOddEven));
            Assert.That(actual.BetCountOneXTwo, Is.EqualTo(expected.BetCountOneXTwo));
            Assert.That(actual.BetCountDrawNoDraw, Is.EqualTo(expected.BetCountDrawNoDraw));
            Assert.That(actual.BetCountOddEven1StHalf, Is.EqualTo(expected.BetCountOddEven1StHalf));
            Assert.That(actual.BetCountOneXTow1StHalf, Is.EqualTo(expected.BetCountOneXTow1StHalf));

            AssertScoreData(actual, expected);
        }

        private static void AssertScoreData(OddEvenDrawNoDraw actual, OddEvenDrawNoDraw expected)
        {
            Assert.That(actual.OddEvenFt, Is.EqualTo(expected.OddEvenFt));
            Assert.That(actual.OneXTwoFt, Is.EqualTo(expected.OneXTwoFt));
            Assert.That(actual.DrawNoDraw, Is.EqualTo(expected.DrawNoDraw));
            Assert.That(actual.OddEvenFirstHalf, Is.EqualTo(expected.OddEvenFirstHalf));
            Assert.That(actual.OneXTwoFirstHalf, Is.EqualTo(expected.OneXTwoFirstHalf));
        }

        private void ConfigureReaderForGetOddEventDrawNoDrawDataListMethod(OddEvenDrawNoDraw expected)
        {
            MapReaderData(new
            {
                showtime = expected.ShowTime,
                leagueid = expected.LeagueId,
                matchid = expected.MatchId,
                homeID = expected.HomeId,
                awayID = expected.AwayId,
                bettype = expected.Bettype,
                livehomescore = expected.LiveHomeScore,
                liveawayscore = expected.LiveAwayScore,
                liveindicator = expected.LiveIndicator,

                betcount_oe = expected.BetCountOddEven,
                betcount_1x2 = expected.BetCountOneXTwo,
                betcount_dnd = expected.BetCountDrawNoDraw,
                betcount_oe_1st = expected.BetCountOddEven1StHalf,
                betcount_1x2_1st = expected.BetCountOneXTow1StHalf,

                home_odds_OE = 10,
                away_odds_OE = 2,
                odds_1 = 1,
                odds_x = 2,
                odds_2 = 3,
                home_odds_DND = 10,
                away_odds_DND = 2,
                home_odds_OE_1st = 10,
                away_odds_OE_1st = 2,
                odds_1_1st = 1,
                odds_x_1st = 2,
                odds_2_1st = 3,
            });
        }
    }
}