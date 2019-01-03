namespace Fanex.Athena.Service.BetSettings.UnitTests
{
    using System;
    using CitiXchangeLibrary;
    using Constant.DataDomain;
    using DataAccess.BetSettings.Interfaces;
    using DataAccess.Core.Common.Constants;
    using DataAccess.Core.Common.Interfaces;
    using Extension.Utility;
    using Logging;
    using Model.Core.MetaModel.Common;
    using NSubstitute;
    using NUnit.Framework;
    using StatelessCore.Interfaces;
    using UnitTest.Common.Helpers;

    [TestFixture]
    public partial class ExchangeCommApiServiceTest
    {
        private readonly int _licenseeId = FakeHelper.Create<int>();
        private readonly int _custId = FakeHelper.Create<int>();
        private readonly int _userId = FakeHelper.Create<int>();

        private IAuthorizationService _authorizationService;
        private ILogger _logger;
        private IUserLogDataAccess _userLogDataAccess;
        private IDcsBetSettingDataAccess _betSettingDataAccess;
        private IAppSettings _appSetting;
        private ICitiXchangeClient _apiClient;
        private ExchangeCommApiService _service;

        [SetUp]
        public void Setup()
        {
            _authorizationService = Substitute.For<IAuthorizationService>();
            _logger = Substitute.For<ILogger>();
            _userLogDataAccess = Substitute.For<IUserLogDataAccess>();
            _betSettingDataAccess = Substitute.For<IDcsBetSettingDataAccess>();
            _appSetting = Substitute.For<IAppSettings>();
            _apiClient = Substitute.For<ICitiXchangeClient>();
            _service = new ExchangeCommApiService(_authorizationService, _logger, _userLogDataAccess, _betSettingDataAccess, _appSetting, _apiClient);

            _appSetting.Environment.Returns("PRO");
        }

        [TearDown]
        public void TearDown()
        {
            _logger = null;
            _userLogDataAccess = null;
            _appSetting = null;
            _apiClient = null;
            _service = null;
        }

        [Test]
        public void GetCommission_NotHavePerrmissonOnData_ReturnFailedResult()
        {
            // Arrange
            _authorizationService.HasPermissionOnData(_licenseeId, _custId, 0).ReturnsForAnyArgs(false);

            // Action
            var result = _service.GetCommission(_licenseeId, _custId);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.IsSuccessful, Is.False);
                _apiClient.DidNotReceiveWithAnyArgs().getProfile(0, Enviro.PRD);
            });
        }

        [TestCase(1, true)]
        [TestCase(0, false)]
        [TestCase(2, false)]
        public void GetCommission_HavePerrmissonOnData_ReturnCorrectResult(int returnStatus, bool isSuccess)
        {
            // Arrange
            var expectedComm = FakeHelper.Create<double>();
            _authorizationService.HasPermissionOnData(_licenseeId, _custId, 0).Returns(true);
            _apiClient.getProfile(_custId, Enviro.PRD).Returns(new GetProfile
            {
                status = (CitiStatus)returnStatus,
                commission = (decimal)expectedComm
            });

            // Action
            var result = _service.GetCommission(_licenseeId, _custId);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.IsSuccessful, Is.EqualTo(isSuccess));
                Assert.That(result.ExtendedData, Is.EqualTo(expectedComm));
                _apiClient.Received(1).getProfile(_custId, Enviro.PRD);
            });
        }

        [Test]
        public void GetCommission_ApiThrowException_LogAndReturnFailedResult()
        {
            // Arrange
            var errorMsg = FakeHelper.Create<string>();
            _authorizationService.HasPermissionOnData(_licenseeId, _custId, 0).ReturnsForAnyArgs(true);
            _apiClient.getProfile(0, Enviro.PRD).ReturnsForAnyArgs(x => { throw new Exception(errorMsg); });

            // Action
            var result = _service.GetCommission(_licenseeId, _custId);

            // Assert
            Assert.Multiple(() =>
            {
                _logger.Received(1).Error($"Exchange Api getProfile (custid: {_custId}): " + errorMsg);
                Assert.That(result.IsSuccessful, Is.False);
            });
        }

        [Test]
        public void UpdateCommission_NotHavePerrmissonOnData_ReturnFailedResult()
        {
            // Arrange
            _authorizationService.HasPermissionOnData(_licenseeId, _custId, 0).ReturnsForAnyArgs(false);

            // Action
            var result = _service.UpdateCommission(_licenseeId, _userId, _custId, 0, 0);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.IsSuccessful, Is.False);
                _apiClient.DidNotReceiveWithAnyArgs().getProfile(0, Enviro.PRD);
            });
        }

        [TestCase(false)]
        [TestCase(null)]
        public void UpdateCommission_StatusIsNotEnable_ReturnFailedResult(bool? isEnable)
        {
            // Arrange
            _authorizationService.HasPermissionOnData(_licenseeId, _custId, 0).ReturnsForAnyArgs(true);
            _betSettingDataAccess.GetStatus(0, 0).ReturnsForAnyArgs(isEnable);

            // Action
            var result = _service.UpdateCommission(_licenseeId, _userId, _custId, 0, 0);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.IsSuccessful, Is.False);
                _apiClient.DidNotReceiveWithAnyArgs().getProfile(0, Enviro.PRD);
            });
        }

        [TestCase(1, true)]
        [TestCase(0, false)]
        [TestCase(2, false)]
        public void UpdateCommission_HavePerrmissonOnData_ReturnCorrectResult(int returnStatus, bool isSuccess)
        {
            // Arrange
            var expectedComm = FakeHelper.Create<double>();
            _authorizationService.HasPermissionOnData(_licenseeId, _custId, 0).Returns(true);
            _betSettingDataAccess.GetStatus(_custId, ProductTypes.Exchange).Returns(true);
            _apiClient.updateCommission(_custId, expectedComm, Enviro.PRD).Returns(new CommissionInfo
            {
                status = (CitiStatus)returnStatus,
                commission = expectedComm
            });

            // Action
            var result = _service.UpdateCommission(_licenseeId, _userId, _custId, 0, expectedComm);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.IsSuccessful, Is.EqualTo(isSuccess));
                Assert.That(result.ExtendedData, Is.EqualTo(expectedComm));
                _apiClient.Received(1).updateCommission(_custId, expectedComm, Enviro.PRD);
            });
        }

        [Test]
        public void UpdateCommission_ApiThrowException_LogAndReturnFailedResult()
        {
            // Arrange
            var errorMsg = FakeHelper.Create<string>();
            _authorizationService.HasPermissionOnData(_licenseeId, _custId, 0).ReturnsForAnyArgs(true);
            _betSettingDataAccess.GetStatus(_custId, ProductTypes.Exchange).ReturnsForAnyArgs(true);
            _apiClient.updateCommission(_custId, 0, Enviro.PRD).ReturnsForAnyArgs(x => { throw new Exception(errorMsg); });

            // Action
            var result = _service.UpdateCommission(_licenseeId, _userId, _custId, 0, 0);

            // Assert
            Assert.Multiple(() =>
            {
                _logger.Received(1).Error("Exchange Api updateCommission: " + errorMsg);
                Assert.That(result.IsSuccessful, Is.False);
            });
        }

        [Test]
        public void UpdateCommission_CallApiSuccess_CallWriteLogRunSP()
        {
            // Arrange
            var oldComm = FakeHelper.Create<double>();
            var newComm = FakeHelper.Create<double>();
            _authorizationService.HasPermissionOnData(_licenseeId, _custId, 0).ReturnsForAnyArgs(true);
            _betSettingDataAccess.GetStatus(_custId, ProductTypes.Exchange).ReturnsForAnyArgs(true);

            // Action
            var result = _service.UpdateCommission(_licenseeId, _userId, _custId, oldComm, newComm);

            // Assert
            Assert.Multiple(() =>
            {
                _userLogDataAccess.Received(1).WriteLog(Arg.Is<UserLogMetaModel>(x =>
                    x.ActionID == UserLogAction.ExchangeCommission &&
                    x.UserId == _userId &&
                    x.CustID == _custId &&
                    x.Doing == $"<ChargeCommission OldValue='{oldComm}' NewValue='{newComm}' />"));
            });
        }
    }
}