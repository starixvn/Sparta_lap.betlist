namespace Fanex.Athena.Service.Customer.UnitTests
{
    using System;
    using Casino.Interfaces;
    using Core.Users.Interfaces;
    using Customer;
    using DataAccess.Customer.Interfaces;
    using Model.Casino.MetaModel;
    using Model.Core.DTO.Common;
    using Model.Customer.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using Session.Model;
    using UnitTest.Common.Helpers;

    [TestFixture]
    public partial class CustomerCommServiceTest
    {
        private const int LicenseeId = 999;

        private ICasinoCustomerService _casinoCustomerService;

        private ICustomerCommDataAccess _customerCommDataAccess;

        private ISessionService _sessionService;

        private IGroupComDataAccess _groupCommDataAccess;

        private CustomerCommService _customerCommService;

        [SetUp]
        public void SetUp()
        {
            _casinoCustomerService = Substitute.For<ICasinoCustomerService>();
            _customerCommDataAccess = Substitute.For<ICustomerCommDataAccess>();
            _sessionService = Substitute.For<ISessionService>();
            _sessionService.LicenseeSession.Returns(new LicenseeSession
            {
                LicenseeId = LicenseeId
            });
            _groupCommDataAccess = Substitute.For<IGroupComDataAccess>();

            _customerCommService = new CustomerCommService(
                _casinoCustomerService,
                _customerCommDataAccess,
                _sessionService,
                _groupCommDataAccess);
        }

        [TearDown]
        public void TearDown()
        {
            _casinoCustomerService = null;
            _customerCommDataAccess = null;
            _sessionService = null;
            _groupCommDataAccess = null;
            _customerCommService = null;
        }

        [Test]
        public void UpdateCustomersCommission_UpdateFailed_ReturnCorrectResultInfo()
        {
            // Arrange
            var expectedResult = new ResultInfo { Code = Math.Abs(FakeHelper.Create<int>()) + 1, Message = FakeHelper.Create<string>() };
            _customerCommDataAccess.UpdateCustomersCommission(Arg.Any<UpdatedCustomerComMetaModel>()).Returns(expectedResult);

            // Action
            var result = _customerCommService.UpdateCustomersCommission(new UpdatedCustomerComMetaModel(), false);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.IsSuccessful, Is.False);
                Assert.That(result, Is.EqualTo(expectedResult));
            });
        }

        [Test]
        public void UpdateCustomersCommission_UpdateSuccessfully_ReturnCorrectResultInfo()
        {
            // Arrange
            var expectedResult = new ResultInfo { Code = 0, Message = null };
            _customerCommDataAccess.UpdateCustomersCommission(Arg.Any<UpdatedCustomerComMetaModel>()).Returns(expectedResult);

            // Action
            var result = _customerCommService.UpdateCustomersCommission(new UpdatedCustomerComMetaModel(), false);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.IsSuccessful, Is.True);
                Assert.That(result, Is.EqualTo(expectedResult));
            });
        }

        [Test]
        public void UpdateCustomersCommission_IsSyncCSComm_CallApiSuccess_ReturnCorrectResultInfo()
        {
            // Arrange
            var isSyncCSComm = true;
            var metaModel = FakeHelper.Create<UpdatedCustomerComMetaModel>();

            var userSession = new UserSession { UserName = FakeHelper.Create<string>() };
            _sessionService.UserSession.Returns(userSession);

            var expectedResult = new ResultInfo { ExtendedData = "1,2,3" };
            _customerCommDataAccess.UpdateCustomersCommission(metaModel).Returns(expectedResult);

            _casinoCustomerService.UpdateCommission(Arg.Any<RNGCasinoUpdateMemberComMetaModel>()).Returns(true);

            // Action
            var result = _customerCommService.UpdateCustomersCommission(metaModel, isSyncCSComm);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void UpdateCustomersCommission_IsSyncCSComm_CallApiFailed_ReturnCorrectResultInfo()
        {
            // Arrange
            var isSyncCSComm = true;
            var metaModel = FakeHelper.Create<UpdatedCustomerComMetaModel>();

            var userSession = new UserSession { UserName = FakeHelper.Create<string>() };
            _sessionService.UserSession.Returns(userSession);

            var resultInfo = new ResultInfo { ExtendedData = "1" };
            _customerCommDataAccess.UpdateCustomersCommission(metaModel).Returns(resultInfo);

            _casinoCustomerService.UpdateCommission(Arg.Any<RNGCasinoUpdateMemberComMetaModel>()).Returns(false);
            var expectedResult = new ResultInfo
            {
                Code = 1,
                Message = "Having errors when calling CasinoAPI to update customer commission."
            };

            // Action
            var result = _customerCommService.UpdateCustomersCommission(metaModel, isSyncCSComm);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Code, Is.EqualTo(expectedResult.Code));
                Assert.That(result.Message, Is.EqualTo(expectedResult.Message));
            });
        }

        [Test]
        public void UpdateCustomersCommission_IsSyncCSComm_CustomerIdsIsEmpty_NotCallApi_ReturnCorrectResultInfo()
        {
            // Arrange
            var isSyncCSComm = true;
            var expectedResult = new ResultInfo();
            _customerCommDataAccess.UpdateCustomersCommission(Arg.Any<UpdatedCustomerComMetaModel>()).Returns(expectedResult);

            // Action
            var result = _customerCommService.UpdateCustomersCommission(new UpdatedCustomerComMetaModel(), isSyncCSComm);

            // Assert
            Assert.Multiple(() =>
            {
                _casinoCustomerService.DidNotReceive().UpdateCommission(Arg.Any<RNGCasinoUpdateMemberComMetaModel>());
                Assert.That(result, Is.EqualTo(expectedResult));
            });
        }
    }
}