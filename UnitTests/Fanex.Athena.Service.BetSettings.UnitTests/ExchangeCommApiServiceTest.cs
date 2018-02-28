namespace Fanex.Athena.Service.BetSettings.UnitTests
{
    using System;
    using CitiXchangeLibrary;
    using Core.Users.Interfaces;
    using DataAccess.BetSettings.Interfaces;
    using DataAccess.Core.Common.Constants;
    using DataAccess.Core.Common.Interfaces;
    using Extension.Utility;
    using Logging;
    using Model.Core.DataDomain;
    using Model.Core.MetaModel.Common;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public partial class ExchangeCommApiServiceTest
    {
        private const int LicenseeId = 1;
        private const int CustId = 2;
        private const int UserId = 4;

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
            _authorizationService.HasPermissionOnData(0, 0, 0).ReturnsForAnyArgs(false);

            // Action
            var result = _service.GetCommission(0, 0);

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
            const double ExpectedComm = 999;
            _authorizationService.HasPermissionOnData(LicenseeId, CustId, 0).Returns(true);
            _apiClient.getProfile(CustId, Enviro.PRD).Returns(new GetProfile
            {
                status = (CitiStatus)returnStatus,
                commission = (decimal)ExpectedComm
            });

            // Action
            var result = _service.GetCommission(LicenseeId, CustId);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.IsSuccessful, Is.EqualTo(isSuccess));
                Assert.That(result.ExtendedData, Is.EqualTo(ExpectedComm));
                _apiClient.Received(1).getProfile(CustId, Enviro.PRD);
            });
        }

        [Test]
        public void GetCommission_ApiThrowException_LogAndReturnFailedResult()
        {
            // Arrange
            const string ErrorMsg = "something wrong";
            _authorizationService.HasPermissionOnData(0, 0, 0).ReturnsForAnyArgs(true);
            _apiClient.getProfile(0, Enviro.PRD).ReturnsForAnyArgs(x => { throw new Exception(ErrorMsg); });

            // Action
            var result = _service.GetCommission(0, 0);

            // Assert
            Assert.Multiple(() =>
            {
                _logger.Received(1).Error("Exchange Api getProfile (custid: 0): " + ErrorMsg);
                Assert.That(result.IsSuccessful, Is.False);
            });
        }

        [Test]
        public void UpdateCommission_NotHavePerrmissonOnData_ReturnFailedResult()
        {
            // Arrange
            _authorizationService.HasPermissionOnData(0, 0, 0).ReturnsForAnyArgs(false);

            // Action
            var result = _service.UpdateCommission(0, 0, 0, 0, 0);

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
            _authorizationService.HasPermissionOnData(0, 0, 0).ReturnsForAnyArgs(true);
            _betSettingDataAccess.GetStatus(0, 0).ReturnsForAnyArgs(isEnable);

            // Action
            var result = _service.UpdateCommission(0, 0, 0, 0, 0);

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
            const double ExpectedComm = 999;
            _authorizationService.HasPermissionOnData(LicenseeId, CustId, 0).Returns(true);
            _betSettingDataAccess.GetStatus(CustId, ProductTypes.Exchange).Returns(true);
            _apiClient.updateCommission(CustId, ExpectedComm, Enviro.PRD).Returns(new CommissionInfo
            {
                status = (CitiStatus)returnStatus,
                commission = ExpectedComm
            });

            // Action
            var result = _service.UpdateCommission(LicenseeId, UserId, CustId, 0, ExpectedComm);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.IsSuccessful, Is.EqualTo(isSuccess));
                Assert.That(result.ExtendedData, Is.EqualTo(ExpectedComm));
                _apiClient.Received(1).updateCommission(CustId, ExpectedComm, Enviro.PRD);
            });
        }

        [Test]
        public void UpdateCommission_ApiThrowException_LogAndReturnFailedResult()
        {
            // Arrange
            const string ErrorMsg = "something wrong";
            _authorizationService.HasPermissionOnData(0, 0, 0).ReturnsForAnyArgs(true);
            _betSettingDataAccess.GetStatus(0, 0).ReturnsForAnyArgs(true);
            _apiClient.updateCommission(0, 0, Enviro.PRD).ReturnsForAnyArgs(x => { throw new Exception(ErrorMsg); });

            // Action
            var result = _service.UpdateCommission(0, 0, 0, 0, 0);

            // Assert
            Assert.Multiple(() =>
            {
                _logger.Received(1).Error("Exchange Api updateCommission: " + ErrorMsg);
                Assert.That(result.IsSuccessful, Is.False);
            });
        }

        [Test]
        public void UpdateCommission_CallApiSuccess_CallWriteLogRunSP()
        {
            // Arrange
            const double OldComm = 2.5;
            const double NewComm = 5.5;
            _authorizationService.HasPermissionOnData(0, 0, 0).ReturnsForAnyArgs(true);
            _betSettingDataAccess.GetStatus(0, 0).ReturnsForAnyArgs(true);

            // Action
            var result = _service.UpdateCommission(LicenseeId, UserId, CustId, OldComm, NewComm);

            // Assert
            Assert.Multiple(() =>
            {
                _userLogDataAccess.Received(1).WriteLog(Arg.Is<UserLogMetaModel>(x =>
                    x.ActionID == UserLogAction.ExchangeCommission &&
                    x.UserId == UserId &&
                    x.CustID == CustId &&
                    x.Doing == $"<ChargeCommission OldValue='{OldComm}' NewValue='{NewComm}' />"));
            });
        }
    }
}