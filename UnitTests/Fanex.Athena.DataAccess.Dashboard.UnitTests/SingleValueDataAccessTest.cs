namespace Fanex.Athena.DataAccess.Dashboard.UnitTests
{
    using System.Collections;
    using Core.UnitTests.TestHelpers;
    using Model.Dashboard.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using UnitTest.Common.Helpers;

    [TestFixture]
    public class SingleValueDataAccessTest : BaseDataAccessHelperTest<SingleValueDataAccess>
    {
        private const string GetCustomerBalanceSp = "Lrf_GetCustomerBalance";

        private const string GetPendingFundsSp = "Lrf_GetLicenseePendingFunds";

        private const string GetTodayDepositSp = "Lrf_GetLicenseeTodayDeposit";

        private const string GetTodayWithdrawalSp = "Lrf_GetLicenseeTodayWithdrawal";

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();

            DataAccess.GetObjectDb(Arg.Any<string>()).Returns(ObjectDb);
        }

        [Test]
        public void GetPendingFunds_WhenCall_PassCorrectParam()
        {
            // Arrange
            var metaModel = new DashboardBaseModel
            {
                LicenseeId = 1,
                CurrencyIDList = "1,2,3",
                BaseCurrency = "EUR"
            };
            Hashtable passedParam = null;
            ObjectDb.When(x => x.ExecuteReader(Arg.Any<Hashtable>())).Do(x => passedParam = (Hashtable)x[0]);

            // Action
            DataAccess.GetPendingFunds(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(passedParam.Count, Is.EqualTo(2));
                Assert.That(passedParam["LicenseeID"], Is.EqualTo(metaModel.LicenseeId));
                Assert.That(passedParam["CurrencyIDList"], Is.EqualTo(metaModel.CurrencyIDList));
            });
        }

        [Test]
        public void GetPendingFunds_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var expected = 500000M;

            Reader.Read().Returns(true, false);
            Reader["PendingFundEUR"].Returns(expected);

            var metaModel = new DashboardBaseModel { BaseCurrency = "EUR" };

            // Action
            var result = DataAccess.GetPendingFunds(metaModel);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void GetTodayWithdrawal_WhenCall_PassCorrectParam()
        {
            // Arrange
            var metaModel = new DashboardBaseModel
            {
                LicenseeId = 1,
                CurrencyIDList = "1,2,3",
                BaseCurrency = "EUR",
                FromDate = new System.DateTime(2017, 1, 1)
            };
            Hashtable passedParam = null;
            ObjectDb.When(x => x.ExecuteReader(Arg.Any<Hashtable>())).Do(x => passedParam = (Hashtable)x[0]);

            // Action
            DataAccess.GetTodayWithdrawal(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(passedParam.Count, Is.EqualTo(3));
                Assert.That(passedParam["LicenseeID"], Is.EqualTo(metaModel.LicenseeId));
                Assert.That(passedParam["CurrencyIDList"], Is.EqualTo(metaModel.CurrencyIDList));
                Assert.That(passedParam["ReportDate"], Is.EqualTo(metaModel.FromDate));
            });
        }

        [Test]
        public void GetTodayWithdrawal_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var expected = 500000M;

            Reader.Read().Returns(true, false);
            Reader["WithdrawalEUR"].Returns(expected);

            var metaModel = new DashboardBaseModel { BaseCurrency = "EUR" };

            // Action
            var result = DataAccess.GetTodayWithdrawal(metaModel);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void GetCustomerBalance_WhenCall_PassCorrectParam()
        {
            // Arrange
            var metaModel = FakeHelper.Create<DashboardBaseModel>();
            Hashtable passedParam = null;
            ObjectDb.When(x => x.ExecuteReader(Arg.Any<Hashtable>())).Do(x => passedParam = (Hashtable)x[0]);

            // Action
            DataAccess.GetCustomerBalance(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(passedParam.Count, Is.EqualTo(3));
                Assert.That(passedParam["LicenseeID"], Is.EqualTo(metaModel.LicenseeId));
                Assert.That(passedParam["CurrencyIDList"], Is.EqualTo(metaModel.CurrencyIDList));
                Assert.That(passedParam["ReportDate"], Is.EqualTo(metaModel.FromDate));
            });
        }

        [Test]
        public void GetCustomerBalance_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var expected = 500000M;

            Reader.Read().Returns(true, false);
            Reader["ClosingBalanceEUR"].Returns(expected);

            var metaModel = new DashboardBaseModel { BaseCurrency = "EUR" };

            // Action
            var result = DataAccess.GetCustomerBalance(metaModel);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void GetTodayDeposit_WhenCall_PassCorrectParam()
        {
            // Arrange
            var metaModel = FakeHelper.Create<DashboardBaseModel>();
            Hashtable passedParam = null;
            ObjectDb.When(x => x.ExecuteReader(Arg.Any<Hashtable>())).Do(x => passedParam = (Hashtable)x[0]);

            // Action
            DataAccess.GetTodayDeposit(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(passedParam.Count, Is.EqualTo(3));
                Assert.That(passedParam["LicenseeID"], Is.EqualTo(metaModel.LicenseeId));
                Assert.That(passedParam["CurrencyIDList"], Is.EqualTo(metaModel.CurrencyIDList));
                Assert.That(passedParam["ReportDate"], Is.EqualTo(metaModel.FromDate));
            });
        }

        [Test]
        public void GetTodayDeposit_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var expected = 500000M;

            Reader.Read().Returns(true, false);
            Reader["DepositEUR"].Returns(expected);

            var metaModel = new DashboardBaseModel { BaseCurrency = "EUR" };

            // Action
            var result = DataAccess.GetTodayDeposit(metaModel);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}