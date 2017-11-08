namespace Fanex.Athena.DataAccess.Dashboard.UnitTests
{
    using System.Collections;
    using Model.Dashboard.MetaModel;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class TodayWithdrawalDataAccessTest
    {
        private SingleValueDataAccess _todayWithDrawalDataAccess;

        [OneTimeSetUp]
        public void SetUp()
        {
            _todayWithDrawalDataAccess = Substitute.ForPartsOf<SingleValueDataAccess>();
            _todayWithDrawalDataAccess.When(x => x.GetObjectDb(Arg.Any<string>())).DoNotCallBase();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _todayWithDrawalDataAccess = null;
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
            var reader = _todayWithDrawalDataAccess.GetObjectDb(Arg.Is("Lrf_GetLicenseeTodayWithdrawal"))
                .ExecuteReader(Arg.Do<Hashtable>(x => passedParam = x));
            reader.Read().Returns(true, false);
            reader["WithdrawalEUR"].Returns(0M);

            // Action
            _todayWithDrawalDataAccess.GetTodayWithdrawal(metaModel);

            // Assert
            Assert.That(passedParam["LicenseeID"], Is.EqualTo(metaModel.LicenseeId));
            Assert.That(passedParam["CurrencyIDList"], Is.EqualTo(metaModel.CurrencyIDList));
            Assert.That(passedParam["ReportDate"], Is.EqualTo(metaModel.FromDate));
        }

        [Test]
        public void GetTodayWithdrawal_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var expected = 500000M;
            var reader = _todayWithDrawalDataAccess.GetObjectDb(Arg.Is("Lrf_GetLicenseeTodayWithdrawal"))
                .ExecuteReader(Arg.Any<object>());
            reader.Read().Returns(true, false);
            reader["WithdrawalEUR"].Returns(expected);

            var metaModel = new DashboardBaseModel { BaseCurrency = "EUR" };

            // Action
            var result = _todayWithDrawalDataAccess.GetTodayWithdrawal(metaModel);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}