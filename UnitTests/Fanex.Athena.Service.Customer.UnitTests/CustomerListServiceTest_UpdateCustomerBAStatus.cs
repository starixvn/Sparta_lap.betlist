namespace Fanex.Athena.Service.Customer.UnitTests
{
    using System.Threading.Tasks;
    using Model.Core.DTO.Common;
    using NSubstitute;
    using NUnit.Framework;
    using Resources;
    using UnitTest.Common.Helpers;

    [TestFixture]
    public partial class CustomerListServiceTest
    {
        [Test]
        public void UpdateCustomerBAStatus_HaveNoPermission_ReturnCorrectResultInfo()
        {
            // Arrange
            var licenseeId = FakeHelper.Create<int>();
            var custId = FakeHelper.Create<int>();

            _authorizationService.HasPermissionOnData(licenseeId, custId, 0).Returns(false);

            var expectedResult = new ResultInfo { Code = 1, Message = ViewResources.UpdateSpecialLevelMsg };

            // Action
            var result = _customerListService.UpdateCustomerBAStatus(
                licenseeId, 
                custId, 
                FakeHelper.Create<int>(), 
                FakeHelper.Create<bool>(), 
                FakeHelper.Create<Request>());

            // Assert
            Assert.That(result.Code, Is.EqualTo(expectedResult.Code));
            Assert.That(result.Message, Is.EqualTo(expectedResult.Message));
        }

        [Test]
        public void UpdateCustomerBAStatus_WhenCallWithCorrectPermission_ReturnCorrectResultInfo()
        {
            // Arrange
            var licenseeId = FakeHelper.Create<int>();
            var custId = FakeHelper.Create<int>();
            var modifiedBy = FakeHelper.Create<int>();
            var allowBA = FakeHelper.Create<bool>();
            var request = FakeHelper.Create<Request>();

            _authorizationService.HasPermissionOnData(licenseeId, custId, 0).Returns(true);

            var expectedResult = FakeHelper.Create<ResultInfo>();
            _customerListDataAccess.UpdateCustomerBAStatus(licenseeId, custId, modifiedBy, allowBA, request).Returns(expectedResult);

            // Action
            var result = _customerListService.UpdateCustomerBAStatus(licenseeId, custId, modifiedBy, allowBA, request);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }
    }
}