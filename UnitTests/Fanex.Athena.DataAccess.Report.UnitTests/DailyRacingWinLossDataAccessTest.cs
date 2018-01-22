namespace Fanex.Athena.DataAccess.Report.UnitTests
{
    using System;
    using Core.UnitTests.TestHelpers;
    using Model.Report.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using UnitTest.Common.Helpers;
    using UnitTest.Common.NUnitExtensions;

    [TestFixture]
    public class DailyRacingWinLossDataAccessTest : BaseDataAccessHelperTest<DailyRacingWinLossDataAccess>
    {
        private const string SpName = "Lrf_Rpt_WinlostRacing";

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();

            DataAccess.GetObjectDb(Arg.Is(SpName)).Returns(ObjectDb);
        }

        [Test]
        public void GetDataList_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            dynamic uspParam = null;
            ObjectDb.ExecuteReader(Arg.Do<dynamic>(arg => uspParam = arg));

            var model = new DailyRacingWinLossMetaModel
            {
                AgentListXml = "1",
                Date = new DateTime(2017, 1, 1)
            };

            var expectedParam = new
            {
                AgentList = model.AgentListXml,
                WinlostDate = model.Date
            };

            // Action
            DataAccess.GetDataList(model);

            // Assert
            Assert.That(uspParam, Tobe.SameProperties(expectedParam));
        }

        [Test]
        public void GetDataList_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            Reader.Read().Returns(true, false);
            var readerDataMapping = new
            {
                Currency = 111,
                LeagueId = 222
            };
            DataAccessTestHelper.MapReaderData(Reader, readerDataMapping);

            // Action
            var dataList = DataAccess.GetDataList(new DailyRacingWinLossMetaModel());

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(dataList.Count, Is.EqualTo(1));
                Assert.That(dataList[0].Currency, Is.EqualTo(111));
                Assert.That(dataList[0].LeagueId, Is.EqualTo(222));
            });
        }

        [Test]
        public void GetDataList_WhenObjectDbThrowsException_ThrowDataAccessException()
        {
            TestException_WhenObjectDbThrowsException_ThrowDataAccessException(
                SpName,
                () => DataAccess.GetDataList(new DailyRacingWinLossMetaModel()));
        }
    }
}