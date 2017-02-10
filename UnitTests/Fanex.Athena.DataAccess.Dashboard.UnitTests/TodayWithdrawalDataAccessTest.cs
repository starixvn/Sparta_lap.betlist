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
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _todayWithDrawalDataAccess = null;
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