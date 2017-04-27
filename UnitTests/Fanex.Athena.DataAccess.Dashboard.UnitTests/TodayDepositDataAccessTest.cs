using Fanex.Athena.Model.Dashboard.MetaModel;
using NSubstitute;
using NUnit.Framework;

namespace Fanex.Athena.DataAccess.Dashboard.UnitTests
{
    [TestFixture]
    public class TodayDepositDataAccessTest
    {
        private TodayDepositDataAccess _todayDepositDataAccess;

        [OneTimeSetUp]
        public void SetUp()
        {
            _todayDepositDataAccess = Substitute.ForPartsOf<TodayDepositDataAccess>();
            _todayDepositDataAccess.When(x => x.GetObjectDb(Arg.Any<string>())).DoNotCallBase();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _todayDepositDataAccess = null;
        }

        [Test]
        public void GetData_WhenCall_PassCorectParam()
        {
            // Arrange
            var metaModel = new DashboardBaseModel { LicenseeId = 1, CurrencyId = 2, BaseCurrency = "EUR", FromDate = new System.DateTime(2017, 1, 1) };
            object passedParam = null;
            var reader = _todayDepositDataAccess.GetObjectDb(Arg.Is<string>("Lrf_GetLicenseeTodayDeposit"))
                .ExecuteReader(Arg.Do<object>(x => passedParam = x));
            reader.Read().Returns(true, false);
            reader["DepositEUR"].Returns(0M);

            // Action
            _todayDepositDataAccess.GetData(metaModel);
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
