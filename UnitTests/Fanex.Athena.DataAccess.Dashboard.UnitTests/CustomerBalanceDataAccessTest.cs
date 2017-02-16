namespace Fanex.Athena.DataAccess.Dashboard.Tests
{
    using Model.Dashboard.MetaModel;
    using NSubstitute;
    using NUnit.Framework;

    public class CustomerBalanceDataAccessTest
    {
        private CustomerBalanceDataAccess _customerBalanceDataAccess;

        [OneTimeSetUp]
        public void SetUp()
        {
            _customerBalanceDataAccess = Substitute.ForPartsOf<CustomerBalanceDataAccess>();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _customerBalanceDataAccess = null;
        }

        [Test]
        public void GetData_WhenCall_PassCorrectParam()
        {
            // Arrange
            var metaModel = new DashboardBaseModel { LicenseeId = 1, CurrencyId = 2, FromDate = new System.DateTime(2017, 1, 1) };
            object passedParam = null;
            _customerBalanceDataAccess.GetObjectDb(Arg.Is<string>("Lrf_GetCustomerBalance"))
                .ExecuteScalar(Arg.Do<object>(x => passedParam = x)).Returns(0M);

            // Action
            _customerBalanceDataAccess.GetData(metaModel);
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
            var expected = 1570000M;
            _customerBalanceDataAccess.GetObjectDb(Arg.Is<string>("Lrf_GetCustomerBalance"))
                .ExecuteScalar(Arg.Any<object>()).Returns(expected);

            // Action
            var result = _customerBalanceDataAccess.GetData(new DashboardBaseModel());

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}
