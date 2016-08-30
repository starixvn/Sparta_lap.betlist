namespace Fanex.Athena.DataAccess.Report.Tests
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

        [SetUp]
        public void Setup()
        {
            _objectDb = Substitute.For<IObjectDb>();

            _dataAccess = Substitute.ForPartsOf<CustomerBalanceDataAccess>();
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_Rpt_CustomerBalance")).Returns(_objectDb);

            _reader = Substitute.For<IDataReader>();
            _objectDb.ExecuteReader().Returns(_reader);
        }

        [Test]
        public void GetDataList_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            var metaModel = new CustomerBalanceMetaModel { ProductOptions = GetProductOptions() };
            var totalData = new CustBalance();
            var totalRow = 0;

            // Action
            _dataAccess.GetDataList(metaModel, ref totalData, ref totalRow);

            // Assert
            _objectDb.Received().AddInParameter("licenseeid", DbType.Int32, metaModel.LicenseeId);
            _objectDb.Received().AddInParameter("userid", DbType.Int32, metaModel.UserId);
            _objectDb.Received().AddInParameter("licenseetype", DbType.String, metaModel.LicenseeType);
            _objectDb.Received().AddInParameter("currencyid", DbType.Int32, metaModel.CurrencyId);
            _objectDb.Received().AddInParameter("fromdate", DbType.DateTime, metaModel.FromDate);
            _objectDb.Received().AddInParameter("todate", DbType.DateTime, metaModel.ToDate);
            _objectDb.Received().AddInParameter("topcount", DbType.Int32, metaModel.TopCount);
            _objectDb.Received().AddInParameter("orderitem", DbType.Byte, metaModel.OrderItem);
            _objectDb.Received().AddInParameter("excludetest", DbType.Boolean, metaModel.IsExcludeTest);
            _objectDb.Received().AddInParameter("pagesize", DbType.Int32, metaModel.PageSize);
            _objectDb.Received().AddInParameter("pageindex", DbType.Int32, metaModel.PageIndex);
            _objectDb.Received().AddInParameter("showSB", DbType.Boolean, true);
            _objectDb.Received().AddInParameter("showNG", DbType.Boolean, false);
        }

        [Test]
        public void GetDataList_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            var metaModel = new CustomerBalanceMetaModel { ProductOptions = GetProductOptions() };
            var totalData = new CustBalance();
            var totalRow = 0;
            var expected = new CustBalance
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
            ConfigureReaderForGetDataListMethod(expected);

            // Action
            var result = _dataAccess.GetDataList(metaModel, ref totalData, ref totalRow)[0];

            // Assert
            Assert.That(result.ExternalId, Is.EqualTo(expected.ExternalId));
            Assert.That(result.SystemId, Is.EqualTo(expected.SystemId));
            Assert.That(result.OpeningBalance, Is.EqualTo(expected.OpeningBalance));
            Assert.That(result.Deposit, Is.EqualTo(expected.Deposit));
            Assert.That(result.Withdrawal, Is.EqualTo(expected.Withdrawal));
            Assert.That(result.Bonus, Is.EqualTo(expected.Bonus));
            Assert.That(result.Adjustment, Is.EqualTo(expected.Adjustment));
            Assert.That(result.Turnover, Is.EqualTo(expected.Turnover));
            Assert.That(result.DrawGame, Is.EqualTo(expected.DrawGame));
            Assert.That(result.NetTurnover, Is.EqualTo(expected.NetTurnover));
            Assert.That(result.PendingFunds, Is.EqualTo(expected.PendingFunds));
            Assert.That(result.WinLoss, Is.EqualTo(expected.WinLoss));
            Assert.That(result.PlayerComm, Is.EqualTo(expected.PlayerComm));
            Assert.That(result.TotalWL, Is.EqualTo(expected.TotalWL));
            Assert.That(result.AffiliateTransfer, Is.EqualTo(expected.AffiliateTransfer));
            Assert.That(result.ClosingBalance, Is.EqualTo(expected.ClosingBalance));
            Assert.That(result.TurnoverBA, Is.EqualTo(expected.TurnoverBA));
            Assert.That(result.CommBA, Is.EqualTo(expected.CommBA));
            Assert.That(result.AmountDueToBA, Is.EqualTo(expected.AmountDueToBA));
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
            _reader.Read().Returns(true, false);

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
    }
}