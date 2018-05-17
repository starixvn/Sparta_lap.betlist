namespace Fanex.Athena.Service.BetSettings.UnitTests
{
    using System;
    using Constant.DataDomain;
    using Model.BetSettings.DTO;
    using NSubstitute;
    using NUnit.Framework;
    using Resources;
    using UnitTest.Common.Helpers;

    [TestFixture]
    public partial class GoldDeluxeBetSettingTest
    {
        private readonly string _group = FakeHelper.Create<string>();

        [Test]
        public void UpdateBetSetting_IsEnabledInSettingIsFlale_ReturnCorrectErrorResult()
        {
            // Arrange
            _dataAccess.GetBetSetting(_custId, ProductTypes.GoldDeluxe).Returns(new ThirdPartyBetSetting
            {
                IsEnabled = false
            });
            _authorizationService.HasPermissionOnData(_licenseeId, _custId, 0).Returns(true);

            // Action
            var result = _service.UpdateBetSetting(_licenseeId, _siteName, _custId, _group);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Code, Is.EqualTo(1));
                Assert.That(result.Message, Is.EqualTo(ViewResources.AccessDeniedMsg));
            });
        }

        [Test]
        public void UpdateBetSetting_HaveNoPermission_ReturnCorrectErrorResult()
        {
            // Arrange
            _dataAccess.GetBetSetting(_custId, ProductTypes.GoldDeluxe).Returns(new ThirdPartyBetSetting
            {
                IsEnabled = true
            });
            _authorizationService.HasPermissionOnData(_licenseeId, _custId, 0).Returns(false);

            // Action
            var result = _service.UpdateBetSetting(_licenseeId, _siteName, _custId, _group);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Code, Is.EqualTo(1));
                Assert.That(result.Message, Is.EqualTo(ViewResources.AccessDeniedMsg));
            });
        }

        [Test]
        public void UpdateBetSetting_ApiReturnCodeIsNotZero_ReturnErrorProcessingResult()
        {
            // Arrange
            _authorizationService.HasPermissionOnData(_licenseeId, _custId, Arg.Any<long>()).Returns(true);
            _dataAccess.GetBetSetting(_custId, ProductTypes.GoldDeluxe).Returns(new ThirdPartyBetSetting
            {
                IsLineEnabled = true,
                IsEnabled = true
            });

            SetUpSetUserGroupApi("<SetUserGroupBettingLimit><ReturnCode>12</ReturnCode></SetUserGroupBettingLimit>");

            // Action
            var result = _service.UpdateBetSetting(_licenseeId, _siteName, _custId, _group);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Code, Is.EqualTo(1));
                Assert.That(result.Message, Is.EqualTo(ViewResources.ErrorProcessingMsg));
            });
        }

        [Test]
        public void UpdateBetSetting_ApiReturnCodeIsZero_ReturnSuccessResult()
        {
            // Arrange
            _authorizationService.HasPermissionOnData(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<long>()).Returns(true);
            _dataAccess.GetBetSetting(_custId, ProductTypes.GoldDeluxe).Returns(new ThirdPartyBetSetting
            {
                IsLineEnabled = true,
                IsEnabled = true
            });

            SetUpSetUserGroupApi("<SetUserGroupBettingLimit><ReturnCode>0</ReturnCode></SetUserGroupBettingLimit>");

            // Action
            var result = _service.UpdateBetSetting(_licenseeId, _siteName, _custId, _group);

            // Assert
            Assert.That(result.Code, Is.EqualTo(0));
        }

        [Test]
        public void UpdateBetSetting_GetErrorWhileProcessing_WriteErrorLog()
        {
            // Arrange
            _authorizationService.HasPermissionOnData(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<long>()).Returns(true);
            _dataAccess.GetBetSetting(_custId, ProductTypes.GoldDeluxe).Returns(new ThirdPartyBetSetting
            {
                IsLineEnabled = true,
                IsEnabled = true
            });

            SetUpSetUserGroupApi("Wrong format");

            // Action
            var result = _service.UpdateBetSetting(_licenseeId, _siteName, _custId, _group);

            // Assert
            _logger.Received().Error(Arg.Any<Exception>());
        }

        private void SetUpSetUserGroupApi(string returnValue)
        {
            _webClientService.UploadString(
                new Uri(SetUserGroupApiUrl),
                $"SiteName={_siteName}&IBCCustID={_custId}&ThirdPartyId={GoldDeluxeThirdPartyId}&GroupType={_group}")
            .Returns(returnValue);
        }
    }
}