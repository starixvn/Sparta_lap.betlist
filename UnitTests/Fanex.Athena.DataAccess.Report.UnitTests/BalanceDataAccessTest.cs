namespace Fanex.Athena.DataAccess.Report.UnitTests
{
    using System;
    using System.Collections.Generic;
    using Core.Configuration.Interfaces;
    using Core.UnitTests.TestHelpers;
    using Extension.Utility;
    using Model.Core.DTO.Configuration;
    using Model.Report.DTO;
    using Model.Report.MetaModel;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class BalanceDataAccessTest : BaseDataAccessHelperTest<BalanceDataAccess>
    {
        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();
            DataAccess.GetObjectDb(Arg.Is("Lrf_CustomerSelBalance")).Returns(ObjectDb);
        }

        protected override void SetUpDataAccess()
        {
            var balanceUpDataAccess = Substitute.For<IBalanceUpDataAccess>();
            balanceUpDataAccess.GetBalanceUp().Returns(new BalanceUp
            {
                BalanceUpDay = new DateTime(2017, 12, 1),
                MoveBetsDay = new DateTime(2017, 12, 2)
            });

            DataAccess = Substitute.ForPartsOf<BalanceDataAccess>(
                AppSetting,
                CreateSessionStorage(),
                balanceUpDataAccess);
        }

        [Test]
        public void GetReportData_NoData_ReturnNullList()
        {
            // Arrange
            var appSetting = Substitute.For<IAppSettings>();
            appSetting.HistoryDate.Returns(new DateTime(2016, 8, 30));
            var metaModel = new BalanceMetaModel(appSetting) { BaseCurrency = "UUS" };
            Reader.Read().Returns(false);

            // Action
            var result = DataAccess.GetReportData(metaModel);

            // Assert
            Assert.That(result.BalanceList, Is.Null);
        }

        [Test]
        public void GetReportData_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            Reader.Read().Returns(true, false);
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
            var result = DataAccess.GetReportData(metaModel);
            var actualSelectedMonthBalance = result.BalanceList[0];
            var actualYesterdayBalance = result.BalanceList[1];
            var actualTodayBalance = result.BalanceList[2];

            // Assert
            Assert.Multiple(() =>
            {
                AssertBalanceData(actualSelectedMonthBalance, selectedMonthBalance);
                AssertBalanceData(actualYesterdayBalance, yesterdayBalance);
                AssertBalanceData(actualTodayBalance, todayBalance);

                Assert.That(result.CustomerPendingFunds, Is.EqualTo(expected.CustomerPendingFunds));
                Assert.That(result.TotalCustomerActive, Is.EqualTo(expected.TotalCustomerActive));
                Assert.That(result.TotalCustomerClosed, Is.EqualTo(expected.TotalCustomerClosed));
                Assert.That(result.TotalCustomerSuspended, Is.EqualTo(expected.TotalCustomerSuspended));
                Assert.That(result.UserName, Is.EqualTo(expected.UserName));
            });
        }

        private static void AssertBalanceData(BalanceDetails actual, BalanceDetails expected)
        {
            Assert.That(actual.LicWinLoss, Is.EqualTo(expected.LicWinLoss));
            Assert.That(actual.BetTradeCommission, Is.EqualTo(expected.BetTradeCommission));
            Assert.That(actual.BACommission, Is.EqualTo(expected.BACommission));
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

            Reader[monthLicColumn].Returns(expected.LicWinLoss);
            Reader[betTradeCommColumn].Returns(expected.BetTradeCommission);
            Reader[bacommColumn].Returns(expected.BACommission);
            Reader[rbcommColumn].Returns(expected.RBCommission);
            Reader[racingRevenueColumn].Returns(expected.RacingRevenue);
            Reader[amtDueToBetTradeColumn].Returns(expected.AmtDueToBetTrade);
            Reader[amtDueToBaColumn].Returns(expected.AmtDueToBA);
            Reader[amtDueToRacingColumn].Returns(expected.AmtDueToRacing);
            Reader[customerCommColumn].Returns(expected.CustomerCommission);
        }

        private void ConfigureReaderForAnotherColumns(Balance expected)
        {
            Reader["MemberOutstandinguus"].Returns(expected.CustomerPendingFunds);
            Reader["TotalMemberActive"].Returns(expected.TotalCustomerActive);
            Reader["TotalMemberClosed"].Returns(expected.TotalCustomerClosed);
            Reader["TotalMemberSuspended"].Returns(expected.TotalCustomerSuspended);
            Reader["UserName"].Returns(expected.UserName);
        }
    }
}