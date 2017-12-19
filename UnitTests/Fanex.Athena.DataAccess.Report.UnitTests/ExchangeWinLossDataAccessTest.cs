namespace Fanex.Athena.DataAccess.Report
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Fanex.Data;
    using Model.Core.DataDomain;
    using Model.Core.ResultModel;
    using Model.Report.DTO;
    using Model.Report.MetaModel;
    using NSubstitute;
    using NUnit.Framework;

    public class ExchangeWinLossDataAccessTest
    {
        private IObjectDb _objectDb;
        private ExchangeWinLossDataAccess _dataAccess;
        private IDataReader _reader;
        private ExchangeWinLossMetaModel _metaModel;
        private IDictionary<string, decimal> _baseCurrencyExchangeRate;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _objectDb = Substitute.For<IObjectDb>();

            _dataAccess = Substitute.ForPartsOf<ExchangeWinLossDataAccess>();
            _dataAccess.GetObjectDb(Arg.Is<string>(x => x.Contains("Lrf_Rpt_ExchangeWinloss"))).Returns(_objectDb);

            _metaModel = new ExchangeWinLossMetaModel
            {
                FromDate = DateTime.Today,
                ToDate = DateTime.Today.AddDays(1),
                AgentListXml = string.Empty,
                ReportBy = 1,
                CustId = 2
            };

            _baseCurrencyExchangeRate = new Dictionary<string, decimal>();
            for (int i = 0; i < SystemConstants.BaseCurrencyList.Count; i++)
            {
                _baseCurrencyExchangeRate.Add(SystemConstants.BaseCurrencyList[i], (i + 1) * 0.1m);
            };
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _objectDb = null;
            _dataAccess = null;
        }

        [SetUp]
        public void SetUp()
        {
            _reader = Substitute.For<IDataReader>();
            _objectDb.ExecuteReader().Returns(_reader);
        }

        [TearDown]
        public void TearDown()
        {
            _objectDb.ClearReceivedCalls();
            _reader = null;
        }

        [Test]
        public void GetMainWinloss_WhenCall_ParametersPassedCorrectly()
        {
            // Action
            _dataAccess.GetMainWinloss(_metaModel);

            // Assert
            _dataAccess.Received().GetObjectDb("Lrf_Rpt_ExchangeWinloss");
            _objectDb.Received().AddInParameter("ReportType", DbType.Byte, _metaModel.ReportBy);
            _objectDb.Received().AddInParameter("AgentList", DbType.String, _metaModel.AgentListXml);
            _objectDb.Received().AddInParameter("Fromdate", DbType.Date, _metaModel.FromDate.Date);
            _objectDb.Received().AddInParameter("Todate", DbType.Date, _metaModel.ToDate.Date);
        }

        public void GetMainWinloss_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            _reader.Read().Returns(true);

            var currencyList = new[] { "USD", "EUR" };
            var expectedList = CreateExpectedResult(currencyList, DateTime.Today);

            ConfigureReaderForResult(expectedList);

            // Action
            var result = _dataAccess.GetMainWinloss(_metaModel);
            var firstItem = result.DataList.First();

            // Assert
            AssertMainResultData(result, expectedList);
            Assert.That(firstItem.ExternalId, Is.Empty);
            Assert.That(firstItem.SystemId, Is.Empty);
            Assert.That(firstItem.CusId, Is.Zero);
        }

        [Test]
        public void GetLicenseeWinloss_WhenCall_ParametersPassedCorrectly()
        {
            // Action
            _dataAccess.GetLicenseeWinloss(_metaModel);

            // Assert
            _dataAccess.Received().GetObjectDb("Lrf_Rpt_ExchangeWinloss_Mem");
            _objectDb.Received().AddInParameter("CustID", DbType.Int32, _metaModel.CustId);
            _objectDb.Received().AddInParameter("AgentList", DbType.String, _metaModel.AgentListXml);
            _objectDb.Received().AddInParameter("Fromdate", DbType.Date, _metaModel.FromDate.Date);
            _objectDb.Received().AddInParameter("Todate", DbType.Date, _metaModel.ToDate.Date);
        }

        public void GetLicenseeWinloss_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            _reader.Read().Returns(true);

            var currencyList = new[] { "USD", "EUR" };
            var expectedList = CreateExpectedResult(currencyList, DateTime.Today);

            ConfigureReaderForResult(expectedList);
            ConfigureReaderForLicenseeResult(expectedList);

            // Action
            var result = _dataAccess.GetLicenseeWinloss(_metaModel);
            var firstItem = result.DataList.First();
            var firstExpected = expectedList.First();

            // Assert
            AssertMainResultData(result, expectedList);
            Assert.That(firstItem.ExternalId, Is.Empty);
            Assert.That(firstItem.SystemId, Is.EqualTo(firstExpected.SystemId));
            Assert.That(firstItem.CusId, Is.EqualTo(firstExpected.CusId));
        }

        [Test]
        public void GetCustomerWinloss_WhenCall_ParametersPassedCorrectly()
        {
            // Action
            _dataAccess.GetCustomerWinloss(_metaModel);

            // Assert
            _dataAccess.Received().GetObjectDb("Lrf_Rpt_ExchangeWinloss_Ag");
            _objectDb.Received().AddInParameter("AgentList", DbType.String, _metaModel.AgentListXml);
            _objectDb.Received().AddInParameter("Fromdate", DbType.Date, _metaModel.FromDate.Date);
            _objectDb.Received().AddInParameter("Todate", DbType.Date, _metaModel.ToDate.Date);
        }

        public void GetCustomerWinloss_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            _reader.Read().Returns(true);

            var currencyList = new[] { "USD", "EUR" };
            var expectedList = CreateExpectedResult(currencyList, DateTime.Today);

            ConfigureReaderForResult(expectedList);
            ConfigureReaderForCustomerResult(expectedList);

            // Action
            var result = _dataAccess.GetCustomerWinloss(_metaModel);
            var firstItem = result.DataList.First();
            var firstExpected = expectedList.First();

            // Assert
            AssertMainResultData(result, expectedList);
            Assert.That(firstItem.ExternalId, Is.EqualTo(firstExpected.ExternalId));
            Assert.That(firstItem.SystemId, Is.EqualTo(firstExpected.SystemId));
            Assert.That(firstItem.CusId, Is.Zero);
        }

        private List<ExchangeWinLoss> CreateExpectedResult(string[] currencyList, DateTime date)
        {
            var expectedResult = new List<ExchangeWinLoss>();

            for (int i = 0; i < currencyList.Count(); i++)
            {
                expectedResult.Add(
                    new ExchangeWinLoss
                    {
                        Date = date,
                        CurrencyName = currencyList[i],
                        CurrencyId = i,
                        ExternalId = "External" + i,
                        SystemId = "System" + i,
                        CusId = i,
                        Turnover = i,
                        NetTurnover = i,
                        CustomerWL = i,
                        CustomerComm = i,
                        LicenseeWL = i,
                        LicenseeComm = i,
                        TotalTransaction = i,
                        AmountDueTo = i
                    });
            }

            return expectedResult;
        }

        private void ConfigureReaderForResult(IEnumerable<ExchangeWinLoss> expectedList)
        {
            // main data
            ConfigureDataForReader(expectedList, string.Empty, 1);

            foreach (var currency in SystemConstants.BaseCurrencyList)
            {
                ConfigureDataForReader(expectedList, currency, _baseCurrencyExchangeRate[currency]);
            }
        }

        private void ConfigureDataForReader(IEnumerable<ExchangeWinLoss> expectedList, string baseCurrency, decimal rate)
        {
            _reader["Winlostdate" + baseCurrency].Returns(expectedList.Select(x => x.Date).ToArray());
            _reader["CurrencyName" + baseCurrency].Returns(expectedList.Select(x => x.CurrencyName).ToArray());
            _reader["CurrencyID" + baseCurrency].Returns(expectedList.Select(x => x.CurrencyId).ToArray());
            _reader["TurnOver" + baseCurrency].Returns(expectedList.Select(x => x.Turnover * rate).ToArray());
            _reader["NetTurnOver" + baseCurrency].Returns(expectedList.Select(x => x.NetTurnover * rate).ToArray());
            _reader["Winlost" + baseCurrency].Returns(expectedList.Select(x => x.CustomerWL * rate).ToArray());
            _reader["CustComm" + baseCurrency].Returns(expectedList.Select(x => x.CustomerComm * rate).ToArray());
            _reader["LicenseeWinlost" + baseCurrency].Returns(expectedList.Select(x => x.LicenseeWL * rate).ToArray());
            _reader["AgentComm" + baseCurrency].Returns(expectedList.Select(x => x.LicenseeComm * rate).ToArray());
            _reader["TotalTrans" + baseCurrency].Returns(expectedList.Select(x => x.TotalTransaction).ToArray());
            _reader["AmountDueTo" + baseCurrency].Returns(expectedList.Select(x => x.AmountDueTo * rate).ToArray());
        }

        private void ConfigureReaderForLicenseeResult(IEnumerable<ExchangeWinLoss> expectedList)
        {
            _reader["CustID"].Returns(expectedList.Select(x => x.CusId).ToArray());
            _reader["username"].Returns(expectedList.Select(x => x.SystemId).ToArray());
        }

        private void ConfigureReaderForCustomerResult(IEnumerable<ExchangeWinLoss> expectedList)
        {
            _reader["nickname"].Returns(expectedList.Select(x => x.ExternalId).ToArray());
            _reader["username"].Returns(expectedList.Select(x => x.SystemId).ToArray());
        }

        private void AssertMainResultData(ReportResultModel<ExchangeWinLoss> result, IEnumerable<ExchangeWinLoss> expectedList)
        {
            var firstItem = result.DataList.First();
            var firstExpected = expectedList.First();

            Assert.That(firstItem.Date, Is.EqualTo(firstExpected.Date));
            Assert.That(firstItem.CurrencyName, Is.EqualTo(firstExpected.CurrencyName));
            Assert.That(firstItem.CurrencyId, Is.EqualTo(firstExpected.CurrencyId));

            AssertCommonResultData(firstItem, firstExpected);
            AssertTotalResultData(result, expectedList);
            AssertSummaryTotalResultData(result, expectedList);
        }

        private void AssertTotalResultData(ReportResultModel<ExchangeWinLoss> result, IEnumerable<ExchangeWinLoss> expectedList)
        {
            var expectedTotal = expectedList.GroupBy(x => new { x.CurrencyId, x.CurrencyName }).Select(g => new ExchangeWinLoss
            {
                CurrencyId = g.Key.CurrencyId,
                CurrencyName = g.Key.CurrencyName,
                Turnover = g.Sum(x => x.Turnover),
                TotalTransaction = g.Sum(x => x.TotalTransaction),
                CustomerWL = g.Sum(x => x.CustomerWL),
                CustomerComm = g.Sum(x => x.CustomerComm),
                LicenseeWL = g.Sum(x => x.LicenseeWL),
                LicenseeComm = g.Sum(x => x.LicenseeComm),
                AmountDueTo = g.Sum(x => x.AmountDueTo),
                NetTurnover = g.Sum(x => x.NetTurnover)
            }).ToList();

            Assert.That(result.TotalDataList.Count, Is.EqualTo(expectedTotal.Count));
            foreach (var expected in expectedTotal)
            {
                var actual = result.TotalDataList.FirstOrDefault(x => x.CurrencyName == expected.CurrencyName);

                AssertCommonResultData(actual, expected);
            }
        }

        private void AssertSummaryTotalResultData(ReportResultModel<ExchangeWinLoss> result, IEnumerable<ExchangeWinLoss> expectedList)
        {
            Assert.That(result.SummaryTotal.Count, Is.EqualTo(SystemConstants.BaseCurrencyList.Count));

            foreach (var currency in SystemConstants.BaseCurrencyList)
            {
                var expected = new ExchangeWinLoss
                {
                    Turnover = expectedList.Sum(x => x.Turnover) * _baseCurrencyExchangeRate[currency],
                    TotalTransaction = expectedList.Sum(x => x.TotalTransaction) * _baseCurrencyExchangeRate[currency],
                    CustomerWL = expectedList.Sum(x => x.CustomerWL) * _baseCurrencyExchangeRate[currency],
                    CustomerComm = expectedList.Sum(x => x.CustomerComm) * _baseCurrencyExchangeRate[currency],
                    LicenseeWL = expectedList.Sum(x => x.LicenseeWL) * _baseCurrencyExchangeRate[currency],
                    LicenseeComm = expectedList.Sum(x => x.LicenseeComm) * _baseCurrencyExchangeRate[currency],
                    AmountDueTo = expectedList.Sum(x => x.AmountDueTo) * _baseCurrencyExchangeRate[currency],
                    NetTurnover = expectedList.Sum(x => x.NetTurnover) * _baseCurrencyExchangeRate[currency],
                };

                var actual = result.SummaryTotal[currency];

                AssertCommonResultData(actual, expected);
            }
        }

        private void AssertCommonResultData(ExchangeWinLoss actual, ExchangeWinLoss expected)
        {
            Assert.That(actual.Turnover, Is.EqualTo(expected.Turnover));
            Assert.That(actual.NetTurnover, Is.EqualTo(expected.NetTurnover));
            Assert.That(actual.CustomerWL, Is.EqualTo(expected.CustomerWL));
            Assert.That(actual.CustomerComm, Is.EqualTo(expected.CustomerComm));
            Assert.That(actual.LicenseeWL, Is.EqualTo(expected.LicenseeWL));
            Assert.That(actual.LicenseeComm, Is.EqualTo(expected.LicenseeComm));
            Assert.That(actual.TotalTransaction, Is.EqualTo(expected.TotalTransaction));
            Assert.That(actual.AmountDueTo, Is.EqualTo(expected.AmountDueTo));
        }
    }
}