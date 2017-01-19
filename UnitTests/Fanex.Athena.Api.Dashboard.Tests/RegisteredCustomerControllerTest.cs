namespace Fanex.Athena.Api.Dashboard.Tests
{
    using Model.Dashboard.MetaModel;
    using Model.Dashboard.ViewModel;
    using NSubstitute;
    using NUnit.Framework;
    using Service.Dashboard.Interfaces;

    [TestFixture]
    public class RegisteredCustomerControllerTest
    {
        private IRegisteredCustomerService _registeredCustomerService;
        private RegisteredCustomerController _registeredCustomerController;

        [OneTimeSetUp]
        public void Setup()
        {
            _registeredCustomerService = Substitute.For<IRegisteredCustomerService>();
            _registeredCustomerController = new RegisteredCustomerController();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _registeredCustomerService = null;
            _registeredCustomerController = null;
        }

        [Test]
        public void Index_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var metaModel = new DashboardBaseModel();
            var expected = new RegisteredCustomerViewModel { DepositCount = 400, NonDepositCount = 100 };
            _registeredCustomerService.GetData(metaModel).Returns(expected);

            // Action
            var result = _registeredCustomerController.Index(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.DepositCount, Is.EqualTo(expected.DepositCount));
                Assert.That(result.NonDepositCount, Is.EqualTo(expected.NonDepositCount));
            });
        }
    }
}