namespace Fanex.Athena.Service.Customer.UnitTests
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Model.Core.DTO.Common;
    using Model.Customer.DTO;
    using Model.Customer.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using Resources;
    using UnitTest.Common.Helpers;

    [TestFixture]
    public partial class CustomerListServiceTest
    {
        [Test]
        public async Task UpdateCustClosed_HaveNoPermission_ReturnCorrectResultInfo()
        {
            // Arrange
            var metaModel = FakeHelper.Create<CustomerStatusMetaModel>();
            _authorizationService.HasPermissionOnData(metaModel.LicenseeId, metaModel.CustId, 0).Returns(false);

            var expectedResult = new ResultInfo { Code = 1, Message = ViewResources.UpdateSpecialLevelMsg };

            // Action
            var result = await _customerListService.UpdateCustClosed(metaModel, string.Empty).ConfigureAwait(false);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Code, Is.EqualTo(expectedResult.Code));
                Assert.That(result.Message, Is.EqualTo(expectedResult.Message));
            });
        }

        [Test]
        public async Task UpdateCustClosed_NotSyncCasinoInCustomerInfo_NoUpdateCasinoCustomer()
        {
            // Arrange
            var metaModel = FakeHelper.Create<CustomerStatusMetaModel>();
            _authorizationService.HasPermissionOnData(metaModel.LicenseeId, metaModel.CustId, 0).Returns(true);

            var customerResult = new CustomerInfo { SyncCasino = false };
            _customerDataAccess.GetCustomerInfo(metaModel.CustId).Returns(customerResult);
            _productService.GetProductAccess(metaModel.LicenseeId, string.Empty)
                .Returns(new ProductAccess(new List<Product> { new Product { ProductCode = Product.CS } }));

            var expectedResult = FakeHelper.Create<ResultInfo>();
            _customerListDataAccess.UpdateCustClosed(metaModel).Returns(expectedResult);

            // Action
            var result = await _customerListService.UpdateCustClosed(metaModel, string.Empty).ConfigureAwait(false);

            // Assert
            Assert.Multiple(() =>
            {
                _casinoCustomerService.DidNotReceive().UpdateClosed(Arg.Any<CustomerStatusMetaModel>());
                Assert.That(result, Is.EqualTo(expectedResult));
            });
        }

        [Test]
        public async Task UpdateCustClosed_NotSupportCasino_NoUpdateCasinoCustomer()
        {
            // Arrange
            var metaModel = FakeHelper.Create<CustomerStatusMetaModel>();
            _authorizationService.HasPermissionOnData(metaModel.LicenseeId, metaModel.CustId, 0).Returns(true);

            var customerResult = new CustomerInfo { SyncCasino = true };
            _customerDataAccess.GetCustomerInfo(metaModel.CustId).Returns(customerResult);
            _productService.GetProductAccess(metaModel.LicenseeId, string.Empty)
                .Returns(new ProductAccess(new List<Product>()));

            var expectedResult = FakeHelper.Create<ResultInfo>();
            _customerListDataAccess.UpdateCustClosed(metaModel).Returns(expectedResult);

            // Action
            var result = await _customerListService.UpdateCustClosed(metaModel, string.Empty).ConfigureAwait(false);

            // Assert
            Assert.Multiple(() =>
            {
                _casinoCustomerService.DidNotReceive().UpdateClosed(Arg.Any<CustomerStatusMetaModel>());
                Assert.That(result, Is.EqualTo(expectedResult));
            });
        }

        [Test]
        public async Task UpdateCustClosed_IsUpdateCasinoCustomer_IsDisableRngCasinoInAppSetting_NoUpdateCasinoCustomer()
        {
            // Arrange
            var metaModel = FakeHelper.Create<CustomerStatusMetaModel>();
            _authorizationService.HasPermissionOnData(metaModel.LicenseeId, metaModel.CustId, 0).Returns(true);

            var customerResult = new CustomerInfo { SyncCasino = true };
            _customerDataAccess.GetCustomerInfo(metaModel.CustId).Returns(customerResult);
            _productService.GetProductAccess(metaModel.LicenseeId, string.Empty)
                .Returns(new ProductAccess(new List<Product> { new Product { ProductCode = Product.CS } }));

            _appSettings.IsDisableRngCasino.Returns(true);
            var expectedResult = new ResultInfo { Code = 1, Message = "RNG Casino Database is not currently available. Please try again later." };

            // Action
            var result = await _customerListService.UpdateCustClosed(metaModel, string.Empty).ConfigureAwait(false);

            // Assert
            Assert.Multiple(() =>
            {
                _casinoCustomerService.DidNotReceive().UpdateClosed(Arg.Any<CustomerStatusMetaModel>());
                Assert.That(result.Code, Is.EqualTo(expectedResult.Code));
                Assert.That(result.Message, Is.EqualTo(expectedResult.Message));
            });
        }

        [Test]
        public async Task UpdateCustClosed_IsUpdateCasinoCustomer_CloseCasinoSuccess_ReturnResultInfoFromDataAccess()
        {
            // Arrange
            var metaModel = FakeHelper.Create<CustomerStatusMetaModel>();
            _authorizationService.HasPermissionOnData(metaModel.LicenseeId, metaModel.CustId, 0).Returns(true);

            var customerResult = new CustomerInfo { SyncCasino = true };
            _customerDataAccess.GetCustomerInfo(metaModel.CustId).Returns(customerResult);
            _productService.GetProductAccess(metaModel.LicenseeId, string.Empty)
                .Returns(new ProductAccess(new List<Product> { new Product { ProductCode = Product.CS } }));

            _appSettings.IsDisableRngCasino.Returns(false);

            _casinoCustomerService.UpdateClosed(Arg.Any<CustomerStatusMetaModel>()).Returns(true);

            var expectedResult = FakeHelper.Create<ResultInfo>();
            _customerListDataAccess.UpdateCustClosed(metaModel).Returns(expectedResult);

            // Action
            var result = await _customerListService.UpdateCustClosed(metaModel, string.Empty);

            // Assert
            Assert.Multiple(() =>
            {
                _casinoCustomerService.Received().UpdateClosed(Arg.Is<CustomerStatusMetaModel>(param =>
                                        param.CustId == metaModel.CustId &&
                                        param.LicenseeName == metaModel.LicenseeName.ToUpperInvariant() &&
                                        param.IsOn == metaModel.IsOn &&
                                        param.ModifyById == metaModel.ModifyById &&
                                        param.ModifyByUsername == metaModel.ModifyByUsername &&
                                        param.Ip == metaModel.Ip));
                Assert.That(result, Is.EqualTo(expectedResult));
            });
        }

        [Test]
        public async Task UpdateCustClosed_IsUpdateCasinoCustomer_CloseCasinoFailed_DoNotCallDataAccessAndReturnErrorResult()
        {
            // Arrange
            var metaModel = FakeHelper.Create<CustomerStatusMetaModel>();
            _authorizationService.HasPermissionOnData(metaModel.LicenseeId, metaModel.CustId, 0).Returns(true);

            var customerResult = new CustomerInfo { SyncCasino = true };
            _customerDataAccess.GetCustomerInfo(metaModel.CustId).Returns(customerResult);
            _productService.GetProductAccess(metaModel.LicenseeId, string.Empty)
                .Returns(new ProductAccess(new List<Product> { new Product { ProductCode = Product.CS } }));

            _appSettings.IsDisableRngCasino.Returns(false);

            _casinoCustomerService.UpdateClosed(Arg.Any<CustomerStatusMetaModel>()).Returns(false);

            var expectedResult = new ResultInfo { Code = 1, Message = "Error when update closed status on casino db" };

            // Action
            var result = await _customerListService.UpdateCustClosed(metaModel, string.Empty).ConfigureAwait(false);

            // Assert
            Assert.Multiple(() =>
            {
                _casinoCustomerService.Received().UpdateClosed(Arg.Is<CustomerStatusMetaModel>(param =>
                                        param.CustId == metaModel.CustId &&
                                        param.LicenseeName == metaModel.LicenseeName.ToUpperInvariant() &&
                                        param.IsOn == metaModel.IsOn &&
                                        param.ModifyById == metaModel.ModifyById &&
                                        param.ModifyByUsername == metaModel.ModifyByUsername &&
                                        param.Ip == metaModel.Ip));
                _customerListDataAccess.DidNotReceive().UpdateCustClosed(metaModel);
                Assert.That(result.Code, Is.EqualTo(expectedResult.Code));
                Assert.That(result.Message, Is.EqualTo(expectedResult.Message));
            });
        }

        [Test]
        public async Task UpdateCustSuspend_HaveNoPermission_ReturnCorrectResultInfo()
        {
            // Arrange
            var metaModel = FakeHelper.Create<CustomerStatusMetaModel>();
            _authorizationService.HasPermissionOnData(metaModel.LicenseeId, metaModel.CustId, 0).Returns(false);

            var expectedResult = new ResultInfo { Code = 1, Message = ViewResources.UpdateSpecialLevelMsg };

            // Action
            var result = await _customerListService.UpdateCustSuspend(metaModel, string.Empty).ConfigureAwait(false);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Code, Is.EqualTo(expectedResult.Code));
                Assert.That(result.Message, Is.EqualTo(expectedResult.Message));
            });
        }

        [Test]
        public async Task UpdateCustSuspend_NotSyncCasinoInCustomerInfo_NoUpdateCasinoCustomer()
        {
            // Arrange
            var metaModel = FakeHelper.Create<CustomerStatusMetaModel>();
            _authorizationService.HasPermissionOnData(metaModel.LicenseeId, metaModel.CustId, 0).Returns(true);

            var customerResult = new CustomerInfo { SyncCasino = false };
            _customerDataAccess.GetCustomerInfo(metaModel.CustId).Returns(customerResult);
            _productService.GetProductAccess(metaModel.LicenseeId, string.Empty)
                .Returns(new ProductAccess(new List<Product> { new Product { ProductCode = Product.CS } }));

            var expectedResult = FakeHelper.Create<ResultInfo>();
            _customerListDataAccess.UpdateCustSuspend(metaModel).Returns(expectedResult);

            // Action
            var result = await _customerListService.UpdateCustSuspend(metaModel, string.Empty);

            // Assert
            Assert.Multiple(() =>
            {
                _casinoCustomerService.DidNotReceive().UpdateSuspend(Arg.Any<CustomerStatusMetaModel>());
                Assert.That(result, Is.EqualTo(expectedResult));
            });
        }

        [Test]
        public async Task UpdateCustSuspend_NotSupportCasino_NoUpdateCasinoCustomer()
        {
            // Arrange
            var metaModel = FakeHelper.Create<CustomerStatusMetaModel>();
            _authorizationService.HasPermissionOnData(metaModel.LicenseeId, metaModel.CustId, 0).Returns(true);

            var customerResult = new CustomerInfo { SyncCasino = true };
            _customerDataAccess.GetCustomerInfo(metaModel.CustId).Returns(customerResult);
            _productService.GetProductAccess(metaModel.LicenseeId, string.Empty)
                .Returns(new ProductAccess(new List<Product>()));

            var expectedResult = FakeHelper.Create<ResultInfo>();
            _customerListDataAccess.UpdateCustSuspend(metaModel).Returns(expectedResult);

            // Action
            var result = await _customerListService.UpdateCustSuspend(metaModel, string.Empty);

            // Assert
            Assert.Multiple(() =>
            {
                _casinoCustomerService.DidNotReceive().UpdateSuspend(Arg.Any<CustomerStatusMetaModel>());
                Assert.That(result, Is.EqualTo(expectedResult));
            });
        }

        [Test]
        public async Task UpdateCustSuspend_IsUpdateCasinoCustomer_IsDisableRngCasinoInAppSetting_NoUpdateCasinoCustomer()
        {
            // Arrange
            var metaModel = FakeHelper.Create<CustomerStatusMetaModel>();
            _authorizationService.HasPermissionOnData(metaModel.LicenseeId, metaModel.CustId, 0).Returns(true);

            var customerResult = new CustomerInfo { SyncCasino = true };
            _customerDataAccess.GetCustomerInfo(metaModel.CustId).Returns(customerResult);
            _productService.GetProductAccess(metaModel.LicenseeId, string.Empty)
                .Returns(new ProductAccess(new List<Product> { new Product { ProductCode = Product.CS } }));

            _appSettings.IsDisableRngCasino.Returns(true);
            var expectedResult = new ResultInfo { Code = 1, Message = "RNG Casino Database is not currently available. Please try again later." };

            // Action
            var result = await _customerListService.UpdateCustSuspend(metaModel, string.Empty);

            // Assert
            Assert.Multiple(() =>
            {
                _casinoCustomerService.DidNotReceive().UpdateSuspend(Arg.Any<CustomerStatusMetaModel>());
                Assert.That(result.Code, Is.EqualTo(expectedResult.Code));
                Assert.That(result.Message, Is.EqualTo(expectedResult.Message));
            });
        }

        [Test]
        public async Task UpdateCustSuspend_IsUpdateCasinoCustomer_CloseCasinoSuccess_ReturnResultInfoFromDataAccess()
        {
            // Arrange
            var metaModel = FakeHelper.Create<CustomerStatusMetaModel>();
            _authorizationService.HasPermissionOnData(metaModel.LicenseeId, metaModel.CustId, 0).Returns(true);

            var customerResult = new CustomerInfo { SyncCasino = true };
            _customerDataAccess.GetCustomerInfo(metaModel.CustId).Returns(customerResult);
            _productService.GetProductAccess(metaModel.LicenseeId, string.Empty)
                .Returns(new ProductAccess(new List<Product> { new Product { ProductCode = Product.CS } }));

            _appSettings.IsDisableRngCasino.Returns(false);

            _casinoCustomerService.UpdateSuspend(Arg.Any<CustomerStatusMetaModel>()).Returns(true);

            var expectedResult = FakeHelper.Create<ResultInfo>();
            _customerListDataAccess.UpdateCustSuspend(metaModel).Returns(expectedResult);

            // Action
            var result = await _customerListService.UpdateCustSuspend(metaModel, string.Empty);

            // Assert
            Assert.Multiple(() =>
            {
                _casinoCustomerService.Received().UpdateSuspend(Arg.Is<CustomerStatusMetaModel>(param =>
                                        param.CustId == metaModel.CustId &&
                                        param.LicenseeName == metaModel.LicenseeName.ToUpperInvariant() &&
                                        param.IsOn == metaModel.IsOn &&
                                        param.ModifyById == metaModel.ModifyById &&
                                        param.ModifyByUsername == metaModel.ModifyByUsername &&
                                        param.Ip == metaModel.Ip));
                Assert.That(result, Is.EqualTo(expectedResult));
            });
        }

        [Test]
        public async Task UpdateCustSuspend_IsUpdateCasinoCustomer_CloseCasinoFailed_DoNotCallDataAccessAndReturnErrorResult()
        {
            // Arrange
            var metaModel = FakeHelper.Create<CustomerStatusMetaModel>();
            _authorizationService.HasPermissionOnData(metaModel.LicenseeId, metaModel.CustId, 0).Returns(true);

            var customerResult = new CustomerInfo { SyncCasino = true };
            _customerDataAccess.GetCustomerInfo(metaModel.CustId).Returns(customerResult);
            _productService.GetProductAccess(metaModel.LicenseeId, string.Empty)
                .Returns(new ProductAccess(new List<Product> { new Product { ProductCode = Product.CS } }));

            _appSettings.IsDisableRngCasino.Returns(false);

            _casinoCustomerService.UpdateSuspend(Arg.Any<CustomerStatusMetaModel>()).Returns(false);

            var expectedResult = new ResultInfo { Code = 1, Message = "Error when update suspend status on casino db" };

            // Action
            var result = await _customerListService.UpdateCustSuspend(metaModel, string.Empty).ConfigureAwait(false);

            // Assert
            Assert.Multiple(() =>
            {
                _casinoCustomerService.Received().UpdateSuspend(Arg.Is<CustomerStatusMetaModel>(param =>
                                        param.CustId == metaModel.CustId &&
                                        param.LicenseeName == metaModel.LicenseeName.ToUpperInvariant() &&
                                        param.IsOn == metaModel.IsOn &&
                                        param.ModifyById == metaModel.ModifyById &&
                                        param.ModifyByUsername == metaModel.ModifyByUsername &&
                                        param.Ip == metaModel.Ip));
                _customerListDataAccess.DidNotReceive().UpdateCustSuspend(metaModel);
                Assert.That(result.Code, Is.EqualTo(expectedResult.Code));
                Assert.That(result.Message, Is.EqualTo(expectedResult.Message));
            });
        }
    }
}