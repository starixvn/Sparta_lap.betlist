namespace Fanex.Athena.DataAccess.Report.UnitTests
{
    using System;
    using System.Data;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.UnitTests.TestHelpers;
    using Model.Report.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using VendorAdapter.Interfaces;

    [TestFixture]
    public class WeeklyLiveMatchDataAccessTest : BaseDataAccessHelperTest<WeeklyLiveMatchDataAccess>
    {
        protected override void SetUpDataAccess()
        {
            DataAccess = Substitute.ForPartsOf<WeeklyLiveMatchDataAccess>(
                Substitute.For<ICacheRefAdapter>());
        }

        [Test]
        public async Task GetDataList_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            var metaModel = new WeeklyLiveMatchMetaModel
            {
                LicenseeId = 1,
                UserId = 2,
                FromDate = new DateTime(2017, 1, 1),
                ToDate = new DateTime(2017, 2, 2)
            };
            DataAccess.GetObjectDb(Arg.Is("Lrf_Rpt_LiveMatchWeekly")).Returns(ObjectDb);

            // Action
            await DataAccess.GetDataList(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received(1).SetParameterValue("FromDate", metaModel.FromDate);
                ObjectDb.Received(1).SetParameterValue("ToDate", metaModel.ToDate);
            });
        }

        [TestCase("Week0", 100, 100)]
        [TestCase("", 100, 0)]
        public async Task GetDataList_WhenCall_ReturnCorrectResult(string sortColumn, int numbersOfMatchs, int expectedNumberOfMatches)
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is("Lrf_Rpt_LiveMatchWeekly")).Returns(ObjectDb);

            Reader.Read().Returns(true, false);
            Reader.FieldCount.Returns(3);
            Reader.GetInt32(Arg.Any<int>()).Returns(numbersOfMatchs);

            var readerDataMapping = new
            {
                sporttype = 1
            };
            MapReaderData(readerDataMapping);

            // Action
            var result = await DataAccess.GetDataList(
                new WeeklyLiveMatchMetaModel
                {
                    SortDirection = "asc",
                    SortColumn = sortColumn,
                    BaseCurrency = "USD"
                });

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.DataList.Count, Is.EqualTo(1));
                Assert.That(result.DataList.ElementAt(0).NumbersOfMatches, Is.EqualTo(new[] { numbersOfMatchs }));
                Assert.That(result.DataList.ElementAt(0).SortingWeekValue, Is.EqualTo(expectedNumberOfMatches));
            });
        }

        [Test]
        public async Task GetDataList_SortWeek0_ReturnCorrectResult()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is("Lrf_Rpt_LiveMatchWeekly")).Returns(ObjectDb);

            const int NumberOfMatches1 = 1;
            const int NumberOfMatches2 = 2;
            Reader.Read().Returns(true, true, false);
            Reader.FieldCount.Returns(3);
            Reader.GetInt32(Arg.Any<int>()).Returns(NumberOfMatches2, NumberOfMatches1);

            var readerDataMapping = new
            {
                sporttype = 1
            };
            MapReaderData(readerDataMapping);

            // Action
            var result = await DataAccess.GetDataList(
                new WeeklyLiveMatchMetaModel
                {
                    SortDirection = "asc",
                    SortColumn = "Week0",
                    BaseCurrency = "USD"
                });

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.DataList.Count, Is.EqualTo(2));
                Assert.That(result.DataList.ElementAt(0).NumbersOfMatches, Is.EqualTo(new[] { NumberOfMatches1 }));
                Assert.That(result.DataList.ElementAt(0).SortingWeekValue, Is.EqualTo(NumberOfMatches1));
            });
        }

        [TestCase("Week1", "asc")]
        [TestCase("Week0", "")]
        [TestCase("Week0", null)]
        [TestCase("", "asc")]
        [TestCase(null, "asc")]
        public async Task GetDataList_InvalidSortIndex_ReturnCorrectResult(string sortColumn, string sortDirection)
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is("Lrf_Rpt_LiveMatchWeekly")).Returns(ObjectDb);

            const int NumberOfMatches1 = 1;
            const int NumberOfMatches2 = 2;
            Reader.Read().Returns(true, true, false);
            Reader.FieldCount.Returns(3);
            Reader.GetInt32(Arg.Any<int>()).Returns(NumberOfMatches2, NumberOfMatches1);

            var readerDataMapping = new
            {
                sporttype = 1
            };
            MapReaderData(readerDataMapping);

            // Action
            var result = await DataAccess.GetDataList(
                new WeeklyLiveMatchMetaModel
                {
                    SortDirection = sortDirection,
                    SortColumn = sortColumn,
                    BaseCurrency = "USD"
                });

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.DataList.Count, Is.EqualTo(2));
                Assert.That(result.DataList.ElementAt(0).NumbersOfMatches, Is.EqualTo(new[] { NumberOfMatches2 }));
                Assert.That(result.DataList.ElementAt(0).Index, Is.EqualTo(1));
            });
        }

        [TestCase(0, 0)]
        [TestCase(1, 0)]
        [TestCase(2, 0)]
        [TestCase(3, 1)]
        [TestCase(4, 2)]
        [TestCase(2, 0)]
        public async Task GetDataList_WhenCall_ReturnCorrectNumberOfWeeks(int fieldCount, int expectedNumberOfWeeks)
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is("Lrf_Rpt_LiveMatchWeekly")).Returns(ObjectDb);

            Reader.Read().Returns(false);
            Reader.FieldCount.Returns(fieldCount);

            // Action
            var result = await DataAccess.GetDataList(
                new WeeklyLiveMatchMetaModel
                {
                    SortDirection = "asc",
                    BaseCurrency = "USD"
                });

            // Assert
            Assert.That(result.NumberOfWeeks, Is.EqualTo(expectedNumberOfWeeks));
        }
    }
}