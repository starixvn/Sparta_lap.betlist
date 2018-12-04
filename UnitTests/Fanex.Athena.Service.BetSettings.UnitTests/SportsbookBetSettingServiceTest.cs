namespace Fanex.Athena.Service.BetSettings.UnitTests
{
    using System.Collections.Generic;
    using BetSettings;
    using BetSettings.Interfaces;
    using Core.Users.Interfaces;
    using DataAccess.BetSettings.Interfaces;
    using Model.BetSettings.DTO;
    using Model.BetSettings.ViewModel;
    using Model.Core.DTO.Common;
    using NSubstitute;
    using NUnit.Framework;
    using Resources;
    using UnitTest.Common.Helpers;

    [TestFixture]
    public class SportsbookBetSettingServiceTest
    {
        private const int LicenseeId = 1;
        private const int CustId = 3;

        private IAuthorizationService _authorizationService;
        private ISportsbookBetSettingDataAccess _dataAccess;
        private ISportsbookBetSettingService _service;
        private IDcsBetSettingService _dcsBetSettingService;

        [SetUp]
        public void SetUp()
        {
            _authorizationService = Substitute.For<IAuthorizationService>();
            _dataAccess = Substitute.For<ISportsbookBetSettingDataAccess>();
            _dcsBetSettingService = Substitute.For<IDcsBetSettingService>();
            _service = new SportsbookBetSettingService(_authorizationService, _dataAccess, _dcsBetSettingService);
        }

        [Test]
        public void GetViewModel_HaveNoPermission_ReturnEmptyData()
        {
            // Arrange
            _authorizationService.HasPermissionOnData(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<long>()).Returns(false);

            // Action
            var result = _service.GetViewModel(LicenseeId, CustId);

            // Assert
            Assert.That(result.MainData, Is.Empty);
        }

        [Test]
        public void GetViewModel_WhenCall_ReturnDataFromDataAcccess()
        {
            // Arrange
            _authorizationService.HasPermissionOnData(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<long>()).Returns(true);

            var viewModel = new BetSettingViewModel
            {
                MainData = new List<BetSetting>
                {
                    new BetSetting(),
                    new BetSetting(),
                    new BetSetting()
                }
            };
            _dataAccess.GetBetSetting(CustId).Returns(viewModel);
            _dcsBetSettingService.GetBetSettings(LicenseeId, CustId, Arg.Any<int>()).Returns(new List<BetSetting>
            {
                new BetSetting()
            });

            // Action
            var result = _service.GetViewModel(LicenseeId, CustId);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.MainData.Count, Is.EqualTo(viewModel.MainData.Count));
            });
        }

        [Test]
        public void GetViewModel_NoEsportData_ThrowException()
        {
            // Arrange
            _authorizationService.HasPermissionOnData(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<long>()).Returns(true);
            var viewModel = new BetSettingViewModel
            {
                MainData = new List<BetSetting>()
            };
            _dataAccess.GetBetSetting(CustId).Returns(viewModel);
            _dcsBetSettingService.GetBetSettings(LicenseeId, CustId, Arg.Any<int>()).Returns(new List<BetSetting>());

            // Action
            TestDelegate testDelegate = () => _service.GetViewModel(LicenseeId, CustId);

            // Assert
            Assert.Throws<System.InvalidOperationException>(testDelegate, "sdfsdf");
        }

        [Test]
        public void UpdateBetSetting_WhenCall_CallUpdateBetSettingMethods()
        {
            // Arrange
            _authorizationService.HasPermissionOnData(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<long>()).Returns(true);
            _dataAccess.UpdateBetSetting(Arg.Any<CommonBetSetting>(), Arg.Any<Dictionary<string, object>>()).Returns(new ResultInfo());
            var commonBetSetting = FakeHelper.Create<CommonBetSetting>();

            // Action
            var result = _service.UpdateBetSetting(commonBetSetting, new Dictionary<string, object>(), new DcsBetSetting());

            // Assert
            Assert.Multiple(() =>
            {
                _dataAccess.Received(1).UpdateBetSetting(commonBetSetting, Arg.Any<Dictionary<string, object>>());
                _dcsBetSettingService.Received(1).UpdateBetSetting(
                    commonBetSetting.LicenseeId,
                    commonBetSetting.CustId,
                    commonBetSetting.UserId,
                    commonBetSetting.Ip,
                    Arg.Any<IEnumerable<DcsBetSetting>>());
            });
        }

        [Test]
        public void UpdateBetSetting_UpdateSportsbookFail_DoNotCallUpdateEsport()
        {
            // Arrange
            _authorizationService.HasPermissionOnData(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<long>()).Returns(true);
            _dataAccess.UpdateBetSetting(Arg.Any<CommonBetSetting>(), Arg.Any<Dictionary<string, object>>()).Returns(
                new ResultInfo
                {
                    Code = 1,
                    Message = "Error"
                });

            // Action
            var result = _service.UpdateBetSetting(new CommonBetSetting(), new Dictionary<string, object>(), new DcsBetSetting());

            // Assert
            Assert.Multiple(() =>
            {
                _dataAccess.Received(1).UpdateBetSetting(Arg.Any<CommonBetSetting>(), Arg.Any<Dictionary<string, object>>());
                _dcsBetSettingService.DidNotReceive().UpdateBetSetting(
                    Arg.Any<int>(),
                    Arg.Any<int>(),
                    Arg.Any<int>(),
                    Arg.Any<string>(),
                    Arg.Any<IEnumerable<DcsBetSetting>>());
            });
        }

        [Test]
        public void UpdateBetSetting_SettingParamIsNull_ReturnCorrectErrorResult()
        {
            // Arrange
            _authorizationService.HasPermissionOnData(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<long>()).Returns(true);

            // Action
            var result = _service.UpdateBetSetting(null, new Dictionary<string, object>(), new DcsBetSetting());

            // Assert
            Assert.That(result.Code, Is.EqualTo(1));
            Assert.That(result.Message, Is.EqualTo("Invalid parameters. bet setting is null"));
        }

        [Test]
        public void UpdateBetSetting_HaveNoPermission_ReturnCorrectErrorResult()
        {
            // Arrange
            _authorizationService.HasPermissionOnData(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<long>()).Returns(false);

            // Action
            var result = _service.UpdateBetSetting(new CommonBetSetting(), new Dictionary<string, object>(), new DcsBetSetting());

            // Assert
            Assert.That(result.Code, Is.EqualTo(1));
            Assert.That(result.Message, Is.EqualTo(ViewResources.AccessDeniedMsg));
        }
    }
}