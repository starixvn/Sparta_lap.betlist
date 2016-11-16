namespace Fanex.Athena.DataAccess.Report.UnitTests
{
    using System.Collections.Generic;
    using System.Data;
    using Data;
    using Model.Core.MetaModel.Product;
    using Model.Report.DTO;
    using Model.Report.MetaModel;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class CustomerBalanceDataAccessTest
    {
        private IObjectDb _objectDb;
        private CustomerBalanceDataAccess _dataAccess;
        private IDataReader _reader;
        private CustomerBalanceMetaModel _metaModel;

        [OneTimeSetUp]
        public void Setup()
        {
            _objectDb = Substitute.For<IObjectDb>();

            _dataAccess = Substitute.ForPartsOf<CustomerBalanceDataAccess>();
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_Rpt_CustomerBalance")).Returns(_objectDb);

            _reader = Substitute.For<IDataReader>();
            _objectDb.ExecuteReader().Returns(_reader);

            _metaModel = new CustomerBalanceMetaModel { ProductOptions = GetProductOptions() };
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _objectDb = null;
            _dataAccess = null;
            _reader = null;
            _metaModel = null;
        }

        [Test]
        public void GetDataList_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            var totalData = new CustBalance();
            int totalRow = 0;

            // Action
            _dataAccess.GetDataList(_metaModel, ref totalData, ref totalRow);

            // Assert
            _objectDb.Received().AddInParameter("licenseeid", DbType.Int32, _metaModel.LicenseeId);
            _objectDb.Received().AddInParameter("userid", DbType.Int32, _metaModel.UserId);
            _objectDb.Received().AddInParameter("licenseetype", DbType.String, _metaModel.LicenseeType);
            _objectDb.Received().AddInParameter("currencyid", DbType.Int32, _metaModel.CurrencyId);
            _objectDb.Received().AddInParameter("fromdate", DbType.DateTime, _metaModel.FromDate);
            _objectDb.Received().AddInParameter("todate", DbType.DateTime, _metaModel.ToDate);
            _objectDb.Received().AddInParameter("topcount", DbType.Int32, _metaModel.TopCount);
            _objectDb.Received().AddInParameter("orderitem", DbType.Byte, _metaModel.OrderItem);
            _objectDb.Received().AddInParameter("excludetest", DbType.Boolean, _metaModel.IsExcludeTest);
            _objectDb.Received().AddInParameter("pagesize", DbType.Int32, _metaModel.PageSize);
            _objectDb.Received().AddInParameter("pageindex", DbType.Int32, _metaModel.PageIndex);
            _objectDb.Received().AddInParameter("showSB", DbType.Boolean, true);
            _objectDb.Received().AddInParameter("showNG", DbType.Boolean, false);
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

            var totalData = new CustBalance();
            int totalRow = 0;
            _reader.Read().Returns(true, false, true, false);
            _reader.NextResult().Returns(true);
            ConfigureReaderForGetDataListMethod(expectedResult);
            ConfigureReaderForTotalData(expectedTotalData);

            // Action
            var result = _dataAccess.GetDataList(_metaModel, ref totalData, ref totalRow)[0];

            // Assert
            Assert.That(result.ExternalId, Is.EqualTo(expectedResult.ExternalId));
            Assert.That(result.SystemId, Is.EqualTo(expectedResult.SystemId));
            Assert.That(result.Turnover, Is.EqualTo(expectedResult.Turnover));
            Assert.That(result.DrawGame, Is.EqualTo(expectedResult.DrawGame));
            Assert.That(result.NetTurnover, Is.EqualTo(expectedResult.NetTurnover));
            Assert.That(result.WinLoss, Is.EqualTo(expectedResult.WinLoss));
            Assert.That(result.PlayerComm, Is.EqualTo(expectedResult.PlayerComm));
            Assert.That(result.TotalWL, Is.EqualTo(expectedResult.TotalWL));
            Assert.That(totalData.Turnover, Is.EqualTo(expectedTotalData.Turnover));
            Assert.That(totalData.DrawGame, Is.EqualTo(expectedTotalData.DrawGame));
            Assert.That(totalData.NetTurnover, Is.EqualTo(expectedTotalData.NetTurnover));
            Assert.That(totalData.WinLoss, Is.EqualTo(expectedTotalData.WinLoss));
            Assert.That(totalData.PlayerComm, Is.EqualTo(expectedTotalData.PlayerComm));
            Assert.That(totalData.TotalWL, Is.EqualTo(expectedTotalData.TotalWL));
            Assert.That(totalRow, Is.EqualTo(100));
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
            _reader["nickname"].Returns(expected.ExternalId);
            _reader["username"].Returns(expected.SystemId);
            _reader["openingbalance"].Returns(expected.OpeningBalance);
            _reader["deposit"].Returns(expected.Deposit);
            _reader["withdrawal"].Returns(expected.Withdrawal);
            _reader["bonus"].Returns(expected.Bonus);
            _reader["adjustment"].Returns(expected.Adjustment);
            _reader["turnover"].Returns(expected.Turnover);
            _reader["drawgame"].Returns(expected.DrawGame);
            _reader["outstanding"].Returns(expected.PendingFunds);
            _reader["winloss"].Returns(expected.WinLoss);
            _reader["playercomm"].Returns(expected.PlayerComm);
            _reader["affiliatetransfer"].Returns(expected.AffiliateTransfer);
            _reader["closingbalance"].Returns(expected.ClosingBalance);
            _reader["TurnOverBA"].Returns(expected.TurnoverBA);
            _reader["CommBA"].Returns(expected.CommBA);
            _reader["AmountDueToBA"].Returns(expected.AmountDueToBA);
        }

        private void ConfigureReaderForTotalData(CustBalance expected)
        {
            _reader["sumopeningbalance"].Returns(expected.OpeningBalance);
            _reader["sumdeposit"].Returns(expected.Deposit);
            _reader["sumwithdrawal"].Returns(expected.Withdrawal);
            _reader["sumbonus"].Returns(expected.Bonus);
            _reader["sumadjustment"].Returns(expected.Adjustment);
            _reader["sumturnover"].Returns(expected.Turnover);
            _reader["sumdrawgame"].Returns(expected.DrawGame);
            _reader["sumoutstanding"].Returns(expected.PendingFunds);
            _reader["sumwinloss"].Returns(expected.WinLoss);
            _reader["sumaffiliatetransfer"].Returns(expected.AffiliateTransfer);
            _reader["sumclosingbalance"].Returns(expected.ClosingBalance);
            _reader["sumturnoverba"].Returns(expected.TurnoverBA);
            _reader["sumcommba"].Returns(expected.CommBA);
            _reader["sumamountduetoba"].Returns(expected.AmountDueToBA);
            _reader["sumplayercomm"].Returns(expected.PlayerComm);

            _reader["TotalRows"].Returns(100);
        }
    }
}