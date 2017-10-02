namespace Fanex.Athena.Service.Customer.UnitTests
{
    using Core.Users.Interfaces;
    using DataAccess.Customer.Interfaces;
    using Model.Customer.DTO;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class CustomerServiceTest
    {
        private ICustomerDataAccess _customerDataAccess;

        private IAuthorizationService _authorizationService;

        private CustomerService _customerService;

        [SetUp]
        public void SetUp()
        {
            _customerDataAccess = Substitute.For<ICustomerDataAccess>();

            _authorizationService = Substitute.For<IAuthorizationService>();
            _customerService = new CustomerService(_customerDataAccess, _authorizationService);
        }

        [TearDown]
        public void TearDown()
        {
            _customerDataAccess = null;
            _authorizationService = null;
        }

        [Test]
        public void GetCustomerInfo_WhenCallWithCorrectPermission_ReturnCorrectData()
        {
            // Arrange
            const int LicenseeId = 1, CustId = 2;
            var expectedResult = new CustomerInfo();
            _authorizationService.HasPermissionOnData(LicenseeId, CustId, Arg.Any<long>()).Returns(true);
            _customerDataAccess.GetCustomerInfo(CustId).Returns(expectedResult);

            // Action
            var result = _customerService.GetCustomerInfo(LicenseeId, CustId);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void GetCustomerInfo_HaveNoPermission_DataAccessIsNotCalled()
        {
            // Arrange
            const int LicenseeId = 1, CustId = 2;
            _authorizationService.HasPermissionOnData(LicenseeId, CustId, Arg.Any<long>()).Returns(false);

            // Action
            var result = _customerService.GetCustomerInfo(LicenseeId, CustId);

            // Assert
            _customerDataAccess.DidNotReceive().GetCustomerInfo(CustId);
            Assert.That(result, Is.Not.Null);
        }
    }
}