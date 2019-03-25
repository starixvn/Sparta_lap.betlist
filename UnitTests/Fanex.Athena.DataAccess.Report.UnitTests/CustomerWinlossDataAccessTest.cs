namespace Fanex.Athena.DataAccess.Report.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Core.Configuration.Interfaces;
    using Core.UnitTests.TestHelpers;
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

        private readonly BalanceUp _balance = new BalanceUp
        {
            MoveBetsDay = new DateTime(2016, 8, 10),
            BalanceUpDay = new DateTime(2016, 8, 31)
        };

        private CustomerWinLossMetaModel _metaModel;

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();
            _metaModel = new CustomerWinLossMetaModel
            {
                ProductOptions = GetProductOptions(),
                BaseCurrency = "UUS"
            };
        }

        protected override void SetUpDataAccess()
        {
            var balanceUpDataAccess = Substitute.For<IBalanceUpDataAccess>();
            balanceUpDataAccess.GetBalanceUp().Returns(_balance);

            DataAccess = Substitute.ForPartsOf<CustomerWinlossDataAccess>(
                balanceUpDataAccess);
            DataAccess.GetObjectDb(Arg.Is(SpName)).Returns(ObjectDb);
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
                ObjectDb.Received(1).SetParameterValue("fromdate", _metaModel.FromDate);
                ObjectDb.Received(1).SetParameterValue("todate", _metaModel.ToDate);
                ObjectDb.Received(1).SetParameterValue("pagesize", expectedPageSize);
                ObjectDb.Received(1).SetParameterValue("pageindex", _metaModel.PageIndex);
                ObjectDb.Received(1).SetParameterValue("movebetsday", _balance.MoveBetsDay);
                ObjectDb.Received(1).SetParameterValue("AgentList", _metaModel.AgentListXml);
                ObjectDb.Received(1).SetParameterValue("showSB", true);
                ObjectDb.Received(1).SetParameterValue("showNG", false);
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
            const int ExpectedSumPromotionPrize = 1;
            ConfigureReaderForResult(expectedResult);
            ConfigureReaderForTotalData(expectedTotalData, ExpectedTotalRow, ExpectedSumPromotionPrize);

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
                Assert.That(result.AdditionalInfo, Is.EqualTo(ExpectedSumPromotionPrize.ToString()));
            });
        }

        private static void AssertCommonData(CustomerWinLoss actual, CustomerWinLoss expected)
        {
            Assert.That(actual.Turnover, Is.EqualTo(expected.Turnover));
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
            Reader["CustID"].Returns(expected.CustId);
            Reader["nickname"].Returns(expected.ExternalId);
            Reader["username"].Returns(expected.SystemId);
            MapReader(expected, false);
        }

        private void ConfigureReaderForTotalData(CustomerWinLoss expected, int expectedTotalRow, int expectedSumPromotionPrize)
        {
            Reader["TotalRows"].Returns(expectedTotalRow);
            Reader["SumPromotionPrize"].Returns(expectedSumPromotionPrize);
            MapReader(expected, true);
        }

        private void MapReader(CustomerWinLoss expected, bool isSum)
        {
            var prefix = isSum ? "sum" : string.Empty;
            var turnoverColumn = prefix + "turnover" + _metaModel.BaseCurrency;
            var memberWinLostColumn = prefix + "memberwinlost" + _metaModel.BaseCurrency;
            var betEffectiveColumn = prefix + "beteffective" + _metaModel.BaseCurrency;
            var playerCommColumn = prefix + "playercomm" + _metaModel.BaseCurrency;
            var licenseeWinLostColumn = prefix + "licenseewl" + _metaModel.BaseCurrency;
            var bacommColumn = prefix + "commba" + _metaModel.BaseCurrency;
            var winlossrbColumn = prefix + "winlossrb" + _metaModel.BaseCurrency;
            var commrbColumn = prefix + "commrb" + _metaModel.BaseCurrency;
            var amtduetobaColumn = prefix + "amtduetoba" + _metaModel.BaseCurrency;
            var sumcustomertotalwlColumn = prefix + "customertotalwl" + _metaModel.BaseCurrency;

            Reader[turnoverColumn].Returns(expected.Turnover);
            Reader[prefix + "betcount"].Returns(expected.TotalTransaction);
            Reader[memberWinLostColumn].Returns(expected.CustomerWL);
            Reader[betEffectiveColumn].Returns(expected.BetEffective);
            Reader[playerCommColumn].Returns(expected.PlayerComm);
            Reader[licenseeWinLostColumn].Returns(expected.LicenseeWL);
            Reader[bacommColumn].Returns(expected.BAComm);
            Reader[winlossrbColumn].Returns(15m);
            Reader[commrbColumn].Returns(5m);
            Reader[amtduetobaColumn].Returns(expected.AmtDueToBA);
            Reader[sumcustomertotalwlColumn].Returns(expected.CustomerTotalWL);
        }
    }
}