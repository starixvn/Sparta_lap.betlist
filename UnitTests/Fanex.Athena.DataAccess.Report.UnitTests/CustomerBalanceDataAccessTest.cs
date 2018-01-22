namespace Fanex.Athena.DataAccess.Report.UnitTests
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Core.UnitTests.TestHelpers;
    using Model.Core.MetaModel.Product;
    using Model.Report.DTO;
    using Model.Report.MetaModel;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class CustomerBalanceDataAccessTest : BaseDataAccessHelperTest<CustomerBalanceDataAccess>
    {
        private CustomerBalanceMetaModel _metaModel;

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();

            DataAccess.GetObjectDb(Arg.Is("Lrf_Rpt_CustomerBalance")).Returns(ObjectDb);

            _metaModel = new CustomerBalanceMetaModel { ProductOptions = GetProductOptions() };
        }

        [Test]
        public void GetDataList_WhenCall_ParametersPassedCorrectly()
        {
            // Action
            DataAccess.GetDataList(_metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received().AddInParameter("fromdate", DbType.DateTime, _metaModel.FromDate);
                ObjectDb.Received().AddInParameter("todate", DbType.DateTime, _metaModel.ToDate);
                ObjectDb.Received().AddInParameter("topcount", DbType.Int32, _metaModel.TopCount);
                ObjectDb.Received().AddInParameter("orderitem", DbType.Byte, _metaModel.OrderItem);
                ObjectDb.Received().AddInParameter("AgentList", DbType.String, _metaModel.AgentListXml);
                ObjectDb.Received().AddInParameter("pagesize", DbType.Int32, _metaModel.PageSize);
                ObjectDb.Received().AddInParameter("pageindex", DbType.Int32, _metaModel.PageIndex);
                ObjectDb.Received().AddInParameter("showSB", DbType.Boolean, true);
                ObjectDb.Received().AddInParameter("showNG", DbType.Boolean, false);
            });
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
            var expectedResult = new CustBalance
            {
                ExternalId = "ExternalIdTest",
                SystemId = "SystemIdTest",
            };

            Reader.Read().Returns(true, false);
            Reader.NextResult().Returns(false);
            ConfigureReaderForGetDataListMethod(expectedResult);

            // Action
            var result = DataAccess.GetDataList(_metaModel);
            var totalData = result.TotalData;

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.DataList, Is.Not.Empty);
                Assert.That(result.TotalRows, Is.Zero);
            });
        }

        [Test]
        public void GetDataList_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            var expectedTotalData = new CustBalance
            {
                Turnover = 10m,
                DrawGame = 5m,
                NetTurnover = 5m,
                WinLoss = 15m,
                PlayerComm = 10m,
                TotalWL = 25m
            };
            var expectedResult = new CustBalance
            {
                ExternalId = "ExternalIdTest",
                SystemId = "SystemIdTest",
                Turnover = 15m,
                DrawGame = 10m,
                NetTurnover = 5m,
                WinLoss = 10m,
                PlayerComm = 10m,
                TotalWL = 20m
            };
            const int ExpectedTotalRow = 100;

            Reader.Read().Returns(true, false, true, false);
            Reader.NextResult().Returns(true);
            ConfigureReaderForGetDataListMethod(expectedResult);
            ConfigureReaderForTotalData(expectedTotalData, ExpectedTotalRow);

            // Action
            var result = DataAccess.GetDataList(_metaModel);
            var firstItem = result.DataList.First();
            var totalData = result.TotalData;

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(firstItem.ExternalId, Is.EqualTo(expectedResult.ExternalId));
                AssertData(firstItem, expectedResult);
                AssertData(totalData, expectedTotalData);
                Assert.That(result.TotalRows, Is.EqualTo(ExpectedTotalRow));
            });
        }

        private static void AssertData(CustBalance actual, CustBalance expected)
        {
            Assert.That(actual.Turnover, Is.EqualTo(expected.Turnover));
            Assert.That(actual.DrawGame, Is.EqualTo(expected.DrawGame));
            Assert.That(actual.NetTurnover, Is.EqualTo(expected.NetTurnover));
            Assert.That(actual.WinLoss, Is.EqualTo(expected.WinLoss));
            Assert.That(actual.PlayerComm, Is.EqualTo(expected.PlayerComm));
            Assert.That(actual.TotalWL, Is.EqualTo(expected.TotalWL));
        }

        private IList<ProductOption> GetProductOptions()
        {
            return new List<ProductOption>
            {
                new ProductOption { Code = "SB", Status = true },
                new ProductOption { Code = "NG", Status = false }
            };
        }

        private void ConfigureReaderForGetDataListMethod(CustBalance expected)
        {
            Reader["nickname"].Returns(expected.ExternalId);
            Reader["username"].Returns(expected.SystemId);
            Reader["openingbalance"].Returns(expected.OpeningBalance);
            Reader["deposit"].Returns(expected.Deposit);
            Reader["withdrawal"].Returns(expected.Withdrawal);
            Reader["bonus"].Returns(expected.Bonus);
            Reader["adjustment"].Returns(expected.Adjustment);
            Reader["turnover"].Returns(expected.Turnover);
            Reader["drawgame"].Returns(expected.DrawGame);
            Reader["outstanding"].Returns(expected.PendingFunds);
            Reader["winloss"].Returns(expected.WinLoss);
            Reader["playercomm"].Returns(expected.PlayerComm);
            Reader["affiliatetransfer"].Returns(expected.AffiliateTransfer);
            Reader["closingbalance"].Returns(expected.ClosingBalance);
            Reader["TurnOverBA"].Returns(expected.TurnoverBA);
            Reader["CommBA"].Returns(expected.CommBA);
            Reader["AmountDueToBA"].Returns(expected.AmountDueToBA);
        }

        private void ConfigureReaderForTotalData(CustBalance expected, int expectedTotalRow)
        {
            Reader["sumopeningbalance"].Returns(expected.OpeningBalance);
            Reader["sumdeposit"].Returns(expected.Deposit);
            Reader["sumwithdrawal"].Returns(expected.Withdrawal);
            Reader["sumbonus"].Returns(expected.Bonus);
            Reader["sumadjustment"].Returns(expected.Adjustment);
            Reader["sumturnover"].Returns(expected.Turnover);
            Reader["sumdrawgame"].Returns(expected.DrawGame);
            Reader["sumoutstanding"].Returns(expected.PendingFunds);
            Reader["sumwinloss"].Returns(expected.WinLoss);
            Reader["sumaffiliatetransfer"].Returns(expected.AffiliateTransfer);
            Reader["sumclosingbalance"].Returns(expected.ClosingBalance);
            Reader["sumturnoverba"].Returns(expected.TurnoverBA);
            Reader["sumcommba"].Returns(expected.CommBA);
            Reader["sumamountduetoba"].Returns(expected.AmountDueToBA);
            Reader["sumplayercomm"].Returns(expected.PlayerComm);

            Reader["TotalRows"].Returns(expectedTotalRow);
        }
    }
}