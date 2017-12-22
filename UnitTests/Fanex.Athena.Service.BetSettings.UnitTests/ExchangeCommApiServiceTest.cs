namespace Fanex.Athena.Service.BetSettings.UnitTests
{
    using CitiXchangeLibrary;
    using Core.Users.Interfaces;
    using DataAccess.Core.Common.Interfaces;
    using Extension.Utility;
    using Logging;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public partial class ExchangeCommApiServiceTest
    {
        private const int LicenseeId = 1;
        private const int CustId = 2;
        private const int ProductId = 3;
        private const int UserId = 4;

        private IAuthorizationService _authorizationService;
        private ILogger _logger;
        private IUserLogDataAccess _userLogDataAccess;
        private IAppSettings _appSetting;
        private ICitiXchangeClient _apiClient;
        private ExchangeCommApiService _service;

        [SetUp]
        public void Setup()
        {
            _authorizationService = Substitute.For<IAuthorizationService>();
            _logger = Substitute.For<ILogger>();
            _userLogDataAccess = Substitute.For<IUserLogDataAccess>();
            _appSetting = Substitute.For<IAppSettings>();
            _apiClient = Substitute.For<ICitiXchangeClient>();
            _service = new ExchangeCommApiService(_authorizationService, _logger, _userLogDataAccess, _appSetting, _apiClient);

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
    }
}