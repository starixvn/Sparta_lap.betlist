namespace Fanex.Athena.DataAccess.Dashboard.Tests
{
    using Interfaces;
    using Model.Dashboard.MetaModel;
    using Model.Dashboard.ViewModel;
    using NUnit.Framework;

    [TestFixture]
    public class RegisteredCustomerDataAccessTest
    {
        private IRegisteredCustomerDataAccess _registeredCustomerDataAccess;

        [OneTimeSetUp]
        public void SetUp()
        {
            _registeredCustomerDataAccess = new RegisteredCustomerDataAccess();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _registeredCustomerDataAccess = null;
        }

        [Test]
        public void GetData_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var metaModel = new DashboardBaseModel();
            var expected = new RegisteredCustomerViewModel { DepositCount = 400, NonDepositCount = 100 };

            // Action
            var result = _registeredCustomerDataAccess.GetData(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.DepositCount, Is.EqualTo(expected.DepositCount));
                Assert.That(result.NonDepositCount, Is.EqualTo(expected.NonDepositCount));
            });
        }
    }
}