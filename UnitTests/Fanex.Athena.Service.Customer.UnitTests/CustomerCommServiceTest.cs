namespace Fanex.Athena.Service.Customer.UnitTests
{
    using Casino.Interfaces;
    using Core.Users.Interfaces;
    using Customer;
    using DataAccess.Customer.Interfaces;
    using Interfaces;
    using Logging;
    using Model.Casino.MetaModel;
    using Model.Core.DTO.Common;
    using Model.Core.DTO.Users;
    using Model.Customer.MetaModel;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class CustomerCommServiceTest
    {
        private ILogger _logger;

        private ICustomerApiWrapper _customerApiWrapper;

        private ICustomerCommDataAccess _customerCommDataAccess;

        private ISessionService _sessionService;

        private IGroupComService _groupCommService;

        private CustomerCommService _customerCommService;

        [SetUp]
        public void SetUp()
        {
            _logger = Substitute.For<ILogger>();
            _customerApiWrapper = Substitute.For<ICustomerApiWrapper>();
            _customerCommDataAccess = Substitute.For<ICustomerCommDataAccess>();
            _sessionService = Substitute.For<ISessionService>();
            _groupCommService = Substitute.For<IGroupComService>();

            _customerCommService = new CustomerCommService(
                _logger,
                _customerApiWrapper,
                _customerCommDataAccess,
                _sessionService,
                _groupCommService);
        }

        [TearDown]
        public void TearDown()
        {
            _logger = null;
            _customerApiWrapper = null;
            _customerCommDataAccess = null;
            _sessionService = null;
            _groupCommService = null;
            _customerCommService = null;
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
            _customerCommDataAccess.UpdateCustomersCommission(metaModel).Returns(expectedResult);
            _customerApiWrapper.UpdateComm(null, 0, null, 0, null, 0).ReturnsForAnyArgs(true);

            // Action
            var result = _customerCommService.UpdateCustomersCommission(metaModel, isSyncCSComm);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void UpdateCustomersCommission_UpdateFailed_ReturnCorrectResultInfo()
        {
            // Arrange
            var isSyncCSComm = true;
            var expectedResult = new ResultInfo { Code = 100, Message = "Error", ExtendedData = "1,2,3" };
            _customerCommDataAccess.UpdateCustomersCommission(Arg.Any<UpdatedCustomerComMetaModel>()).Returns(expectedResult);

            // Action
            var result = _customerCommService.UpdateCustomersCommission(new UpdatedCustomerComMetaModel(), isSyncCSComm);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void UpdateCustomersCommission_SyncCSCommIsFalse_ReturnCorrectResultInfo()
        {
            // Arrange
            var isSyncCSComm = false;
            var expectedResult = new ResultInfo { Code = 100, Message = "Error", ExtendedData = "1,2,3" };
            _customerCommDataAccess.UpdateCustomersCommission(Arg.Any<UpdatedCustomerComMetaModel>()).Returns(expectedResult);

            // Action
            var result = _customerCommService.UpdateCustomersCommission(new UpdatedCustomerComMetaModel(), isSyncCSComm);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void UpdateCustomersCommission_ExtendedDataInResultIsEmpty_ReturnCorrectResultInfo()
        {
            // Arrange
            var isSyncCSComm = true;
            var expectedResult = new ResultInfo();
            _customerCommDataAccess.UpdateCustomersCommission(Arg.Any<UpdatedCustomerComMetaModel>()).Returns(expectedResult);

            // Action
            var result = _customerCommService.UpdateCustomersCommission(new UpdatedCustomerComMetaModel(), isSyncCSComm);

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
            _customerCommDataAccess.UpdateCustomersCommission(metaModel).Returns(resultInfo);
            _customerApiWrapper.UpdateComm(null, 0, null, 0, null, 0).ReturnsForAnyArgs(false);
            var expectedResult = new ResultInfo
            {
                Code = 1,
                Message = "Having errors when calling CasinoAPI to update customer commission."
            };

            // Action
            var result = _customerCommService.UpdateCustomersCommission(metaModel, isSyncCSComm);

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
            _customerCommDataAccess.RemoveCustomersFromGroup(metaModel).Returns(expectedResult);
            var userSession = new UserSession { UserName = "tester", UserId = 1 };
            _sessionService.UserSession.Returns(userSession);
            _customerApiWrapper.UpdateComm(null, 0, null, 0, null, 0).ReturnsForAnyArgs(true);

            // Action
            var result = _customerCommService.RemoveCustomersFromGroup(metaModel, isAllowedRNGCasino);

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
            _customerCommDataAccess.RemoveCustomersFromGroup(metaModel).Returns(resultInfo);
            var userSession = new UserSession { UserName = "tester", UserId = 1 };
            _sessionService.UserSession.Returns(userSession);
            _customerApiWrapper.UpdateComm(null, 0, null, 0, null, 0).ReturnsForAnyArgs(false);
            var expectedResult = new ResultInfo
            {
                Code = 1,
                Message = "Having errors when calling CasinoAPI to update customer commission."
            };

            // Action
            var result = _customerCommService.RemoveCustomersFromGroup(metaModel, isAllowedRNGCasino);

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
            _customerCommDataAccess.RemoveCustomersFromGroup(Arg.Any<RemoveFromGroupComMetaModel>()).Returns(expectedResult);

            // Action
            var result = _customerCommService.RemoveCustomersFromGroup(new RemoveFromGroupComMetaModel(), isAllowedRNGCasino);

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
            _customerCommDataAccess.RemoveCustomersFromGroup(metaModel).Returns(expectedResult);

            // Action
            var result = _customerCommService.RemoveCustomersFromGroup(metaModel, isAllowedRNGCasino);

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
            _customerCommDataAccess.RemoveCustomersFromGroup(metaModel).Returns(expectedResult);

            // Action
            var result = _customerCommService.RemoveCustomersFromGroup(metaModel, isAllowedRNGCasino);

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
            _customerCommDataAccess.RemoveCustomersFromGroup(metaModel).Returns(expectedResult);

            // Action
            var result = _customerCommService.RemoveCustomersFromGroup(metaModel, isAllowedRNGCasino);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }
    }
}