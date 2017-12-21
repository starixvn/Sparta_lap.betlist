namespace Fanex.Athena.Service.BetSettings.UnitTests
{
    using CitiXchangeLibrary;
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

        private ILogger _logger;
        private IUserLogDataAccess _userLogDataAccess;
        private IAppSettings _appSetting;
        private ICitiXchangeClient _apiClient;
        private ExchangeCommApiService _service;

        [OneTimeSetUp]
        public void Setup()
        {
            _logger = Substitute.For<ILogger>();
            _userLogDataAccess = Substitute.For<IUserLogDataAccess>();
            _appSetting = Substitute.For<IAppSettings>();
            _apiClient = Substitute.For<ICitiXchangeClient>();
            _service = new ExchangeCommApiService(_logger, _userLogDataAccess, _appSetting, _apiClient);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _logger = null;
            _userLogDataAccess = null;
            _appSetting = null;
            _apiClient = null;
            _service = null;
        }
    }
}