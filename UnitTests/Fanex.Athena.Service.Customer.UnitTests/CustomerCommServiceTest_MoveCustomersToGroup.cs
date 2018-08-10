namespace Fanex.Athena.Service.Customer.UnitTests
{
    using System;
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
        [Test]
        public void MoveCustomersToGroup_GroupDoesNotExists_NotCallMoveCustomersToGroup()
        {
            // Arrange
            var metaModel = FakeHelper.Create<MoveToGroupComMetaModel>();
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
        public void MoveCustomersToGroup_CallDataAccessFailed_NotCallApi()
        {
            // Arrange
            var metaModel = FakeHelper.Create<MoveToGroupComMetaModel>();

            _groupCommDataAccess.GetById(metaModel.GroupComId).Returns(FakeHelper.Create<GroupCommMetaModel>());
            _customerCommDataAccess.MoveCustomersToGroup(metaModel).Returns(new ResultInfo
            {
                Code = Math.Abs(FakeHelper.Create<int>()) + 1,
                Message = FakeHelper.Create<string>()
            });

            // Action
            var result = _customerCommService.MoveCustomersToGroup(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                _casinoCustomerService.DidNotReceiveWithAnyArgs().UpdateCommission(null);
                Assert.That(result.IsSuccessful, Is.False);
            });
        }

        [Test]
        public void MoveCustomersToGroup_IsAllowedRNGTrue_CallApiUpdateComm()
        {
            // Arrange
            var metaModel = new MoveToGroupComMetaModel
            {
                IsAllowedRNG = true
            };
            _groupCommDataAccess.GetById(metaModel.GroupComId).Returns(FakeHelper.Create<GroupCommMetaModel>());

            var userSession = new UserSession { UserName = FakeHelper.Create<string>() };
            _sessionService.UserSession.Returns(userSession);

            var expectedResult = new ResultInfo { ExtendedData = "1,2,3" };
            _customerCommDataAccess.MoveCustomersToGroup(metaModel).Returns(expectedResult);

            _casinoCustomerService.UpdateCommission(Arg.Any<RNGCasinoUpdateMemberComMetaModel>()).Returns(true);

            // Action
            var result = _customerCommService.MoveCustomersToGroup(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                _casinoCustomerService.Received(1).UpdateCommission(Arg.Any<RNGCasinoUpdateMemberComMetaModel>());
                Assert.That(result, Is.EqualTo(expectedResult));
            });
        }

        [Test]
        public void MoveCustomersToGroup_IsAllowedRNGTrue_CustomerIdListIsEmpty_NotCallApiUpdateComm()
        {
            // Arrange
            var metaModel = FakeHelper.Create<MoveToGroupComMetaModel>();
            _groupCommDataAccess.GetById(metaModel.GroupComId).Returns(FakeHelper.Create<GroupCommMetaModel>());

            var expectedResult = new ResultInfo { ExtendedData = string.Empty };
            _customerCommDataAccess.MoveCustomersToGroup(metaModel).Returns(expectedResult);

            // Action
            var result = _customerCommService.MoveCustomersToGroup(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                _casinoCustomerService.DidNotReceive().UpdateCommission(Arg.Any<RNGCasinoUpdateMemberComMetaModel>());
                Assert.That(result, Is.EqualTo(expectedResult));
            });
        }
    }
}