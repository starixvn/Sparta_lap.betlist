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
    public class ColossusBetsWinLossDataAccessTest
    {
        private readonly DateTime _moveBetsDay = new DateTime(2010, 1, 1);
        private IObjectDb _objectDb;
        private ColossusBetsWinLossDataAccess _dataAccess;
        private IDataReader _reader;

        [SetUp]
        public void Setup()
        {
            _objectDb = Substitute.For<IObjectDb>();

            var balanceUp = new BalanceUp
            {
                MoveBetsDay = _moveBetsDay
            };
            _dataAccess = Substitute.ForPartsOf<ColossusBetsWinLossDataAccess>(balanceUp);

            _reader = Substitute.For<IDataReader>();
            _objectDb.ExecuteReader().Returns(_reader);
        }

        [TearDown]
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
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_Rpt_ColossusBetsWinloss")).Returns(_objectDb);
            var metaModel = new ThirdPartyWinLossMetaModel
            {
                CurrencyId = 1,
                FromDate = new DateTime(2016, 8, 10),
                ToDate = new DateTime(2016, 8, 29)
            };
            IDictionary<string, ColossusBetWinLoss> summaryTotal = new Dictionary<string, ColossusBetWinLoss>();

            // Action
            _dataAccess.GetSummaryWinloss(metaModel, out summaryTotal);

            // Assert
            _objectDb.Received().AddInParameter("licenseeid", DbType.Int32, metaModel.LicenseeId);
            _objectDb.Received().AddInParameter("currencyid", DbType.Int32, metaModel.CurrencyId);
            _objectDb.Received().AddInParameter("userid", DbType.Int32, metaModel.UserId);
            _objectDb.Received().AddInParameter("fromdate", DbType.Date, metaModel.FromDate.Date);
            _objectDb.Received().AddInParameter("todate", DbType.Date, metaModel.ToDate.Date);
            _objectDb.Received().AddInParameter("excludetest", DbType.Int32, metaModel.IsExcludedTestData);
            _objectDb.Received().AddInParameter("movebetsday", DbType.DateTime, this._moveBetsDay);
            _objectDb.Received().AddInParameter("ReportType", DbType.Int32, metaModel.ReportBy);
        }

        [Test]
        public void GetSummaryWinloss_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_Rpt_ColossusBetsWinloss")).Returns(_objectDb);
            var metaModel = new ThirdPartyWinLossMetaModel
            {
                CurrencyId = 1,
                FromDate = new DateTime(2016, 8, 10),
                ToDate = new DateTime(2016, 8, 29),
                DrillDownFromDate = new DateTime(2016, 8, 29),
                BaseCurrency = "UUS"
            };
            IDictionary<string, ColossusBetWinLoss> summaryTotal = new Dictionary<string, ColossusBetWinLoss>();

            var expected = new ColossusBetWinLoss
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
            Assert.That(result.TurnOver, Is.EqualTo(expected.TurnOver));
            Assert.That(result.TotalTransaction, Is.EqualTo(expected.TotalTransaction));
            Assert.That(result.Date, Is.EqualTo(expected.Date));
        }

        [Test]
        public void GetWinlossDetail_WhenCall_ParameterPassedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_Rpt_ColossusBetsWinloss_detail")).Returns(_objectDb);
            var metaModel = new ThirdPartyWinLossMetaModel
            {
                CurrencyId = 1,
                DrillDownCustomerId = 2,
                FromDate = new DateTime(2016, 8, 10),
                ToDate = new DateTime(2016, 8, 29),
                DrillDownFromDate = new DateTime(2016, 8, 10),
                DrillDownToDate = new DateTime(2016, 8, 29),
                DrillDownLevel = 1
            };
            IDictionary<string, ColossusBetWinLoss> summaryTotal = new Dictionary<string, ColossusBetWinLoss>();

            // Action
            _dataAccess.GetWinlossDetail(metaModel, out summaryTotal);

            // Assert
            _objectDb.Received().AddInParameter("licenseeid", DbType.Int32, metaModel.LicenseeId);
            _objectDb.Received().AddInParameter("currencyid", DbType.Int32, metaModel.DrillDownCurrencyId);
            _objectDb.Received().AddInParameter("userid", DbType.Int32, metaModel.UserId);
            _objectDb.Received().AddInParameter("fromdate", DbType.Date, metaModel.DrillDownFromDate);
            _objectDb.Received().AddInParameter("todate", DbType.Date, metaModel.DrillDownToDate);
            _objectDb.Received().AddInParameter("excludetest", DbType.Int32, metaModel.IsExcludedTestData);
            _objectDb.Received().AddInParameter("movebetsday", DbType.DateTime, this._moveBetsDay);
            _objectDb.Received().AddInParameter("ReportType", DbType.Int32, metaModel.ReportBy);
        }

        [Test]
        public void GetWinlossDetail_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_Rpt_ColossusBetsWinloss_detail")).Returns(_objectDb);
            var metaModel = new ThirdPartyWinLossMetaModel
            {
                CurrencyId = 1,
                FromDate = new DateTime(2016, 8, 10),
                ToDate = new DateTime(2016, 8, 29),
                DrillDownFromDate = new DateTime(2016, 8, 29),
                BaseCurrency = "UUS"
            };
            IDictionary<string, ColossusBetWinLoss> summaryTotal = new Dictionary<string, ColossusBetWinLoss>();

            var expected = new ColossusBetWinLoss
            {
                SystemId = string.Empty,
                ExternalId = string.Empty,
                CurrencyName = "CurrencyNameTest",
                Date = new DateTime(2016, 8, 29)
            };
            ConfigureReaderForGetSummaryWinlossMethod(expected);

            // Action
            var result = _dataAccess.GetWinlossDetail(metaModel, out summaryTotal)[0];

            // Assert
            Assert.That(result.SystemId, Is.EqualTo(expected.SystemId));
            Assert.That(result.ExternalId, Is.EqualTo(expected.ExternalId));
            Assert.That(result.CustomerId, Is.EqualTo(expected.CustomerId));
            Assert.That(result.CurrencyId, Is.EqualTo(expected.CurrencyId));
            Assert.That(result.CurrencyName, Is.EqualTo(expected.CurrencyName));
            Assert.That(result.TurnOver, Is.EqualTo(expected.TurnOver));
            Assert.That(result.TotalTransaction, Is.EqualTo(expected.TotalTransaction));
            Assert.That(result.Date, Is.EqualTo(expected.Date));
        }

        private void ConfigureReaderForGetSummaryWinlossMethod(ColossusBetWinLoss expected)
        {
            _reader.Read().Returns(true, false);

            _reader["currencyid"].Returns(expected.CurrencyId);
            _reader["currencyname"].Returns(expected.CurrencyName);
            _reader["totaltrans"].Returns(expected.TotalTransaction);
            _reader["reporttype"].Returns(expected.Date.ToString("yyyyMMdd"));
            _reader["turnoveruus"].Returns(0);
            _reader["winningprizeuus"].Returns(1);
            _reader["cashoutamtuus"].Returns(0);
            _reader["cashinamtuus"].Returns(0);
            _reader["grandamtuus"].Returns(0);
            _reader["commamtuus"].Returns(0);

            foreach (var suffix in SystemConstants.BaseCurrencyList)
            {
                _reader["turnover" + suffix].Returns(0);
                _reader["winningprize" + suffix].Returns(1);
                _reader["cashoutamt" + suffix].Returns(0);
                _reader["cashinamt" + suffix].Returns(0);
                _reader["grandamt" + suffix].Returns(0);
                _reader["commamt" + suffix].Returns(0);
            }
        }
    }
}