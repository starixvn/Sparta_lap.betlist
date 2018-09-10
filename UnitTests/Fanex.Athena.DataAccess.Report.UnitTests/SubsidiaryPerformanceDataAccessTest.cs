namespace Fanex.Athena.DataAccess.Report.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Web.Script.Serialization;
    using Core.Configuration.Interfaces;
    using Core.UnitTests.TestHelpers;
    using Data;
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

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            _balance = new BalanceUp
            {
                MoveBetsDay = new DateTime(2017, 8, 25),
                BalanceUpDay = new DateTime(2017, 8, 31)
            };

            ObjectDb = Substitute.For<IObjectDb>();

            var balanceUpDataAccess = Substitute.For<IBalanceUpDataAccess>();
            balanceUpDataAccess.GetBalanceUp().Returns(_balance);
            DataAccess = Substitute.ForPartsOf<SubsidiaryPerformanceDataAccess>(
                CreateSessionStorage(),
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
            DataAccess.GetData(metaModel);

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

            var readerDataMapping = GetReaderDataMapping(metaModel.BaseCurrency);
            DataAccessTestHelper.MapReaderData(Reader, readerDataMapping);
            var expectedResult = ObjectHelper.ToJson(GetExpectedResult(readerDataMapping));

            // Action
            var result = ObjectHelper.ToJson(DataAccess.GetData(metaModel));

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

            var readerDataMapping = GetReaderDataMapping(metaModel.BaseCurrency);
            DataAccessTestHelper.MapReaderData(Reader, readerDataMapping);
            var expectedResult = ObjectHelper.ToJson(GetExpectedResult(readerDataMapping, metaModel.BaseCurrency));

            // Action
            var result = ObjectHelper.ToJson(DataAccess.GetData(metaModel));

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void GetData_WhenCall_ReturnCorrectTotalDataList()
        {
            // Arrange
            const string SpName = "Lrf_Rpt_SubsidiaryWinloss";
            DataAccess.GetObjectDb(Arg.Is(SpName)).Returns(ObjectDb);
            var metaModel = GetDefaultMetaModel();
            metaModel.ConvertAll = false;

            Reader.Read().Returns(true, false);

            var readerDataMapping = GetReaderDataMapping(metaModel.BaseCurrency);
            DataAccessTestHelper.MapReaderData(Reader, readerDataMapping);

            var total = GetExpectedResult(readerDataMapping, metaModel.BaseCurrency);
            var expectedResult = ObjectHelper.ToJson(total);

            // Action
            var dataResult = DataAccess.GetData(metaModel);
            var result = ObjectHelper.ToJson(dataResult);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        private static List<SubsidiaryPerformance> GetExpectedResult(IDictionary<string, object> readerDataMapping, string baseCurrency = "")
        {
            return new List<SubsidiaryPerformance>
            {
                new SubsidiaryPerformance
                {
                    SubsidiaryId = Convert.ToInt32(readerDataMapping["recommend"]),
                    CurrencyId = Convert.ToInt32(readerDataMapping["Currency"]),
                    CurrencyName = CurrencyName,
                    Product = readerDataMapping["ProductCode"].ToString(),
                    BetCount = Convert.ToInt32(readerDataMapping["BetCount"]),
                    Turnover = Convert.ToDecimal(readerDataMapping["Turnover" + baseCurrency]),
                    CustomerWinLoss = Convert.ToDecimal(readerDataMapping["Winlost" + baseCurrency]),
                    SubsidiaryWinLoss = Convert.ToDecimal(readerDataMapping["LicenseeWL" + baseCurrency]),
                    SubsidiaryComm = Convert.ToDecimal(readerDataMapping["SubsidiaryComm" + baseCurrency])
                }
            };
        }

        private static IDictionary<string, object> GetReaderDataMapping(string baseCurrency)
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
                { "SubsidiaryComm", 10m },
                { "Turnover" + baseCurrency, 20000m },
                { "Winlost" + baseCurrency, 50000m },
                { "LicenseeWL" + baseCurrency, 20000m },
                { "SubsidiaryComm" + baseCurrency, 10m }
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
            ObjectDb.DidNotReceive().AddInParameter("BaseCurrency", Arg.Any<DbType>(), Arg.Any<object>());
            ObjectDb.DidNotReceive().AddInParameter("ConvertAll", Arg.Any<DbType>(), Arg.Any<object>());
            ObjectDb.Received(1).AddInParameter("FromDate", DbType.DateTime, metaModel.FromDate);
            ObjectDb.Received(1).AddInParameter("ToDate", DbType.DateTime, metaModel.ToDate);
            ObjectDb.Received(1).AddInParameter("MoveBetsDay", DbType.DateTime, _balance.MoveBetsDay);
            ObjectDb.Received(1).AddInParameter("ProductList", DbType.String, metaModel.ProductCodeList);
            ObjectDb.Received(1).AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
        }
    }
}