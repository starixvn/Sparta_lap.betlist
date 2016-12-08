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
    public class AgCasinoWinLossDataAccessTest
    {
        private IObjectDb _objectDb;
        private AgCasinoWinLossDataAccess _dataAccess;
        private IDataReader _reader;

        [OneTimeSetUp]
        public void Setup()
        {
            _objectDb = Substitute.For<IObjectDb>();

            var balanceUp = new BalanceUp();
            _dataAccess = Substitute.ForPartsOf<AgCasinoWinLossDataAccess>(balanceUp);

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
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_Rpt_AGCasinoWinloss")).Returns(_objectDb);
            var metaModel = new AgCasinoWinLossMetaModel
            {
                CurrencyId = 1,
                FromDate = new DateTime(2016, 8, 10),
                ToDate = new DateTime(2016, 8, 29)
            };
            IDictionary<string, AgCasinoWinLoss> summaryTotal = new Dictionary<string, AgCasinoWinLoss>();

            // Action
            _dataAccess.GetSummaryWinloss(metaModel, out summaryTotal);

            // Assert
            _objectDb.Received().AddInParameter("licenseeid", DbType.Int32, metaModel.LicenseeId);
            _objectDb.Received().AddInParameter("currency", DbType.Int32, metaModel.CurrencyId);
            _objectDb.Received().AddInParameter("userid", DbType.Int32, metaModel.UserId);
            _objectDb.Received().AddInParameter("fromdate", DbType.Date, metaModel.FromDate.Date);
            _objectDb.Received().AddInParameter("todate", DbType.Date, metaModel.ToDate.Date);
            _objectDb.Received().AddInParameter("excludetest", DbType.Int32, metaModel.IsExcludedTestData);
            _objectDb.Received().AddInParameter("ReportType", DbType.Int32, metaModel.ReportBy);
        }

        [Test]
        public void GetSummaryWinloss_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_Rpt_AGCasinoWinloss")).Returns(_objectDb);
            var metaModel = new AgCasinoWinLossMetaModel
            {
                CurrencyId = 1,
                FromDate = new DateTime(2016, 8, 10),
                ToDate = new DateTime(2016, 8, 29),
                DrillDownFromDate = new DateTime(2016, 8, 29),
                BaseCurrency = "UUS"
            };
            IDictionary<string, AgCasinoWinLoss> summaryTotal = new Dictionary<string, AgCasinoWinLoss>();

            var expected = new AgCasinoWinLoss
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
        }

        [Test]
        public void GetCustomerWinlossDetail_WhenCall_ParameterPassedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("LRF_Rpt_AGCasinoWinloss_Mem")).Returns(_objectDb);
            var metaModel = new AgCasinoWinLossMetaModel
            {
                CurrencyId = 1,
                DrillDownCustomerId = 2,
                FromDate = new DateTime(2016, 8, 10),
                ToDate = new DateTime(2016, 8, 29),
                DrillDownFromDate = new DateTime(2016, 8, 10),
                DrillDownToDate = new DateTime(2016, 8, 29)
            };
            IDictionary<string, AgCasinoWinLoss> summaryTotal = new Dictionary<string, AgCasinoWinLoss>();

            // Action
            _dataAccess.GetCustomerWinlossDetail(metaModel, out summaryTotal);

            // Assert
            _objectDb.Received().AddInParameter("licenseeid", DbType.Int32, metaModel.LicenseeId);
            _objectDb.Received().AddInParameter("currency", DbType.Int32, metaModel.DrillDownCurrencyId);
            _objectDb.Received().AddInParameter("userid", DbType.Int32, metaModel.UserId);
            _objectDb.Received().AddInParameter("fromdate", DbType.Date, metaModel.DrillDownFromDate);
            _objectDb.Received().AddInParameter("todate", DbType.Date, metaModel.DrillDownToDate);
            _objectDb.Received().AddInParameter("excludetest", DbType.Int32, metaModel.IsExcludedTestData);
            _objectDb.Received().AddInParameter("custid", DbType.Int32, metaModel.DrillDownCustomerId);
        }

        [Test]
        public void GetCustomerWinlossDetail_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("LRF_Rpt_AGCasinoWinloss_Mem")).Returns(_objectDb);
            var metaModel = new AgCasinoWinLossMetaModel
            {
                CurrencyId = 1,
                FromDate = new DateTime(2016, 8, 10),
                ToDate = new DateTime(2016, 8, 29),
                DrillDownFromDate = new DateTime(2016, 8, 29),
                BaseCurrency = "UUS"
            };
            IDictionary<string, AgCasinoWinLoss> summaryTotal = new Dictionary<string, AgCasinoWinLoss>();

            var expected = new AgCasinoWinLoss
            {
                SystemId = string.Empty,
                ExternalId = string.Empty,
                CurrencyName = "CurrencyNameTest",
                Date = new DateTime(2016, 8, 29)
            };
            ConfigureReaderForGetSummaryWinlossMethod(expected);

            // Action
            var result = _dataAccess.GetCustomerWinlossDetail(metaModel, out summaryTotal)[0];

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
        }

        [Test]
        public void GetAgentWinlossDetail_WhenCall_ParameterPassedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("LRF_Rpt_AGCasinoWinloss_Ag")).Returns(_objectDb);
            var metaModel = new AgCasinoWinLossMetaModel
            {
                CurrencyId = 1,
                DrillDownCustomerId = 2,
                FromDate = new DateTime(2016, 8, 10),
                ToDate = new DateTime(2016, 8, 29),
                DrillDownFromDate = new DateTime(2016, 8, 10),
                DrillDownToDate = new DateTime(2016, 8, 29)
            };
            IDictionary<string, AgCasinoWinLoss> summaryTotal = new Dictionary<string, AgCasinoWinLoss>();

            // Action
            _dataAccess.GetAgentWinlossDetail(metaModel, out summaryTotal);

            // Assert
            _objectDb.Received().AddInParameter("licenseeid", DbType.Int32, metaModel.LicenseeId);
            _objectDb.Received().AddInParameter("currency", DbType.Int32, metaModel.DrillDownCurrencyId);
            _objectDb.Received().AddInParameter("userid", DbType.Int32, metaModel.UserId);
            _objectDb.Received().AddInParameter("fromdate", DbType.Date, metaModel.DrillDownFromDate);
            _objectDb.Received().AddInParameter("todate", DbType.Date, metaModel.DrillDownToDate);
            _objectDb.Received().AddInParameter("excludetest", DbType.Int32, metaModel.IsExcludedTestData);
        }

        [Test]
        public void GetAgentWinlossDetail_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("LRF_Rpt_AGCasinoWinloss_Ag")).Returns(_objectDb);
            var metaModel = new AgCasinoWinLossMetaModel
            {
                CurrencyId = 1,
                FromDate = new DateTime(2016, 8, 10),
                ToDate = new DateTime(2016, 8, 29),
                DrillDownFromDate = new DateTime(2016, 8, 29),
                BaseCurrency = "UUS"
            };
            IDictionary<string, AgCasinoWinLoss> summaryTotal = new Dictionary<string, AgCasinoWinLoss>();

            var expected = new AgCasinoWinLoss
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
            Assert.That(result.CustomerWinLoss, Is.EqualTo(expected.CustomerWinLoss));
            Assert.That(result.LicenseeWL, Is.EqualTo(expected.LicenseeWL));
            Assert.That(result.TurnOver, Is.EqualTo(expected.TurnOver));
            Assert.That(result.TotalTransaction, Is.EqualTo(expected.TotalTransaction));
            Assert.That(result.Date, Is.EqualTo(expected.Date));
        }

        private void ConfigureReaderForGetSummaryWinlossMethod(AgCasinoWinLoss expected)
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

            foreach (var currencySuffix in SystemConstants.BaseCurrencyList)
            {
                _reader["winlost" + currencySuffix].Returns(1);
                _reader["licenseewinlost" + currencySuffix].Returns(0);
                _reader["turnover" + currencySuffix].Returns(0);
            }
        }
    }
}