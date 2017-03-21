namespace Fanex.Athena.DataAccess.Report.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Web.Script.Serialization;
    using Data;
    using Model.Core.DTO.Configuration;
    using Model.Report.MetaModel;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class WinLossByBetTypeDataAccessTest
    {
        private WinLossByBetTypeDataAccess _dataAccess;

        [OneTimeSetUp]
        public void Setup()
        {
            var balanceUp = new BalanceUp
            {
                BalanceUpDay = new DateTime(2018, 1, 1),
                MoveBetsDay = new DateTime(2018, 2, 2),
            };
            _dataAccess = Substitute.ForPartsOf<WinLossByBetTypeDataAccess>(balanceUp);
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
            var expectedResult = @"{""AgentList"":""xml"",""FromDate"":""\/Date(1483203600000)\/"",""ToDate"":""\/Date(1485968400000)\/""," +
                            @"""MoveBetsDay"":""\/Date(1517504400000)\/"",""BalanceUpDay"":""\/Date(1514739600000)\/""}";
            var metaModel = new WinLossByBetTypeMetaModel
            {
                AgentListXml = "xml",
                FromDate = new DateTime(2017, 1, 1),
                ToDate = new DateTime(2017, 2, 2)
            };
            dynamic uspParam = null;
            var objectDb = Substitute.For<IObjectDb>();
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_Rpt_WinLossByBettype")).Returns(objectDb);
            objectDb.ExecuteReader(Arg.Do<dynamic>(arg => uspParam = arg));

            // Action
            _dataAccess.GetDataList(metaModel);

            // Assert
            Assert.That(new JavaScriptSerializer().Serialize(uspParam), Is.EqualTo(expectedResult));
        }

        [Test]
        public void GetDataList_WhenCall_ReturnCorrectResult()
        {
            // Arrange
            var reader = Substitute.For<IDataReader>();
            var objectDb = Substitute.For<IObjectDb>();
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_Rpt_WinLossByBettype")).Returns(objectDb);
            objectDb.ExecuteReader(Arg.Any<object>()).ReturnsForAnyArgs(reader);
            reader.Read().Returns(true, false);
            var readerDataMapping = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase)
            {
                { "TurnoverUSD", 999 }
            };

            MapReaderData(reader, readerDataMapping);

            // Action
            var result = _dataAccess.GetDataList(new WinLossByBetTypeMetaModel
            {
                BaseCurrency = "USD"
            });

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Count, Is.EqualTo(1));
                Assert.That(result[0].Turnover, Is.EqualTo(999));
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