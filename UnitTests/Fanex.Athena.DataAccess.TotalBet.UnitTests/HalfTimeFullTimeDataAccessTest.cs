namespace Fanex.Athena.DataAccess.TotalBet.UnitTests
{
    using System.Linq;
    using Core.UnitTests.TestHelpers;
    using Model.TotalBet.DTO;
    using Model.TotalBet.MetaModel;
    using NSubstitute;
    using NUnit.Framework;

    public class HalfTimeFullTimeDataAccessTest : BaseDataAccessHelperTest<HalfTimeFullTimeDataAccess>
    {
        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();

            DataAccess.GetObjectDb(Arg.Is("Lrf_TotalBets_Sel16")).Returns(ObjectDb);
        }

        [Test]
        public void GetHalfTimeFullTimeDataList_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            var metaModel = new HalfTimeFullTimeMetaModel();
            var expected = new HalfTimeFullTime
            {
                MatchId = 1,
                HomeId = 2,
                AwayId = 3,
                IsNeutral = 0,
                HomeHome = 10m,
                DrawHome = 11m,
                AwayHome = 12m
            };
            ConfigureReaderForGetHalfTimeFullTimeDataListMethod(expected);
            Reader.Read().Returns(true, false, true, false);
            Reader.NextResult().Returns(true);

            // Action
            var result = DataAccess.GetHalfTimeFullTimeDataList(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                AssertInfoData(result.DataList.ElementAt(0), expected);
                AssertTotalData(result.TotalData, expected);
            });
        }

        [Test]
        public void GetHalfTimeFullTimeDataList_NoDataReturned_ReturnEmptyData()
        {
            // Arrange
            var metaModel = new HalfTimeFullTimeMetaModel();
            var expected = new HalfTimeFullTime();

            Reader.Read().Returns(false);
            Reader.NextResult().Returns(false);

            // Action
            var result = DataAccess.GetHalfTimeFullTimeDataList(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.DataList, Is.Empty);
                AssertTotalData(result.TotalData, expected);
            });
        }

        private void AssertInfoData(HalfTimeFullTime actual, HalfTimeFullTime expected)
        {
            Assert.That(actual.MatchId, Is.EqualTo(expected.MatchId));
            Assert.That(actual.HomeId, Is.EqualTo(expected.HomeId));
            Assert.That(actual.AwayId, Is.EqualTo(expected.AwayId));
            Assert.That(actual.IsNeutral, Is.EqualTo(expected.IsNeutral));

            AssertTotalData(actual, expected);
        }

        private void AssertTotalData(HalfTimeFullTime actual, HalfTimeFullTime expected)
        {
            Assert.That(actual.HomeHome, Is.EqualTo(expected.HomeHome));
            Assert.That(actual.HomeDraw, Is.EqualTo(expected.HomeDraw));
            Assert.That(actual.HomeAway, Is.EqualTo(expected.HomeAway));
            Assert.That(actual.DrawHome, Is.EqualTo(expected.DrawHome));
            Assert.That(actual.DrawDraw, Is.EqualTo(expected.DrawDraw));
            Assert.That(actual.DrawAway, Is.EqualTo(expected.DrawAway));
            Assert.That(actual.AwayHome, Is.EqualTo(expected.AwayHome));
            Assert.That(actual.AwayDraw, Is.EqualTo(expected.AwayDraw));
            Assert.That(actual.AwayAway, Is.EqualTo(expected.AwayAway));
        }

        private void ConfigureReaderForGetHalfTimeFullTimeDataListMethod(HalfTimeFullTime expected)
        {
            MapReaderData(new
            {
                matchid = expected.MatchId,
                isNeutral = expected.IsNeutral,
                homeID = expected.HomeId,
                awayID = expected.AwayId,

                totalHH = expected.HomeHome,
                totalHD = expected.HomeDraw,
                totalHA = expected.HomeAway,
                totalDH = expected.DrawHome,
                totalDD = expected.DrawDraw,
                totalDA = expected.DrawAway,
                totalAH = expected.AwayHome,
                totalAD = expected.AwayDraw,
                totalAA = expected.AwayAway,
            });
        }
    }
}