namespace Fanex.Athena.DataAccess.Report.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Web.Script.Serialization;
    using Core.Configuration.Interfaces;
    using Data;
    using Model.Core.DTO.Configuration;
    using Model.Report.DTO;
    using Model.Report.DTO.SubsidiaryPerformance;
    using Model.Report.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using UnitTest.Common.Helpers;

    [TestFixture]
    public class SubsidiaryPerformanceDataAccessTest
    {
        private SubsidiaryPerformanceDataAccess _dataAccess;
        private IBalanceUpDataAccess _balanceUpDataAccess;
        private IObjectDb _objectDb;
        private IDataReader _dataReader;
        private BalanceUp _balance;
        private JavaScriptSerializer _serialize;

        [SetUp]
        public void SetUp()
        {
            _balance = new BalanceUp
            {
                MoveBetsDay = new DateTime(2017, 8, 25),
                BalanceUpDay = new DateTime(2017, 8, 31)
            };

            _balanceUpDataAccess = Substitute.For<IBalanceUpDataAccess>();
            _balanceUpDataAccess.GetBalanceUp().Returns(_balance);
            _dataAccess = Substitute.ForPartsOf<SubsidiaryPerformanceDataAccess>(_balanceUpDataAccess);

            _objectDb = Substitute.For<IObjectDb>();
            _dataReader = Substitute.For<IDataReader>();
            _serialize = new JavaScriptSerializer();
        }

        [TearDown]
        public void TearDown()
        {
            _balanceUpDataAccess = null;
            _balance = null;
            _dataAccess = null;
            _objectDb = null;
        }

        [Test]
        public void GetOperatorList_WhenCall_PassCorrectParams()
        {
            // Arrange
            const string SpName = "Lrf_SubsidiarySel";
            _dataAccess.GetObjectDb(Arg.Is<string>(SpName)).Returns(_objectDb);
            _objectDb.ExecuteReader().Returns(_dataReader);
            _dataReader.Read().Returns(false);

            const string LisenseeName = "12Bet";

            // Action
            _dataAccess.GetOperatorList(LisenseeName);

            // Assert
            _objectDb.Received().AddInParameter("LicenseeName", DbType.String, LisenseeName);
        }

        [Test]
        public void GetOperatorList_WhenCall_ReturnCorrectData()
        {
            // Arrange
            const string SpName = "Lrf_SubsidiarySel";
            _dataAccess.GetObjectDb(Arg.Is<string>(SpName)).Returns(_objectDb);
            _objectDb.ExecuteReader().Returns(_dataReader);
            _dataReader.Read().Returns(true, false);

            const string LisenseeName = "12Bet";
            const int AgentId = 1234;
            const int CurrencyId = 13;
            const string OperatorName = "India";

            var readerDataMapping = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase)
            {
                { "AgentID", AgentId },
                { "OperatorID", OperatorName },
                { "Currency", CurrencyId }
            };
            DataAccessTestHelper.MapReaderData(_dataReader, readerDataMapping);

            var expectedData = new List<Operator>
            {
                new Operator
                {
                    AgentId = AgentId,
                    OperatorName = OperatorName,
                    CurrencyId = CurrencyId
                }
            };
            var expectedResult = _serialize.Serialize(expectedData);

            // Action
            var result = _serialize.Serialize(_dataAccess.GetOperatorList(LisenseeName));

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void GetData_WhenCall_PassCorrectParams()
        {
            // Arrange
            const string SpName = "Lrf_Rpt_SubsidiaryWinloss";
            _dataAccess.GetObjectDb(Arg.Is<string>(SpName)).Returns(_objectDb);
            _objectDb.ExecuteReader().Returns(_dataReader);
            _dataReader.Read().Returns(false);

            var metaModel = GetDefaultMetaModel();
            IEnumerable<SubsidiaryPerformanceEntity> totalList = null;

            // Action
            _dataAccess.GetData(metaModel, out totalList);

            // Assert
            AssertParamsPassCorrectly(metaModel);
        }

        [Test]
        public void GetData_DoNotConvertToBasedCurrency_ReturnDataWithOriginalCurrency()
        {
            // Arrange
            const string SpName = "Lrf_Rpt_SubsidiaryWinloss";
            _dataAccess.GetObjectDb(Arg.Is<string>(SpName)).Returns(_objectDb);
            _objectDb.ExecuteReader().Returns(_dataReader);
            var metaModel = GetDefaultMetaModel();
            metaModel.ConvertAll = false;

            _dataReader.Read().Returns(true, false);

            var readerDataMapping = GetReaderDataMapping(metaModel.BaseCurrency);
            DataAccessTestHelper.MapReaderData(_dataReader, readerDataMapping);

            var serializer = new JavaScriptSerializer();
            var expectedResult = serializer.Serialize(GetExpectedResult(readerDataMapping));
            IEnumerable<SubsidiaryPerformanceEntity> totalList = null;

            // Action
            var result = serializer.Serialize(_dataAccess.GetData(metaModel, out totalList));

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void GetData_ConvertToBasedCurrency_ReturnDataWithBasedCurrency()
        {
            // Arrange
            const string SpName = "Lrf_Rpt_SubsidiaryWinloss";
            _dataAccess.GetObjectDb(Arg.Is<string>(SpName)).Returns(_objectDb);
            _objectDb.ExecuteReader().Returns(_dataReader);
            var metaModel = GetDefaultMetaModel();
            metaModel.ConvertAll = true;

            _dataReader.Read().Returns(true, false);

            var readerDataMapping = GetReaderDataMapping(metaModel.BaseCurrency);
            DataAccessTestHelper.MapReaderData(_dataReader, readerDataMapping);

            var serializer = new JavaScriptSerializer();
            var expectedResult = serializer.Serialize(GetExpectedResult(readerDataMapping, metaModel.BaseCurrency));
            IEnumerable<SubsidiaryPerformanceEntity> totalList = null;

            // Action
            var result = serializer.Serialize(_dataAccess.GetData(metaModel, out totalList));

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void GetData_WhenCall_ReturnCorrectTotalDataList()
        {
            // Arrange
            const string SpName = "Lrf_Rpt_SubsidiaryWinloss";
            _dataAccess.GetObjectDb(Arg.Is<string>(SpName)).Returns(_objectDb);
            _objectDb.ExecuteReader().Returns(_dataReader);
            IEnumerable<SubsidiaryPerformanceEntity> totalList = null;
            var metaModel = GetDefaultMetaModel();
            metaModel.ConvertAll = false;

            _dataReader.Read().Returns(true, false);

            var readerDataMapping = GetReaderDataMapping(metaModel.BaseCurrency);
            DataAccessTestHelper.MapReaderData(_dataReader, readerDataMapping);

            var serializer = new JavaScriptSerializer();
            var total = GetExpectedResult(readerDataMapping, metaModel.BaseCurrency);
            var expectedResult = serializer.Serialize(total);

            // Action
            _dataAccess.GetData(metaModel, out totalList);
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
                { "Currency", 1 },
                { "CurrencyName", "EUR" },
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
            _objectDb.DidNotReceive().AddInParameter("BaseCurrency", Arg.Any<DbType>(), Arg.Any<object>());
            _objectDb.DidNotReceive().AddInParameter("ConvertAll", Arg.Any<DbType>(), Arg.Any<object>());
            _objectDb.Received().AddInParameter("FromDate", DbType.DateTime, metaModel.FromDate);
            _objectDb.Received().AddInParameter("ToDate", DbType.DateTime, metaModel.ToDate);
            _objectDb.Received().AddInParameter("MoveBetsDay", DbType.DateTime, _balance.MoveBetsDay);
            _objectDb.Received().AddInParameter("ProductList", DbType.String, metaModel.ProductCodeList);
            _objectDb.Received().AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
        }
    }
}