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
            const string LicenseeName = "Name";
            var licenseeId = FakeHelper.Create<int>();
            var custId = FakeHelper.Create<int>();
            var siteName = FakeHelper.Create<string>();
            var expectedResult = FakeHelper.Create<LogInfo>();

            _authorizationService.HasPermissionOnData(licenseeId, custId, 0).Returns(true);
            _customerListDataAccess.GetCustomerLoginInfo(custId, licenseeId, LicenseeName).Returns(expectedResult);

            // Action
            var result = _customerListService.GetCustomerLoginInfo(custId, licenseeId, LicenseeName);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void GetCustomerLoginInfo_HaveNoPermission_DataAccessIsNotCalled()
        {
            // Arrange
            const string LicenseeName = "Name";
            var licenseeId = FakeHelper.Create<int>();
            var custId = FakeHelper.Create<int>();
            var siteName = FakeHelper.Create<string>();

            _authorizationService.HasPermissionOnData(licenseeId, custId, 0).Returns(false);

            // Action
            var result = _customerListService.GetCustomerLoginInfo(custId, licenseeId, LicenseeName);

            // Assert
            Assert.Multiple(() =>
            {
                _customerListDataAccess.DidNotReceive().GetCustomerLoginInfo(custId, licenseeId, LicenseeName);
                Assert.That(result, Is.Not.Null);
            });
        }
    }
}