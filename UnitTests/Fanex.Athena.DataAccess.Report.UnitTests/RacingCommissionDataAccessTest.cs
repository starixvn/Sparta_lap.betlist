namespace Fanex.Athena.DataAccess.Report.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Core.Configuration.Interfaces;
    using Data;
    using Fanex.Athena.UnitTest.Common.Extensions;
    using Model.Core.DTO.Configuration;
    using Model.Report.DTO;
    using Model.Report.MetaModel;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class RacingCommissionDataAccessTest
    {
        private RacingCommissionDataAccess _dataAccess;
        private BalanceUp _balance;
        private RacingCommissionMetaModel _metaModel;

        [OneTimeSetUp]
        public void Setup()
        {
            var balanceUpDataAccess = Substitute.For<IBalanceUpDataAccess>();
            _balance = new BalanceUp
            {
                MoveBetsDay = new DateTime(2016, 8, 10),
                BalanceUpDay = new DateTime(2016, 8, 31)
            };
            balanceUpDataAccess.GetBalanceUp().Returns(_balance);

            _dataAccess = Substitute.ForPartsOf<RacingCommissionDataAccess>(balanceUpDataAccess);

            _metaModel = new RacingCommissionMetaModel
            {
                LicenseeId = 1,
                BaseCurrency = "USD",
                AgentListXml = "xml",
                FromDate = new DateTime(2017, 1, 1),
                ToDate = new DateTime(2017, 2, 2),
                PageIndex = 2,
                PageSize = 100
            };
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _dataAccess = null;
            _metaModel = null;
            _balance = null;
        }

        [Test]
        public void GetDataList_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            var objectDb = Substitute.For<IObjectDb>();
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_Rpt_CommByBettype_HR")).Returns(objectDb);
            var totalData = new RacingCommission();
            dynamic uspParam = null;
            objectDb.ExecuteReader(Arg.Do<dynamic>(arg => uspParam = arg));
            var expectedResult = new
            {
                AgentList = "xml",
                FromDate = _metaModel.FromDate,
                ToDate = _metaModel.ToDate,
                MoveBetsDay = _balance.MoveBetsDay,
                PageSize = _metaModel.PageSize,
                PageIndex = _metaModel.PageIndex
            };

            // Action
            _dataAccess.GetDataList(_metaModel, ref totalData);

            // Assert
            Assert.That(uspParam, Tobe.SameProperties(expectedResult));
        }

        [Test]
        public void GetDataList_WhenCall_ReturnCorrectResult()
        {
            // Arrange
            var reader = Substitute.For<IDataReader>();
            var objectDb = Substitute.For<IObjectDb>();
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_Rpt_CommByBettype_HR")).Returns(objectDb);
            objectDb.ExecuteReader(Arg.Any<object>()).ReturnsForAnyArgs(reader);
            reader.Read().Returns(true, false, true, false);
            reader.NextResult().Returns(true);
            RacingCommission totalData = new RacingCommission();
            var readerDataMapping = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase)
            {
                { "custID", 999 },
                { "Tote_WinUSD", 100 },
                { "SumTote_WinUSD", 1000 }
            };

            MapReaderData(reader, readerDataMapping);

            // Action
            var result = _dataAccess.GetDataList(_metaModel, ref totalData);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Count, Is.EqualTo(1));
                Assert.That(result[0].CustID, Is.EqualTo(999));
                Assert.That(result[0].ToteWin, Is.EqualTo(100));

                Assert.That(totalData.ToteWin, Is.EqualTo(1000));
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