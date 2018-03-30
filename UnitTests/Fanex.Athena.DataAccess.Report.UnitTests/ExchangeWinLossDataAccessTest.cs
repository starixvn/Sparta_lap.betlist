namespace Fanex.Athena.DataAccess.Report
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Core.UnitTests.TestHelpers;
    using Model.Core.DataDomain;
    using Model.Core.ResultModel;
    using Model.Report.DTO;
    using Model.Report.MetaModel;
    using NSubstitute;
    using NUnit.Framework;

    public class ExchangeWinLossDataAccessTest : BaseDataAccessHelperTest<ExchangeWinLossDataAccess>
    {
        private const string GetMainWinlossSp = "Lrf_Rpt_ExchangeWinloss";

        private const string GetCustomerWinlossSp = "Lrf_Rpt_ExchangeWinloss_Mem";

        private const string GetLicenseeWinlossSp = "Lrf_Rpt_ExchangeWinloss_Ag";

        private ExchangeWinLossMetaModel _metaModel;

        private IDictionary<string, decimal> _baseCurrencyExchangeRate;

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();

            _metaModel = new ExchangeWinLossMetaModel
            {
                FromDate = DateTime.Today,
                ToDate = DateTime.Today.AddDays(1),
                AgentListXml = string.Empty,
                ReportBy = 1,
                CustId = 2,
                LicenseeId = 3
            };

            _baseCurrencyExchangeRate = new Dictionary<string, decimal>();
            for (int i = 0; i < SystemConstants.BaseCurrencyList.Count; i++)
            {
                _baseCurrencyExchangeRate.Add(SystemConstants.BaseCurrencyList[i], (i + 1) * 0.1m);
            }

            DataAccess.GetObjectDb(Arg.Is<string>(x => x.Contains(GetMainWinlossSp))).Returns(ObjectDb);
        }

        [Test]
        public void GetMainWinloss_WhenCall_ParametersPassedCorrectly()
        {
            // Action
            DataAccess.GetMainWinloss(_metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                DataAccess.Received(1).GetObjectDb(GetMainWinlossSp);
                ObjectDb.Received(1).AddInParameter("ReportType", DbType.Byte, _metaModel.ReportBy);
                AssertCommonSpParameters();
            });
        }

        [Test]
        public void GetMainWinloss_WhenReceiveObjectDbException_ThrowDataAccessException()
        {
            TestException_WhenObjectDbThrowsException_ThrowDataAccessException(
                GetMainWinlossSp,
                () => DataAccess.GetMainWinloss(_metaModel));
        }

        [Test]
        public void GetMainWinloss_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            Reader.Read().Returns(true, true, false);

            var currencyList = new[] { "USD", "EUR" };
            var expectedList = CreateExpectedResult(currencyList, DateTime.Today);
            var firstExpected = expectedList.First();

            ConfigureReaderForResult(firstExpected, expectedList);
            ConfigureReaderForMainResult(firstExpected, expectedList);

            // Action
            var result = DataAccess.GetMainWinloss(_metaModel);
            var firstItem = result.DataList.First();

            // Assert
            Assert.Multiple(() =>
            {
                AssertMainResultData(result, firstExpected, expectedList);
                Assert.That(firstItem.ExternalId, Is.Empty);
                Assert.That(firstItem.SystemId, Is.Empty);
                Assert.That(firstItem.CustId, Is.Zero);
            });
        }

        [Test]
        public void GetMainWinloss_WhenCallWithBaseCurrencyUSD_DataConvertedToUSD()
        {
            // Arrange
            Reader.Read().Returns(true, false, false);

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
            var result = DataAccess.GetMainWinloss(model);
            var firstItem = result.DataList.First();

            // Assert
            Assert.Multiple(() =>
            {
                AssertMainResultData(result, firstExpected, expectedList, model.BaseCurrency.ToUpperInvariant());
                Assert.That(firstItem.ExternalId, Is.Empty);
                Assert.That(firstItem.SystemId, Is.Empty);
                Assert.That(firstItem.CustId, Is.Zero);
            });
        }

        [Test]
        public void GetLicenseeWinloss_WhenCall_ParametersPassedCorrectly()
        {
            // Action
            DataAccess.GetLicenseeWinloss(_metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                DataAccess.Received(1).GetObjectDb(GetLicenseeWinlossSp);
                AssertCommonSpParameters();
            });
        }

        [Test]
        public void GetLicenseeWinloss_WhenReceiveObjectDbException_ThrowDataAccessException()
        {
            TestException_WhenObjectDbThrowsException_ThrowDataAccessException(
                GetLicenseeWinlossSp,
                () => DataAccess.GetLicenseeWinloss(_metaModel));
        }

        [Test]
        public void GetLicenseeWinloss_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            Reader.Read().Returns(true, true, false);

            var currencyList = new[] { "AUD", "EUR" };
            var expectedList = CreateExpectedResult(currencyList, DateTime.Today);
            var firstExpected = expectedList.First();

            ConfigureReaderForResult(firstExpected, expectedList);
            ConfigureReaderForLicenseeResult(firstExpected, expectedList);

            // Action
            var result = DataAccess.GetLicenseeWinloss(_metaModel);
            var firstItem = result.DataList.First();

            firstExpected.Date = _metaModel.ToDate;

            // Assert
            Assert.Multiple(() =>
            {
                AssertMainResultData(result, firstExpected, expectedList);
                Assert.That(firstItem.ExternalId, Is.Empty);
                Assert.That(firstItem.SystemId, Is.EqualTo(firstExpected.SystemId));
                Assert.That(firstItem.CustId, Is.EqualTo(firstExpected.CustId));
            });
        }

        [Test]
        public void GetCustomerWinloss_WhenCall_ParametersPassedCorrectly()
        {
            // Action
            DataAccess.GetCustomerWinloss(_metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                DataAccess.Received(1).GetObjectDb(GetCustomerWinlossSp);
                ObjectDb.Received(1).AddInParameter("CustID", DbType.Int32, _metaModel.CustId);
                AssertCommonSpParameters();
            });
        }

        [Test]
        public void GetCustomerWinloss_WhenReceiveObjectDbException_ThrowDataAccessException()
        {
            TestException_WhenObjectDbThrowsException_ThrowDataAccessException(
                GetCustomerWinlossSp,
                () => DataAccess.GetCustomerWinloss(_metaModel));
        }

        [Test]
        public void GetCustomerWinloss_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            Reader.Read().Returns(true, true, false);

            var currencyList = new[] { "AUD", "SSS" };
            var expectedList = CreateExpectedResult(currencyList, DateTime.Today);
            var firstExpected = expectedList.First();

            ConfigureReaderForResult(firstExpected, expectedList);
            ConfigureReaderForCustomerResult(firstExpected, expectedList);

            // Action
            var result = DataAccess.GetCustomerWinloss(_metaModel);
            var firstItem = result.DataList.First();

            firstExpected.Date = _metaModel.ToDate;

            // Assert
            Assert.Multiple(() =>
            {
                AssertMainResultData(result, firstExpected, expectedList);
                Assert.That(firstItem.ExternalId, Is.EqualTo(firstExpected.ExternalId));
                Assert.That(firstItem.SystemId, Is.EqualTo(firstExpected.SystemId));
                Assert.That(firstItem.CustId, Is.Zero);
            });
        }

        private static List<ExchangeWinLoss> CreateExpectedResult(string[] currencyList, DateTime date)
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
                        CustId = i + 1,
                        Turnover = i + 2,
                        NetTurnover = i + 3,
                        CustomerWL = i + 4,
                        CustomerComm = i + 5,
                        LicenseeWL = i + 6,
                        LicenseeNetWL = i + 7,
                        LicenseeComm = i + 8,
                        TotalTransaction = i + 8,
                        AmountDueTo = i + 10
                    });
            }

            return expectedResult;
        }

        private static void AssertTotalResultData(ReportResultModel<ExchangeWinLoss> result, IEnumerable<ExchangeWinLoss> expectedList)
        {
            var expectedTotal = expectedList.GroupBy(x => new { x.CurrencyId, x.CurrencyName }).Select(g => new ExchangeWinLoss
            {
                CurrencyId = g.Key.CurrencyId,
                CurrencyName = g.Key.CurrencyName,
                Turnover = g.Sum(x => x.Turnover),
                CustomerWL = g.Sum(x => x.CustomerWL),
                CustomerComm = g.Sum(x => x.CustomerComm),
                LicenseeWL = g.Sum(x => x.LicenseeWL),
                LicenseeNetWL = g.Sum(x => x.LicenseeNetWL),
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

        private static void AssertCommonResultData(ExchangeWinLoss actual, ExchangeWinLoss expected)
        {
            Assert.That(actual.Turnover, Is.EqualTo(expected.Turnover));
            Assert.That(actual.NetTurnover, Is.EqualTo(expected.NetTurnover));
            Assert.That(actual.CustomerWL, Is.EqualTo(expected.CustomerWL));
            Assert.That(actual.CustomerComm, Is.EqualTo(expected.CustomerComm));
            Assert.That(actual.LicenseeWL, Is.EqualTo(expected.LicenseeWL));
            Assert.That(actual.LicenseeNetWL, Is.EqualTo(expected.LicenseeNetWL));
            Assert.That(actual.LicenseeComm, Is.EqualTo(expected.LicenseeComm));
            Assert.That(actual.AmountDueTo, Is.EqualTo(expected.AmountDueTo));
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
            const string DecimalFormat = "G";

            Reader["CurrencyName"].Returns(firstExpected.CurrencyName, expectedList.Select(x => x.CurrencyName).ToArray());
            Reader["CurrencyID"].Returns(firstExpected.CurrencyId.ToString(), expectedList.Select(x => x.CurrencyId.ToString()).ToArray());
            Reader["TotalTrans"].Returns(firstExpected.TotalTransaction.ToString(), expectedList.Select(x => x.TotalTransaction.ToString()).ToArray());
            Reader["TurnOver" + baseCurrency].Returns(
                (firstExpected.Turnover * rate).ToString(DecimalFormat),
                expectedList.Select(x => (x.Turnover * rate).ToString(DecimalFormat)).ToArray());
            Reader["NetTurnOver" + baseCurrency].Returns(
                (firstExpected.NetTurnover * rate).ToString(DecimalFormat),
                expectedList.Select(x => (x.NetTurnover * rate).ToString(DecimalFormat)).ToArray());
            Reader["Winlost" + baseCurrency].Returns(
                (firstExpected.CustomerWL * rate).ToString(DecimalFormat),
                expectedList.Select(x => (x.CustomerWL * rate).ToString(DecimalFormat)).ToArray());
            Reader["CustComm" + baseCurrency].Returns(
                (firstExpected.CustomerComm * rate).ToString(DecimalFormat),
                expectedList.Select(x => (x.CustomerComm * rate).ToString(DecimalFormat)).ToArray());
            Reader["LicenseeWinlost" + baseCurrency].Returns(
                (firstExpected.LicenseeWL * rate).ToString(DecimalFormat),
                expectedList.Select(x => (x.LicenseeWL * rate).ToString(DecimalFormat)).ToArray());
            Reader["LicNetWL" + baseCurrency].Returns(
                (firstExpected.LicenseeNetWL * rate).ToString(DecimalFormat),
                expectedList.Select(x => (x.LicenseeNetWL * rate).ToString(DecimalFormat)).ToArray());
            Reader["AgentComm" + baseCurrency].Returns(
                (firstExpected.LicenseeComm * rate).ToString(DecimalFormat),
                expectedList.Select(x => (x.LicenseeComm * rate).ToString(DecimalFormat)).ToArray());
            Reader["AmountDueTo" + baseCurrency].Returns(
                (firstExpected.AmountDueTo * rate).ToString(DecimalFormat),
                expectedList.Select(x => (x.AmountDueTo * rate).ToString(DecimalFormat)).ToArray());
        }

        private void ConfigureReaderForMainResult(ExchangeWinLoss firstExpected, IEnumerable<ExchangeWinLoss> expectedList)
        {
            Reader.FieldCount.Returns(10);
            Reader.GetName(Arg.Any<int>()).Returns("Winlostdate");

            Reader["Winlostdate"].Returns(firstExpected.Date.ToString("yyyy-MM-dd"), expectedList.Select(x => x.Date.ToString("yyyy-MM-dd")).ToArray());
        }

        private void ConfigureReaderForLicenseeResult(ExchangeWinLoss firstExpected, IEnumerable<ExchangeWinLoss> expectedList)
        {
            Reader.FieldCount.Returns(10);
            Reader.GetName(Arg.Any<int>()).Returns("username", "CustID");

            Reader["CustID"].Returns(firstExpected.CustId.ToString(), expectedList.Select(x => x.CustId.ToString()).ToArray());
            Reader["username"].Returns(firstExpected.SystemId, expectedList.Select(x => x.SystemId).ToArray());
        }

        private void ConfigureReaderForCustomerResult(ExchangeWinLoss firstExpected, IEnumerable<ExchangeWinLoss> expectedList)
        {
            Reader.FieldCount.Returns(10);
            Reader.GetName(Arg.Any<int>()).Returns("username", "nickname");

            Reader["nickname"].Returns(firstExpected.ExternalId, expectedList.Select(x => x.ExternalId).ToArray());
            Reader["username"].Returns(firstExpected.SystemId, expectedList.Select(x => x.SystemId).ToArray());
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
                item.LicenseeNetWL = item.LicenseeNetWL * exchangeRate;
                item.LicenseeComm = item.LicenseeComm * exchangeRate;
                item.AmountDueTo = item.AmountDueTo * exchangeRate;
                item.NetTurnover = item.NetTurnover * exchangeRate;
            }
        }

        private void AssertMainResultData(
            ReportResultModel<ExchangeWinLoss> result,
            ExchangeWinLoss firstExpected,
            List<ExchangeWinLoss> expectedList,
            string baseCurrenct = "")
        {
            var firstItem = result.DataList.First();

            Assert.That(firstItem.Date, Is.EqualTo(firstExpected.Date));
            Assert.That(firstItem.CurrencyName, Is.EqualTo(firstExpected.CurrencyName));
            Assert.That(firstItem.CurrencyId, Is.EqualTo(firstExpected.CurrencyId));

            AssertCommonResultData(firstItem, firstExpected);
            AssertTotalResultData(result, expectedList);
            AssertSummaryTotalResultData(result, expectedList, baseCurrenct);
        }

        private void AssertCommonSpParameters()
        {
            ObjectDb.Received(1).AddInParameter("AgentList", DbType.String, _metaModel.AgentListXml);
            ObjectDb.Received(1).AddInParameter("Fromdate", DbType.Date, _metaModel.FromDate.Date);
            ObjectDb.Received(1).AddInParameter("Todate", DbType.Date, _metaModel.ToDate.Date);
            ObjectDb.Received().AddInParameter("LicenseeId", DbType.Int32, _metaModel.LicenseeId);
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
                    LicenseeNetWL = expectedList.Sum(x => x.LicenseeNetWL) * exchangeRate / exchangeBaseRate,
                    LicenseeComm = expectedList.Sum(x => x.LicenseeComm) * exchangeRate / exchangeBaseRate,
                    AmountDueTo = expectedList.Sum(x => x.AmountDueTo) * exchangeRate / exchangeBaseRate,
                    NetTurnover = expectedList.Sum(x => x.NetTurnover) * exchangeRate / exchangeBaseRate,
                };

                var actual = result.SummaryTotal[currency];

                AssertCommonResultData(actual, expected);
            }
        }
    }
}