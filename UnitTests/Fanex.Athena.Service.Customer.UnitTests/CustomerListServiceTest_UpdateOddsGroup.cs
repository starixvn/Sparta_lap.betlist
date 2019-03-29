namespace Fanex.Athena.Service.Customer.UnitTests
{
    using System.Threading.Tasks;
    using Model.Core.DTO.Common;
    using Model.Customer.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using Resources;
    using UnitTest.Common.Helpers;

    [TestFixture]
    public partial class CustomerListServiceTest
    {
        [Test]
        public async Task UpdateOddsGroup_WhenCallWithCorrectPermission_ReturnCorrectData()
        {
            // Arrange
            var metaModel = FakeHelper.Create<CustomerOddsGroupMetaModel>();
            _authorizationService.HasPermissionOnData(metaModel.LicenseeId, metaModel.CustId, 0).Returns(true);

            var expectedResult = FakeHelper.Create<ResultInfo>();
            _customerListDataAccess.UpdateOddsGroup(metaModel).Returns(expectedResult);

            // Action
            var result = await _customerListService.UpdateOddsGroup(metaModel);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public async Task UpdateOddsGroup_HaveNoPermission_ReturnCorrectData()
        {
            // Arrange
            var metaModel = FakeHelper.Create<CustomerOddsGroupMetaModel>();
            _authorizationService.HasPermissionOnData(metaModel.LicenseeId, metaModel.CustId, 0).Returns(false);

            var expectedResult = new ResultInfo { Code = 1, Message = ViewResources.UpdateSpecialLevelMsg };

            // Action
            var result = await _customerListService.UpdateOddsGroup(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Code, Is.EqualTo(expectedResult.Code));
                Assert.That(result.Message, Is.EqualTo(expectedResult.Message));
            });
        }
    }
}