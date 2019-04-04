namespace Fanex.Athena.Service.BetSettings.UnitTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using BetSettings;
    using Casino.Interfaces;
    using Casino.Wrappers;
    using DataAccess.BetSettings.Interfaces;
    using Extension.Utility;
    using Interfaces;
    using Model.BetSettings.DTO.RngCasino;
    using Model.BetSettings.MetaModel;
    using Model.Customer.DTO;
    using NSubstitute;
    using NUnit.Framework;
    using Resources;
    using StatelessCore.Interfaces;

    [TestFixture]
    public partial class RngCasinoBetSettingServiceTest
    {
        private const int CustId = 1;
        private const int CurrencyId = 1;
        private const string Language = "en-US";

        private IAuthorizationService _authorizationService;
        private IAppSettings _appSettings;
        private ICasinoSettingService _casinoSetting;
        private ICustomerApiWrapper _customerApiWrapper;
        private IRngCasinoBetSettingDataAccess _dataAccess;
        private IRngCasinoBetSettingService _service;

        [SetUp]
        public void SetUp()
        {
            _authorizationService = Substitute.For<IAuthorizationService>();
            _appSettings = Substitute.For<IAppSettings>();
            _casinoSetting = Substitute.For<ICasinoSettingService>();
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
            var result = _service.GetViewModel(CustId, customer, string.Empty);

            // Assert
            Assert.Multiple(() =>
            {
                _casinoSetting.DidNotReceiveWithAnyArgs().GetRngCasinoSettings(CustId, CurrencyId, string.Empty);
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
            _casinoSetting.GetRngCasinoSettings(CustId, CurrencyId, Language).Returns(resultGameKindList);
            var customer = new CustomerInfo
            {
                SyncCasino = true,
                CurrencyID = CurrencyId
            };

            // Action
            var result = _service.GetViewModel(CustId, customer, Language);

            // Assert
            Assert.Multiple(() =>
            {
                _casinoSetting.Received().GetRngCasinoSettings(CustId, CurrencyId, Language);
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

        [Test]
        public async Task UpdateStatusAsync_NoPermission_ReturnErrorResult()
        {
            // Arrange
            var metaModel = new BetSettingCasinoStatusMetaModel
            {
                LicenseeId = 1,
                CustId = 2
            };

            _authorizationService.HasPermissionOnData(metaModel.LicenseeId, metaModel.CustId, Arg.Any<long>()).Returns(false);

            // Action
            var result = await _service.UpdateStatusAsync(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                _dataAccess.DidNotReceive().UpdateStatusAsync(metaModel);
                Assert.That(result.IsSuccessful, Is.False);
                Assert.That(result.Message, Is.EqualTo(ViewResources.AccessDeniedMsg));
            });
        }

        [Test]
        public async Task UpdateStatusAsync_UpdateSettingIsNotAvailable_ReturnErrorResult()
        {
            // Arrange
            var metaModel = new BetSettingCasinoStatusMetaModel
            {
                LicenseeId = 1,
                CustId = 2,
                IsSyncCasino = false
            };

            _authorizationService.HasPermissionOnData(metaModel.LicenseeId, metaModel.CustId, Arg.Any<long>()).Returns(true);

            // Action
            var result = await _service.UpdateStatusAsync(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                _dataAccess.DidNotReceive().UpdateStatusAsync(metaModel);
                Assert.That(result.IsSuccessful, Is.False);
                Assert.That(result.Message, Is.EqualTo(ViewResources.RNGCasinoUpdateSettingUnAvailableMsg));
            });
        }

        [Test]
        public async Task UpdateStatusAsync_ApiReturnsFalse_ReturnErrorResult()
        {
            // Arrange
            var metaModel = new BetSettingCasinoStatusMetaModel
            {
                LicenseeId = 1,
                CustId = 2,
                IsSyncCasino = true
            };

            _appSettings.IsDisableRngCasino.Returns(false);
            _authorizationService.HasPermissionOnData(metaModel.LicenseeId, metaModel.CustId, Arg.Any<long>()).Returns(true);
            _customerApiWrapper.UpdateCustEnableCasino(metaModel)
                .Returns(false);

            // Action
            var result = await _service.UpdateStatusAsync(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                _dataAccess.DidNotReceive().UpdateStatusAsync(metaModel);
                Assert.That(result.IsSuccessful, Is.False);
                Assert.That(result.Message, Is.EqualTo(ViewResources.UpdateUnSuccess));
            });
        }

        [Test]
        public async Task UpdateStatusAsync_ApiReturnsTrue_CallDataAccessMethod()
        {
            // Arrange
            var metaModel = new BetSettingCasinoStatusMetaModel
            {
                LicenseeId = 1,
                CustId = 2,
                IsSyncCasino = true
            };

            _appSettings.IsDisableRngCasino.Returns(false);
            _authorizationService.HasPermissionOnData(metaModel.LicenseeId, metaModel.CustId, Arg.Any<long>()).Returns(true);
            _customerApiWrapper.UpdateCustEnableCasino(metaModel)
                .Returns(true);

            // Action
            await _service.UpdateStatusAsync(metaModel);

            // Assert
            await _dataAccess.Received(1).UpdateStatusAsync(metaModel);
        }

        [Test]
        public void UpdateSetting_IsDisableCasino_ReturnErrorResult()
        {
            // Arrange
            var metaModel = new RngCasinoBetSettingMetaModel();

            // Action
            var result = _service.UpdateSetting(metaModel, false, true);

            // Assert
            Assert.Multiple(() =>
            {
                _casinoSetting.DidNotReceive().UpdateSetting(metaModel.TableParam);
                Assert.That(result.IsSuccessful, Is.False);
                Assert.That(result.Message, Is.EqualTo(ViewResources.AccessDeniedMsg));
            });
        }

        [Test]
        public void UpdateSetting_NoPermission_ReturnErrorResult()
        {
            // Arrange
            var metaModel = new RngCasinoBetSettingMetaModel
            {
                LicenseeId = 3,
                CustId = 4
            };

            _authorizationService.HasPermissionOnData(metaModel.LicenseeId, metaModel.CustId, Arg.Any<long>()).Returns(false);

            // Action
            var result = _service.UpdateSetting(metaModel, false, false);

            // Assert
            Assert.Multiple(() =>
            {
                _casinoSetting.DidNotReceive().UpdateSetting(metaModel.TableParam);
                Assert.That(result.IsSuccessful, Is.False);
                Assert.That(result.Message, Is.EqualTo(ViewResources.AccessDeniedMsg));
            });
        }

        [Test]
        public void UpdateSetting_IsNotSyncCasino_ReturnErrorResult()
        {
            // Arrange
            var metaModel = new RngCasinoBetSettingMetaModel
            {
                LicenseeId = 5,
                CustId = 4
            };

            _authorizationService.HasPermissionOnData(metaModel.LicenseeId, metaModel.CustId, Arg.Any<long>()).Returns(true);

            // Action
            var result = _service.UpdateSetting(metaModel, false, false);

            // Assert
            Assert.Multiple(() =>
            {
                _casinoSetting.DidNotReceive().UpdateSetting(metaModel.TableParam);
                Assert.That(result.IsSuccessful, Is.False);
                Assert.That(result.Message, Is.EqualTo(ViewResources.RNGCasinoUpdateSettingUnAvailableMsg));
            });
        }

        [Test]
        public void UpdateSetting_IsDisableRngCasino_ReturnErrorResult()
        {
            // Arrange
            var metaModel = new RngCasinoBetSettingMetaModel
            {
                LicenseeId = 5,
                CustId = 4
            };

            _authorizationService.HasPermissionOnData(metaModel.LicenseeId, metaModel.CustId, Arg.Any<long>()).Returns(true);
            _appSettings.IsDisableRngCasino.Returns(true);

            // Action
            var result = _service.UpdateSetting(metaModel, true, false);

            // Assert
            Assert.Multiple(() =>
            {
                _casinoSetting.DidNotReceive().UpdateSetting(metaModel.TableParam);
                Assert.That(result.IsSuccessful, Is.False);
                Assert.That(result.Message, Is.EqualTo(ViewResources.RNGCasinoUpdateSettingUnAvailableMsg));
            });
        }

        [Test]
        public void UpdateSetting_ValidData_CallDataAccessMethodFail_ReturnErrorResult()
        {
            // Arrange
            var metaModel = new RngCasinoBetSettingMetaModel
            {
                LicenseeId = 5,
                CustId = 4
            };

            _authorizationService.HasPermissionOnData(metaModel.LicenseeId, metaModel.CustId, Arg.Any<long>()).Returns(true);
            _appSettings.IsDisableRngCasino.Returns(false);
            _casinoSetting.UpdateSetting(metaModel.TableParam).Returns(false);

            // Action
            var result = _service.UpdateSetting(metaModel, true, false);

            // Assert
            Assert.Multiple(() =>
            {
                _casinoSetting.Received().UpdateSetting(metaModel.TableParam);
                Assert.That(result.IsSuccessful, Is.False);
                Assert.That(result.Message, Is.EqualTo(ViewResources.Fail));
            });
        }

        [Test]
        public void UpdateSetting_ValidData_CallDataAccessMethodSuccessfully_ReturnSuccessfulResult()
        {
            // Arrange
            var metaModel = new RngCasinoBetSettingMetaModel
            {
                LicenseeId = 5,
                CustId = 4
            };

            _authorizationService.HasPermissionOnData(metaModel.LicenseeId, metaModel.CustId, Arg.Any<long>()).Returns(true);
            _appSettings.IsDisableRngCasino.Returns(false);
            _casinoSetting.UpdateSetting(metaModel.TableParam).Returns(true);

            // Action
            var result = _service.UpdateSetting(metaModel, true, false);

            // Assert
            Assert.Multiple(() =>
            {
                _casinoSetting.Received().UpdateSetting(metaModel.TableParam);
                Assert.That(result.IsSuccessful, Is.True);
                Assert.That(result.Message, Is.EqualTo(ViewResources.MessageSuccess));
            });
        }
    }
}