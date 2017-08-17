namespace Fanex.Athena.Service.BetSettings.UnitTests
{
    using System.Collections.Generic;
    using Model.BetSettings.DTO;
    using NSubstitute;
    using NUnit.Framework;
    using Resources;

    [TestFixture]
    public partial class DcsBetSettingServiceTest
    {
        [Test]
        public void GetBetSettings_WhenCall_ReturnDataFromDataAcccess()
        {
            // Arrange
            _authorizationService.HasPermissionOnData(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<long>()).Returns(true);
            var betSettings = new List<BetSetting>
            {
                new BetSetting()
            };
            _betSettingDataAccess.GetBetSetting(CustId, ProductId).Returns(betSettings);

            // Action
            var result = _dcsBettingService.GetBetSettings(LicenseeId, CustId, ProductId);

            // Assert
            Assert.That(result.Count, Is.EqualTo(betSettings.Count));
        }

        [Test]
        public void UpdateBetSetting_WhenCall_CallDataAccessWithCorrectParameters()
        {
            // Arrange
            _authorizationService.HasPermissionOnData(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<long>()).Returns(true);
            const string ExpectedXmlParam = "<ROOT><BetLimit SportType=\"1\" BetType=\"1\" MinBet=\"100\" MaxBet=\"200\" MaxPerMatch=\"300\" ></BetLimit></ROOT>";

            var betSettings = new List<DcsBetSetting>
            {
                new DcsBetSetting
                {
                    SportId = "1",
                    MinBet = 100,
                    MaxBet = 200,
                    MaxPerMatch = 300,
                    BetType = 1
                }
            };

            // Action
            _dcsBettingService.UpdateBetSetting(LicenseeId, CustId, UserId, "ip", betSettings);

            // Assert
            _betSettingDataAccess.Received().UpdateBetSetting(CustId, UserId, "ip", ExpectedXmlParam);
        }

        [Test]
        public void UpdateBetSetting_HaveNoPermission_ReturnCorrectErrorResult()
        {
            // Arrange
            _authorizationService.HasPermissionOnData(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<long>()).Returns(false);
            var betSettings = new List<DcsBetSetting>
            {
                new DcsBetSetting()
            };

            // Action
            var result = _dcsBettingService.UpdateBetSetting(LicenseeId, CustId, UserId, "ip", betSettings);

            // Assert
            Assert.That(result.Code, Is.EqualTo(1));
            Assert.That(result.Message, Is.EqualTo(ViewResources.AccessDeniedMsg));
        }
    }
}