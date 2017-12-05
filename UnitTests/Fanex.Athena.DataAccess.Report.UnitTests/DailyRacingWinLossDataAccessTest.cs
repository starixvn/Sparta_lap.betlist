namespace Fanex.Athena.DataAccess.Report.UnitTests
{
    using System;
    using System.Data;
    using Data;
    using Model.Report.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using UnitTest.Common.Helpers;
    using UnitTest.Common.NUnitExtensions;

    [TestFixture]
    public class DailyRacingWinLossDataAccessTest
    {
        private IObjectDb _objectDb;
        private DailyRacingWinLossDataAccess _dataAccess;
        private IDataReader _reader;

        [SetUp]
        public void Setup()
        {
            _objectDb = Substitute.For<IObjectDb>();
            _dataAccess = Substitute.ForPartsOf<DailyRacingWinLossDataAccess>();
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_Rpt_WinlostRacing")).Returns(_objectDb);

            _reader = Substitute.For<IDataReader>();
            _objectDb.ExecuteReader(Arg.Any<object>()).ReturnsForAnyArgs(_reader);
        }

        [TearDown]
        public void TearDown()
        {
            _objectDb = null;
            _dataAccess = null;
            _reader = null;
        }

        [Test]
        public void GetDataList_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            dynamic uspParam = null;
            _objectDb.ExecuteReader(Arg.Do<dynamic>(arg => uspParam = arg));

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
            _dataAccess.GetDataList(model);

            // Assert
            Assert.That(uspParam, Tobe.SameProperties(expectedParam));
        }

        [Test]
        public void GetDataList_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            _reader.Read().Returns(true, false);
            var readerDataMapping = new
            {
                Currency = 111,
                LeagueId = 222
            };
            DataAccessTestHelper.MapReaderData(_reader, readerDataMapping);

            // Action
            var dataList = _dataAccess.GetDataList(new DailyRacingWinLossMetaModel());

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(dataList.Count, Is.EqualTo(1));
                Assert.That(dataList[0].Currency, Is.EqualTo(111));
                Assert.That(dataList[0].LeagueId, Is.EqualTo(222));
            });
        }
    }
}