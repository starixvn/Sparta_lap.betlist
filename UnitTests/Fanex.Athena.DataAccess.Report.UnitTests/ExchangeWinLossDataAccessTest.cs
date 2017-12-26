namespace Fanex.Athena.DataAccess.Report
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Core.Exception;
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
            _dataAccess = Substitute.ForPartsOf<ExchangeWinLossDataAccess>();

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
            }
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _dataAccess = null;
        }

        [SetUp]
        public void SetUp()
        {
            _objectDb = Substitute.For<IObjectDb>();
            _reader = Substitute.For<IDataReader>();
            _objectDb.ExecuteReader().Returns(_reader);

            _dataAccess.GetObjectDb(Arg.Is<string>(x => x.Contains("Lrf_Rpt_ExchangeWinloss"))).Returns(_objectDb);
        }

        [TearDown]
        public void TearDown()
        {
            _reader = null;
            _objectDb = null;
        }

        [Test]
        public void GetMainWinloss_WhenCall_ParametersPassedCorrectly()
        {
            // Action
            _dataAccess.GetMainWinloss(_metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                _dataAccess.Received().GetObjectDb("Lrf_Rpt_ExchangeWinloss");
                _objectDb.Received().AddInParameter("ReportType", DbType.Byte, _metaModel.ReportBy);
                _objectDb.Received().AddInParameter("AgentList", DbType.String, _metaModel.AgentListXml);
                _objectDb.Received().AddInParameter("Fromdate", DbType.Date, _metaModel.FromDate.Date);
                _objectDb.Received().AddInParameter("Todate", DbType.Date, _metaModel.ToDate.Date);
            });
        }

        [Test]
        public void GetMainWinloss_WhenReceiveObjectDbException_ThrowDataAccessException()
        {
            // Arrange
            _objectDb.ExecuteReader().Returns(x => { throw new ObjectDbException(); });

            // Action
            TestDelegate result = () => _dataAccess.GetMainWinloss(_metaModel);

            // Assert
            Assert.Throws<DataAccessException>(result, Arg.Is<string>(x => x.Contains("Lrf_Rpt_ExchangeWinloss")));
        }

        [Test]
        public void GetMainWinloss_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            _reader.Read().Returns(true, true, false);

            var currencyList = new[] { "USD", "EUR" };
            var expectedList = CreateExpectedResult(currencyList, DateTime.Today);
            var firstExpected = expectedList.First();

            ConfigureReaderForResult(firstExpected, expectedList);
            ConfigureReaderForMainResult(firstExpected, expectedList);

            // Action
            var result = _dataAccess.GetMainWinloss(_metaModel);
            var firstItem = result.DataList.First();

            // Assert
            Assert.Multiple(() =>
            {
                AssertMainResultData(result, firstExpected, expectedList);
                Assert.That(firstItem.ExternalId, Is.Empty);
                Assert.That(firstItem.SystemId, Is.Empty);
                Assert.That(firstItem.CusId, Is.Zero);
            });
        }

        [Test]
        public void GetMainWinloss_WhenCallWithBaseCurrencyUSD_DataConvertedToUSD()
        {
            // Arrange
            _reader.Read().Returns(true, true, false);

            var model = new ExchangeWinLossMetaModel
            {
                FromDate = DateTime.Today,
                ToDate = DateTime.Today.AddDays(1),
                AgentListXml = string.Empty,
                ReportBy = 1,
                CustId = 2,
                BaseCurrency = "USD"
            };

            var currencyList = new[] { "EUR" };
            var expectedList = CreateExpectedResult(currencyList, DateTime.Today);
            var firstExpected = expectedList.First();

            ConfigureReaderForResult(firstExpected, expectedList);
            ConfigureReaderForMainResult(firstExpected, expectedList);

            ConvertExpectedValueToBaseCurrency(expectedList, model.BaseCurrency.ToUpperInvariant());

            // Action
            var result = _dataAccess.GetMainWinloss(model);
            var firstItem = result.DataList.First();

            // Assert
            Assert.Multiple(() =>
            {
                AssertMainResultData(result, firstExpected, expectedList, model.BaseCurrency.ToUpperInvariant());
                Assert.That(firstItem.ExternalId, Is.Empty);
                Assert.That(firstItem.SystemId, Is.Empty);
                Assert.That(firstItem.CusId, Is.Zero);
            });
        }

        [Test]
        public void GetLicenseeWinloss_WhenCall_ParametersPassedCorrectly()
        {
            // Action
            _dataAccess.GetLicenseeWinloss(_metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                _dataAccess.Received().GetObjectDb("Lrf_Rpt_ExchangeWinloss_Ag");
                _objectDb.Received().AddInParameter("AgentList", DbType.String, _metaModel.AgentListXml);
                _objectDb.Received().AddInParameter("Fromdate", DbType.Date, _metaModel.FromDate.Date);
                _objectDb.Received().AddInParameter("Todate", DbType.Date, _metaModel.ToDate.Date);
            });
        }

        [Test]
        public void GetLicenseeWinloss_WhenReceiveObjectDbException_ThrowDataAccessException()
        {
            // Arrange
            _objectDb.ExecuteReader().Returns(x => { throw new ObjectDbException(); });

            // Action
            TestDelegate result = () => _dataAccess.GetLicenseeWinloss(_metaModel);

            // Assert
            Assert.Throws<DataAccessException>(result, Arg.Is<string>(x => x.Contains("Lrf_Rpt_ExchangeWinloss_Ag")));
        }

        [Test]
        public void GetLicenseeWinloss_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            _reader.Read().Returns(true, true, false);

            var currencyList = new[] { "AUD", "EUR" };
            var expectedList = CreateExpectedResult(currencyList, DateTime.Today);
            var firstExpected = expectedList.First();

            ConfigureReaderForResult(firstExpected, expectedList);
            ConfigureReaderForLicenseeResult(firstExpected, expectedList);

            // Action
            var result = _dataAccess.GetLicenseeWinloss(_metaModel);
            var firstItem = result.DataList.First();

            firstExpected.Date = _metaModel.ToDate;

            // Assert
            Assert.Multiple(() =>
            {
                AssertMainResultData(result, firstExpected, expectedList);
                Assert.That(firstItem.ExternalId, Is.Empty);
                Assert.That(firstItem.SystemId, Is.EqualTo(firstExpected.SystemId));
                Assert.That(firstItem.CusId, Is.EqualTo(firstExpected.CusId));
            });
        }

        [Test]
        public void GetCustomerWinloss_WhenCall_ParametersPassedCorrectly()
        {
            // Action
            _dataAccess.GetCustomerWinloss(_metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                _dataAccess.Received().GetObjectDb("Lrf_Rpt_ExchangeWinloss_Mem");
                _objectDb.Received().AddInParameter("CustID", DbType.Int32, _metaModel.CustId);
                _objectDb.Received().AddInParameter("AgentList", DbType.String, _metaModel.AgentListXml);
                _objectDb.Received().AddInParameter("Fromdate", DbType.Date, _metaModel.FromDate.Date);
                _objectDb.Received().AddInParameter("Todate", DbType.Date, _metaModel.ToDate.Date);
            });
        }

        [Test]
        public void GetCustomerWinloss_WhenReceiveObjectDbException_ThrowDataAccessException()
        {
            // Arrange
            _objectDb.ExecuteReader().Returns(x => { throw new ObjectDbException(); });

            // Action
            TestDelegate result = () => _dataAccess.GetCustomerWinloss(_metaModel);

            // Assert
            Assert.Throws<DataAccessException>(result, Arg.Is<string>(x => x.Contains("Lrf_Rpt_ExchangeWinloss_Mem")));
        }

        [Test]
        public void GetCustomerWinloss_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            _reader.Read().Returns(true, true, false);

            var currencyList = new[] { "AUD", "SSS" };
            var expectedList = CreateExpectedResult(currencyList, DateTime.Today);
            var firstExpected = expectedList.First();

            ConfigureReaderForResult(firstExpected, expectedList);
            ConfigureReaderForCustomerResult(firstExpected, expectedList);

            // Action
            var result = _dataAccess.GetCustomerWinloss(_metaModel);
            var firstItem = result.DataList.First();

            firstExpected.Date = _metaModel.ToDate;

            // Assert
            Assert.Multiple(() =>
            {
                AssertMainResultData(result, firstExpected, expectedList);
                Assert.That(firstItem.ExternalId, Is.EqualTo(firstExpected.ExternalId));
                Assert.That(firstItem.SystemId, Is.EqualTo(firstExpected.SystemId));
                Assert.That(firstItem.CusId, Is.Zero);
            });
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

        private void ConfigureReaderForResult(ExchangeWinLoss firstExpected, IEnumerable<ExchangeWinLoss> expectedList)
        {
            // main data
            ConfigureDataForReader(firstExpected, expectedList, string.Empty, 1);

            foreach (var currency in SystemConstants.BaseCurrencyList)
            {
                ConfigureDataForReader(firstExpected, expectedList, currency, _baseCurrencyExchangeRate[currency]);
            }
        }

        private void ConfigureDataForReader(ExchangeWinLoss firstExpected, IEnumerable<ExchangeWinLoss> expectedList, string baseCurrency, decimal rate)
        {
            _reader["CurrencyName"].Returns(firstExpected.CurrencyName, expectedList.Select(x => x.CurrencyName).ToArray());
            _reader["CurrencyID"].Returns(firstExpected.CurrencyId.ToString(), expectedList.Select(x => x.CurrencyId.ToString()).ToArray());
            _reader["TotalTrans"].Returns(firstExpected.TotalTransaction.ToString(), expectedList.Select(x => x.TotalTransaction.ToString()).ToArray());
            _reader["TurnOver" + baseCurrency].Returns(
                (firstExpected.Turnover * rate).ToString(),
                expectedList.Select(x => (x.Turnover * rate).ToString()).ToArray());
            _reader["NetTurnOver" + baseCurrency].Returns(
                (firstExpected.NetTurnover * rate).ToString(),
                expectedList.Select(x => (x.NetTurnover * rate).ToString()).ToArray());
            _reader["Winlost" + baseCurrency].Returns(
                (firstExpected.CustomerWL * rate).ToString(),
                expectedList.Select(x => (x.CustomerWL * rate).ToString()).ToArray());
            _reader["CustComm" + baseCurrency].Returns(
                (firstExpected.CustomerComm * rate).ToString(),
                expectedList.Select(x => (x.CustomerComm * rate).ToString()).ToArray());
            _reader["LicenseeWinlost" + baseCurrency].Returns(
                (firstExpected.LicenseeWL * rate).ToString(),
                expectedList.Select(x => (x.LicenseeWL * rate).ToString()).ToArray());
            _reader["AgentComm" + baseCurrency].Returns(
                (firstExpected.LicenseeComm * rate).ToString(),
                expectedList.Select(x => (x.LicenseeComm * rate).ToString()).ToArray());
            _reader["AmountDueTo" + baseCurrency].Returns(
                (firstExpected.AmountDueTo * rate).ToString(),
                expectedList.Select(x => (x.AmountDueTo * rate).ToString()).ToArray());
        }

        private void ConfigureReaderForMainResult(ExchangeWinLoss firstExpected, IEnumerable<ExchangeWinLoss> expectedList)
        {
            _reader.FieldCount.Returns(10);
            _reader.GetName(Arg.Any<int>()).Returns("Winlostdate");

            _reader["Winlostdate"].Returns(firstExpected.Date.ToString("yyyy-MM-dd"), expectedList.Select(x => x.Date.ToString("yyyy-MM-dd")).ToArray());
        }

        private void ConfigureReaderForLicenseeResult(ExchangeWinLoss firstExpected, IEnumerable<ExchangeWinLoss> expectedList)
        {
            _reader.FieldCount.Returns(10);
            _reader.GetName(Arg.Any<int>()).Returns("username", "CustID");

            _reader["CustID"].Returns(firstExpected.CusId.ToString(), expectedList.Select(x => x.CusId.ToString()).ToArray());
            _reader["username"].Returns(firstExpected.SystemId, expectedList.Select(x => x.SystemId).ToArray());
        }

        private void ConfigureReaderForCustomerResult(ExchangeWinLoss firstExpected, IEnumerable<ExchangeWinLoss> expectedList)
        {
            _reader.FieldCount.Returns(10);
            _reader.GetName(Arg.Any<int>()).Returns("username", "nickname");

            _reader["nickname"].Returns(firstExpected.ExternalId, expectedList.Select(x => x.ExternalId).ToArray());
            _reader["username"].Returns(firstExpected.SystemId, expectedList.Select(x => x.SystemId).ToArray());
        }

        private void ConvertExpectedValueToBaseCurrency(List<ExchangeWinLoss> expectedList, string baseCurrency)
        {
            foreach (var item in expectedList)
            {
                var exchangeRate = _baseCurrencyExchangeRate[baseCurrency];

                item.Turnover = item.Turnover * exchangeRate;
                item.CustomerWL = item.CustomerWL * exchangeRate;
                item.CustomerComm = item.CustomerComm * exchangeRate;
                item.LicenseeWL = item.LicenseeWL * exchangeRate;
                item.LicenseeComm = item.LicenseeComm * exchangeRate;
                item.AmountDueTo = item.AmountDueTo * exchangeRate;
                item.NetTurnover = item.NetTurnover * exchangeRate;
            }
        }

        private void AssertMainResultData(ReportResultModel<ExchangeWinLoss> result, ExchangeWinLoss firstExpected, List<ExchangeWinLoss> expectedList, string baseCurrenct = "")
        {
            var firstItem = result.DataList.First();

            Assert.That(firstItem.Date, Is.EqualTo(firstExpected.Date));
            Assert.That(firstItem.CurrencyName, Is.EqualTo(firstExpected.CurrencyName));
            Assert.That(firstItem.CurrencyId, Is.EqualTo(firstExpected.CurrencyId));

            AssertCommonResultData(firstItem, firstExpected);
            AssertTotalResultData(result, expectedList);
            AssertSummaryTotalResultData(result, expectedList, baseCurrenct);
        }

        private void AssertTotalResultData(ReportResultModel<ExchangeWinLoss> result, IEnumerable<ExchangeWinLoss> expectedList)
        {
            var expectedTotal = expectedList.GroupBy(x => new { x.CurrencyId, x.CurrencyName }).Select(g => new ExchangeWinLoss
            {
                CurrencyId = g.Key.CurrencyId,
                CurrencyName = g.Key.CurrencyName,
                Turnover = g.Sum(x => x.Turnover),
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

        private void AssertSummaryTotalResultData(ReportResultModel<ExchangeWinLoss> result, IEnumerable<ExchangeWinLoss> expectedList, string baseCurrency)
        {
            Assert.That(result.SummaryTotal.Count, Is.EqualTo(SystemConstants.BaseCurrencyList.Count));

            var exchangeBaseRate = string.IsNullOrEmpty(baseCurrency) ? 1 : _baseCurrencyExchangeRate[baseCurrency];
            foreach (var currency in SystemConstants.BaseCurrencyList)
            {
                var exchangeRate = _baseCurrencyExchangeRate[currency];

                var expected = new ExchangeWinLoss
                {
                    Turnover = expectedList.Sum(x => x.Turnover) * exchangeRate / exchangeBaseRate,
                    CustomerWL = expectedList.Sum(x => x.CustomerWL) * exchangeRate / exchangeBaseRate,
                    CustomerComm = expectedList.Sum(x => x.CustomerComm) * exchangeRate / exchangeBaseRate,
                    LicenseeWL = expectedList.Sum(x => x.LicenseeWL) * exchangeRate / exchangeBaseRate,
                    LicenseeComm = expectedList.Sum(x => x.LicenseeComm) * exchangeRate / exchangeBaseRate,
                    AmountDueTo = expectedList.Sum(x => x.AmountDueTo) * exchangeRate / exchangeBaseRate,
                    NetTurnover = expectedList.Sum(x => x.NetTurnover) * exchangeRate / exchangeBaseRate,
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
            Assert.That(actual.AmountDueTo, Is.EqualTo(expected.AmountDueTo));
        }
    }
}