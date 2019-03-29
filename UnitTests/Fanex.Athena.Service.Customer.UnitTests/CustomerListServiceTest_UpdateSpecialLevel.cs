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
        public async Task UpdateSpecialLevel_HaveNoPermission_ReturnCorrectResultInfo()
        {
            // Arrange
            var licenseeId = FakeHelper.Create<int>();
            var custId = FakeHelper.Create<int>();
            var userId = FakeHelper.Create<int>();
            var level = FakeHelper.Create<int>();
            var ip = FakeHelper.Create<string>();
            var language = FakeHelper.Create<string>();

            _authorizationService.HasPermissionOnData(licenseeId, custId, 0).Returns(false);
            var expectedResult = new ResultInfo { Code = 1, Message = ViewResources.UpdateSpecialLevelMsg };

            // Action
            var result = await _customerListService.UpdateSpecialLevel(licenseeId, userId, custId, level, ip, language);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Code, Is.EqualTo(expectedResult.Code));
                Assert.That(result.Message, Is.EqualTo(expectedResult.Message));
            });
        }

        [Test]
        public async Task UpdateSpecialLevel_WhenCallWithCorrectPermission_ReturnCorrectResultInfo()
        {
            // Arrange
            var licenseeId = FakeHelper.Create<int>();
            var custId = FakeHelper.Create<int>();
            var userId = FakeHelper.Create<int>();
            var level = FakeHelper.Create<int>();
            var ip = FakeHelper.Create<string>();
            var language = FakeHelper.Create<string>();

            _authorizationService.HasPermissionOnData(licenseeId, custId, 0).Returns(true);

            var expectedResult = FakeHelper.Create<ResultInfo>();
            _customerListDataAccess.UpdateSpecialLevel(userId, custId, level, ip, language).Returns(expectedResult);

            // Action
            var result = await _customerListService.UpdateSpecialLevel(licenseeId, userId, custId, level, ip, language);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }
    }
}