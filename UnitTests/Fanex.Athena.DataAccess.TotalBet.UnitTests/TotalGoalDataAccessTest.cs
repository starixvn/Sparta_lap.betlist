namespace Fanex.Athena.DataAccess.TotalBet.UnitTests
{
    using System.Linq;
    using Core.UnitTests.TestHelpers;
    using Model.TotalBet.DTO;
    using Model.TotalBet.MetaModel;
    using NSubstitute;
    using NUnit.Framework;

    public class TotalGoalDataAccessTest : BaseDataAccessHelperTest<TotalGoalDataAccess>
    {
        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();

            DataAccess.GetObjectDb(Arg.Is("Lrf_TotalBets_Sel6")).Returns(ObjectDb);
        }

        [Test]
        public void GetTotalGoalDataList_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            var metaModel = new TotalGoalMetaModel();
            var expected = new TotalGoal
            {
                MatchId = 1,
                HomeId = 2,
                AwayId = 3,
                Total01 = 10m,
                Total23 = 11m,
                Total46 = 12m,
                Total7Over = 13m,
                TotalFirstHalf01 = 20m,
                TotalFirstHalf23 = 20.5m,
                Total4Over = 21m
            };
            var expectedTotal = new TotalGoal
            {
                Total01 = expected.Total01 + expected.Total23 + expected.Total46 + expected.Total7Over,
                TotalFirstHalf01 = expected.TotalFirstHalf01 + expected.TotalFirstHalf23 + expected.Total4Over
            };
            ConfigureReaderForGetTotalGoalDataListMethod(expected);
            Reader.Read().Returns(true, false, true, false);
            Reader.NextResult().Returns(true);

            // Action
            var result = DataAccess.GetTotalGoalDataList(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                AssertMainData(result.DataList.ElementAt(0), expected);
                AssertTotalData(result.TotalData, expectedTotal);
            });
        }

        [Test]
        public void GetTotalGoalDataList_NoDataReturned_ReturnEmptyData()
        {
            // Arrange
            var metaModel = new TotalGoalMetaModel();
            var expectedTotal = new TotalGoal();

            Reader.Read().Returns(false);
            Reader.NextResult().Returns(false);

            // Action
            var result = DataAccess.GetTotalGoalDataList(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.DataList, Is.Empty);
                AssertTotalData(result.TotalData, expectedTotal);
            });
        }

        private void AssertMainData(TotalGoal actual, TotalGoal expected)
        {
            Assert.That(actual.MatchId, Is.EqualTo(expected.MatchId));
            Assert.That(actual.HomeId, Is.EqualTo(expected.HomeId));
            Assert.That(actual.AwayId, Is.EqualTo(expected.AwayId));
            Assert.That(actual.IsNeutral, Is.EqualTo(expected.IsNeutral));

            Assert.That(actual.Total23, Is.EqualTo(expected.Total23));
            Assert.That(actual.Total46, Is.EqualTo(expected.Total46));
            Assert.That(actual.Total7Over, Is.EqualTo(expected.Total7Over));
            Assert.That(actual.TotalFirstHalf23, Is.EqualTo(expected.TotalFirstHalf23));
            Assert.That(actual.Total4Over, Is.EqualTo(expected.Total4Over));

            AssertTotalData(actual, expected);
        }

        private void AssertTotalData(TotalGoal actual, TotalGoal expected)
        {
            Assert.That(actual.Total01, Is.EqualTo(expected.Total01));
            Assert.That(actual.TotalFirstHalf01, Is.EqualTo(expected.TotalFirstHalf01));
        }

        private void ConfigureReaderForGetTotalGoalDataListMethod(TotalGoal expected)
        {
            MapReaderData(new
            {
                matchid = expected.MatchId,
                homeID = expected.HomeId,
                awayID = expected.AwayId,
                isNeutral = expected.IsNeutral,
                total01 = expected.Total01,
                total23 = expected.Total23,
                total46 = expected.Total46,
                total7over = expected.Total7Over,
                fhtotal01 = expected.TotalFirstHalf01,
                fhtotal23 = expected.TotalFirstHalf23,
                fhtotal4over = expected.Total4Over,
            });
        }
    }
}