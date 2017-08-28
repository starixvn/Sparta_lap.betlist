namespace Fanex.Athena.DataAccess.Report.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Web.Script.Serialization;
    using Data;
    using Model.Core.DTO.Configuration;
    using Model.Report.DTO;
    using Model.Report.MetaModel;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class SubLicenseePerformanceDataAccessTest
    {
        private SubLicenseePerformanceDataAccess _dataAccess;
        private BalanceUp _balance;
        private IObjectDb _objectDb;
        private IDataReader _dataReader;

        [SetUp]
        public void SetUp()
        {
            _balance = new BalanceUp
            {
                MoveBetsDay = new DateTime(2017, 8, 25),
                BalanceUpDay = new DateTime(2017, 8, 31)
            };
            _dataAccess = Substitute.ForPartsOf<SubLicenseePerformanceDataAccess>(_balance);
            _objectDb = Substitute.For<IObjectDb>();
            _dataReader = Substitute.For<IDataReader>();

            const string SpName = "Lrf_Rpt_SubLicenseePerformance";
            _dataAccess.GetObjectDb(Arg.Is<string>(SpName)).Returns(_objectDb);
            _objectDb.ExecuteReader().Returns(_dataReader);
        }

        [TearDown]
        public void TearDown()
        {
            _balance = null;
            _dataAccess = null;
            _objectDb = null;
        }

        [Test]
        public void GetData_WhenCall_PassCorrectParams()
        {
            // Arrange
            _dataReader.Read().Returns(false);

            var metaModel = GetDefaultMetaModel();

            // Action
            _dataAccess.GetData(metaModel);

            // Assert
            AssertParamsPassCorrectly(metaModel);
        }

        [Test]
        public void GetData_DoNotConvertToBasedCurrency_ReturnCorrectDataWithOriginalCurrency()
        {
            // Arrange
            var metaModel = GetDefaultMetaModel();
            metaModel.ConvertAll = false;

            _dataReader.Read().Returns(true, false);

            var readerDataMapping = GetReaderDataMapping();
            MapReaderData(_dataReader, readerDataMapping);

            var serializer = new JavaScriptSerializer();
            var expectedResult = serializer.Serialize(GetExpectedResult(readerDataMapping));

            // Action
            var result = serializer.Serialize(_dataAccess.GetData(metaModel));

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void GetData_ConvertToBasedCurrency_ReturnCorrectDataWithBasedCurrency()
        {
            // Arrange
            var metaModel = GetDefaultMetaModel();
            metaModel.ConvertAll = true;

            _dataReader.Read().Returns(true, false);

            var readerDataMapping = GetReaderDataMapping(metaModel.BaseCurrency);
            MapReaderData(_dataReader, readerDataMapping);

            var serializer = new JavaScriptSerializer();
            var expectedResult = serializer.Serialize(GetExpectedResult(readerDataMapping, metaModel.BaseCurrency));

            // Action
            var result = serializer.Serialize(_dataAccess.GetData(metaModel));

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        private static void MapReaderData(IDataReader reader, IDictionary<string, object> readerDataMapping)
        {
            reader[Arg.Any<string>()].Returns(x =>
            {
                var paramName = x.ArgAt<string>(0);
                if (readerDataMapping.ContainsKey(paramName))
                {
                    return readerDataMapping[paramName];
                }

                return string.Empty;
            });
        }

        private static List<SubLicenseePerformance> GetExpectedResult(IDictionary<string, object> readerDataMapping, string baseCurrency = "")
        {
            return new List<SubLicenseePerformance>
            {
                new SubLicenseePerformance
                {
                    SubLicensee = readerDataMapping["SubLicensee"].ToString(),
                    Currency = readerDataMapping["Currency"].ToString(),
                    Product = readerDataMapping["Product"].ToString(),
                    BetCount = Convert.ToInt32(readerDataMapping["BetCount"]),
                    Turnover = Convert.ToDecimal(readerDataMapping["Turnover" + baseCurrency]),
                    CustomerWinLoss = Convert.ToDecimal(readerDataMapping["CustomerWinLoss" + baseCurrency]),
                    SubLicenseeWinLoss = Convert.ToDecimal(readerDataMapping["SubLicenseeWinLoss" + baseCurrency]),
                    SubLicenseeComm = Convert.ToDecimal(readerDataMapping["SubLicenseeComm" + baseCurrency])
                }
            };
        }

        private static IDictionary<string, object> GetReaderDataMapping(string baseCurrency = "")
        {
            return new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase)
            {
                { "SubLicensee", "New York" },
                { "Currency", "USD" },
                { "Product", "Sporsbook" },
                { "BetCount", 10 },
                { "Turnover" + baseCurrency, 10000m },
                { "CustomerWinLoss" + baseCurrency, 5000m },
                { "SubLicenseeWinLoss" + baseCurrency, 2000m },
                { "SubLicenseeComm" + baseCurrency, 50m }
            };
        }

        private static SubLicenseePerformanceMetaModel GetDefaultMetaModel()
        {
            return new SubLicenseePerformanceMetaModel
            {
                BaseCurrency = "EUR",
                ConvertAll = true,
                FromDate = new DateTime(2017, 8, 25),
                ToDate = new DateTime(2017, 8, 31),
                IsExportExcel = false,
                LicenseeId = 1,
                ProductCodeList = "SB,BA",
                SubLicenseeList = "1,2",
                AgentListXml = "<Param></Param>"
            };
        }

        private void AssertParamsPassCorrectly(SubLicenseePerformanceMetaModel metaModel)
        {
            _objectDb.DidNotReceive().AddInParameter("BaseCurrency", Arg.Any<DbType>(), Arg.Any<object>());
            _objectDb.DidNotReceive().AddInParameter("ConvertAll", Arg.Any<DbType>(), Arg.Any<object>());
            _objectDb.Received().AddInParameter("FromDate", DbType.DateTime, metaModel.FromDate);
            _objectDb.Received().AddInParameter("ToDate", DbType.DateTime, metaModel.ToDate);
            _objectDb.Received().AddInParameter("IsExportExcel", DbType.Boolean, metaModel.IsExportExcel);
            _objectDb.Received().AddInParameter("LicenseeId", DbType.Int32, metaModel.LicenseeId);
            _objectDb.Received().AddInParameter("ProductCodeList", DbType.String, metaModel.ProductCodeList);
            _objectDb.Received().AddInParameter("SubLicenseeList", DbType.String, metaModel.SubLicenseeList);
            _objectDb.Received().AddInParameter("AgentListXml", DbType.String, metaModel.AgentListXml);
        }
    }
}