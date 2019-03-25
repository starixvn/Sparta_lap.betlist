namespace Fanex.Athena.DataAccess.Report.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Core.Configuration.Interfaces;
    using Core.UnitTests.TestHelpers;
    using Model.Core.DTO.Configuration;
    using Model.Report.DTO.SubsidiaryPerformance;
    using Model.Report.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using UnitTest.Common.Helpers;

    [TestFixture]
    public class SubsidiaryPerformanceDataAccessTest : BaseDataAccessHelperTest<SubsidiaryPerformanceDataAccess>
    {
        private BalanceUp _balance;

        protected override void SetUpDataAccess()
        {
            _balance = new BalanceUp
            {
                MoveBetsDay = new DateTime(2017, 8, 25),
                BalanceUpDay = new DateTime(2017, 8, 31)
            };

            var balanceUpDataAccess = Substitute.For<IBalanceUpDataAccess>();
            balanceUpDataAccess.GetBalanceUp().Returns(_balance);
            DataAccess = Substitute.ForPartsOf<SubsidiaryPerformanceDataAccess>(
                balanceUpDataAccess);
        }

        [Test]
        public void GetData_WhenCall_PassCorrectParams()
        {
            // Arrange
            const string SpName = "Lrf_Rpt_SubsidiaryWinloss";
            DataAccess.GetObjectDb(Arg.Is(SpName)).Returns(ObjectDb);
            Reader.Read().Returns(false);

            var metaModel = GetDefaultMetaModel();

            // Action
            DataAccess.GetData(metaModel, UserCurrencyList);

            // Assert
            AssertParamsPassCorrectly(metaModel);
        }

        [Test]
        public void GetData_DoNotConvertToBasedCurrency_ReturnDataWithOriginalCurrency()
        {
            // Arrange
            const string SpName = "Lrf_Rpt_SubsidiaryWinloss";
            DataAccess.GetObjectDb(Arg.Is(SpName)).Returns(ObjectDb);
            var metaModel = GetDefaultMetaModel();
            metaModel.ConvertAll = false;

            Reader.Read().Returns(true, false);

            var readerDataMapping = GetReaderDataMapping();
            DataAccessTestHelper.MapReaderData(Reader, readerDataMapping);
            var expectedResult = ObjectHelper.ToJson(GetExpectedResult(readerDataMapping, false));

            // Action
            var result = ObjectHelper.ToJson(DataAccess.GetData(metaModel, UserCurrencyList));

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void GetData_ConvertToBasedCurrency_ReturnDataWithBasedCurrency()
        {
            // Arrange
            const string SpName = "Lrf_Rpt_SubsidiaryWinloss";
            DataAccess.GetObjectDb(Arg.Is(SpName)).Returns(ObjectDb);
            var metaModel = GetDefaultMetaModel();
            metaModel.ConvertAll = true;

            Reader.Read().Returns(true, false);

            var readerDataMapping = GetReaderDataMapping();
            DataAccessTestHelper.MapReaderData(Reader, readerDataMapping);
            var expectedResult = ObjectHelper.ToJson(GetExpectedResult(readerDataMapping, true));

            // Action
            var result = ObjectHelper.ToJson(DataAccess.GetData(metaModel, UserCurrencyList));

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void GetDetailData_WhenCall_PassCorrectParams()
        {
            // Arrange
            const string SpName = "Lrf_Rpt_SubsidiaryWinlossDetail";
            DataAccess.GetObjectDb(Arg.Is(SpName)).Returns(ObjectDb);
            Reader.Read().Returns(false);

            var metaModel = GetDefaultMetaModel();

            // Action
            DataAccess.GetDetailData(metaModel, UserCurrencyList);

            // Assert
            AssertParamsPassCorrectly(metaModel);
        }

        [Test]
        public void GetDetailData_DoNotConvertToBasedCurrency_ReturnDataWithOriginalCurrency()
        {
            // Arrange
            const string SpName = "Lrf_Rpt_SubsidiaryWinlossDetail";
            DataAccess.GetObjectDb(Arg.Is(SpName)).Returns(ObjectDb);
            var metaModel = GetDefaultMetaModel();
            metaModel.ConvertAll = false;

            Reader.Read().Returns(true, false);

            var readerDataMapping = GetReaderDetailDataMapping();
            DataAccessTestHelper.MapReaderData(Reader, readerDataMapping);

            // Action
            var result = DataAccess.GetDetailData(metaModel, UserCurrencyList);
            var firstData = result.DataList.First();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(firstData.Turnover, Is.EqualTo(Convert.ToDecimal(readerDataMapping["Turnover"])));
                Assert.That(firstData.CustomerWinLoss, Is.EqualTo(Convert.ToDecimal(readerDataMapping["Winlost"])));
                Assert.That(firstData.SubsidiaryWinLoss, Is.EqualTo(Convert.ToDecimal(readerDataMapping["LicenseeWL"])));
            });
        }

        [Test]
        public void GetDetailData_ConvertToBasedCurrency_ReturnDataWithBasedCurrency()
        {
            // Arrange
            const string SpName = "Lrf_Rpt_SubsidiaryWinlossDetail";
            DataAccess.GetObjectDb(Arg.Is(SpName)).Returns(ObjectDb);
            var metaModel = GetDefaultMetaModel();
            metaModel.ConvertAll = true;

            Reader.Read().Returns(true, false);

            var readerDataMapping = GetReaderDetailDataMapping();
            DataAccessTestHelper.MapReaderData(Reader, readerDataMapping);

            // Action
            var result = DataAccess.GetDetailData(metaModel, UserCurrencyList);
            var firstData = result.DataList.First();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(firstData.Turnover, Is.EqualTo(Convert.ToDecimal(readerDataMapping["TurnoverBase"])));
                Assert.That(firstData.CustomerWinLoss, Is.EqualTo(Convert.ToDecimal(readerDataMapping["WinlostBase"])));
                Assert.That(firstData.SubsidiaryWinLoss, Is.EqualTo(Convert.ToDecimal(readerDataMapping["LicenseeWLBase"])));
            });
        }

        [Test]
        public void GetDetailData_WhenCall_ReturnTotalDataCorrectly()
        {
            // Arrange
            const string SpName = "Lrf_Rpt_SubsidiaryWinlossDetail";
            DataAccess.GetObjectDb(Arg.Is(SpName)).Returns(ObjectDb);
            var metaModel = GetDefaultMetaModel();
            metaModel.ConvertAll = true;

            Reader.Read().Returns(true, false);

            var readerDataMapping = GetReaderDetailDataMapping();
            DataAccessTestHelper.MapReaderData(Reader, readerDataMapping);

            // Action
            var result = DataAccess.GetDetailData(metaModel, UserCurrencyList);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.SummaryTotal.ContainsKey(CurrencyName), Is.True);
                Assert.That(result.TotalData.CurrencyName, Is.EqualTo(metaModel.BaseCurrency.ToUpperInvariant()));
            });
        }

        private static List<SubsidiaryPerformance> GetExpectedResult(IDictionary<string, object> readerDataMapping, bool isConvertAll)
        {
            var baseCurrencyColumn = isConvertAll ? "Base" : string.Empty;
            return new List<SubsidiaryPerformance>
            {
                new SubsidiaryPerformance
                {
                    ProductCode = readerDataMapping["ProductCode"].ToString(),
                    SubsidiaryId = Convert.ToInt32(readerDataMapping["recommend"]),
                    CurrencyId = Convert.ToInt32(readerDataMapping["Currency"]),
                    CurrencyName = CurrencyName,
                    BetCount = Convert.ToInt32(readerDataMapping["BetCount"]),
                    Turnover = Convert.ToDecimal(readerDataMapping["Turnover" + baseCurrencyColumn]),
                    CustomerWinLoss = Convert.ToDecimal(readerDataMapping["Winlost" + baseCurrencyColumn]),
                    SubsidiaryWinLoss = Convert.ToDecimal(readerDataMapping["LicenseeWL" + baseCurrencyColumn]),
                    BaseCurrencyData = new SubsidiaryPerformanceBase
                    {
                        BetCount = Convert.ToInt32(readerDataMapping["BetCount"]),
                        Turnover = Convert.ToDecimal(readerDataMapping["TurnoverBase"]),
                        CustomerWinLoss = Convert.ToDecimal(readerDataMapping["WinlostBase"]),
                        SubsidiaryWinLoss = Convert.ToDecimal(readerDataMapping["LicenseeWLBase"])
                    }
                }
            };
        }

        private static IDictionary<string, object> GetReaderDataMapping()
        {
            return new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase)
            {
                { "recommend", 1234 },
                { "Currency", CurrencyId },
                { "ProductCode", "SB" },
                { "BetCount", 10 },
                { "Turnover", 10000m },
                { "Winlost", 5000m },
                { "LicenseeWL", 2000m },
                { "TurnoverBase", 20001m },
                { "WinlostBase", 50001m },
                { "LicenseeWLBase", 20001m }
            };
        }

        private static IDictionary<string, object> GetReaderDetailDataMapping()
        {
            return new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase)
            {
                { "SystemId", "123" },
                { "ExternalId", "456" },
                { "Currency", CurrencyId },
                { "ProductCode", "SB" },
                { "BetCount", 10 },
                { "Turnover", 10000m },
                { "Winlost", 5000m },
                { "LicenseeWL", 2000m },
                { "TurnoverBase", 20001m },
                { "WinlostBase", 50001m },
                { "LicenseeWLBase", 20001m }
            };
        }

        private static SubsidiaryPerformanceMetaModel GetDefaultMetaModel()
        {
            return new SubsidiaryPerformanceMetaModel
            {
                BaseCurrency = "EUR",
                ConvertAll = true,
                FromDate = new DateTime(2017, 8, 25),
                ToDate = new DateTime(2017, 8, 31),
                ProductCodeList = "SB,BA",
                AgentListXml = "<Param></Param>"
            };
        }

        private void AssertParamsPassCorrectly(SubsidiaryPerformanceMetaModel metaModel)
        {
            ObjectDb.Received(1).SetParameterValue("FromDate", metaModel.FromDate);
            ObjectDb.Received(1).SetParameterValue("ToDate", metaModel.ToDate);
            ObjectDb.Received(1).SetParameterValue("MoveBetsDay", _balance.MoveBetsDay);
            ObjectDb.Received(1).SetParameterValue("ProductList", metaModel.ProductCodeList);
            ObjectDb.Received(1).SetParameterValue("AgentList", metaModel.AgentListXml);
            ObjectDb.Received(1).SetParameterValue("IncludeRaw", metaModel.IncludeRaw);
            ObjectDb.Received(1).SetParameterValue("BaseCurrency", metaModel.BaseCurrency);
        }
    }
}