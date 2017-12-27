namespace Fanex.Athena.DataAccess.Report.UnitTests
{
    using System;
    using System.Data;
    using System.Linq;
    using Data;
    using Model.Report.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using UnitTest.Common.Helpers;

    [TestFixture]
    public class WeeklyLiveMatchDataAccessTest
    {
        private WeeklyLiveMatchDataAccess _dataAccess;

        [OneTimeSetUp]
        public void Setup()
        {
            _dataAccess = Substitute.ForPartsOf<WeeklyLiveMatchDataAccess>();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _dataAccess = null;
        }

        [Test]
        public void GetDataList_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            var metaModel = new WeeklyLiveMatchMetaModel
            {
                LicenseeId = 1,
                UserId = 2,
                FromDate = new DateTime(2017, 1, 1),
                ToDate = new DateTime(2017, 2, 2)
            };
            var objectDb = Substitute.For<IObjectDb>();
            _dataAccess.GetObjectDb(Arg.Is("Lrf_Rpt_LiveMatchWeekly")).Returns(objectDb);

            // Action
            _dataAccess.GetDataList(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                objectDb.Received().AddInParameter("LicenseeId", DbType.Int32, metaModel.LicenseeId);
                objectDb.Received().AddInParameter("FromDate", DbType.DateTime, metaModel.FromDate);
                objectDb.Received().AddInParameter("ToDate", DbType.DateTime, metaModel.ToDate);
                objectDb.Received().AddInParameter("UserId", DbType.Int32, metaModel.UserId);
            });
        }

        [Test]
        public void GetDataList_WhenCall_ReturnCorrectResult()
        {
            // Arrange
            var reader = Substitute.For<IDataReader>();
            var objectDb = Substitute.For<IObjectDb>();
            _dataAccess.GetObjectDb(Arg.Is("Lrf_Rpt_LiveMatchWeekly")).Returns(objectDb);
            objectDb.ExecuteReader().ReturnsForAnyArgs(reader);
            reader.Read().Returns(true, false);
            reader.FieldCount.Returns(3);
            reader.GetInt32(Arg.Any<int>()).Returns(100);
            var readerDataMapping = new
            {
                sporttype = 1
            };

            DataAccessTestHelper.MapReaderData(reader, readerDataMapping);

            // Action
            var result = _dataAccess.GetDataList(
                new WeeklyLiveMatchMetaModel
                {
                    SortDirection = "asc",
                    SortColumn = "Week0",
                    BaseCurrency = "USD"
                });

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.DataList.Count(), Is.EqualTo(1));
                Assert.That(result.DataList.ElementAt(0).NumbersOfMatches, Is.EqualTo(new int[] { 100 }));
            });
        }
    }
}