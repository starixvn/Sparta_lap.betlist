namespace Fanex.Athena.Service.Casino.UnitTests
{
    using System;
    using Casino;
    using Casino.Wrappers;
    using Core.Exception;
    using Logging;
    using Model.Casino.MetaModel;
    using Model.Customer.MetaModel;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class CasinoCustomerServiceTest
    {
        private ILogger _logger;

        private ICustomerApiWrapper _customerApiWrapper;

        private CasinoCustomerService _casinoCustomerService;

        [SetUp]
        public void SetUp()
        {
            _logger = Substitute.For<ILogger>();
            _customerApiWrapper = Substitute.For<ICustomerApiWrapper>();

            _casinoCustomerService = new CasinoCustomerService(_logger, _customerApiWrapper);
        }

        [TearDown]
        public void TearDown()
        {
            _customerApiWrapper = null;
        }

        [Test]
        public void UpdateSuspend_WhenCall_CallApiCorrectly()
        {
            // Arrange
            var metaModel = CreateStatusMetaModel();

            // Action
            var result = _casinoCustomerService.UpdateSuspend(metaModel);

            // Assert
            _customerApiWrapper.Received().UpdateCustSuspend(metaModel);
        }

        [Test]
        public void UpdateSuspend_ApiThrowException_ThrowServiceException()
        {
            // Arrange
            _customerApiWrapper.UpdateCustSuspend(Arg.Any<CustomerStatusMetaModel>()).ReturnsForAnyArgs(x =>
            {
                throw new Exception();
            });

            // Action
            TestDelegate result = () => _casinoCustomerService.UpdateSuspend(null);

            // Assert
            Assert.Throws<ServiceException>(result);
        }

        [Test]
        public void UpdateClosed_WhenCall_CallApiCorrectly()
        {
            // Arrange
            var metaModel = CreateStatusMetaModel();

            // Action
            var result = _casinoCustomerService.UpdateClosed(metaModel);

            // Assert
            _customerApiWrapper.Received().UpdateCustClosed(
                        metaModel);
        }

        [Test]
        public void UpdateClosed_ApiThrowException_ThrowServiceException()
        {
            // Arrange
            _customerApiWrapper.UpdateCustClosed(Arg.Any<CustomerStatusMetaModel>()).ReturnsForAnyArgs(x =>
            {
                throw new Exception();
            });

            // Action
            TestDelegate result = () => _casinoCustomerService.UpdateClosed(null);

            // Assert
            Assert.Throws<ServiceException>(result);
        }

        [Test]
        public void UpdateCommission_WhenCall_CallApiCorrectly()
        {
            // Arrange
            var metaModel = new RNGCasinoUpdateMemberComMetaModel
            {
                CustomerId = new[] { "1" },
                Comm = 2,
                FromIp = "3",
                ModifyBy = 4,
                ModifyUserName = "5"
            };

            _customerApiWrapper.UpdateComm(
              metaModel).Returns(true);

            // Action
            var result = _casinoCustomerService.UpdateCommission(metaModel);

            // Assert
            _customerApiWrapper.Received().UpdateComm(
                           metaModel);
        }

        [Test]
        public void UpdateCommission_CallApi_UpdateFail_LogError()
        {
            // Arrange
            var metaModel = new RNGCasinoUpdateMemberComMetaModel
            {
                CustomerId = new[] { "1" },
                Comm = 2,
                FromIp = "3",
                ModifyBy = 4,
                ModifyUserName = "5"
            };

            _customerApiWrapper.UpdateComm(
              metaModel).Returns(false);

            // Action
            var result = _casinoCustomerService.UpdateCommission(metaModel);

            // Assert
            _logger.Received().Error(Arg.Is<string>(x => x.Contains(string.Join(",", metaModel.CustomerId))));
        }

        [Test]
        public void UpdateCommission_ApiThrowException_LogException()
        {
            // Arrange
            var exception = new Exception("Hello");
            var metaModel = new RNGCasinoUpdateMemberComMetaModel
            {
                CustomerId = new[] { "1" }
            };
            _customerApiWrapper.UpdateComm(Arg.Any<RNGCasinoUpdateMemberComMetaModel>()).ReturnsForAnyArgs(x =>
            {
                throw exception;
            });

            // Action
            _casinoCustomerService.UpdateCommission(metaModel);

            // Assert
            _logger.ReceivedWithAnyArgs().Error(string.Empty, exception);
        }

        private static CustomerStatusMetaModel CreateStatusMetaModel()
        {
            return new CustomerStatusMetaModel
            {
                CustId = 1,
                IsOn = true,
                Ip = "123",
                ModifyById = 2,
                ModifyByUsername = "donald trumpt"
            };
        }
    }
}