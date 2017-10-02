namespace Fanex.Athena.Service.Customer.UnitTests
{
    using System.Collections.Generic;
    using System.Linq;
    using Casino.Interfaces;
    using Core.Common.Interfaces;
    using Core.Users.Interfaces;
    using DataAccess.Customer.Interfaces;
    using Extension.Utility;
    using Model.Casino.MetaModel;
    using Model.Core.DTO.Common;
    using Model.Core.DTO.Users;
    using Model.Core.ResultModel;
    using Model.Customer.DTO;
    using Model.Customer.MetaModel;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class CustomerListServiceTest
    {
        private ICustomerDataAccess _customerDataAccess;

        private ICustomerListDataAccess _customerListDataAccess;

        private ICasinoSetting _casinoSetting;

        private IAuthorizationService _authorizationService;

        private IProductService _productService;

        private ISessionService _sessionService;

        private IRNGCasinoService _rngCasinoService;

        private IAppSettings _appSettings;

        private CustomerListService _customerListService;

        [SetUp]
        public void SetUp()
        {
            _customerDataAccess = Substitute.For<ICustomerDataAccess>();
            _customerListDataAccess = Substitute.For<ICustomerListDataAccess>();
            _casinoSetting = Substitute.For<ICasinoSetting>();
            _authorizationService = Substitute.For<IAuthorizationService>();
            _productService = Substitute.For<IProductService>();
            _sessionService = Substitute.For<ISessionService>();
            _rngCasinoService = Substitute.For<IRNGCasinoService>();
            _appSettings = Substitute.For<IAppSettings>();

            _customerListService = new CustomerListService(
                _customerDataAccess,
                _customerListDataAccess,
                _casinoSetting,
                _authorizationService,
                _productService,
                _sessionService,
                _rngCasinoService,
                _appSettings);
        }

        [TearDown]
        public void TearDown()
        {
            _customerListDataAccess = null;
            _casinoSetting = null;
            _authorizationService = null;
            _productService = null;
            _sessionService = null;
            _rngCasinoService = null;
            _appSettings = null;
            _customerListService = null;
        }

        [Test]
        public void SearchCustomers_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var userId = 1;
            var username = "tester";
            var agentListXml = "<root></root>";
            var expectedResult = new string[1] { "result1" };
            _sessionService.LicenseeSession.Returns(new LicenseeSession());
            _customerListDataAccess.SearchCustomers(userId, username, agentListXml).Returns(expectedResult);

            // Action
            var result = _customerListService.SearchCustomers(userId, username, agentListXml);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void Search_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var expectedResult = new ReportResultModel<CustomerDTO>
            {
                DataList = new List<CustomerDTO>()
                {
                    new CustomerDTO()
                },
                TotalRows = 10
            };
            _sessionService.LicenseeSession.Returns(new LicenseeSession());
            _customerListDataAccess.Search(Arg.Any<CustomerListMetaModel>()).Returns(expectedResult);

            // Action
            var result = _customerListService.Search(new CustomerListMetaModel());

            // Assert
            Assert.That(result.DataList.Count(), Is.EqualTo(expectedResult.DataList.Count()));
            Assert.That(result.TotalRows, Is.EqualTo(expectedResult.TotalRows));
        }

        [Test]
        public void GetCustomerLoginInfo_WhenCallWithCorrectPermission_ReturnCorrectData()
        {
            // Arrange
            const int LicenseeId = 1, CustId = 2;
            var expectedResult = new LogInfo();
            _authorizationService.HasPermissionOnData(LicenseeId, CustId, Arg.Any<long>()).Returns(true);
            _customerListDataAccess.GetCustomerLoginInfo(CustId, LicenseeId).Returns(expectedResult);

            // Action
            var result = _customerListService.GetCustomerLoginInfo(CustId, LicenseeId);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void GetCustomerLoginInfo_HaveNoPermission_DataAccessIsNotCalled()
        {
            // Arrange
            const int LicenseeId = 1, CustId = 2;
            _authorizationService.HasPermissionOnData(LicenseeId, CustId, Arg.Any<long>()).Returns(false);

            // Action
            var result = _customerListService.GetCustomerLoginInfo(CustId, LicenseeId);

            // Assert
            _customerListDataAccess.DidNotReceive().GetCustomerLoginInfo(CustId, LicenseeId);
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void UpdateCustomersCommission_CallApiSuccess_ReturnCorrectResultInfo()
        {
            // Arrange
            var isSyncCSComm = true;
            var metaModel = new UpdatedCustomerComMetaModel
            {
                CSComm = 10m,
                Ip = "1.1.1.1",
                UserId = 1
            };
            var userSession = new UserSession { UserName = "tester" };
            _sessionService.UserSession.Returns(userSession);
            var expectedResult = new ResultInfo { ExtendedData = "1,2,3" };
            _customerListDataAccess.UpdateCustomersCommission(metaModel).Returns(expectedResult);
            _rngCasinoService.UpdateMemberCommission(Arg.Is<RNGCasinoUpdateMemberComMetaModel>(param =>
                                                    param.Comm == metaModel.CSComm &&
                                                    param.CustomerId.Length == 3 &&
                                                    param.FromIp == metaModel.Ip &&
                                                    param.ModifyBy == metaModel.UserId &&
                                                    param.ModifyUserName == userSession.UserName))
                                                    .Returns(true);

            // Action
            var result = _customerListService.UpdateCustomersCommission(metaModel, isSyncCSComm);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void UpdateCustomersCommission_UpdateFailed_ReturnCorrectResultInfo()
        {
            // Arrange
            var isSyncCSComm = true;
            var expectedResult = new ResultInfo { Code = 100, Message = "Error", ExtendedData = "1,2,3" };
            _customerListDataAccess.UpdateCustomersCommission(Arg.Any<UpdatedCustomerComMetaModel>()).Returns(expectedResult);

            // Action
            var result = _customerListService.UpdateCustomersCommission(new UpdatedCustomerComMetaModel(), isSyncCSComm);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void UpdateCustomersCommission_SyncCSCommIsFalse_ReturnCorrectResultInfo()
        {
            // Arrange
            var isSyncCSComm = false;
            var expectedResult = new ResultInfo { Code = 100, Message = "Error", ExtendedData = "1,2,3" };
            _customerListDataAccess.UpdateCustomersCommission(Arg.Any<UpdatedCustomerComMetaModel>()).Returns(expectedResult);

            // Action
            var result = _customerListService.UpdateCustomersCommission(new UpdatedCustomerComMetaModel(), isSyncCSComm);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void UpdateCustomersCommission_ExtendedDataInResultIsEmpty_ReturnCorrectResultInfo()
        {
            // Arrange
            var isSyncCSComm = true;
            var expectedResult = new ResultInfo();
            _customerListDataAccess.UpdateCustomersCommission(Arg.Any<UpdatedCustomerComMetaModel>()).Returns(expectedResult);

            // Action
            var result = _customerListService.UpdateCustomersCommission(new UpdatedCustomerComMetaModel(), isSyncCSComm);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void UpdateCustomersCommission_CallApiFailed_ReturnCorrectResultInfo()
        {
            // Arrange
            var isSyncCSComm = true;
            var metaModel = new UpdatedCustomerComMetaModel
            {
                CSComm = 10m,
                Ip = "1.1.1.1",
                UserId = 1
            };
            var userSession = new UserSession { UserName = "tester" };
            _sessionService.UserSession.Returns(userSession);
            var resultInfo = new ResultInfo { ExtendedData = "1" };
            _customerListDataAccess.UpdateCustomersCommission(metaModel).Returns(resultInfo);
            _rngCasinoService.UpdateMemberCommission(Arg.Is<RNGCasinoUpdateMemberComMetaModel>(param =>
                                                    param.Comm == metaModel.CSComm &&
                                                    param.CustomerId.Length == 1 &&
                                                    param.FromIp == metaModel.Ip &&
                                                    param.ModifyBy == metaModel.UserId &&
                                                    param.ModifyUserName == userSession.UserName))
                                                    .Returns(false);
            var expectedResult = new ResultInfo
            {
                Code = 1,
                Message = "Having errors when calling CasinoAPI to update customer commission."
            };

            // Action
            var result = _customerListService.UpdateCustomersCommission(metaModel, isSyncCSComm);

            // Assert
            Assert.That(result.Code, Is.EqualTo(expectedResult.Code));
            Assert.That(result.Message, Is.EqualTo(expectedResult.Message));
        }

        [Test]
        public void RemoveCustomersFromGroup_CallApiSuccess_ReturnCorrectData()
        {
            // Arrange
            var isAllowedRNGCasino = true;
            var metaModel = new RemoveFromGroupComMetaModel { Ip = "1.1.1.1", IsRemovePlayercomm = true };
            var expectedResult = new ResultInfo { ExtendedData = "1,2,3" };
            _customerListDataAccess.RemoveCustomersFromGroup(metaModel).Returns(expectedResult);
            var userSession = new UserSession { UserName = "tester", UserId = 1 };
            _sessionService.UserSession.Returns(userSession);
            _rngCasinoService.UpdateMemberCommission(Arg.Is<RNGCasinoUpdateMemberComMetaModel>(param =>
                                                    param.Comm == 0 &&
                                                    param.CustomerId.Length == 3 &&
                                                    param.FromIp == metaModel.Ip &&
                                                    param.ModifyBy == userSession.UserId &&
                                                    param.ModifyUserName == userSession.UserName))
                                                    .Returns(true);

            // Action
            var result = _customerListService.RemoveCustomersFromGroup(metaModel, isAllowedRNGCasino);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void RemoveCustomersFromGroup_CallApiFailed_ReturnCorrectData()
        {
            // Arrange
            var isAllowedRNGCasino = true;
            var metaModel = new RemoveFromGroupComMetaModel { Ip = "1.1.1.1", IsRemovePlayercomm = true };
            var resultInfo = new ResultInfo { ExtendedData = "1,2,3" };
            _customerListDataAccess.RemoveCustomersFromGroup(metaModel).Returns(resultInfo);
            var userSession = new UserSession { UserName = "tester", UserId = 1 };
            _sessionService.UserSession.Returns(userSession);
            _rngCasinoService.UpdateMemberCommission(Arg.Is<RNGCasinoUpdateMemberComMetaModel>(param =>
                                                    param.Comm == 0 &&
                                                    param.CustomerId.Length == 3 &&
                                                    param.FromIp == metaModel.Ip &&
                                                    param.ModifyBy == userSession.UserId &&
                                                    param.ModifyUserName == userSession.UserName))
                                                    .Returns(false);
            var expectedResult = new ResultInfo
            {
                Code = 1,
                Message = "Having errors when calling CasinoAPI to update customer commission."
            };

            // Action
            var result = _customerListService.RemoveCustomersFromGroup(metaModel, isAllowedRNGCasino);

            // Assert
            Assert.That(result.Code, Is.EqualTo(expectedResult.Code));
            Assert.That(result.Message, Is.EqualTo(expectedResult.Message));
        }

        [Test]
        public void RemoveCustomersFromGroup_RemoveFailed_ReturnCorrectData()
        {
            // Arrange
            var isAllowedRNGCasino = true;
            var expectedResult = new ResultInfo { Code = 1, Message = "Failed" };
            _customerListDataAccess.RemoveCustomersFromGroup(Arg.Any<RemoveFromGroupComMetaModel>()).Returns(expectedResult);

            // Action
            var result = _customerListService.RemoveCustomersFromGroup(new RemoveFromGroupComMetaModel(), isAllowedRNGCasino);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void RemoveCustomersFromGroup_IsRemovePlayercommInMetaModelIsFalse_ReturnCorrectData()
        {
            // Arrange
            var isAllowedRNGCasino = true;
            var metaModel = new RemoveFromGroupComMetaModel { IsRemovePlayercomm = false };
            var expectedResult = new ResultInfo();
            _customerListDataAccess.RemoveCustomersFromGroup(metaModel).Returns(expectedResult);

            // Action
            var result = _customerListService.RemoveCustomersFromGroup(metaModel, isAllowedRNGCasino);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void RemoveCustomersFromGroup_IsAllowedRNGCasinoIsFalse_ReturnCorrectData()
        {
            // Arrange
            var isAllowedRNGCasino = false;
            var metaModel = new RemoveFromGroupComMetaModel { IsRemovePlayercomm = true };
            var expectedResult = new ResultInfo();
            _customerListDataAccess.RemoveCustomersFromGroup(metaModel).Returns(expectedResult);

            // Action
            var result = _customerListService.RemoveCustomersFromGroup(metaModel, isAllowedRNGCasino);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void RemoveCustomersFromGroup_ExtendedDataInResultIsEmpty_ReturnCorrectData()
        {
            // Arrange
            var isAllowedRNGCasino = true;
            var metaModel = new RemoveFromGroupComMetaModel { IsRemovePlayercomm = true };
            var expectedResult = new ResultInfo { ExtendedData = string.Empty };
            _customerListDataAccess.RemoveCustomersFromGroup(metaModel).Returns(expectedResult);

            // Action
            var result = _customerListService.RemoveCustomersFromGroup(metaModel, isAllowedRNGCasino);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void UpdateOddsGroup_WhenCallWithCorrectPermission_ReturnCorrectData()
        {
            // Arrange
            const int LicenseeId = 1, CustId = 2;
            var metaModel = new CustomerOddsGroupMetaModel { LicenseeId = LicenseeId, CustId = CustId };
            _authorizationService.HasPermissionOnData(LicenseeId, CustId, Arg.Any<long>()).Returns(true);
            var expectedResult = new ResultInfo();
            _customerListDataAccess.UpdateOddsGroup(metaModel).Returns(expectedResult);

            // Action
            var result = _customerListService.UpdateOddsGroup(metaModel);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void UpdateOddsGroup_HaveNoPermission_ReturnCorrectData()
        {
            // Arrange
            const int LicenseeId = 1, CustId = 2;
            var metaModel = new CustomerOddsGroupMetaModel { LicenseeId = LicenseeId, CustId = CustId };
            _authorizationService.HasPermissionOnData(LicenseeId, CustId, Arg.Any<long>()).Returns(false);
            var expectedResult = new ResultInfo { Code = 1, Message = "Invalid parameter. Invalid custId." };

            // Action
            var result = _customerListService.UpdateOddsGroup(metaModel);

            // Assert
            Assert.That(result.Code, Is.EqualTo(expectedResult.Code));
            Assert.That(result.Message, Is.EqualTo(expectedResult.Message));
        }

        [Test]
        public void UpdateCustClosed_HaveNoPermission_ReturnCorrectResultInfo()
        {
            // Arrange
            const int LicenseeId = 1, CustId = 2;
            var metaModel = new CustomerStatusMetaModel { LicenseeId = LicenseeId, CustId = CustId };
            _authorizationService.HasPermissionOnData(LicenseeId, CustId, Arg.Any<long>()).Returns(false);
            var expectedResult = new ResultInfo { Code = 1, Message = "Invalid parameter. Invalid custId." };

            // Action
            var result = _customerListService.UpdateCustClosed(metaModel, string.Empty);

            // Assert
            Assert.That(result.Code, Is.EqualTo(expectedResult.Code));
            Assert.That(result.Message, Is.EqualTo(expectedResult.Message));
        }

        [Test]
        public void UpdateCustClosed_SyncCasinoInCustomerInfoIsFalse_ReturnCorrectResultInfo()
        {
            // Arrange
            const int LicenseeId = 1, CustId = 2;
            var metaModel = new CustomerStatusMetaModel { LicenseeId = LicenseeId, CustId = CustId };
            _authorizationService.HasPermissionOnData(LicenseeId, CustId, Arg.Any<long>()).Returns(true);
            var customerResult = new CustomerInfo { SyncCasino = false };
            _customerDataAccess.GetCustomerInfo(CustId).Returns(customerResult);
            _productService.GetProductAccess(LicenseeId, string.Empty).Returns(new ProductAccess(new List<Product>()));
            var expectedResult = new ResultInfo();
            _customerListDataAccess.UpdateCustClosed(metaModel).Returns(expectedResult);

            // Action
            var result = _customerListService.UpdateCustClosed(metaModel, string.Empty);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void UpdateCustClosed_IsDisableRngCasinoInAppSettingIsTrue_ReturnCorrectResultInfo()
        {
            // Arrange
            const int LicenseeId = 1, CustId = 2;
            var metaModel = new CustomerStatusMetaModel { LicenseeId = LicenseeId, CustId = CustId };
            _authorizationService.HasPermissionOnData(LicenseeId, CustId, Arg.Any<long>()).Returns(true);
            var customerResult = new CustomerInfo { SyncCasino = true };
            _customerDataAccess.GetCustomerInfo(CustId).Returns(customerResult);
            _productService.GetProductAccess(LicenseeId, string.Empty).Returns(new ProductAccess(new List<Product> { new Product { ProductCode = Product.CS } }));
            _appSettings.IsDisableRngCasino.Returns(true);
            var expectedResult = new ResultInfo { Code = 1, Message = "RNG Casino Database is not currently available. Please try again later." };

            // Action
            var result = _customerListService.UpdateCustClosed(metaModel, string.Empty);

            // Assert
            Assert.That(result.Code, Is.EqualTo(expectedResult.Code));
            Assert.That(result.Message, Is.EqualTo(expectedResult.Message));
        }

        [Test]
        public void UpdateCustClosed_CloseCasinoSuccess_ReturnCorrectResultInfo()
        {
            // Arrange
            const int LicenseeId = 1, CustId = 2;
            var metaModel = new CustomerStatusMetaModel
            {
                LicenseeId = LicenseeId,
                CustId = CustId,
                LicenseeName = "spondemo",
                IsOn = true,
                ModifyById = 3,
                ModifyByUsername = "tester",
                Ip = "1.1.1.1"
            };
            _authorizationService.HasPermissionOnData(LicenseeId, CustId, Arg.Any<long>()).Returns(true);
            var customerResult = new CustomerInfo { SyncCasino = true };
            _customerDataAccess.GetCustomerInfo(CustId).Returns(customerResult);
            _productService.GetProductAccess(LicenseeId, string.Empty).Returns(new ProductAccess(new List<Product> { new Product { ProductCode = Product.CS } }));
            _appSettings.IsDisableRngCasino.Returns(false);

            _casinoSetting.UpdateCustClosed(Arg.Is<CustomerStatusMetaModel>(param =>
                                        param.CustId == metaModel.CustId &&
                                        param.LicenseeName == metaModel.LicenseeName.ToUpperInvariant() &&
                                        param.IsOn == metaModel.IsOn &&
                                        param.ModifyById == metaModel.ModifyById &&
                                        param.ModifyByUsername == metaModel.ModifyByUsername &&
                                        param.Ip == metaModel.Ip))
                                        .Returns(true);
            var expectedResult = new ResultInfo();
            _customerListDataAccess.UpdateCustClosed(metaModel).Returns(expectedResult);

            // Action
            var result = _customerListService.UpdateCustClosed(metaModel, string.Empty);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void UpdateCustClosed_CloseCasinoFailed_ReturnCorrectResultInfo()
        {
            // Arrange
            const int LicenseeId = 1, CustId = 2;
            var metaModel = new CustomerStatusMetaModel
            {
                LicenseeId = LicenseeId,
                CustId = CustId,
                LicenseeName = "spondemo",
                IsOn = true,
                ModifyById = 3,
                ModifyByUsername = "tester",
                Ip = "1.1.1.1"
            };
            _authorizationService.HasPermissionOnData(LicenseeId, CustId, Arg.Any<long>()).Returns(true);
            var customerResult = new CustomerInfo { SyncCasino = true };
            _customerDataAccess.GetCustomerInfo(CustId).Returns(customerResult);
            _productService.GetProductAccess(LicenseeId, string.Empty).Returns(new ProductAccess(new List<Product> { new Product { ProductCode = Product.CS } }));
            _appSettings.IsDisableRngCasino.Returns(false);

            _casinoSetting.UpdateCustClosed(Arg.Is<CustomerStatusMetaModel>(param =>
                                        param.CustId == metaModel.CustId &&
                                        param.LicenseeName == metaModel.LicenseeName.ToUpperInvariant() &&
                                        param.IsOn == metaModel.IsOn &&
                                        param.ModifyById == metaModel.ModifyById &&
                                        param.ModifyByUsername == metaModel.ModifyByUsername &&
                                        param.Ip == metaModel.Ip))
                                        .Returns(false);
            var expectedResult = new ResultInfo { Code = 1, Message = "Error when update closed status on casino db" };

            // Action
            var result = _customerListService.UpdateCustClosed(metaModel, string.Empty);

            // Assert
            Assert.That(result.Code, Is.EqualTo(expectedResult.Code));
            Assert.That(result.Message, Is.EqualTo(expectedResult.Message));
        }

        [Test]
        public void UpdateCustSuspend_HaveNoPermission_ReturnCorrectResultInfo()
        {
            // Arrange
            const int LicenseeId = 1, CustId = 2;
            var metaModel = new CustomerStatusMetaModel { LicenseeId = LicenseeId, CustId = CustId };
            _authorizationService.HasPermissionOnData(LicenseeId, CustId, Arg.Any<long>()).Returns(false);
            var expectedResult = new ResultInfo { Code = 1, Message = "Invalid parameter. Invalid custId." };

            // Action
            var result = _customerListService.UpdateCustSuspend(metaModel, string.Empty);

            // Assert
            Assert.That(result.Code, Is.EqualTo(expectedResult.Code));
            Assert.That(result.Message, Is.EqualTo(expectedResult.Message));
        }

        [Test]
        public void UpdateCustSuspend_SyncCasinoInCustomerInfoIsFalse_ReturnCorrectResultInfo()
        {
            // Arrange
            const int LicenseeId = 1, CustId = 2;
            var metaModel = new CustomerStatusMetaModel { LicenseeId = LicenseeId, CustId = CustId };
            _authorizationService.HasPermissionOnData(LicenseeId, CustId, Arg.Any<long>()).Returns(true);
            var customerResult = new CustomerInfo { SyncCasino = false };
            _customerDataAccess.GetCustomerInfo(CustId).Returns(customerResult);
            _productService.GetProductAccess(LicenseeId, string.Empty).Returns(new ProductAccess(new List<Product>()));
            var expectedResult = new ResultInfo();
            _customerListDataAccess.UpdateCustSuspend(metaModel).Returns(expectedResult);

            // Action
            var result = _customerListService.UpdateCustSuspend(metaModel, string.Empty);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void UpdateCustSuspend_IsDisableRngCasinoInAppSettingIsTrue_ReturnCorrectResultInfo()
        {
            // Arrange
            const int LicenseeId = 1, CustId = 2;
            var metaModel = new CustomerStatusMetaModel { LicenseeId = LicenseeId, CustId = CustId };
            _authorizationService.HasPermissionOnData(LicenseeId, CustId, Arg.Any<long>()).Returns(true);
            var customerResult = new CustomerInfo { SyncCasino = true };
            _customerDataAccess.GetCustomerInfo(CustId).Returns(customerResult);
            _productService.GetProductAccess(LicenseeId, string.Empty).Returns(new ProductAccess(new List<Product> { new Product { ProductCode = Product.CS } }));
            _appSettings.IsDisableRngCasino.Returns(true);
            var expectedResult = new ResultInfo { Code = 1, Message = "RNG Casino Database is not currently available. Please try again later." };

            // Action
            var result = _customerListService.UpdateCustSuspend(metaModel, string.Empty);

            // Assert
            Assert.That(result.Code, Is.EqualTo(expectedResult.Code));
            Assert.That(result.Message, Is.EqualTo(expectedResult.Message));
        }

        [Test]
        public void UpdateCustSuspend_CloseCasinoSuccess_ReturnCorrectResultInfo()
        {
            // Arrange
            const int LicenseeId = 1, CustId = 2;
            var metaModel = new CustomerStatusMetaModel
            {
                LicenseeId = LicenseeId,
                CustId = CustId,
                LicenseeName = "spondemo",
                IsOn = true,
                ModifyById = 3,
                ModifyByUsername = "tester",
                Ip = "1.1.1.1"
            };
            _authorizationService.HasPermissionOnData(LicenseeId, CustId, Arg.Any<long>()).Returns(true);
            var customerResult = new CustomerInfo { SyncCasino = true };
            _customerDataAccess.GetCustomerInfo(CustId).Returns(customerResult);
            _productService.GetProductAccess(LicenseeId, string.Empty).Returns(new ProductAccess(new List<Product> { new Product { ProductCode = Product.CS } }));
            _appSettings.IsDisableRngCasino.Returns(false);

            _casinoSetting.UpdateCustSuspend(Arg.Is<CustomerStatusMetaModel>(param =>
                                        param.CustId == metaModel.CustId &&
                                        param.LicenseeName == metaModel.LicenseeName.ToUpperInvariant() &&
                                        param.IsOn == metaModel.IsOn &&
                                        param.ModifyById == metaModel.ModifyById &&
                                        param.ModifyByUsername == metaModel.ModifyByUsername &&
                                        param.Ip == metaModel.Ip))
                                        .Returns(true);
            var expectedResult = new ResultInfo();
            _customerListDataAccess.UpdateCustSuspend(metaModel).Returns(expectedResult);

            // Action
            var result = _customerListService.UpdateCustSuspend(metaModel, string.Empty);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void UpdateCustSuspend_CloseCasinoFailed_ReturnCorrectResultInfo()
        {
            // Arrange
            const int LicenseeId = 1, CustId = 2;
            var metaModel = new CustomerStatusMetaModel
            {
                LicenseeId = LicenseeId,
                CustId = CustId,
                LicenseeName = "spondemo",
                IsOn = true,
                ModifyById = 3,
                ModifyByUsername = "tester",
                Ip = "1.1.1.1"
            };
            _authorizationService.HasPermissionOnData(LicenseeId, CustId, Arg.Any<long>()).Returns(true);
            var customerResult = new CustomerInfo { SyncCasino = true };
            _customerDataAccess.GetCustomerInfo(CustId).Returns(customerResult);
            _productService.GetProductAccess(LicenseeId, string.Empty).Returns(new ProductAccess(new List<Product> { new Product { ProductCode = Product.CS } }));
            _appSettings.IsDisableRngCasino.Returns(false);

            _casinoSetting.UpdateCustSuspend(Arg.Is<CustomerStatusMetaModel>(param =>
                                        param.CustId == metaModel.CustId &&
                                        param.LicenseeName == metaModel.LicenseeName.ToUpperInvariant() &&
                                        param.IsOn == metaModel.IsOn &&
                                        param.ModifyById == metaModel.ModifyById &&
                                        param.ModifyByUsername == metaModel.ModifyByUsername &&
                                        param.Ip == metaModel.Ip))
                                        .Returns(false);
            var expectedResult = new ResultInfo { Code = 1, Message = "Error when update suspend status on casino db" };

            // Action
            var result = _customerListService.UpdateCustSuspend(metaModel, string.Empty);

            // Assert
            Assert.That(result.Code, Is.EqualTo(expectedResult.Code));
            Assert.That(result.Message, Is.EqualTo(expectedResult.Message));
        }

        [Test]
        public void UpdateCustomerBAStatus_HaveNoPermission_ReturnCorrectResultInfo()
        {
            // Arrange
            const int LicenseeId = 1, CustId = 2;
            _authorizationService.HasPermissionOnData(LicenseeId, CustId, Arg.Any<long>()).Returns(false);
            var expectedResult = new ResultInfo { Code = 1, Message = "Invalid parameter. Invalid custId." };

            // Action
            var result = _customerListService.UpdateCustomerBAStatus(LicenseeId, CustId, Arg.Any<int>(), Arg.Any<bool>(), Arg.Any<Request>());

            // Assert
            Assert.That(result.Code, Is.EqualTo(expectedResult.Code));
            Assert.That(result.Message, Is.EqualTo(expectedResult.Message));
        }

        [Test]
        public void UpdateCustomerBAStatus_WhenCallWithCorrectPermission_ReturnCorrectResultInfo()
        {
            // Arrange
            const int LicenseeId = 1, CustId = 2, ModifiedBy = 3;
            const bool AllowBA = true;
            var request = new Request();
            _authorizationService.HasPermissionOnData(LicenseeId, CustId, Arg.Any<long>()).Returns(true);
            var expectedResult = new ResultInfo();
            _customerListDataAccess.UpdateCustomerBAStatus(LicenseeId, CustId, ModifiedBy, AllowBA, request).Returns(expectedResult);

            // Action
            var result = _customerListService.UpdateCustomerBAStatus(LicenseeId, CustId, ModifiedBy, AllowBA, request);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void UpdateSpecialLevel_HaveNoPermission_ReturnCorrectResultInfo()
        {
            // Arrange
            const int LicenseeId = 1, UserId = 2, CustId = 3, Level = 1;
            const string Ip = "1.1.1.1", Language = "en-US";
            _authorizationService.HasPermissionOnData(LicenseeId, CustId, Arg.Any<long>()).Returns(false);
            var expectedResult = new ResultInfo { Code = 1, Message = "Invalid parameter. Invalid custId." };

            // Action
            var result = _customerListService.UpdateSpecialLevel(LicenseeId, UserId, CustId, Level, Ip, Language);

            // Assert
            Assert.That(result.Code, Is.EqualTo(expectedResult.Code));
            Assert.That(result.Message, Is.EqualTo(expectedResult.Message));
        }

        [Test]
        public void UpdateSpecialLevel_WhenCallWithCorrectPermission_ReturnCorrectResultInfo()
        {
            // Arrange
            const int LicenseeId = 1, UserId = 2, CustId = 3, Level = 1;
            const string Ip = "1.1.1.1", Language = "en-US";
            _authorizationService.HasPermissionOnData(LicenseeId, CustId, Arg.Any<long>()).Returns(true);
            var expectedResult = new ResultInfo();
            _customerListDataAccess.UpdateSpecialLevel(UserId, CustId, Level, Ip, Language).Returns(expectedResult);

            // Action
            var result = _customerListService.UpdateSpecialLevel(LicenseeId, UserId, CustId, Level, Ip, Language);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }
    }
}