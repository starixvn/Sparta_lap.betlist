namespace Fanex.Athena.DataAccess.TotalBet.UnitTests
{
    using System.Linq;
    using System.Threading.Tasks;
    using Core.UnitTests.TestHelpers;
    using Model.TotalBet.DTO;
    using Model.TotalBet.MetaModel;
    using NSubstitute;
    using NUnit.Framework;

    public class MoneyLineDataAccessTest : BaseDataAccessHelperTest<MoneyLineDataAccess>
    {
        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();

            DataAccess.GetObjectDb(Arg.Is("Lrf_TotalBets_Sel2021")).Returns(ObjectDb);
        }

        [Test]
        public async Task GetMoneyLineDataList_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            var metaModel = new MoneyLineMetaModel();
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
            Reader.Read().Returns(true, false, true, false);
            Reader.NextResult().Returns(true);

            // Action
            var result = await DataAccess.GetMoneyLineDataList(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                AssertMainData(result.DataList.ElementAt(0), expected);
                AssertTotalData(result.TotalData, expected);
            });
        }

        [Test]
        public async Task GetMoneyLineDataList_NoDataReturned_ReturnEmptyData()
        {
            // Arrange
            var metaModel = new MoneyLineMetaModel();
            var expected = new MoneyLine();

            Reader.Read().Returns(false);
            Reader.NextResult().Returns(false);

            // Action
            var result = await DataAccess.GetMoneyLineDataList(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.DataList, Is.Empty);
                AssertTotalData(result.TotalData, expected);
            });
        }

        private static void AssertMainData(MoneyLine actual, MoneyLine expected)
        {
            Assert.That(actual.ShowTime, Is.EqualTo(expected.ShowTime));
            Assert.That(actual.LeagueId, Is.EqualTo(expected.LeagueId));
            Assert.That(actual.MatchId, Is.EqualTo(expected.MatchId));
            Assert.That(actual.HomeId, Is.EqualTo(expected.HomeId));
            Assert.That(actual.AwayId, Is.EqualTo(expected.AwayId));

            Assert.That(actual.ForecastHomeFt, Is.EqualTo(expected.ForecastHomeFt));
            Assert.That(actual.ForecastAwayFt, Is.EqualTo(expected.ForecastAwayFt));

            Assert.That(actual.ForecastHomeFirstHalf, Is.EqualTo(expected.ForecastHomeFirstHalf));
            Assert.That(actual.ForecastAwayFirstHalf, Is.EqualTo(expected.ForecastAwayFirstHalf));

            Assert.That(actual.LiveHomeScore, Is.EqualTo(expected.LiveHomeScore));
            Assert.That(actual.LiveAwayScore, Is.EqualTo(expected.LiveAwayScore));
            Assert.That(actual.LiveIndicator, Is.EqualTo(expected.LiveIndicator));

            AssertTotalData(actual, expected);
        }

        private static void AssertTotalData(MoneyLine actual, MoneyLine expected)
        {
            Assert.That(actual.TotalHomeFt, Is.EqualTo(expected.TotalHomeFt));
            Assert.That(actual.TotalAwayFt, Is.EqualTo(expected.TotalAwayFt));

            Assert.That(actual.TotalHomeFirstHalf, Is.EqualTo(expected.TotalHomeFirstHalf));
            Assert.That(actual.TotalAwayFirstHalf, Is.EqualTo(expected.TotalAwayFirstHalf));
        }

        private void ConfigureReaderForGetHomeDrawAwayNoBetDataListMethod(MoneyLine expected)
        {
            MapReaderData(new
            {
                showtime = expected.ShowTime,
                leagueid = expected.LeagueId,
                matchid = expected.MatchId,
                homeID = expected.HomeId,
                awayID = expected.AwayId,

                home_odds = expected.TotalHomeFt,
                away_odds = expected.TotalAwayFt,
                winlost_home = expected.ForecastHomeFt,
                winlost_away = expected.ForecastAwayFt,

                home_odds_HT = expected.TotalHomeFirstHalf,
                away_odds_HT = expected.TotalAwayFirstHalf,
                winlost_home_HT = expected.ForecastHomeFirstHalf,
                winlost_away_HT = expected.ForecastAwayFirstHalf,

                livehomescore = expected.LiveHomeScore,
                liveawayscore = expected.LiveAwayScore,
                liveindicator = expected.LiveIndicator
            });
        }
    }
}