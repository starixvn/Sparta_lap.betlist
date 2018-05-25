namespace Fanex.Athena.Service.Customer.UnitTests
{
    using Model.Customer.DTO;
    using NSubstitute;
    using NUnit.Framework;
    using UnitTest.Common.Helpers;

    [TestFixture]
    public partial class CustomerListServiceTest
    {
        [Test]
        public void GetCustomerLoginInfo_WhenCallWithCorrectPermission_ReturnCorrectData()
        {
            // Arrange
            var licenseeId = FakeHelper.Create<int>();
            var custId = FakeHelper.Create<int>();
            var expectedResult = FakeHelper.Create<LogInfo>();

            _authorizationService.HasPermissionOnData(licenseeId, custId, 0).Returns(true);
            _customerListDataAccess.GetCustomerLoginInfo(custId, licenseeId).Returns(expectedResult);

            // Action
            var result = _customerListService.GetCustomerLoginInfo(custId, licenseeId);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void GetCustomerLoginInfo_HaveNoPermission_DataAccessIsNotCalled()
        {
            // Arrange
            var licenseeId = FakeHelper.Create<int>();
            var custId = FakeHelper.Create<int>();

            _authorizationService.HasPermissionOnData(licenseeId, custId, 0).Returns(false);

            // Action
            var result = _customerListService.GetCustomerLoginInfo(custId, licenseeId);

            // Assert
            Assert.Multiple(() =>
            {
                _customerListDataAccess.DidNotReceive().GetCustomerLoginInfo(custId, licenseeId);
                Assert.That(result, Is.Not.Null);
            });
        }
    }
}