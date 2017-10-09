namespace Fanex.Athena.DataAccess.Report.UnitTests
{
    using System;
    using System.Data;
    using System.Linq;
    using Core.Configuration.Interfaces;
    using Data;
    using Model.Core.DTO.Configuration;
    using Model.Report.DTO;
    using Model.Report.MetaModel;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class CustomerTurnoverDataAccessTest
    {
        private const string LanguageCode = "en-us";
        private IObjectDb _objectDb;
        private CustomerTurnoverDataAccess _dataAccess;
        private BalanceUp _balance;
        private IDataReader _reader;
        private CustomerTurnoverMetaModel _metaModel;

        [OneTimeSetUp]
        public void Setup()
        {
            _objectDb = Substitute.For<IObjectDb>();

            var balanceUpDataAccess = Substitute.For<IBalanceUpDataAccess>();
            _balance = new BalanceUp
            {
                MoveBetsDay = new DateTime(2016, 8, 10),
                BalanceUpDay = new DateTime(2016, 8, 31)
            };
            balanceUpDataAccess.GetBalanceUp().Returns(_balance);

            _dataAccess = Substitute.ForPartsOf<CustomerTurnoverDataAccess>(balanceUpDataAccess);
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_Rpt_CustomerTurnOver")).Returns(_objectDb);

            _reader = Substitute.For<IDataReader>();
            _objectDb.ExecuteReader().Returns(_reader);

            _metaModel = new CustomerTurnoverMetaModel
            {
                Username = "UsernameTest"
            };
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _objectDb = null;
            _dataAccess = null;
            _reader = null;
            _metaModel = null;
            _balance = null;
        }

        [Test]
        public void GetDataList_WhenCall_ParametersPassedCorrectly()
        {
            // Action
            _dataAccess.GetDataList(_metaModel, LanguageCode);

            // Assert
            _objectDb.Received().AddInParameter("licenseeid", DbType.Int32, _metaModel.LicenseeId);
            _objectDb.Received().AddInParameter("userid", DbType.Int32, _metaModel.UserId);
            _objectDb.Received().AddInParameter("fromdate", DbType.DateTime, _metaModel.FromDate);
            _objectDb.Received().AddInParameter("todate", DbType.DateTime, _metaModel.ToDate);
            _objectDb.Received().AddInParameter("odds", DbType.Decimal, _metaModel.Odds);
            _objectDb.Received().AddInParameter("reportby", DbType.Int32, _metaModel.ReportBy);
            _objectDb.Received().AddInParameter("username", DbType.String, _metaModel.Username);
            _objectDb.Received().AddInParameter("movebetsday", DbType.DateTime, _balance.MoveBetsDay);
            _objectDb.Received().AddInParameter("balanceupday", DbType.DateTime, _balance.BalanceUpDay);
            _objectDb.Received().AddInParameter("Langkey", DbType.String, LanguageCode);
            _objectDb.Received().AddInParameter("PageSize", DbType.Int32, _metaModel.PageSize);
            _objectDb.Received().AddInParameter("PageIndex", DbType.Int32, _metaModel.PageIndex);
            _objectDb.Received().AddInParameter("currencyid", DbType.Int32, _metaModel.CurrencyId);
            _objectDb.Received().AddInParameter("IsExportExcel", DbType.Boolean, _metaModel.IsExportExcel);
            _objectDb.Received().AddInParameter("showwin", DbType.Boolean, _metaModel.ShowWin);
            _objectDb.Received().AddInParameter("showloss", DbType.Boolean, _metaModel.ShowLoss);
            _objectDb.Received().AddInParameter("showdraw", DbType.Boolean, _metaModel.ShowDraw);
            _objectDb.Received().AddInParameter("Product", DbType.String, _metaModel.ProductParameters);
            _objectDb.Received().AddInParameter("Bettype", DbType.String, _metaModel.BettypeParameters);
        }

        [Test]
        public void GetDataList_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            _reader.Read().Returns(true, false, true, false);
            _reader.NextResult().Returns(true);
            var expectedTotalData = new CustTurnover
            {
                TotalTransactions = 5,
                Turnover = 10m,
                TotalTurnover = 10m
            };
            var expectedResult = new CustTurnover
            {
                UserName = "UserNameTest",
                Currency = "CurrencyTest",
                Bettype = "BettypeTest",
                SportId = 1,
                ReportBy = new DateTime(2016, 8, 31)
            };

            ConfigureReaderForTotalData(expectedTotalData);
            ConfigureReaderForResult(expectedResult);

            // Action
            var result = _dataAccess.GetDataList(_metaModel, LanguageCode);
            var firstItem = result.DataList.First();
            var totalData = result.TotalData;

            // Assert
            Assert.That(firstItem.Turnover, Is.EqualTo(expectedResult.Turnover));
            Assert.That(firstItem.SportId, Is.EqualTo(expectedResult.SportId));
            Assert.That(firstItem.ReportBy, Is.EqualTo(expectedResult.ReportBy));
            Assert.That(firstItem.UserName, Is.EqualTo(expectedResult.UserName));
            Assert.That(firstItem.Currency, Is.EqualTo(expectedResult.Currency));
            Assert.That(firstItem.TotalTransactions, Is.EqualTo(expectedResult.TotalTransactions));
            Assert.That(firstItem.Bettype, Is.EqualTo(expectedResult.Bettype));
            Assert.That(firstItem.CustomerWinLoss, Is.EqualTo(expectedResult.CustomerWinLoss));
            Assert.That(firstItem.EffectiveBettingValue, Is.EqualTo(expectedResult.EffectiveBettingValue));
            Assert.That(firstItem.CustomerComm, Is.EqualTo(expectedResult.CustomerComm));

            Assert.That(totalData.TotalTransactions, Is.EqualTo(expectedTotalData.TotalTransactions));
            Assert.That(totalData.Turnover, Is.EqualTo(expectedTotalData.Turnover));
            Assert.That(totalData.CustomerWinLoss, Is.EqualTo(expectedTotalData.CustomerWinLoss));
            Assert.That(totalData.CustomerComm, Is.EqualTo(expectedTotalData.CustomerComm));
            Assert.That(totalData.EffectiveBettingValue, Is.EqualTo(expectedTotalData.EffectiveBettingValue));

            Assert.That(result.TotalRows, Is.EqualTo(100));
        }

        private void ConfigureReaderForResult(CustTurnover expected)
        {
            _reader["Turnover"].Returns(expected.Turnover);
            _reader["sporttype"].Returns(expected.SportId);
            _reader["ReportBy"].Returns("20160831");
            _reader["username"].Returns(expected.UserName);
            _reader["currency"].Returns(expected.Currency);
            _reader["BetCount"].Returns(expected.TotalTransactions);
            _reader["Bettype"].Returns(expected.Bettype);
            _reader["Winlost"].Returns(expected.CustomerWinLoss);
            _reader["BetEffective"].Returns(expected.EffectiveBettingValue);
            _reader["PlayerComm"].Returns(expected.CustomerComm);
        }

        private void ConfigureReaderForTotalData(CustTurnover expected)
        {
            _reader["SumBetCount"].Returns(expected.TotalTransactions);
            _reader["SumTurnover"].Returns(expected.Turnover);
            _reader["SumWinlost"].Returns(expected.CustomerWinLoss);
            _reader["SumComm"].Returns(expected.CustomerComm);
            _reader["SumBetEffective"].Returns(expected.EffectiveBettingValue);

            _reader["TotalRows"].Returns(100);
        }
    }
}