namespace Fanex.Athena.DataAccess.TotalBet.UnitTests
{
    using System.Linq;
    using Core.UnitTests.TestHelpers;
    using Model.TotalBet.DTO;
    using Model.TotalBet.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using TotalBet;

    public class FirstGoalLastGoalDataAccessTest : BaseDataAccessHelperTest<FirstGoalLastGoalDataAccess>
    {
        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();

            DataAccess.GetObjectDb(Arg.Is("Lrf_TotalBets_Sel14")).Returns(ObjectDb);
        }

        [Test]
        public void GetFirstGoalLastGoalDataList_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            var metaModel = new FirstGoalLastGoalMetaModel();
            var expected = new FirstGoalLastGoal
            {
                LeagueId = 1,
                MatchId = 2,
                ShowTime = "ShowTimeTest",
                IsNeutral = 0,
                HomeId = 3,
                AwayId = 4,
                HomeFirstGoal = 10m,
                HomeLastGoal = 10.5m,
                HomeFirstHalfFirstGoal = 11m,
                HomeFirstHalfLastGoal = 11.5m,
                AwayFirstHalfFirstGoal = 12m,
                AwayFirstHalfLastGoal = 12.5m,
                FirstHalfNoGoal = 13m
            };

            ConfigureReaderForGetFirstGoalLastGoalDataListMethod(expected);
            Reader.Read().Returns(true, false, true, false);
            Reader.NextResult().Returns(true);

            // Action
            var result = DataAccess.GetFirstGoalLastGoalDataList(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                AssertMainData(result.DataList.ElementAt(0), expected);
                AssertTotalData(result.TotalData, expected);
            });
        }

        [Test]
        public void GetFirstGoalLastGoalDataList_NoDataReturned_ReturnEmptyData()
        {
            // Arrange
            var metaModel = new FirstGoalLastGoalMetaModel();
            var expected = new FirstGoalLastGoal();

            Reader.Read().Returns(false);
            Reader.NextResult().Returns(false);

            // Action
            var result = DataAccess.GetFirstGoalLastGoalDataList(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.DataList, Is.Empty);
                AssertTotalData(result.TotalData, expected);
            });
        }

        private void AssertMainData(FirstGoalLastGoal actual, FirstGoalLastGoal expected)
        {
            Assert.That(actual.LeagueId, Is.EqualTo(expected.LeagueId));
            Assert.That(actual.MatchId, Is.EqualTo(expected.MatchId));
            Assert.That(actual.ShowTime, Is.EqualTo(expected.ShowTime));
            Assert.That(actual.IsNeutral, Is.EqualTo(expected.IsNeutral));
            Assert.That(actual.HomeId, Is.EqualTo(expected.HomeId));
            Assert.That(actual.AwayId, Is.EqualTo(expected.AwayId));

            Assert.That(actual.HomeFirstGoal, Is.EqualTo(expected.HomeFirstGoal));
            Assert.That(actual.HomeLastGoal, Is.EqualTo(expected.HomeLastGoal));
            Assert.That(actual.AwayFirstGoal, Is.EqualTo(expected.AwayFirstGoal));
            Assert.That(actual.AwayLastGoal, Is.EqualTo(expected.AwayLastGoal));
            Assert.That(actual.NoGoal, Is.EqualTo(expected.NoGoal));
            Assert.That(actual.HomeFirstHalfFirstGoal, Is.EqualTo(expected.HomeFirstHalfFirstGoal));
            Assert.That(actual.HomeFirstHalfLastGoal, Is.EqualTo(expected.HomeFirstHalfLastGoal));
            Assert.That(actual.AwayFirstHalfFirstGoal, Is.EqualTo(expected.AwayFirstHalfFirstGoal));
            Assert.That(actual.AwayFirstHalfLastGoal, Is.EqualTo(expected.AwayFirstHalfLastGoal));
            Assert.That(actual.FirstHalfNoGoal, Is.EqualTo(expected.FirstHalfNoGoal));
        }

        private void AssertTotalData(FirstGoalLastGoal actual, FirstGoalLastGoal expected)
        {
            var totalHomeFirstGoal = expected.HomeFirstGoal + expected.HomeLastGoal + expected.AwayFirstGoal
                + expected.AwayLastGoal + expected.NoGoal;
            var totalHomeFirstHalfFirstGoal = expected.HomeFirstHalfFirstGoal + expected.HomeFirstHalfLastGoal
                + expected.AwayFirstHalfFirstGoal + expected.AwayFirstHalfLastGoal + expected.FirstHalfNoGoal;

            Assert.That(actual.HomeFirstGoal, Is.EqualTo(totalHomeFirstGoal));
            Assert.That(actual.HomeFirstHalfFirstGoal, Is.EqualTo(totalHomeFirstHalfFirstGoal));
        }

        private void ConfigureReaderForGetFirstGoalLastGoalDataListMethod(FirstGoalLastGoal expected)
        {
            MapReaderData(new
            {
                leagueid = expected.LeagueId,
                matchid = expected.MatchId,
                showtime = expected.ShowTime,
                isNeutral = expected.IsNeutral,
                homeid = expected.HomeId,
                awayid = expected.AwayId,

                winlost_homeFG = expected.HomeFirstGoal,
                winlost_homeLG = expected.HomeLastGoal,
                winlost_awayFG = expected.AwayFirstGoal,
                winlost_awayLG = expected.AwayLastGoal,
                winlost_NoGoal = expected.NoGoal,
                winlost_1HhomeFG = expected.HomeFirstHalfFirstGoal,
                winlost_1HhomeLG = expected.HomeFirstHalfLastGoal,
                winlost_1HawayFG = expected.AwayFirstHalfFirstGoal,
                winlost_1HawayLG = expected.AwayFirstHalfLastGoal,
                winlost_1HNoGoal = expected.FirstHalfNoGoal,
            });
        }
    }
}