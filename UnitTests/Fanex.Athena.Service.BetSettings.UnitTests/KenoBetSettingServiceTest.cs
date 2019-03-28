namespace Fanex.Athena.Service.BetSettings.UnitTests
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using BetSettings;
    using DataAccess.BetSettings.Interfaces;
    using Interfaces;
    using Model.BetSettings.DTO;
    using NSubstitute;
    using NUnit.Framework;
    using Resources;
    using StatelessCore.Interfaces;

    [TestFixture]
    public class KenoBetSettingServiceTest
    {
        private const int LicenseeId = 1;
        private const int CustId = 2;

        private IAuthorizationService _authorizationService;
        private IKenoBetSettingDataAccess _dataAccess;
        private IKenoBetSettingService _service;

        [OneTimeSetUp]
        public void SetUp()
        {
            _authorizationService = Substitute.For<IAuthorizationService>();
            _dataAccess = Substitute.For<IKenoBetSettingDataAccess>();
            _service = new KenoBetSettingService(_authorizationService, _dataAccess);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _authorizationService = null;
            _dataAccess = null;
            _service = null;
        }

        [Test]
        public async Task GetViewModelAsync_WhenCall_BetSettingListReturnedFromDataAccess()
        {
            // Arrange
            _authorizationService.HasPermissionOnData(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<long>()).Returns(true);
            var betSetting = new List<BetSetting>
            {
                new BetSetting()
            };
            _dataAccess.GetBetSettingsAsync(CustId).Returns(Task.FromResult<IList<BetSetting>>(betSetting));

            // Action
            var result = await _service.GetViewModelAsync(LicenseeId, CustId);

            // Assert
            Assert.That(result.MainData.Count, Is.EqualTo(betSetting.Count));
        }

        [Test]
        public async Task GetViewModelAsync_HaveNoPermission_ReturnEmptyDataList()
        {
            // Arrange
            _authorizationService.HasPermissionOnData(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<long>()).Returns(false);

            // Action
            var result = await _service.GetViewModelAsync(LicenseeId, CustId);

            // Assert
            Assert.That(result.MainData.Count, Is.EqualTo(0));
        }

        [Test]
        public async Task UpdateBetSettingAsync_WhenCall_DataAccessExecuteWithCorrectParams()
        {
            // Arrange
            _authorizationService.HasPermissionOnData(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<long>()).Returns(true);
            var licenseeId = 1;
            var userId = 2;
            var custId = 3;
            var language = "en-US";
            var ip = "ip";
            var setting = new KenoBetSetting
            {
                GameTypeID = "1",
                MinBet = 2,
                MaxBet = 3,
                MaxPerMatch = 4
            };
            var xmlParam = "<settings><setting GameTypeID=\"1\" minbet=\"2\" maxbet=\"3\" maxpermatch=\"4\" ></setting></settings>";

            // Action
            var result = await _service.UpdateBetSettingAsync(licenseeId, custId, userId, language, ip, setting);

            // Assert
            _dataAccess.Received().UpdateBetSettingAsync(custId, userId, language, ip, xmlParam);
        }

        [Test]
        public async Task UpdateBetSettingAsync_HaveNoPermission_ReturnCorrectErrorResult()
        {
            // Arrange
            _authorizationService.HasPermissionOnData(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<long>()).Returns(false);
            var licenseeId = 1;
            var userId = 2;
            var custId = 3;
            var language = "en-US";
            var ip = "ip";
            var setting = new KenoBetSetting
            {
                GameTypeID = "1",
                MinBet = 2,
                MaxBet = 3,
                MaxPerMatch = 4
            };

            // Action
            var result = await _service.UpdateBetSettingAsync(licenseeId, custId, userId, language, ip, setting);

            // Assert
            Assert.That(result.Code, Is.EqualTo(1));
            Assert.That(result.Message, Is.EqualTo(ViewResources.AccessDeniedMsg));
        }
    }
}