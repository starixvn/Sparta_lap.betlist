namespace Fanex.Athena.DataAccess.Report.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Web.Script.Serialization;
    using Data;
    using Model.Report.MetaModel;
    using NSubstitute;
    using NUnit.Framework;

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
            dynamic result = null;
            _objectDb.ExecuteReader(Arg.Do<dynamic>(arg => result = arg));

            // Action
            _dataAccess.GetDataList(new DailyRacingWinLossMetaModel
            {
                AgentListXml = "1",
                Date = new DateTime(2017, 1, 1)
            });

            // Assert
            Assert.That(new JavaScriptSerializer().Serialize(result), Is.EqualTo(@"{""AgentList"":""1"",""WinlostDate"":""\/Date(1483203600000)\/""}"));
        }

        [Test]
        public void GetDataList_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            _reader.Read().Returns(true, false);
            var readerDataMapping = new Dictionary<string, object>
            {
                { "Currency", 111 },
                { "LeagueId", 222 }
            };

            _reader[Arg.Any<string>()].Returns(x =>
            {
                var paramName = x.ArgAt<string>(0);
                if (readerDataMapping.ContainsKey(paramName))
                {
                    return readerDataMapping[paramName];
                }

                return string.Empty;
            });

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