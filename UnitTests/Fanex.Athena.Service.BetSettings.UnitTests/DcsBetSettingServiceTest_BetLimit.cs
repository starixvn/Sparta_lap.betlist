namespace Fanex.Athena.Service.BetSettings.UnitTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Model.BetSettings.DTO;
    using NSubstitute;
    using NUnit.Framework;
    using Resources;
    using UnitTest.Common.Helpers;

    [TestFixture]
    public partial class DcsBetSettingServiceTest
    {
        [Test]
        public async Task GetBetSettingsAsync_WhenCall_ReturnDataFromDataAcccess()
        {
            // Arrange
            var betSettings = FakeHelper.CreateMany<BetSetting>(100).ToList();

            _betSettingDataAccess.GetBetSettingAsync(_custId, _productId).Returns(Task.FromResult<IList<BetSetting>>(betSettings));

            _authorizationService.HasPermissionOnData(_licenseeId, _custId, 0).Returns(true);

            // Action
            var result = await _dcsBettingService.GetBetSettingsAsync(_licenseeId, _custId, _productId);

            // Assert
            Assert.That(result.Count, Is.EqualTo(betSettings.Count));
        }

        [Test]
        public async Task UpdateBetSettingAsync_WhenCall_CallDataAccessWithCorrectParameters()
        {
            // Arrange
            var setting = FakeHelper.Create<DcsBetSetting>();
            var betSettings = new List<DcsBetSetting> { setting };

            var expectedXmlParam = string.Format(
                "<ROOT><BetLimit SportType=\"{0}\" BetType=\"{1}\" MinBet=\"{2}\" MaxBet=\"{3}\" MaxPerMatch=\"{4}\" ></BetLimit></ROOT>",
                setting.SportId,
                setting.BetType,
                setting.MinBet,
                setting.MaxBet,
                setting.MaxPerMatch);

            _authorizationService.HasPermissionOnData(_licenseeId, _custId, 0).Returns(true);

            // Action
            await _dcsBettingService.UpdateBetSettingAsync(_licenseeId, _custId, _userId, "ip", betSettings);

            // Assert
            _betSettingDataAccess.Received().UpdateBetSettingAsync(_custId, _userId, "ip", expectedXmlParam);
        }

        [Test]
        public async Task UpdateBetSetting_HaveNoPermission_ReturnCorrectErrorResult()
        {
            // Arrange
            var betSettings = new List<DcsBetSetting>
            {
                FakeHelper.Create<DcsBetSetting>()
            };
            _authorizationService.HasPermissionOnData(_licenseeId, _custId, 0).Returns(false);

            // Action
            var result = await _dcsBettingService.UpdateBetSettingAsync(_licenseeId, _custId, _userId, "ip", betSettings);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Code, Is.EqualTo(1));
                Assert.That(result.Message, Is.EqualTo(ViewResources.AccessDeniedMsg));
            });
        }
    }
}