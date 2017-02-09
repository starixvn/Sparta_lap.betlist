using Fanex.Athena.Model.Dashboard.MetaModel;
using NSubstitute;
using NUnit.Framework;

namespace Fanex.Athena.DataAccess.Dashboard.Tests
{
    [TestFixture]
    public class TodayDepositDataAccessTest
    {
        private TodayDepositDataAccess _todayDepositDataAccess;

        [OneTimeSetUp]
        public void SetUp()
        {
            _todayDepositDataAccess = Substitute.ForPartsOf<TodayDepositDataAccess>();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _todayDepositDataAccess = null;
        }

        [Test]
        public void GetData_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var expected = 2500000M;
            var reader = _todayDepositDataAccess.GetObjectDb(Arg.Is<string>("Lrf_GetLicenseeTodayDeposit"))
                .ExecuteReader(Arg.Any<object>());
            reader.Read().Returns(true, false);
            reader["DepositEUR"].Returns(expected);

            var metaModel = new DashboardBaseModel { BaseCurrency = "EUR" };

            // Action
            var result = _todayDepositDataAccess.GetData(metaModel);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}
