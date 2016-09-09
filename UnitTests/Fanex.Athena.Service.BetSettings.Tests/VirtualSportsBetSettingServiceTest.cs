namespace Fanex.Athena.Service.Core.Tests.Common
{
    using System.Collections.Generic;
    using BetSettings;
    using DataAccess.BetSettings;
    using Model.BetSettings.DTO;
    using NSubstitute;
    using NUnit.Framework;
    using Users.Interfaces;

    [TestFixture]
    public class VirtualSportsBetSettingServiceTest
    {
        private const int LicenseeId = 1;
        private const int UserId = 2;
        private const int CustId = 3;
        private const string Language = "en-US";

        private IAuthorizationService _authorizationService;
        private IVirtualSportsBetSettingDataAccess _dataAccess;
        private IVirtualSportsBetSettingService _service;

        [SetUp]
        public void SetUp()
        {
            _authorizationService = Substitute.For<IAuthorizationService>();
            _dataAccess = Substitute.For<IVirtualSportsBetSettingDataAccess>();
            _service = new VirtualSportsBetSettingService(_authorizationService, _dataAccess);

            _authorizationService.HasPermissionOnData(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<long>()).Returns(true);
        }

        [Test]
        public void GetViewModel_WhenCall_ReturnDataFromDataAcccess()
        {
            // Arrange
            var betSettings = new List<BetSetting>()
            {
                new BetSetting()
            };
            _dataAccess.GetBetSetting(CustId).Returns(betSettings);

            // Action
            var result = _service.GetViewModel(LicenseeId, CustId);

            // Assert
            Assert.That(result.MainData.Count, Is.EqualTo(betSettings.Count));
        }

        [Test]
        public void UpdateBetSetting_WhenCall_CallDataAccessWithCorrectParameters()
        {
            // Arrange
            var expectedXmlParam = "<settings><setting sporttype=\"1\" minbet=\"100\" maxbet=\"200\" maxpermatch=\"300\" ></setting></settings>";
            var betSettings = new List<VSBetSetting>()
            {
                new VSBetSetting()
                {
                    SportId = "1",
                    MinBet = 100,
                    MaxBet = 200,
                    MaxPerMatch = 300
                }
            };

            // Action
            var result = _service.UpdateBetSetting(LicenseeId, CustId, UserId, Language, "ip", betSettings);

            // Assert
            _dataAccess.Received().UpdateBetSetting(CustId, UserId, Language, "ip", expectedXmlParam);
        }
    }
}