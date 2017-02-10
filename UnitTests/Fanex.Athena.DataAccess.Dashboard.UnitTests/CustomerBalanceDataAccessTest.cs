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
