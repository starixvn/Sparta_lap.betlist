namespace Fanex.Athena.Service.Customer.UnitTests
{
    using Casino.Wrappers;
    using Core.Users.Interfaces;
    using Customer;
    using DataAccess.Customer.Interfaces;
    using Logging;
    using Model.Core.DTO.Common;
    using Model.Core.DTO.Users;
    using Model.Customer.MetaModel;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public partial class CustomerCommServiceTest
    {
        private ILogger _logger;

        private ICustomerApiWrapper _customerApiWrapper;

        private ICustomerCommDataAccess _customerCommDataAccess;

        private ISessionService _sessionService;

        private IGroupComDataAccess _groupCommDataAccess;

        private CustomerCommService _customerCommService;

        [SetUp]
        public void SetUp()
        {
            _logger = Substitute.For<ILogger>();
            _customerApiWrapper = Substitute.For<ICustomerApiWrapper>();
            _customerCommDataAccess = Substitute.For<ICustomerCommDataAccess>();
            _sessionService = Substitute.For<ISessionService>();
            _groupCommDataAccess = Substitute.For<IGroupComDataAccess>();

            _customerCommService = new CustomerCommService(
                _logger,
                _customerApiWrapper,
                _customerCommDataAccess,
                _sessionService,
                _groupCommDataAccess);
        }

        [TearDown]
        public void TearDown()
        {
            _logger = null;
            _customerApiWrapper = null;
            _customerCommDataAccess = null;
            _sessionService = null;
            _groupCommDataAccess = null;
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
        public void MoveCustomersToGroup_GroupNotExists_NotCallMoveCustomersToGroup()
        {
            // Arrange
            var metaModel = new MoveToGroupComMetaModel
            {
                GroupComId = 1
            };
            _groupCommDataAccess.GetById(metaModel.GroupComId).Returns(x => null);

            // Action
            var result = _customerCommService.MoveCustomersToGroup(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                _customerCommDataAccess.DidNotReceiveWithAnyArgs().MoveCustomersToGroup(null);
                Assert.That(result.IsSuccessful, Is.False);
            });
        }

        [Test]
        public void MoveCustomersToGroup_CallDataAccessNotSuccess_NotCallApi()
        {
            // Arrange
            var metaModel = new MoveToGroupComMetaModel
            {
                IsAllowedRNG = true,
                GroupComId = 1,
                Ip = "1.1.1.1",
                UserId = 1
            };
            _groupCommDataAccess.GetById(metaModel.GroupComId).Returns(new GroupCommMetaModel());
            _customerCommDataAccess.MoveCustomersToGroup(metaModel).Returns(new ResultInfo
            {
                Code = 1,
                Message = "..."
            });

            // Action
            var result = _customerCommService.MoveCustomersToGroup(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                _customerApiWrapper.DidNotReceiveWithAnyArgs().UpdateComm(null, 0, null, 0, null, 0);
                Assert.That(result.IsSuccessful, Is.False);
            });
        }

        [Test]
        public void MoveCustomersToGroup_IsAllowedRNGTrue_CallApiUpdateComm()
        {
            // Arrange
            var metaModel = new MoveToGroupComMetaModel
            {
                IsAllowedRNG = true,
                GroupComId = 1,
                Ip = "1.1.1.1",
                UserId = 1
            };
            var userSession = new UserSession { UserName = "tester" };
            _sessionService.UserSession.Returns(userSession);
            var expectedResult = new ResultInfo { ExtendedData = "1,2,3" };
            _groupCommDataAccess.GetById(metaModel.GroupComId).Returns(new GroupCommMetaModel());
            _customerCommDataAccess.MoveCustomersToGroup(metaModel).Returns(expectedResult);
            _customerApiWrapper.UpdateComm(null, 0, null, 0, null, 0).ReturnsForAnyArgs(true);

            // Action
            var result = _customerCommService.MoveCustomersToGroup(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                _customerApiWrapper.ReceivedWithAnyArgs().UpdateComm(null, 0, null, 0, null, 0);
                Assert.That(result, Is.EqualTo(expectedResult));
            });
        }
    }
}