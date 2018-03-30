namespace Fanex.Athena.DataAccess.Report.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Core.Configuration.Interfaces;
    using Core.UnitTests.TestHelpers;
    using Data;
    using Extension.Utility;
    using Model.Core.DTO.Configuration;
    using Model.Core.MetaModel.Product;
    using Model.Report.DTO;
    using Model.Report.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using UnitTest.Common.Helpers;

    [TestFixture]
    public class CustomerWinlossDataAccessTest : BaseDataAccessHelperTest<CustomerWinlossDataAccess>
    {
        private const string SpName = "Lrf_Rpt_WinLoss";

        private const int PageSizeExcel = int.MaxValue - 1000;

        private BalanceUp _balance;

        private CustomerWinLossMetaModel _metaModel;

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            ObjectDb = Substitute.For<IObjectDb>();

            var balanceUpDataAccess = Substitute.For<IBalanceUpDataAccess>();
            _balance = new BalanceUp
            {
                MoveBetsDay = new DateTime(2016, 8, 10),
                BalanceUpDay = new DateTime(2016, 8, 31)
            };
            balanceUpDataAccess.GetBalanceUp().Returns(_balance);

            DataAccess = Substitute.ForPartsOf<CustomerWinlossDataAccess>(balanceUpDataAccess);
            DataAccess.GetObjectDb(Arg.Is(SpName)).Returns(ObjectDb);

            var appSetting = Substitute.For<IAppSettings>();
            appSetting.HistoryDate.Returns(new DateTime(2016, 8, 31));
            _metaModel = new CustomerWinLossMetaModel(appSetting)
            {
                ProductOptions = GetProductOptions(),
                BaseCurrency = "UUS"
            };
        }

        [OneTimeTearDown]
        public override void FixtureTearDown()
        {
            _balance = null;
            _metaModel = null;

            base.FixtureTearDown();
        }

        [TestCase(true, 5, PageSizeExcel)]
        [TestCase(false, 5, 5)]
        public void GetDataList_WhenCall_ParametersPassedCorrectly(bool isExportExcel, int pageSize, int expectedPageSize)
        {
            // Arrange
            _metaModel.PageSize = pageSize;
            _metaModel.IsExportExcel = isExportExcel;

            // Action
            DataAccess.GetDataList(_metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received(1).AddInParameter("fromdate", DbType.DateTime, _metaModel.FromDate);
                ObjectDb.Received(1).AddInParameter("todate", DbType.DateTime, _metaModel.ToDate);
                ObjectDb.Received(1).AddInParameter("pagesize", DbType.Int32, expectedPageSize);
                ObjectDb.Received(1).AddInParameter("pageindex", DbType.Int32, _metaModel.PageIndex);
                ObjectDb.Received(1).AddInParameter("movebetsday", DbType.DateTime, _balance.MoveBetsDay);
                ObjectDb.Received(1).AddInParameter("isold", DbType.Boolean, _metaModel.IsOld);
                ObjectDb.Received(1).AddInParameter("AgentList", DbType.String, _metaModel.AgentListXml);
                ObjectDb.Received(1).AddInParameter("showSB", DbType.Boolean, true);
                ObjectDb.Received(1).AddInParameter("showNG", DbType.Boolean, false);
            });
        }

        [Test]
        public void GetDataList_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            Reader.Read().Returns(true, false, true, false);
            Reader.NextResult().Returns(true);
            var expectedTotalData = FakeHelper.Create<CustomerWinLoss>();
            var expectedResult = FakeHelper.Create<CustomerWinLoss>();
            const int ExpectedTotalRow = 100;
            ConfigureReaderForResult(expectedResult);
            ConfigureReaderForTotalData(expectedTotalData, ExpectedTotalRow);

            // Action
            var result = DataAccess.GetDataList(_metaModel);
            var firstItem = result.DataList.First();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(firstItem.CustId, Is.EqualTo(expectedResult.CustId));
                Assert.That(firstItem.ExternalId, Is.EqualTo(expectedResult.ExternalId));
                Assert.That(firstItem.SystemId, Is.EqualTo(expectedResult.SystemId));
                Assert.That(firstItem.BetEffective, Is.EqualTo(expectedResult.BetEffective));
                Assert.That(firstItem.CustomerTotalWL, Is.EqualTo(expectedResult.CustomerTotalWL));
                AssertCommonData(firstItem, expectedResult);

                AssertCommonData(result.TotalData, expectedTotalData);
                Assert.That(result.TotalData.LicenseeWL, Is.EqualTo(expectedTotalData.LicenseeWL));

                Assert.That(result.TotalRows, Is.EqualTo(ExpectedTotalRow));
            });
        }

        private static void AssertCommonData(CustomerWinLoss actual, CustomerWinLoss expected)
        {
            Assert.That(actual.Turnover, Is.EqualTo(expected.Turnover));
            Assert.That(actual.RacingRevenue, Is.EqualTo(expected.RacingRevenue));
            Assert.That(actual.TotalTransaction, Is.EqualTo(expected.TotalTransaction));
            Assert.That(actual.CustomerWL, Is.EqualTo(expected.CustomerWL));
            Assert.That(actual.PlayerComm, Is.EqualTo(expected.PlayerComm));
        }

        [Test]
        public void GetDataList_NoData_ReturnEmptyResult()
        {
            // Arrange
            Reader.Read().Returns(false);
            Reader.NextResult().Returns(false);

            // Action
            var result = DataAccess.GetDataList(_metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.DataList, Is.Empty);
                Assert.That(result.TotalRows, Is.Zero);
            });
        }

        [Test]
        public void GetDataList_NoTotalDataReturned_DataReturnedCorrectly()
        {
            // Arrange
            var expectedResult = FakeHelper.Create<CustomerWinLoss>();

            Reader.Read().Returns(true, false);
            Reader.NextResult().Returns(false);
            ConfigureReaderForResult(expectedResult);

            // Action
            var result = DataAccess.GetDataList(_metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.DataList, Is.Not.Empty);
                Assert.That(result.TotalRows, Is.Zero);
            });
        }

        [Test]
        public void GetDataList_WhenObjectDbThrowsException_ThrowDataAccessException()
        {
            TestException_WhenObjectDbThrowsException_ThrowDataAccessException(
                SpName,
                () => DataAccess.GetDataList(_metaModel));
        }

        private IList<ProductOption> GetProductOptions()
        {
            return new List<ProductOption>
            {
                new ProductOption { Code = "SB", Status = true },
                new ProductOption { Code = "NG", Status = false }
            };
        }

        private void ConfigureReaderForResult(CustomerWinLoss expected)
        {
            var turnoverColumn = "turnover" + _metaModel.BaseCurrency;
            var memberWinLostColumn = "memberwinlost" + _metaModel.BaseCurrency;
            var betEffectiveColumn = "beteffective" + _metaModel.BaseCurrency;
            var playerCommColumn = "playercomm" + _metaModel.BaseCurrency;
            var licenseeWinLostColumn = "licenseewl" + _metaModel.BaseCurrency;
            var bacommColumn = "commba" + _metaModel.BaseCurrency;
            var winlossrbColumn = "winlossrb" + _metaModel.BaseCurrency;
            var commrbColumn = "commrb" + _metaModel.BaseCurrency;
            var amtduetobaColumn = "amtduetoba" + _metaModel.BaseCurrency;
            var amtduetorbColumn = "amtduetorb" + _metaModel.BaseCurrency;
            var customertotalwlColumn = "customertotalwl" + _metaModel.BaseCurrency;
            var racingrevenueColumn = "racingrevenue" + _metaModel.BaseCurrency;

            Reader["CustID"].Returns(expected.CustId);
            Reader["nickname"].Returns(expected.ExternalId);
            Reader["username"].Returns(expected.SystemId);
            Reader[turnoverColumn].Returns(expected.Turnover);
            Reader["betcount"].Returns(expected.TotalTransaction);
            Reader[memberWinLostColumn].Returns(expected.CustomerWL);
            Reader[betEffectiveColumn].Returns(expected.BetEffective);
            Reader[playerCommColumn].Returns(expected.PlayerComm);
            Reader[licenseeWinLostColumn].Returns(expected.LicenseeWL);
            Reader[bacommColumn].Returns(expected.BAComm);
            Reader[winlossrbColumn].Returns(10m);
            Reader[commrbColumn].Returns(5m);
            Reader[amtduetobaColumn].Returns(expected.AmtDueToBA);
            Reader[amtduetorbColumn].Returns(expected.AmtDueToRacing);
            Reader[customertotalwlColumn].Returns(expected.CustomerTotalWL);
            Reader[racingrevenueColumn].Returns(expected.RacingRevenue);
        }

        private void ConfigureReaderForTotalData(CustomerWinLoss expected, int expectedTotalRow)
        {
            var turnoverColumn = "sumturnover" + _metaModel.BaseCurrency;
            var memberWinLostColumn = "summemberwinlost" + _metaModel.BaseCurrency;
            var betEffectiveColumn = "sumbeteffective" + _metaModel.BaseCurrency;
            var playerCommColumn = "sumplayercomm" + _metaModel.BaseCurrency;
            var licenseeWinLostColumn = "sumlicenseewl" + _metaModel.BaseCurrency;
            var bacommColumn = "sumcommba" + _metaModel.BaseCurrency;
            var winlossrbColumn = "sumwinlossrb" + _metaModel.BaseCurrency;
            var commrbColumn = "sumcommrb" + _metaModel.BaseCurrency;
            var amtduetobaColumn = "sumamtduetoba" + _metaModel.BaseCurrency;
            var amtduetorbColumn = "sumamtduetorb" + _metaModel.BaseCurrency;
            var sumracingrevenueColumn = "sumracingrevenue" + _metaModel.BaseCurrency;
            var sumcustomertotalwlColumn = "sumcustomertotalwl" + _metaModel.BaseCurrency;

            Reader[turnoverColumn].Returns(expected.Turnover);
            Reader["sumbetcount"].Returns(expected.TotalTransaction);
            Reader[memberWinLostColumn].Returns(expected.CustomerWL);
            Reader[betEffectiveColumn].Returns(expected.BetEffective);
            Reader[playerCommColumn].Returns(expected.PlayerComm);
            Reader[licenseeWinLostColumn].Returns(expected.LicenseeWL);
            Reader[bacommColumn].Returns(expected.BAComm);
            Reader[winlossrbColumn].Returns(15m);
            Reader[commrbColumn].Returns(5m);
            Reader[amtduetobaColumn].Returns(expected.AmtDueToBA);
            Reader[amtduetorbColumn].Returns(expected.AmtDueToRacing);
            Reader[sumracingrevenueColumn].Returns(expected.RacingRevenue);
            Reader[sumcustomertotalwlColumn].Returns(expected.CustomerTotalWL);

            Reader["TotalRows"].Returns(expectedTotalRow);
        }
    }
}