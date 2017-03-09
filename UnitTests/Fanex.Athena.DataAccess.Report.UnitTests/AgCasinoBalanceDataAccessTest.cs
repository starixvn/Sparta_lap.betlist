namespace Fanex.Athena.DataAccess.Report.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Data;
    using Model.Core.DataDomain;
    using Model.Core.DTO.Configuration;
    using Model.Report.DTO;
    using Model.Report.MetaModel;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class AgCasinoBalanceDataAccessTest
    {
        private IObjectDb _objectDb;
        private AgCasinoBalanceDataAccess _dataAccess;
        private IDataReader _reader;

        [OneTimeSetUp]
        public void Setup()
        {
            _objectDb = Substitute.For<IObjectDb>();

            var balanceUp = new BalanceUp();
            _dataAccess = Substitute.ForPartsOf<AgCasinoBalanceDataAccess>(balanceUp);

            _reader = Substitute.For<IDataReader>();
            _objectDb.ExecuteReader().Returns(_reader);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _objectDb = null;
            _dataAccess = null;
            _reader = null;
        }

        [Test]
        public void GetSummaryWinloss_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_Rpt_AGCasinoBalance")).Returns(_objectDb);
            var metaModel = new AgCasinoBalanceMetaModel
            {
                CurrencyId = 1,
                FromDate = new DateTime(2016, 8, 10),
                ToDate = new DateTime(2016, 8, 29)
            };
            IDictionary<string, AgCasinoBalance> summaryTotal = new Dictionary<string, AgCasinoBalance>();

            // Action
            _dataAccess.GetSummaryWinloss(metaModel, out summaryTotal);

            // Assert
            _objectDb.Received().AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
            _objectDb.Received().AddInParameter("fromdate", DbType.Date, metaModel.FromDate.Date);
            _objectDb.Received().AddInParameter("todate", DbType.Date, metaModel.ToDate.Date);
            _objectDb.Received().AddInParameter("excludetest", DbType.Int32, metaModel.IsExcludedTestData);
        }

        [Test]
        public void GetSummaryWinloss_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_Rpt_AGCasinoBalance")).Returns(_objectDb);
            var metaModel = new AgCasinoBalanceMetaModel
            {
                CurrencyId = 1,
                FromDate = new DateTime(2016, 8, 10),
                ToDate = new DateTime(2016, 8, 29),
                DrillDownFromDate = new DateTime(2016, 8, 29),
                BaseCurrency = "UUS"
            };
            IDictionary<string, AgCasinoBalance> summaryTotal = new Dictionary<string, AgCasinoBalance>();

            var expected = new AgCasinoBalance
            {
                SystemId = string.Empty,
                ExternalId = string.Empty,
                CurrencyName = "CurrencyNameTest",
                Date = new DateTime(2016, 8, 29)
            };
            ConfigureReaderForGetSummaryWinlossMethod(expected);

            // Action
            var result = _dataAccess.GetSummaryWinloss(metaModel, out summaryTotal)[0];

            // Assert
            Assert.That(result.SystemId, Is.EqualTo(expected.SystemId));
            Assert.That(result.ExternalId, Is.EqualTo(expected.ExternalId));
            Assert.That(result.CustomerId, Is.EqualTo(expected.CustomerId));
            Assert.That(result.CurrencyId, Is.EqualTo(expected.CurrencyId));
            Assert.That(result.CurrencyName, Is.EqualTo(expected.CurrencyName));
            Assert.That(result.CustomerWinLoss, Is.EqualTo(expected.CustomerWinLoss));
            Assert.That(result.LicenseeWL, Is.EqualTo(expected.LicenseeWL));
            Assert.That(result.TurnOver, Is.EqualTo(expected.TurnOver));
            Assert.That(result.TotalTransaction, Is.EqualTo(expected.TotalTransaction));
            Assert.That(result.Date, Is.EqualTo(expected.Date));
            Assert.That(result.OpeningBalance, Is.EqualTo(expected.OpeningBalance));
            Assert.That(result.TransferIn, Is.EqualTo(expected.TransferIn));
            Assert.That(result.TransferOut, Is.EqualTo(expected.TransferOut));
            Assert.That(result.ClosingBalance, Is.EqualTo(expected.ClosingBalance));
        }

        [Test]
        public void GetCustomerWinlossDetail_WhenCall_ParametersReturnedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("LRF_Rpt_AGCasinoBalance_Mem")).Returns(_objectDb);
            var metaModel = new AgCasinoBalanceMetaModel
            {
                CurrencyId = 1,
                FromDate = new DateTime(2016, 8, 10),
                ToDate = new DateTime(2016, 8, 29),
                DrillDownFromDate = new DateTime(2016, 8, 10),
                DrillDownToDate = new DateTime(2016, 8, 29)
            };
            IDictionary<string, AgCasinoBalance> summaryTotal = new Dictionary<string, AgCasinoBalance>();
            var totalRows = 0;
            _objectDb.GetParameterValue("TotalRows").Returns(10);

            // Action
            _dataAccess.GetCustomerWinlossDetail(metaModel, out summaryTotal, out totalRows);

            // Assert
            _objectDb.Received().AddInParameter("custid", DbType.Int32, metaModel.DrillDownCustomerId);
            _objectDb.Received().AddInParameter("pagesize", DbType.Int32, metaModel.PageSize);
            _objectDb.Received().AddInParameter("pageindex", DbType.Int32, metaModel.PageIndex);
            _objectDb.Received().AddOutParameter("TotalRows", DbType.Int32, 10);
            _objectDb.Received().AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
            _objectDb.Received().AddInParameter("fromdate", DbType.Date, metaModel.DrillDownFromDate);
            _objectDb.Received().AddInParameter("todate", DbType.Date, metaModel.DrillDownToDate);
        }

        [Test]
        public void GetCustomerWinlossDetail_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("LRF_Rpt_AGCasinoBalance_Mem")).Returns(_objectDb);
            var metaModel = new AgCasinoBalanceMetaModel
            {
                CurrencyId = 1,
                FromDate = new DateTime(2016, 8, 10),
                ToDate = new DateTime(2016, 8, 29),
                DrillDownFromDate = new DateTime(2016, 8, 10),
                DrillDownToDate = new DateTime(2016, 8, 29),
                BaseCurrency = "UUS"
            };
            IDictionary<string, AgCasinoBalance> summaryTotal = new Dictionary<string, AgCasinoBalance>();
            var totalRows = 0;
            _objectDb.GetParameterValue("TotalRows").Returns(10);

            var expected = new AgCasinoBalance
            {
                SystemId = string.Empty,
                ExternalId = string.Empty,
                CurrencyName = "CurrencyNameTest",
                Date = new DateTime(2016, 8, 29)
            };
            ConfigureReaderForGetSummaryWinlossMethod(expected);

            // Action
            var result = _dataAccess.GetCustomerWinlossDetail(metaModel, out summaryTotal, out totalRows)[0];

            // Assert
            Assert.That(result.SystemId, Is.EqualTo(expected.SystemId));
            Assert.That(result.ExternalId, Is.EqualTo(expected.ExternalId));
            Assert.That(result.CustomerId, Is.EqualTo(expected.CustomerId));
            Assert.That(result.CurrencyId, Is.EqualTo(expected.CurrencyId));
            Assert.That(result.CurrencyName, Is.EqualTo(expected.CurrencyName));
        }

        [Test]
        public void GetAgentWinlossDetail_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("LRF_Rpt_AGCasinoBalance_Ag")).Returns(_objectDb);
            var metaModel = new AgCasinoBalanceMetaModel
            {
                CurrencyId = 1,
                FromDate = new DateTime(2016, 8, 10),
                ToDate = new DateTime(2016, 8, 29),
                DrillDownFromDate = new DateTime(2016, 8, 29),
                BaseCurrency = "UUS"
            };
            IDictionary<string, AgCasinoBalance> summaryTotal = new Dictionary<string, AgCasinoBalance>();

            var expected = new AgCasinoBalance
            {
                SystemId = string.Empty,
                ExternalId = string.Empty,
                CurrencyName = "CurrencyNameTest",
                Date = new DateTime(2016, 8, 29)
            };
            ConfigureReaderForGetSummaryWinlossMethod(expected);

            // Action
            var result = _dataAccess.GetAgentWinlossDetail(metaModel, out summaryTotal)[0];

            // Assert
            Assert.That(result.SystemId, Is.EqualTo(expected.SystemId));
            Assert.That(result.ExternalId, Is.EqualTo(expected.ExternalId));
            Assert.That(result.CustomerId, Is.EqualTo(expected.CustomerId));
            Assert.That(result.CurrencyId, Is.EqualTo(expected.CurrencyId));
            Assert.That(result.CurrencyName, Is.EqualTo(expected.CurrencyName));
        }

        private void ConfigureReaderForGetSummaryWinlossMethod(AgCasinoBalance expected)
        {
            _reader.Read().Returns(true, false);

            _reader["username"].Returns(expected.SystemId);
            _reader["nickname"].Returns(expected.ExternalId);
            _reader["custid"].Returns(expected.CustomerId);
            _reader["currency"].Returns(expected.CurrencyId);
            _reader["currencyname"].Returns(expected.CurrencyName);
            _reader["winlostuus"].Returns(expected.CustomerWinLoss);
            _reader["licenseewinlostuus"].Returns(expected.LicenseeWL);
            _reader["turnoveruus"].Returns(expected.TurnOver);
            _reader["totaltrans"].Returns(expected.TotalTransaction);
            _reader["date"].Returns(new DateTime(2016, 8, 29));
            _reader["OpeningBalanceuus"].Returns(expected.OpeningBalance);
            _reader["TransferInuus"].Returns(expected.TransferIn);
            _reader["TransferOutuus"].Returns(expected.TransferOut);
            _reader["ClosingBalanceuus"].Returns(expected.ClosingBalance);

            foreach (var currencySuffix in SystemConstants.BaseCurrencyList)
            {
                _reader["winlost" + currencySuffix].Returns(1);
                _reader["licenseewinlost" + currencySuffix].Returns(0);
                _reader["turnover" + currencySuffix].Returns(0);
                _reader["OpeningBalance" + currencySuffix].Returns(0);
                _reader["TransferIn" + currencySuffix].Returns(0);
                _reader["TransferOut" + currencySuffix].Returns(0);
                _reader["ClosingBalance" + currencySuffix].Returns(0);
            }
        }
    }
}