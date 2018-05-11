namespace Fanex.Athena.DataAccess.Report.UnitTests
{
    using System;
    using System.Data;
    using System.Linq;
    using Core.Configuration.Interfaces;
    using Core.UnitTests.TestHelpers;
    using Data;
    using Model.Core.DTO.Configuration;
    using Model.Report.DTO;
    using Model.Report.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using VendorAdapter.Interfaces;

    [TestFixture]
    public class CustomerTurnoverDataAccessTest : BaseDataAccessHelperTest<CustomerTurnoverDataAccess>
    {
        private const string LanguageCode = "en-us";

        private BalanceUp _balance;

        private CustomerTurnoverMetaModel _metaModel;

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

            DataAccess = Substitute.ForPartsOf<CustomerTurnoverDataAccess>(
                CreateSessionStorage(),
                balanceUpDataAccess,
                Substitute.For<ICacheRefAdapter>());
            DataAccess.GetObjectDb(Arg.Is("Lrf_Rpt_CustomerTurnOver")).Returns(ObjectDb);

            _metaModel = new CustomerTurnoverMetaModel
            {
                Username = "UsernameTest"
            };
        }

        [OneTimeTearDown]
        public override void FixtureTearDown()
        {
            _balance = null;
            _metaModel = null;

            base.FixtureTearDown();
        }

        [TestCase(null, null)]
        [TestCase("", null)]
        [TestCase("User from test", "User from test")]
        public void GetDataList_WhenCall_ParametersPassedCorrectly(string metaModelUser, string expectedUser)
        {
            // Arrange
            _metaModel.Username = metaModelUser;

            // Action
            DataAccess.GetDataList(_metaModel, LanguageCode);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received(1).AddInParameter("licenseeid", DbType.Int32, _metaModel.LicenseeId);
                ObjectDb.Received(1).AddInParameter("userid", DbType.Int32, _metaModel.UserId);
                ObjectDb.Received(1).AddInParameter("fromdate", DbType.DateTime, _metaModel.FromDate);
                ObjectDb.Received(1).AddInParameter("todate", DbType.DateTime, _metaModel.ToDate);
                ObjectDb.Received(1).AddInParameter("odds", DbType.Decimal, _metaModel.Odds);
                ObjectDb.Received(1).AddInParameter("reportby", DbType.Int32, _metaModel.ReportBy);
                ObjectDb.Received(1).AddInParameter("username", DbType.String, expectedUser);
                ObjectDb.Received(1).AddInParameter("movebetsday", DbType.DateTime, _balance.MoveBetsDay);
                ObjectDb.Received(1).AddInParameter("balanceupday", DbType.DateTime, _balance.BalanceUpDay);
                ObjectDb.Received(1).AddInParameter("Langkey", DbType.String, LanguageCode);
                ObjectDb.Received(1).AddInParameter("PageSize", DbType.Int32, _metaModel.PageSize);
                ObjectDb.Received(1).AddInParameter("PageIndex", DbType.Int32, _metaModel.PageIndex);
                ObjectDb.Received(1).AddInParameter("currencyid", DbType.Int32, _metaModel.CurrencyId);
                ObjectDb.Received(1).AddInParameter("IsExportExcel", DbType.Boolean, _metaModel.IsExportExcel);
                ObjectDb.Received(1).AddInParameter("showwin", DbType.Boolean, _metaModel.ShowWin);
                ObjectDb.Received(1).AddInParameter("showloss", DbType.Boolean, _metaModel.ShowLoss);
                ObjectDb.Received(1).AddInParameter("showdraw", DbType.Boolean, _metaModel.ShowDraw);
                ObjectDb.Received(1).AddInParameter("Product", DbType.String, _metaModel.ProductParameters);
                ObjectDb.Received(1).AddInParameter("Bettype", DbType.String, _metaModel.BettypeParameters);
            });
        }

        [Test]
        public void GetDataList_NoData_ReturnEmptyResult()
        {
            // Arrange
            Reader.Read().Returns(false);
            Reader.NextResult().Returns(false);

            // Action
            var result = DataAccess.GetDataList(_metaModel, LanguageCode);

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
            var expectedResult = new CustTurnover
            {
                UserName = "UserNameTest",
                Bettype = "BettypeTest"
            };

            Reader.Read().Returns(true, false);
            Reader.NextResult().Returns(false);
            ConfigureReaderForResult(expectedResult);

            // Action
            var result = DataAccess.GetDataList(_metaModel, LanguageCode);

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
            Reader.Read().Returns(true, false, true, false);
            Reader.NextResult().Returns(true);
            var expectedTotalData = new CustTurnover
            {
                TotalTransactions = 5,
                Turnover = 10m,
                TotalTurnover = 10m
            };
            var expectedResult = new CustTurnover
            {
                UserName = "User name test",
                Bettype = "BettypeTest",
                SportId = 1,
                ReportBy = new DateTime(2016, 8, 31)
            };
            const int ExpectedTotalRow = 100;

            ConfigureReaderForTotalData(expectedTotalData, ExpectedTotalRow);
            ConfigureReaderForResult(expectedResult);

            // Action
            var result = DataAccess.GetDataList(_metaModel, LanguageCode);
            var firstItem = result.DataList.First();
            var totalData = result.TotalData;

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(firstItem.SportId, Is.EqualTo(expectedResult.SportId));
                Assert.That(firstItem.ReportBy, Is.EqualTo(expectedResult.ReportBy));
                Assert.That(firstItem.UserName, Is.EqualTo(expectedResult.UserName));
                Assert.That(firstItem.Currency, Is.EqualTo(CurrencyName));
                Assert.That(firstItem.Bettype, Is.EqualTo(expectedResult.Bettype));
                AssertCommonData(firstItem, expectedResult);

                AssertCommonData(totalData, expectedTotalData);

                Assert.That(result.TotalRows, Is.EqualTo(ExpectedTotalRow));
            });
        }

        private static void AssertCommonData(CustTurnover actual, CustTurnover expected)
        {
            Assert.That(actual.TotalTransactions, Is.EqualTo(expected.TotalTransactions));
            Assert.That(actual.Turnover, Is.EqualTo(expected.Turnover));
            Assert.That(actual.CustomerWinLoss, Is.EqualTo(expected.CustomerWinLoss));
            Assert.That(actual.CustomerComm, Is.EqualTo(expected.CustomerComm));
            Assert.That(actual.EffectiveBettingValue, Is.EqualTo(expected.EffectiveBettingValue));
        }

        private void ConfigureReaderForResult(CustTurnover expected)
        {
            Reader["RowNo"].Returns(expected.Index);
            Reader["Turnover"].Returns(expected.Turnover);
            Reader["sporttype"].Returns(expected.SportId);
            Reader["ReportBy"].Returns("20160831");
            Reader["username"].Returns(expected.UserName);
            Reader["currencyid"].Returns(CurrencyId);
            Reader["BetCount"].Returns(expected.TotalTransactions);
            Reader["Bettype"].Returns(expected.Bettype);
            Reader["Winlost"].Returns(expected.CustomerWinLoss);
            Reader["BetEffective"].Returns(expected.EffectiveBettingValue);
            Reader["PlayerComm"].Returns(expected.CustomerComm);
        }

        private void ConfigureReaderForTotalData(CustTurnover expected, int expectedTotalRow)
        {
            Reader["SumBetCount"].Returns(expected.TotalTransactions);
            Reader["SumTurnover"].Returns(expected.Turnover);
            Reader["SumWinlost"].Returns(expected.CustomerWinLoss);
            Reader["SumComm"].Returns(expected.CustomerComm);
            Reader["SumBetEffective"].Returns(expected.EffectiveBettingValue);

            Reader["TotalRows"].Returns(expectedTotalRow);
        }
    }
}