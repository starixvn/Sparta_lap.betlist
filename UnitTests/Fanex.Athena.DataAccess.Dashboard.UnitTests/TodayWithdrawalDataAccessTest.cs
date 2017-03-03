namespace Fanex.Athena.DataAccess.Dashboard.Tests
{
    using Model.Dashboard.MetaModel;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class TodayWithdrawalDataAccessTest
    {
        private TodayWithdrawalDataAccess _todayWithDrawalDataAccess;

        [OneTimeSetUp]
        public void SetUp()
        {
            _todayWithDrawalDataAccess = Substitute.ForPartsOf<TodayWithdrawalDataAccess>();
            _todayWithDrawalDataAccess.When(x => x.GetObjectDb(Arg.Any<string>())).DoNotCallBase();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _todayWithDrawalDataAccess = null;
        }

        [Test]
        public void GetData_WhenCall_PassCorrectParam()
        {
            // Arrange
            var metaModel = new DashboardBaseModel { LicenseeId = 1, CurrencyId = 2, BaseCurrency = "EUR", FromDate = new System.DateTime(2017, 1, 1) };
            object passedParam = null;
            var reader = _todayWithDrawalDataAccess.GetObjectDb(Arg.Is<string>("Lrf_GetLicenseeTodayWithdrawal"))
                .ExecuteReader(Arg.Do<object>(x => passedParam = x));
            reader.Read().Returns(true, false);
            reader["WithdrawalEUR"].Returns(0M);

            // Action
            var result = _todayWithDrawalDataAccess.GetData(metaModel);
            var type = passedParam.GetType();

            // Assert
            Assert.That(type.GetProperty("LicenseeID").GetValue(passedParam, null), Is.EqualTo(metaModel.LicenseeId));
            Assert.That(type.GetProperty("CurrencyID").GetValue(passedParam, null), Is.EqualTo(metaModel.CurrencyId));
            Assert.That(type.GetProperty("ReportDate").GetValue(passedParam, null), Is.EqualTo(metaModel.FromDate));
        }

        [Test]
        public void GetData_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var expected = 500000M;
            var reader = _todayWithDrawalDataAccess.GetObjectDb(Arg.Is<string>("Lrf_GetLicenseeTodayWithdrawal"))
                .ExecuteReader(Arg.Any<object>());
            reader.Read().Returns(true, false);
            reader["WithdrawalEUR"].Returns(expected);

            var metaModel = new DashboardBaseModel { BaseCurrency = "EUR" };

            // Action
            var result = _todayWithDrawalDataAccess.GetData(metaModel);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}