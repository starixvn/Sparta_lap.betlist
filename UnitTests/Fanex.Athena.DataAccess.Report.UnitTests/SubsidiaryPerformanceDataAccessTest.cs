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

    [TestFixture]
    public class SubsidiaryPerformanceDataAccessTest : BaseDataAccessHelperTest<SubsidiaryPerformanceDataAccess>
    {
        private IBalanceUpDataAccess _balanceUpDataAccess;

        private BalanceUp _balance;

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            _balance = new BalanceUp
            {
                MoveBetsDay = new DateTime(2017, 8, 25),
                BalanceUpDay = new DateTime(2017, 8, 31)
            };

            _balanceUpDataAccess = Substitute.For<IBalanceUpDataAccess>();
            _balanceUpDataAccess.GetBalanceUp().Returns(_balance);
            DataAccess = Substitute.ForPartsOf<SubsidiaryPerformanceDataAccess>(_balanceUpDataAccess);

            ObjectDb = Substitute.For<IObjectDb>();

            const string SpName = "Lrf_Rpt_SubsidiaryWinloss";
            DataAccess.GetObjectDb(Arg.Is(SpName)).Returns(ObjectDb);
        }

        [OneTimeTearDown]
        public override void FixtureTearDown()
        {
            _balanceUpDataAccess = null;
            _balance = null;

            base.FixtureTearDown();
        }

        [Test]
        public void GetData_WhenCall_PassCorrectParams()
        {
            // Arrange
            Reader.Read().Returns(false);

            var metaModel = GetDefaultMetaModel();
            IEnumerable<SubsidiaryPerformanceEntity> totalList = null;

            // Action
            DataAccess.GetData(metaModel, out totalList);

            // Assert
            AssertParamsPassCorrectly(metaModel);
        }

        [Test]
        public void GetData_DoNotConvertToBasedCurrency_ReturnDataWithOriginalCurrency()
        {
            // Arrange
            var metaModel = GetDefaultMetaModel();
            metaModel.ConvertAll = false;

            Reader.Read().Returns(true, false);

            var readerDataMapping = GetReaderDataMapping(metaModel.BaseCurrency);
            MapReaderData(readerDataMapping);

            var serializer = new JavaScriptSerializer();
            var expectedResult = serializer.Serialize(GetExpectedResult(readerDataMapping));
            IEnumerable<SubsidiaryPerformanceEntity> totalList = null;

            // Action
            var result = serializer.Serialize(DataAccess.GetData(metaModel, out totalList));

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void GetData_ConvertToBasedCurrency_ReturnDataWithBasedCurrency()
        {
            // Arrange
            var metaModel = GetDefaultMetaModel();
            metaModel.ConvertAll = true;

            Reader.Read().Returns(true, false);

            var readerDataMapping = GetReaderDataMapping(metaModel.BaseCurrency);
            MapReaderData(readerDataMapping);

            var serializer = new JavaScriptSerializer();
            var expectedResult = serializer.Serialize(GetExpectedResult(readerDataMapping, metaModel.BaseCurrency));
            IEnumerable<SubsidiaryPerformanceEntity> totalList = null;

            // Action
            var result = serializer.Serialize(DataAccess.GetData(metaModel, out totalList));

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void GetData_WhenCall_ReturnCorrectTotalDataList()
        {
            // Arrange
            IEnumerable<SubsidiaryPerformanceEntity> totalList = null;
            var metaModel = GetDefaultMetaModel();
            metaModel.ConvertAll = false;

            Reader.Read().Returns(true, false);

            var readerDataMapping = GetReaderDataMapping(metaModel.BaseCurrency);
            MapReaderData(readerDataMapping);

            var serializer = new JavaScriptSerializer();
            var total = GetExpectedResult(readerDataMapping, metaModel.BaseCurrency);
            var expectedResult = serializer.Serialize(total);

            // Action
            DataAccess.GetData(metaModel, out totalList);
            var result = serializer.Serialize(totalList);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        private static List<SubsidiaryPerformanceEntity> GetExpectedResult(IDictionary<string, object> readerDataMapping, string baseCurrency = "")
        {
            return new List<SubsidiaryPerformanceEntity>
            {
                new SubsidiaryPerformanceEntity
                {
                    SubsidiaryId = Convert.ToInt32(readerDataMapping["recommend"]),
                    CurrencyId = Convert.ToInt32(readerDataMapping["Currency"]),
                    CurrencyName = readerDataMapping["CurrencyName"].ToString(),
                    Product = readerDataMapping["ProductCode"].ToString(),
                    BetCount = Convert.ToInt32(readerDataMapping["BetCount"]),
                    Turnover = Convert.ToDecimal(readerDataMapping["Turnover" + baseCurrency]),
                    CustomerWinLoss = Convert.ToDecimal(readerDataMapping["Winlost" + baseCurrency]),
                    SubsidiaryWinLoss = Convert.ToDecimal(readerDataMapping["LicenseeWL" + baseCurrency])
                }
            };
        }

        private static IDictionary<string, object> GetReaderDataMapping(string baseCurrency)
        {
            return new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase)
            {
                { "recommend", 1234 },
                { "Currency", 1 },
                { "CurrencyName", "EUR" },
                { "ProductCode", "SB" },
                { "BetCount", 10 },
                { "Turnover", 10000m },
                { "Winlost", 5000m },
                { "LicenseeWL", 2000m },
                { "Turnover" + baseCurrency, 20000m },
                { "Winlost" + baseCurrency, 50000m },
                { "LicenseeWL" + baseCurrency, 20000m }
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
            ObjectDb.Received().AddInParameter("FromDate", DbType.DateTime, metaModel.FromDate);
            ObjectDb.Received().AddInParameter("ToDate", DbType.DateTime, metaModel.ToDate);
            ObjectDb.Received().AddInParameter("MoveBetsDay", DbType.DateTime, _balance.MoveBetsDay);
            ObjectDb.Received().AddInParameter("ProductList", DbType.String, metaModel.ProductCodeList);
            ObjectDb.Received().AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
        }
    }
}