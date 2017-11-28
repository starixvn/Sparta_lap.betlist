namespace Fanex.Athena.DataAccess.Report.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Core.Configuration.Interfaces;
    using Data;
    using Extension.Utility;
    using Model.Core.DTO.Configuration;
    using Model.Core.MetaModel.Product;
    using Model.Report.DTO;
    using Model.Report.MetaModel;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class CustomerWinlossDataAccessTest
    {
        private IObjectDb _objectDb;
        private CustomerWinlossDataAccess _dataAccess;
        private BalanceUp _balance;
        private IDataReader _reader;
        private CustomerWinLossMetaModel _metaModel;

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

            _dataAccess = Substitute.ForPartsOf<CustomerWinlossDataAccess>(balanceUpDataAccess);
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_Rpt_WinLoss")).Returns(_objectDb);

            _reader = Substitute.For<IDataReader>();
            _objectDb.ExecuteReader().Returns(_reader);

            var appSetting = Substitute.For<IAppSettings>();
            appSetting.HistoryDate.Returns(new DateTime(2016, 8, 31));
            _metaModel = new CustomerWinLossMetaModel(appSetting)
            {
                ProductOptions = GetProductOptions(),
                BaseCurrency = "UUS"
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
            _dataAccess.GetDataList(_metaModel);

            // Assert
            _objectDb.Received().AddInParameter("fromdate", DbType.DateTime, _metaModel.FromDate);
            _objectDb.Received().AddInParameter("todate", DbType.DateTime, _metaModel.ToDate);
            _objectDb.Received().AddInParameter("pagesize", DbType.Int32, _metaModel.PageSize);
            _objectDb.Received().AddInParameter("pageindex", DbType.Int32, _metaModel.PageIndex);
            _objectDb.Received().AddInParameter("movebetsday", DbType.DateTime, _balance.MoveBetsDay);
            _objectDb.Received().AddInParameter("isold", DbType.Boolean, _metaModel.IsOld);
            _objectDb.Received().AddInParameter("AgentList", DbType.String, _metaModel.AgentListXml);
            _objectDb.Received().AddInParameter("showSB", DbType.Boolean, true);
            _objectDb.Received().AddInParameter("showNG", DbType.Boolean, false);
        }

        [Test]
        public void GetDataList_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            _reader.Read().Returns(true, false, true, false);
            _reader.NextResult().Returns(true);
            var expectedTotalData = new CustomerWinLoss
            {
                LicenseeWL = 10m,
                RacingRevenue = 20m,
                Turnover = 25m
            };
            var expectedResult = new CustomerWinLoss
            {
                ExternalId = "ExternalIdTest",
                SystemId = "SystemIdTest",
                CustomerWL = 10m,
                PlayerComm = 5m,
                CustomerTotalWL = 15m,
                RacingRevenue = 15m
            };
            ConfigureReaderForResult(expectedResult);
            ConfigureReaderForTotalData(expectedTotalData);

            // Action
            var result = _dataAccess.GetDataList(_metaModel);
            var firstItem = result.DataList.First();

            // Assert
            Assert.That(firstItem.CustId, Is.EqualTo(expectedResult.CustId));
            Assert.That(firstItem.ExternalId, Is.EqualTo(expectedResult.ExternalId));
            Assert.That(firstItem.SystemId, Is.EqualTo(expectedResult.SystemId));
            Assert.That(firstItem.Turnover, Is.EqualTo(expectedResult.Turnover));
            Assert.That(firstItem.TotalTransaction, Is.EqualTo(expectedResult.TotalTransaction));
            Assert.That(firstItem.CustomerWL, Is.EqualTo(expectedResult.CustomerWL));
            Assert.That(firstItem.BetEffective, Is.EqualTo(expectedResult.BetEffective));
            Assert.That(firstItem.PlayerComm, Is.EqualTo(expectedResult.PlayerComm));
            Assert.That(firstItem.CustomerTotalWL, Is.EqualTo(expectedResult.CustomerTotalWL));
            Assert.That(firstItem.RacingRevenue, Is.EqualTo(expectedResult.RacingRevenue));

            Assert.That(result.TotalData.Turnover, Is.EqualTo(expectedTotalData.Turnover));
            Assert.That(result.TotalData.LicenseeWL, Is.EqualTo(expectedTotalData.LicenseeWL));
            Assert.That(result.TotalData.RacingRevenue, Is.EqualTo(expectedTotalData.RacingRevenue));
            Assert.That(result.TotalData.TotalTransaction, Is.EqualTo(expectedTotalData.TotalTransaction));
            Assert.That(result.TotalData.CustomerWL, Is.EqualTo(expectedTotalData.CustomerWL));
            Assert.That(result.TotalData.PlayerComm, Is.EqualTo(expectedTotalData.PlayerComm));

            Assert.That(result.TotalRows, Is.EqualTo(100));
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

            _reader["CustID"].Returns(expected.CustId);
            _reader["nickname"].Returns(expected.ExternalId);
            _reader["username"].Returns(expected.SystemId);
            _reader[turnoverColumn].Returns(expected.Turnover);
            _reader["betcount"].Returns(expected.TotalTransaction);
            _reader[memberWinLostColumn].Returns(expected.CustomerWL);
            _reader[betEffectiveColumn].Returns(expected.BetEffective);
            _reader[playerCommColumn].Returns(expected.PlayerComm);
            _reader[licenseeWinLostColumn].Returns(expected.LicenseeWL);
            _reader[bacommColumn].Returns(expected.BAComm);
            _reader[winlossrbColumn].Returns(10m);
            _reader[commrbColumn].Returns(5m);
            _reader[amtduetobaColumn].Returns(expected.AmtDueToBA);
            _reader[amtduetorbColumn].Returns(expected.AmtDueToRacing);
            _reader[customertotalwlColumn].Returns(expected.CustomerTotalWL);
            _reader[racingrevenueColumn].Returns(expected.RacingRevenue);
        }

        private void ConfigureReaderForTotalData(CustomerWinLoss expected)
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

            _reader[turnoverColumn].Returns(expected.Turnover);
            _reader["sumbetcount"].Returns(expected.TotalTransaction);
            _reader[memberWinLostColumn].Returns(expected.CustomerWL);
            _reader[betEffectiveColumn].Returns(expected.BetEffective);
            _reader[playerCommColumn].Returns(expected.PlayerComm);
            _reader[licenseeWinLostColumn].Returns(expected.LicenseeWL);
            _reader[bacommColumn].Returns(expected.BAComm);
            _reader[winlossrbColumn].Returns(15m);
            _reader[commrbColumn].Returns(5m);
            _reader[amtduetobaColumn].Returns(expected.AmtDueToBA);
            _reader[amtduetorbColumn].Returns(expected.AmtDueToRacing);
            _reader[sumracingrevenueColumn].Returns(expected.RacingRevenue);
            _reader[sumcustomertotalwlColumn].Returns(expected.CustomerTotalWL);

            _reader["TotalRows"].Returns(100);
        }
    }
}