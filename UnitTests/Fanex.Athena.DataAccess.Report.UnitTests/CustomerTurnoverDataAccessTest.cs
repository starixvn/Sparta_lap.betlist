namespace Fanex.Athena.DataAccess.Report.UnitTests
{
    using System;
    using System.Data;
    using Data;
    using Model.Core.DTO.Configuration;
    using Model.Report.DTO;
    using Model.Report.MetaModel;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class CustomerTurnoverDataAccessTest
    {
        private IObjectDb _objectDb;
        private CustomerTurnoverDataAccess _dataAccess;
        private BalanceUp _balance;
        private IDataReader _reader;
        private CustomerTurnoverMetaModel _metaModel;
        private CustTurnover _totalData;
        private int _totalRow = 0;
        private string _languageCode = "en-us";

        [SetUp]
        public void Setup()
        {
            _objectDb = Substitute.For<IObjectDb>();
            _balance = new BalanceUp
            {
                MoveBetsDay = new DateTime(2016, 8, 10),
                BalanceUpDay = new DateTime(2016, 8, 31)
            };

            _dataAccess = Substitute.ForPartsOf<CustomerTurnoverDataAccess>(_balance);
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_Rpt_CustomerTurnOver")).Returns(_objectDb);

            _reader = Substitute.For<IDataReader>();
            _objectDb.ExecuteReader().Returns(_reader);

            _metaModel = new CustomerTurnoverMetaModel
            {
                Username = "UsernameTest"
            };
            _totalData = new CustTurnover();
        }

        [Test]
        public void GetDataList_WhenCall_ParametersPassedCorrectly()
        {
            // Action
            _dataAccess.GetDataList(_metaModel, _languageCode, out _totalData, out _totalRow);

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
            _objectDb.Received().AddInParameter("Langkey", DbType.String, _languageCode);
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
            var result = _dataAccess.GetDataList(_metaModel, _languageCode, out _totalData, out _totalRow)[0];

            // Assert
            Assert.That(result.Turnover, Is.EqualTo(expectedResult.Turnover));
            Assert.That(result.SportId, Is.EqualTo(expectedResult.SportId));
            Assert.That(result.ReportBy, Is.EqualTo(expectedResult.ReportBy));
            Assert.That(result.UserName, Is.EqualTo(expectedResult.UserName));
            Assert.That(result.Currency, Is.EqualTo(expectedResult.Currency));
            Assert.That(result.TotalTransactions, Is.EqualTo(expectedResult.TotalTransactions));
            Assert.That(result.Bettype, Is.EqualTo(expectedResult.Bettype));
            Assert.That(result.CustomerWinLoss, Is.EqualTo(expectedResult.CustomerWinLoss));
            Assert.That(result.EffectiveBettingValue, Is.EqualTo(expectedResult.EffectiveBettingValue));
            Assert.That(result.CustomerComm, Is.EqualTo(expectedResult.CustomerComm));

            Assert.That(_totalData.TotalTransactions, Is.EqualTo(expectedTotalData.TotalTransactions));
            Assert.That(_totalData.Turnover, Is.EqualTo(expectedTotalData.Turnover));
            Assert.That(_totalData.CustomerWinLoss, Is.EqualTo(expectedTotalData.CustomerWinLoss));
            Assert.That(_totalData.CustomerComm, Is.EqualTo(expectedTotalData.CustomerComm));
            Assert.That(_totalData.EffectiveBettingValue, Is.EqualTo(expectedTotalData.EffectiveBettingValue));

            Assert.That(_totalRow, Is.EqualTo(100));
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