namespace Fanex.Athena.DataAccess.TotalBet.UnitTests
{
    using System.Threading.Tasks;
    using Core.UnitTests.TestHelpers;
    using Model.TotalBet.DTO;
    using Model.TotalBet.MetaModel;
    using NSubstitute;
    using NUnit.Framework;

    public class HomeDrawAwayNoBetDataAccessTest : BaseDataAccessHelperTest<HomeDrawAwayNoBetDataAccess>
    {
        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();

            DataAccess.GetObjectDb(Arg.Is("Lrf_TotalBets_Sel121")).Returns(ObjectDb);
        }

        [Test]
        public async Task GetHomeDrawAwayNoBetDataList_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
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
            var result = (await DataAccess.GetHomeDrawAwayNoBetDataList(metaModel))[0];

            // Assert
            Assert.Multiple(() =>
            {
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
            });
        }

        private void ConfigureReaderForGetHomeDrawAwayNoBetDataList(HomeDrawAwayNoBet expected)
        {
            Reader.Read().Returns(true, false);

            MapReaderData(new
            {
                liveindicator = expected.LiveIndicator,
                livehomescore = expected.LiveHomeScore,
                liveawayscore = expected.LiveAwayScore,
                showtime = expected.ShowTime,
                leagueid = expected.LeagueId,
                matchid = expected.MatchId,
                homeid = expected.HomeId,
                awayid = expected.AwayId,
                isNeutral = expected.IsNeutral,
                betcount_hnb = expected.BetCountHomeNoBet,
                betcount_dnb = expected.BetCountDrawNoBet,
                betcount_anb = expected.BetCountAwayNoBet,
                home_odds_hnb = expected.HomeOddsHomeNoBet,
                away_odds_hnb = expected.AwayOddsHomeNoBet,
                home_odds_dnb = expected.HomeOddsDrawNoBet,
                away_odds_dnb = expected.AwayOddsDrawNoBet,
                home_odds_anb = expected.HomeOddsAwayNoBet,
                away_odds_anb = expected.AwayOddsAwayNoBet
            });
        }
    }
}