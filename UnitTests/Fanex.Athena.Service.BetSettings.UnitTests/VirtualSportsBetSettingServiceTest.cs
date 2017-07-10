namespace Fanex.Athena.Service.BetSettings.UnitTests
{
    using System.Collections.Generic;
    using BetSettings;
    using Core.Users.Interfaces;
    using DataAccess.BetSettings.Interfaces;
    using Interfaces;
    using Model.BetSettings.DTO;
    using NSubstitute;
    using NUnit.Framework;
    using Resources;

    [TestFixture]
    public class VirtualSportsBetSettingServiceTest
    {
        private const int LicenseeId = 1;
        private const int UserId = 2;
        private const int CustId = 3;
        private const int ProductId = 5;

        private IAuthorizationService _authorizationService;
        private IVirtualSportsBetSettingDataAccess _dataAccess;
        private IVirtualSportsBetSettingService _service;

        [OneTimeSetUp]
        public void SetUp()
        {
            _authorizationService = Substitute.For<IAuthorizationService>();
            _dataAccess = Substitute.For<IVirtualSportsBetSettingDataAccess>();
            _service = new VirtualSportsBetSettingService(_authorizationService, _dataAccess);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _authorizationService = null;
            _dataAccess = null;
            _service = null;
        }

        [Test]
        public void GetViewModel_WhenCall_ReturnDataFromDataAcccess()
        {
            // Arrange
            _authorizationService.HasPermissionOnData(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<long>()).Returns(true);
            var betSettings = new List<BetSetting>
            {
                new BetSetting()
            };
            _dataAccess.GetBetSetting(CustId, ProductId).Returns(betSettings);

            // Action
            var result = _service.GetViewModel(LicenseeId, CustId, ProductId);

            // Assert
            Assert.That(result.MainData.Count, Is.EqualTo(betSettings.Count));
        }

        [Test]
        public void UpdateBetSetting_WhenCall_CallDataAccessWithCorrectParameters()
        {
            // Arrange
            _authorizationService.HasPermissionOnData(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<long>()).Returns(true);
            const string ExpectedXmlParam = "<ROOT><BetLimit SportType=\"1\" BetType=\"1\" MinBet=\"100\" MaxBet=\"200\" MaxPerMatch=\"300\" ></BetLimit></ROOT>";

            var betSettings = new List<VSBetSetting>
            {
                new VSBetSetting
                {
                    SportId = "1",
                    MinBet = 100,
                    MaxBet = 200,
                    MaxPerMatch = 300,
                    BetType = 1
                }
            };

            // Action
            var result = _service.UpdateBetSetting(LicenseeId, CustId, UserId, "ip", betSettings);

            // Assert
            _dataAccess.Received().UpdateBetSetting(CustId, UserId, "ip", ExpectedXmlParam);
        }

        [Test]
        public void UpdateBetSetting_HaveNoPermission_ReturnCorrectErrorResult()
        {
            // Arrange
            _authorizationService.HasPermissionOnData(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<long>()).Returns(false);
            var betSettings = new List<VSBetSetting>
            {
                new VSBetSetting()
            };

            // Action
            var result = _service.UpdateBetSetting(LicenseeId, CustId, UserId, "ip", betSettings);

            // Assert
            Assert.That(result.Code, Is.EqualTo(1));
            Assert.That(result.Message, Is.EqualTo(ViewResources.AccessDeniedMsg));
        }
    }
}