namespace Fanex.Athena.DataAccess.Report.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Data;
    using Extension.Utility;
    using Model.Report.DTO;
    using Model.Report.MetaModel;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class CreditBalanceDataAccessTest
    {
        private IObjectDb _objectDb;
        private CreditBalanceDataAccess _dataAccess;
        private IDataReader _reader;

        [SetUp]
        public void Setup()
        {
            _objectDb = Substitute.For<IObjectDb>();

            _dataAccess = Substitute.ForPartsOf<CreditBalanceDataAccess>();
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_CustomerSelBalance")).Returns(_objectDb);

            _reader = Substitute.For<IDataReader>();
            _objectDb.ExecuteReader(Arg.Any<object>()).Returns(_reader);
        }

        [Test]
        public void GetReportData_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            _reader.Read().Returns(true, false);
            var appSetting = Substitute.For<IAppSettings>();
            appSetting.HistoryDate.Returns(new DateTime(2016, 8, 30));
            var metaModel = new BalanceMetaModel(appSetting) { BaseCurrency = "UUS" };

            var selectedMonthBalance = new BalanceDetails { LicWinLoss = 10m, BetTradeCommission = 10.5m };
            ConfigureReaderForBalanceList(selectedMonthBalance, metaModel.BaseCurrency, "SelectedMonth");

            var yesterdayBalance = new BalanceDetails { LicWinLoss = 11m, BetTradeCommission = 11.5m };
            ConfigureReaderForBalanceList(yesterdayBalance, metaModel.BaseCurrency, "Yesterday");

            var todayBalance = new BalanceDetails { LicWinLoss = 12m, BetTradeCommission = 12.5m };
            ConfigureReaderForBalanceList(todayBalance, metaModel.BaseCurrency, "Today");

            var expected = new Balance
            {
                BalanceList = new List<BalanceDetails> { selectedMonthBalance, yesterdayBalance, todayBalance },
                CustomerPendingFunds = 20m,
                TotalCustomerActive = 100,
                TotalCustomerClosed = 200,
                TotalCustomerSuspended = 300,
                UserName = "UserNameTest"
            };
            ConfigureReaderForAnotherColumns(expected);

            // Action
            var result = _dataAccess.GetReportData(metaModel);

            // Assert
            Assert.That(result.BalanceList[0].LicWinLoss, Is.EqualTo(selectedMonthBalance.LicWinLoss));
            Assert.That(result.BalanceList[0].BetTradeCommission, Is.EqualTo(selectedMonthBalance.BetTradeCommission));
            Assert.That(result.BalanceList[0].BACommission, Is.EqualTo(selectedMonthBalance.BACommission));

            Assert.That(result.BalanceList[1].LicWinLoss, Is.EqualTo(yesterdayBalance.LicWinLoss));
            Assert.That(result.BalanceList[1].BetTradeCommission, Is.EqualTo(yesterdayBalance.BetTradeCommission));
            Assert.That(result.BalanceList[1].BACommission, Is.EqualTo(yesterdayBalance.BACommission));

            Assert.That(result.BalanceList[2].LicWinLoss, Is.EqualTo(todayBalance.LicWinLoss));
            Assert.That(result.BalanceList[2].BetTradeCommission, Is.EqualTo(todayBalance.BetTradeCommission));
            Assert.That(result.BalanceList[2].BACommission, Is.EqualTo(todayBalance.BACommission));

            Assert.That(result.CustomerPendingFunds, Is.EqualTo(expected.CustomerPendingFunds));
            Assert.That(result.TotalCustomerActive, Is.EqualTo(expected.TotalCustomerActive));
            Assert.That(result.TotalCustomerClosed, Is.EqualTo(expected.TotalCustomerClosed));
            Assert.That(result.TotalCustomerSuspended, Is.EqualTo(expected.TotalCustomerSuspended));
            Assert.That(result.UserName, Is.EqualTo(expected.UserName));
        }

        private void ConfigureReaderForBalanceList(BalanceDetails expected, string baseCurrency, string prefix)
        {
            var monthLicColumn = prefix + "LicenseeWL" + baseCurrency;
            var betTradeCommColumn = prefix + "BetTradeComm" + baseCurrency;
            var bacommColumn = prefix + "CommBA" + baseCurrency;
            var rbcommColumn = prefix + "CommRB" + baseCurrency;
            var racingRevenueColumn = prefix + "WinLossRB" + baseCurrency;
            var amtDueToBetTradeColumn = prefix + "AmtDueToBT" + baseCurrency;
            var amtDueToBaColumn = prefix + "AmtDuetoBA" + baseCurrency;
            var amtDueToRacingColumn = prefix + "AmtDuetoRB" + baseCurrency;
            var customerCommColumn = prefix + "PlayerComm" + baseCurrency;

            _reader[monthLicColumn].Returns(expected.LicWinLoss);
            _reader[betTradeCommColumn].Returns(expected.BetTradeCommission);
            _reader[bacommColumn].Returns(expected.BACommission);
            _reader[rbcommColumn].Returns(expected.RBCommission);
            _reader[racingRevenueColumn].Returns(expected.RacingRevenue);
            _reader[amtDueToBetTradeColumn].Returns(expected.AmtDueToBetTrade);
            _reader[amtDueToBaColumn].Returns(expected.AmtDueToBA);
            _reader[amtDueToRacingColumn].Returns(expected.AmtDueToRacing);
            _reader[customerCommColumn].Returns(expected.CustomerCommission);
        }

        private void ConfigureReaderForAnotherColumns(Balance expected)
        {
            _reader["MemberOutstandinguus"].Returns(expected.CustomerPendingFunds);
            _reader["TotalMemberActive"].Returns(expected.TotalCustomerActive);
            _reader["TotalMemberClosed"].Returns(expected.TotalCustomerClosed);
            _reader["TotalMemberSuspended"].Returns(expected.TotalCustomerSuspended);
            _reader["UserName"].Returns(expected.UserName);
        }
    }
}