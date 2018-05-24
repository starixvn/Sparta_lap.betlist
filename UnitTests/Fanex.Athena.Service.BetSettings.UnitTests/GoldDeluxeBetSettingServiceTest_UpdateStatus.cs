namespace Fanex.Athena.Service.BetSettings.UnitTests
{
    using Constant.DataDomain;
    using Model.BetSettings.DTO;
    using Model.Core.DTO.Common;
    using NSubstitute;
    using NUnit.Framework;
    using Resources;
    using UnitTest.Common.Helpers;

    [TestFixture]
    public partial class GoldDeluxeBetSettingTest
    {
        private readonly int _userId = FakeHelper.Create<int>();
        private readonly string _ip = FakeHelper.Create<string>();
        private readonly string _language = FakeHelper.Create<string>();

        [Test]
        public void UpdateStatus_WhenCall_ReturnDataFromDataAccess()
        {
            // Arrange
            _authorizationService.HasPermissionOnData(_licenseeId, _custId, 0).Returns(true);
            _dataAccess.GetBetSetting(_custId, ProductTypes.GoldDeluxe).Returns(new ThirdPartyBetSetting
            {
                IsLineEnabled = true,
                IsEnabled = true
            });

            var expectedResult = FakeHelper.Create<ResultInfo>();

            var isEnable = FakeHelper.Create<bool>();

            _dataAccess.UpdateStatus(ProductTypes.GoldDeluxe, _userId, _custId, _language, _ip, isEnable).Returns(expectedResult);

            // Action
            var result = _service.UpdateStatus(_licenseeId, _userId, _custId, _language, _ip, isEnable);

            // Assert
            Assert.That(result.Code, Is.EqualTo(expectedResult.Code));
        }

        [Test]
        public void UpdateStatus_HaveNoPermission_ReturnAccessDeniedErrorResult()
        {
            // Arrange
            _authorizationService.HasPermissionOnData(_licenseeId, _custId, 0).Returns(false);

            var expectedResult = new ResultInfo
            {
                Code = 1,
                Message = ViewResources.AccessDeniedMsg
            };

            // Action
            var result = _service.UpdateStatus(_licenseeId, _userId, _custId, _language, _ip, FakeHelper.Create<bool>());

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Code, Is.EqualTo(expectedResult.Code));
                Assert.That(result.Message, Is.EqualTo(expectedResult.Message));
            });
        }

        [Test]
        public void UpdateStatus_IsLineEnabledInSettingIsFalse_ReturnAccessDeniedErrorResult()
        {
            // Arrange
            _authorizationService.HasPermissionOnData(_licenseeId, _custId, 0).Returns(true);
            _dataAccess.GetBetSetting(_custId, ProductTypes.GoldDeluxe).Returns(new ThirdPartyBetSetting
            {
                IsLineEnabled = false
            });

            var expectedResult = new ResultInfo
            {
                Code = 1,
                Message = ViewResources.AccessDeniedMsg
            };

            // Action
            var result = _service.UpdateStatus(_licenseeId, _userId, _custId, _language, _ip, FakeHelper.Create<bool>());

            // Assert
            Assert.That(result.Code, Is.EqualTo(expectedResult.Code));
            Assert.That(result.Message, Is.EqualTo(expectedResult.Message));
        }
    }
}