namespace Fanex.Athena.DataAccess.TotalBet.UnitTests
{
    using System.Linq;
    using Core.UnitTests.TestHelpers;
    using Model.TotalBet.DTO;
    using Model.TotalBet.MetaModel;
    using NSubstitute;
    using NUnit.Framework;

    public class HandicapOverUnderDataAccessTest : BaseDataAccessHelperTest<HandicapOverUnderDataAccess>
    {
        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();

            DataAccess.GetObjectDb(Arg.Is("Lrf_TotalBets_Sel1378")).Returns(ObjectDb);
        }

        [Test]
        public void GetHandicapOverUnderDataList_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            var metaModel = new HandicapOverUnderMetaModel();
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
            var expectedTotal = new HandicapOverUnder
            {
                FullTimeHandicap = 12m,
                FullTimeOverUnder = 12m,
                FirstTimeHandicap = 12m,
                FirstTimeOverUnder = 12m
            };
            ConfigureReaderForGetHandicapOverUnderDataListMethod(expected);
            Reader.Read().Returns(true, false, true, false);
            Reader.NextResult().Returns(true);

            // Action
            var result = DataAccess.GetHandicapOverUnderDataList(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                AssertMainData(result.DataList.ElementAt(0), expected);
                AssertTotalData(result.TotalData, expectedTotal);
            });
        }

        [Test]
        public void GetHandicapOverUnderDataList_NoDataReturned_ReturnEmptyData()
        {
            // Arrange
            var metaModel = new HandicapOverUnderMetaModel();
            var expectedTotal = new HandicapOverUnder();

            Reader.Read().Returns(false);
            Reader.NextResult().Returns(false);

            // Action
            var result = DataAccess.GetHandicapOverUnderDataList(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.DataList, Is.Empty);
                AssertTotalData(result.TotalData, expectedTotal);
            });
        }

        private static void AssertMainData(HandicapOverUnder actual, HandicapOverUnder expected)
        {
            Assert.That(actual.ShowTime, Is.EqualTo(expected.ShowTime));
            Assert.That(actual.LeagueId, Is.EqualTo(expected.LeagueId));
            Assert.That(actual.MatchId, Is.EqualTo(expected.MatchId));
            Assert.That(actual.HomeId, Is.EqualTo(expected.HomeId));
            Assert.That(actual.AwayId, Is.EqualTo(expected.AwayId));
            Assert.That(actual.LiveHomeScore, Is.EqualTo(expected.LiveHomeScore));
            Assert.That(actual.LiveAwayScore, Is.EqualTo(expected.LiveAwayScore));
            Assert.That(actual.LiveIndicator, Is.EqualTo(expected.LiveIndicator));

            Assert.That(actual.BetCountHandicap, Is.EqualTo(expected.BetCountHandicap));
            Assert.That(actual.BetCountOverUnder, Is.EqualTo(expected.BetCountOverUnder));
            Assert.That(actual.BetCountHandicap1StHalf, Is.EqualTo(expected.BetCountHandicap1StHalf));
            Assert.That(actual.BetCountOverUnder1StHalf, Is.EqualTo(expected.BetCountOverUnder1StHalf));

            Assert.That(actual.FullTimeHandicap, Is.EqualTo(expected.FullTimeHandicap));
            Assert.That(actual.FullTimeOverUnder, Is.EqualTo(expected.FullTimeOverUnder));
            Assert.That(actual.FirstTimeHandicap, Is.EqualTo(expected.FirstTimeHandicap));
            Assert.That(actual.FirstTimeOverUnder, Is.EqualTo(expected.FirstTimeOverUnder));
        }

        private static void AssertTotalData(HandicapOverUnder actual, HandicapOverUnder expected)
        {
            Assert.That(actual.FullTimeHandicap, Is.EqualTo(expected.FullTimeHandicap));
            Assert.That(actual.FullTimeOverUnder, Is.EqualTo(expected.FullTimeOverUnder));
            Assert.That(actual.FirstTimeHandicap, Is.EqualTo(expected.FirstTimeHandicap));
            Assert.That(actual.FirstTimeOverUnder, Is.EqualTo(expected.FirstTimeOverUnder));
        }

        private void ConfigureReaderForGetHandicapOverUnderDataListMethod(HandicapOverUnder expected)
        {
            MapReaderData(new
            {
                showtime = expected.ShowTime,
                leagueid = expected.LeagueId,
                matchid = expected.MatchId,
                homeID = expected.HomeId,
                awayID = expected.AwayId,
                livehomescore = expected.LiveHomeScore,
                liveawayscore = expected.LiveAwayScore,
                liveindicator = expected.LiveIndicator,

                betcount_hdp = expected.BetCountHandicap,
                betcount_ou = expected.BetCountOverUnder,
                BetCount_HTHDP = expected.BetCountHandicap1StHalf,
                BetCount_HTOU = expected.BetCountOverUnder1StHalf,

                home_odds_HDP = 10,
                away_odds_HDP = 2,
                home_odds_OU = 10,
                away_odds_OU = 2,

                home_odds_HTHDP = 10,
                away_odds_HTHDP = 2,
                home_odds_HTOU = 10,
                away_odds_HTOU = 2
            });
        }
    }
}