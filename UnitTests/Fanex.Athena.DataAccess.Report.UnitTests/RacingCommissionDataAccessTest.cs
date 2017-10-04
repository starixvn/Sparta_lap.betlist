namespace Fanex.Athena.DataAccess.Report.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Core.Configuration.Interfaces;
    using Data;
    using Fanex.Athena.UnitTest.Common.Extensions;
    using Model.Core.DTO.Configuration;
    using Model.Report.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using UnitTest.Common.Helpers;

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
            _dataAccess.GetData(_metaModel);

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
            var readerDataMapping = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase)
            {
                { "custID", 999 },
                { "Tote_WinUSD", 100 },
                { "SumTote_WinUSD", 1000 }
            };

            DataAccessTestHelper.MapReaderData(reader, readerDataMapping);

            // Action
            var result = _dataAccess.GetData(_metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.DataList.Count, Is.EqualTo(1));
                Assert.That(result.DataList[0].CustID, Is.EqualTo(999));
                Assert.That(result.DataList[0].ToteWin, Is.EqualTo(100));

                Assert.That(result.TotalData.ToteWin, Is.EqualTo(1000));
            });
        }
    }
}