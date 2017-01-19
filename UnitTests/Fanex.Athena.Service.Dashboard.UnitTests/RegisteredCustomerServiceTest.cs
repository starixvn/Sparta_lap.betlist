namespace Fanex.Athena.Service.Dashboard.UnitTests
{
    using DataAccess.Dashboard.Interfaces;
    using Interfaces;
    using Model.Dashboard.MetaModel;
    using Model.Dashboard.ViewModel;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class RegisteredCustomerServiceTest
    {
        private IRegisteredCustomerDataAccess _registeredCustomerDataAccess;
        private IRegisteredCustomerService _registeredCustomerService;

        [OneTimeSetUp]
        public void SetUp()
        {
            _registeredCustomerDataAccess = Substitute.For<IRegisteredCustomerDataAccess>();
            _registeredCustomerService = new RegisteredCustomerService(_registeredCustomerDataAccess);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _registeredCustomerDataAccess = null;
            _registeredCustomerService = null;
        }

        [Test]
        public void GetData_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var metaModel = new DashboardBaseModel();
            var expected = new RegisteredCustomerViewModel { DepositCount = 400, NonDepositCount = 100 };
            _registeredCustomerDataAccess.GetData(metaModel).Returns(expected);

            // Action
            var result = _registeredCustomerService.GetData(metaModel);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}