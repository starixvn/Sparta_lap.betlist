namespace Fanex.Athena.DataAccess.Report.UnitTests
{
    using System;
    using System.Data;
    using System.Linq;
    using Core.Configuration.Interfaces;
    using Core.UnitTests.TestHelpers;
    using Model.Core.DTO.Common;
    using Model.Core.DTO.Configuration;
    using Model.Report.DTO;
    using Model.Report.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using Resources;

    [TestFixture]
    public class LicenseeWinLossByTimeZoneDataAccessTest : BaseDataAccessHelperTest<LicenseeWinLossByTimeZoneDataAccess>
    {
        private const byte GroupByProduct = 1;

        private const int ReportBySummary = 1;

        private const int ReportByMonthly = 3;

        private readonly BalanceUp _balance = new BalanceUp
        {
            MoveBetsDay = new DateTime(2017, 8, 25),
            BalanceUpDay = new DateTime(2017, 8, 31)
        };

        protected override void SetUpDataAccess()
        {
            var balanceUpDataAccess = Substitute.For<IBalanceUpDataAccess>();
            balanceUpDataAccess.GetBalanceUp().Returns(_balance);
            DataAccess = Substitute.ForPartsOf<LicenseeWinLossByTimeZoneDataAccess>(
                balanceUpDataAccess);

            const string SpName = "Lrf_Rpt_LicWinlossByTimeZone";
            DataAccess.GetObjectDb(Arg.Is(SpName)).Returns(ObjectDb);
        }

        [Test]
        public void GetData_WhenCall_PassCorrectParams()
        {
            // Arrange
            Reader.Read().Returns(false);

            var metaModel = GetDefaultMetaModel();

            // Action
            DataAccess.GetData(metaModel, UserCurrencyList);

            // Assert
            AssertParamsPassCorrectly(metaModel);
        }

        [Test]
        public void GetData_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var metaModel = GetDefaultMetaModel();

            var expectedData = new LicenseeWinLossByTimeZone
            {
                GroupBy = CurrencyId.ToString(),
                BetCount = 1,
                Turnover = 2,
                CustomerComm = 3,
                LicenseeWinLoss = 4,
                LicenseeComm = 5
            };
            MapReaderData(expectedData, metaModel.BaseCurrency);

            // Action
            var result = DataAccess.GetData(metaModel, UserCurrencyList);
            var firstItem = result.DataList.ElementAt(0);
            var total = result.TotalData;

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(firstItem.GroupBy, Is.EqualTo(CurrencyName));
                AssertData(firstItem, expectedData);
                AssertData(total, expectedData);
            });
        }

        [Test]
        public void GetData_DoNotConvertAll_ReturnCorrectData()
        {
            // Arrange
            var metaModel = GetDefaultMetaModel();
            metaModel.BaseCurrency = "BASE";
            metaModel.IsConvertAll = false;

            var expectedData = new LicenseeWinLossByTimeZone
            {
                GroupBy = CurrencyId.ToString(),
                BetCount = 1,
                Turnover = 2,
                CustomerComm = 3,
                LicenseeWinLoss = 4,
                LicenseeComm = 5
            };
            MapReaderData(expectedData, string.Empty);

            var expectedBaseData = new LicenseeWinLossByTimeZone
            {
                GroupBy = CurrencyId.ToString(),
                BetCount = expectedData.BetCount,
                Turnover = 20,
                CustomerComm = 30,
                LicenseeWinLoss = 40,
                LicenseeComm = 50
            };
            MapReaderData(expectedBaseData, metaModel.BaseCurrency);

            // Action
            var result = DataAccess.GetData(metaModel, UserCurrencyList);
            var firstItem = result.DataList.ElementAt(0);
            var total = result.TotalData;

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(firstItem.GroupBy, Is.EqualTo(CurrencyName));
                AssertData(firstItem, expectedData);
                AssertData(total, expectedBaseData);
            });
        }

        [Test]
        public void GetData_ReportByIsNotSummary_ReturnCorrectReportByData()
        {
            // Arrange
            var metaModel = GetDefaultMetaModel();
            metaModel.ReportBy = ReportByMonthly;

            const string ReportBy = "20170102";
            Reader["reportby"].Returns(ReportBy);

            var data = new LicenseeWinLossByTimeZone
            {
                GroupBy = CurrencyId.ToString(),
            };
            MapReaderData(data, string.Empty);

            var expected = new DateTime(2017, 1, 1);

            // Action
            var result = DataAccess.GetData(metaModel, UserCurrencyList);
            var firstItem = result.DataList.ElementAt(0);

            // Assert
            Assert.That(firstItem.ReportBy, Is.EqualTo(expected));
        }

        [Test]
        public void GetData_GroupByProduct_ReturnCorrectGroupByData()
        {
            // Arrange
            var metaModel = GetDefaultMetaModel();
            metaModel.GroupBy = GroupByProduct;

            var data = new LicenseeWinLossByTimeZone
            {
                GroupBy = Product.BA,
            };
            MapReaderData(data, string.Empty);

            // Action
            var result = DataAccess.GetData(metaModel, UserCurrencyList);
            var firstItem = result.DataList.ElementAt(0);

            // Assert
            Assert.That(firstItem.GroupBy, Is.EqualTo(ViewResources.ProductBA));
        }

        private static void AssertData(LicenseeWinLossByTimeZone actual, LicenseeWinLossByTimeZone expected)
        {
            Assert.That(actual.BetCount, Is.EqualTo(expected.BetCount));
            Assert.That(actual.Turnover, Is.EqualTo(expected.Turnover));
            Assert.That(actual.CustomerComm, Is.EqualTo(expected.CustomerComm));
            Assert.That(actual.LicenseeWinLoss, Is.EqualTo(expected.LicenseeWinLoss));
            Assert.That(actual.LicenseeComm, Is.EqualTo(expected.LicenseeComm));
        }

        private static LicenseeWinLossByTimeZoneMetaModel GetDefaultMetaModel()
        {
            return new LicenseeWinLossByTimeZoneMetaModel
            {
                BaseCurrency = string.Empty,
                FromDate = new DateTime(2017, 1, 1),
                ToDate = new DateTime(2017, 2, 2),
                ProductCodeList = "SB,BA",
                AgentListXml = "<Param></Param>",
                TimeZone = -4,
                IsIncludeDrawData = true,
                ReportBy = ReportBySummary,
                GroupBy = 2,
                IsConvertAll = true
            };
        }

        private void AssertParamsPassCorrectly(LicenseeWinLossByTimeZoneMetaModel metaModel)
        {
            ObjectDb.Received(1).AddInParameter("FromDate", DbType.DateTime, metaModel.FromDate);
            ObjectDb.Received(1).AddInParameter("ToDate", DbType.DateTime, metaModel.ToDate);
            ObjectDb.Received(1).AddInParameter("MoveBetsDay", DbType.DateTime, _balance.MoveBetsDay);
            ObjectDb.Received(1).AddInParameter("ProductList", DbType.String, metaModel.ProductCodeList);
            ObjectDb.Received(1).AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
            ObjectDb.Received(1).AddInParameter("TimeZone", DbType.Int16, metaModel.TimeZone);
            ObjectDb.Received(1).AddInParameter("ReportBy", DbType.Byte, metaModel.ReportBy);
            ObjectDb.Received(1).AddInParameter("GroupBy", DbType.Byte, metaModel.GroupBy);
            ObjectDb.Received(1).AddInParameter("IncludeDraw", DbType.Boolean, metaModel.IsIncludeDrawData);
        }

        private void MapReaderData(LicenseeWinLossByTimeZone data, string baseCurrency)
        {
            Reader.Read().Returns(true, false);

            Reader["GroupBy" + baseCurrency].Returns(data.GroupBy);
            Reader["BetCount" + baseCurrency].Returns(data.BetCount);
            Reader["Turnover" + baseCurrency].Returns(data.Turnover);
            Reader["PlayerComm" + baseCurrency].Returns(data.CustomerComm);
            Reader["LicenseeWL" + baseCurrency].Returns(data.LicenseeWinLoss);
            Reader["LicenseeComm" + baseCurrency].Returns(data.LicenseeComm);
        }
    }
}