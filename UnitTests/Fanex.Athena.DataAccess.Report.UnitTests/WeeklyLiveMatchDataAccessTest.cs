namespace Fanex.Athena.DataAccess.Report.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Data;
    using Model.Report.MetaModel;
    using NSubstitute;
    using NUnit.Framework;

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
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_Rpt_LiveMatchWeekly")).Returns(objectDb);
            int[] totalData;

            // Action
            _dataAccess.GetDataList(metaModel, out totalData);

            // Assert
            Assert.Multiple(() =>
            {
                objectDb.Received().AddInParameter("LicenseeId", DbType.Int32, metaModel.LicenseeId);
                objectDb.Received().AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
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
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_Rpt_LiveMatchWeekly")).Returns(objectDb);
            objectDb.ExecuteReader().ReturnsForAnyArgs(reader);
            reader.Read().Returns(true, false);
            reader.FieldCount.Returns(3);
            reader.GetInt32(Arg.Any<int>()).Returns(100);
            var readerDataMapping = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase)
            {
                { "sporttype", 1 }
            };

            MapReaderData(reader, readerDataMapping);
            int[] totalData;

            // Action
            var result = _dataAccess.GetDataList(
                new WeeklyLiveMatchMetaModel
                {
                    BaseCurrency = "USD"
                }, 
                out totalData);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Count(), Is.EqualTo(1));
                Assert.That(result.ElementAt(0).NumbersOfMatches, Is.EqualTo(new int[] { 100 }));
            });
        }

        private static void MapReaderData(IDataReader reader, Dictionary<string, object> readerDataMapping)
        {
            reader[Arg.Any<string>()].Returns(x =>
            {
                var paramName = x.ArgAt<string>(0);
                if (readerDataMapping.ContainsKey(paramName))
                {
                    return readerDataMapping[paramName];
                }

                return string.Empty;
            });
        }
    }
}