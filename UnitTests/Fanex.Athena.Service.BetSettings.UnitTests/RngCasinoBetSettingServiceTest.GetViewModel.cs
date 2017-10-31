namespace Fanex.Athena.Service.BetSettings.UnitTests
{
    using System.Collections.Generic;
    using System.Linq;
    using BetSettings;
    using Casino.Interfaces;
    using Core.Users.Interfaces;
    using DataAccess.BetSettings.Interfaces;
    using Extension.Utility;
    using Interfaces;
    using Model.BetSettings.DTO.RngCasino;
    using Model.Customer.DTO;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public partial class RngCasinoBetSettingServiceTest
    {
        private const int CustId = 1;
        private const string Language = "en-US";

        private IAuthorizationService _authorizationService;
        private IAppSettings _appSettings;
        private ICasinoSetting _casinoSetting;
        private ICustomerApiWrapper _customerApiWrapper;
        private IRngCasinoBetSettingDataAccess _dataAccess;
        private IRngCasinoBetSettingService _service;

        [SetUp]
        public void SetUp()
        {
            _authorizationService = Substitute.For<IAuthorizationService>();
            _appSettings = Substitute.For<IAppSettings>();
            _casinoSetting = Substitute.For<ICasinoSetting>();
            _customerApiWrapper = Substitute.For<ICustomerApiWrapper>();
            _dataAccess = Substitute.For<IRngCasinoBetSettingDataAccess>();
            _service = new RngCasinoBetSettingService(_authorizationService, _appSettings, _casinoSetting, _customerApiWrapper, _dataAccess);
        }

        [TearDown]
        public void TearDown()
        {
            _appSettings = null;
            _casinoSetting = null;
            _dataAccess = null;
            _service = null;
        }

        [Test]
        public void GetViewModel_CustomerNotSyncCasino_NotCallCasinoSetting()
        {
            // Arrange
            var customer = new CustomerInfo
            {
                SyncCasino = false
            };

            // Action
            var result = _service.GetViewModel(0, customer, string.Empty);

            // Assert
            Assert.Multiple(() =>
            {
                _casinoSetting.DidNotReceiveWithAnyArgs().GetRngCasinoSettings(0, string.Empty);
                Assert.That(result.GameKinds.Any(), Is.False);
            });
        }

        [Test]
        public void GetViewModel_CustomerIsSyncCasino_GetDataFromCasinoSetting()
        {
            // Arrange
            var resultGameKindList = new List<GameKind>
            {
                new GameKind()
            };
            _casinoSetting.GetRngCasinoSettings(CustId, Language).Returns(resultGameKindList);
            var customer = new CustomerInfo
            {
                SyncCasino = true
            };

            // Action
            var result = _service.GetViewModel(CustId, customer, Language);

            // Assert
            Assert.Multiple(() =>
            {
                _casinoSetting.Received().GetRngCasinoSettings(CustId, Language);
                Assert.That(result.GameKinds.Count(), Is.EqualTo(resultGameKindList.Count));
            });
        }

        [TestCase(false, true, true)]
        [TestCase(false, false, null)]
        [TestCase(true, false, null)]
        [TestCase(true, true, false)]
        public void GetViewModel_CustomerIsSyncCasino_ReturnCorrectIsEnabled(
            bool isDiabledCasino,
            bool isSyncCasino,
            bool? expectedIsEnabled)
        {
            // Arrange
            var customer = new CustomerInfo
            {
                DisableCasino = isDiabledCasino,
                SyncCasino = isSyncCasino
            };

            // Action
            var result = _service.GetViewModel(CustId, customer, Language);

            // Assert
            Assert.That(result.IsEnabled, Is.EqualTo(expectedIsEnabled));
        }
    }
}