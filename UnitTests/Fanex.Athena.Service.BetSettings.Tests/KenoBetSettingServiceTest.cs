namespace Fanex.Athena.Service.Core.Tests.Common
{
    using BetSettings;
    using DataAccess.BetSettings;
    using Model.BetSettings.DTO;
    using NSubstitute;
    using NUnit.Framework;
    using Users.Interfaces;

    [TestFixture]
    public class KenoBetSettingServiceTest
    {
        private IAuthorizationService _authorizationService;
        private IKenoBetSettingDataAccess _dataAccess;
        private IKenoBetSettingService _service;

        [SetUp]
        public void SetUp()
        {
            _authorizationService = Substitute.For<IAuthorizationService>();
            _dataAccess = Substitute.For<IKenoBetSettingDataAccess>();
            _service = new KenoBetSettingService(_authorizationService, _dataAccess);

            _authorizationService.HasPermissionOnData(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<long>()).Returns(true);
        }

        [Test]
        public void UpdateStatus_WhenCall_DataAccessExecuteWithCorrectParams()
        {
            // Arrange
            var licenseeId = 1;
            var userId = 1;
            var custId = 1;
            var language = "en-US";
            var ip = "ip";
            var setting = new KenoBetSetting()
            {
                GameTypeID = "1",
                MinBet = 2,
                MaxBet = 3,
                MaxPerMatch = 4
            };
            var xmlParam = "<settings><setting GameTypeID=\"1\" minbet=\"2\" maxbet=\"3\" maxpermatch=\"4\" ></setting></settings>";

            // Action
            var result = _service.UpdateBetSetting(licenseeId, custId, userId, language, ip, setting);

            // Assert
            _dataAccess.Received().UpdateBetSetting(custId, userId, language, ip, xmlParam);
        }
    }
}