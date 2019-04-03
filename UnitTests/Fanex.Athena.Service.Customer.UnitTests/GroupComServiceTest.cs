namespace Fanex.Athena.Service.Customer.UnitTests
{
    using System.Threading.Tasks;
    using Casino.Interfaces;
    using DataAccess.Customer.Interfaces;
    using Model.Casino.MetaModel;
    using Model.Core.DTO.Common;
    using Model.Customer.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using Session.Model;
    using UnitTest.Common.Helpers;

    [TestFixture]
    public class GroupComServiceTest
    {
        private ICasinoCustomerService _casinoCustomerService;

        private IGroupComDataAccess _groupDataAccess;

        private GroupComService _groupComService;

        [SetUp]
        public void SetUp()
        {
            _casinoCustomerService = Substitute.For<ICasinoCustomerService>();
            _groupDataAccess = Substitute.For<IGroupComDataAccess>();
            _groupComService = new GroupComService(_casinoCustomerService, _groupDataAccess);
        }

        [TearDown]
        public void TearDown()
        {
            _groupDataAccess = null;
            _groupComService = null;
        }

        [Test]
        public void GetAll_WhenCall_ReturnDataFromDataAccess()
        {
            // Arrange
            var licenseeId = FakeHelper.Create<int>();

            // Action
            var result = _groupComService.GetAll(licenseeId);

            // Assert
            _groupDataAccess.Received().GetAll(licenseeId);
        }

        [Test]
        public void GetDataForExcel_WhenCall_ReturnDataFromDataAccess()
        {
            // Arrange
            var licenseeId = FakeHelper.Create<int>();

            // Action
            var result = _groupComService.GetDataForExcel(licenseeId);

            // Assert
            _groupDataAccess.Received().GetDataForExcel(licenseeId);
        }

        [Test]
        public void GetById_WhenCall_ReturnDataFromDataAccess()
        {
            // Arrange
            var groupCommId = FakeHelper.Create<int>();
            var licenseeId = FakeHelper.Create<int>();

            // Action
            var result = _groupComService.GetById(groupCommId, licenseeId);

            // Assert
            _groupDataAccess.Received().GetById(groupCommId, licenseeId);
        }

        [Test]
        public void InsertGroup_WhenCall_ReturnDataFromDataAccess()
        {
            // Arrange
            var item = FakeHelper.Create<GroupCommMetaModel>();
            var request = FakeHelper.Create<Request>();
            var userId = FakeHelper.Create<int>();

            // Action
            var result = _groupComService.InsertGroup(item, request, userId);

            // Assert
            _groupDataAccess.Received().InsertGroup(item, request, userId);
        }

        [Test]
        public async Task UpdateGroup_IsNotUpdateCS_NotCallCasinoApi()
        {
            // Arrange
            var groupCommMetaModel = new GroupCommMetaModel { IsUpdateCS = false };
            var userSession = FakeHelper.Create<UserSession>();
            var request = FakeHelper.Create<Request>();

            var expectedResult = FakeHelper.Create<ResultInfo>();
            _groupDataAccess.UpdateGroup(groupCommMetaModel, request, userSession.UserId)
                .Returns(expectedResult);

            // Action
            var result = await _groupComService.UpdateGroup(groupCommMetaModel, request, userSession);

            // Assert
            Assert.Multiple(() =>
            {
                _casinoCustomerService.DidNotReceive().UpdateCommission(Arg.Any<RNGCasinoUpdateMemberComMetaModel>());
                Assert.That(result, Is.EqualTo(expectedResult));
            });
        }

        [Test]
        public async Task UpdateGroup_IsUpdateCS_CustomerListIsEmpty_NotCallCasinoApi()
        {
            // Arrange
            var newItem = new GroupCommMetaModel { IsUpdateCS = true };
            var userSession = FakeHelper.Create<UserSession>();
            var request = FakeHelper.Create<Request>();

            var expectedResult = new ResultInfo { ExtendedData = string.Empty };
            _groupDataAccess.UpdateGroup(newItem, request, userSession.UserId)
                .Returns(expectedResult);

            // Action
            var result = await _groupComService.UpdateGroup(newItem, request, userSession);

            // Assert
            Assert.Multiple(() =>
            {
                _casinoCustomerService.DidNotReceive().UpdateCommission(Arg.Any<RNGCasinoUpdateMemberComMetaModel>());
                Assert.That(result, Is.EqualTo(expectedResult));
            });
        }

        [Test]
        public async Task UpdateGroup_IsUpdateCS_CallCasinoApi()
        {
            // Arrange
            var newItem = new GroupCommMetaModel { IsUpdateCS = true };
            var userSession = FakeHelper.Create<UserSession>();
            var request = FakeHelper.Create<Request>();

            var expectedResult = new ResultInfo { ExtendedData = "1,2,3" };
            _groupDataAccess.UpdateGroup(newItem, request, userSession.UserId)
                .Returns(expectedResult);

            _casinoCustomerService.UpdateCommission(Arg.Any<RNGCasinoUpdateMemberComMetaModel>()).Returns(true);

            // Action
            var result = await _groupComService.UpdateGroup(newItem, request, userSession);

            // Assert
            Assert.Multiple(() =>
            {
                _casinoCustomerService.Received().UpdateCommission(Arg.Is<RNGCasinoUpdateMemberComMetaModel>(
                    x => x.Comm == newItem.CSComm
                        && x.FromIp == request.Ip
                        && string.Join(",", x.CustomerId) == expectedResult.ExtendedData
                        && x.ModifyBy == userSession.UserId
                        && x.ModifyUserName == userSession.UserName));
                Assert.That(result, Is.EqualTo(expectedResult));
            });
        }

        [Test]
        public async Task UpdateGroup_IsUpdateCS_CallCasinoApiFailed_ReturnErrorResult()
        {
            // Arrange
            var newItem = new GroupCommMetaModel { IsUpdateCS = true };
            var userSession = FakeHelper.Create<UserSession>();
            var request = FakeHelper.Create<Request>();

            _groupDataAccess.UpdateGroup(newItem, request, userSession.UserId)
                .Returns(new ResultInfo { ExtendedData = "1,2,3" });

            _casinoCustomerService.UpdateCommission(Arg.Any<RNGCasinoUpdateMemberComMetaModel>()).Returns(false);

            var expectedResult = new ResultInfo
            {
                Code = 1,
                Message = "Having errors when calling CasinoAPI to update customer commission."
            };

            // Action
            var result = await _groupComService.UpdateGroup(newItem, request, userSession);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Code, Is.EqualTo(expectedResult.Code));
                Assert.That(result.Message, Is.EqualTo(expectedResult.Message));
            });
        }

        [Test]
        public async Task DeleteGroup_IsNotRemoveCommSettingIsFalse_NotCallCasinoApi()
        {
            // Arrange
            var metaModel = FakeHelper.Create<DeleteGroupMetaModel>();
            metaModel.IsRemoveCommSetting = false;

            var expectedResult = FakeHelper.Create<ResultInfo>();
            _groupDataAccess.DeleteGroup(
                metaModel.GroupId,
                metaModel.LicenseeId,
                metaModel.UserId,
                metaModel.Ip,
                metaModel.Language,
                metaModel.IsRemoveCommSetting,
                metaModel.IsResetDefault)
            .Returns(expectedResult);

            // Action
            var result = await _groupComService.DeleteGroup(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                _casinoCustomerService.DidNotReceive().UpdateCommission(Arg.Any<RNGCasinoUpdateMemberComMetaModel>());
                Assert.That(result, Is.EqualTo(expectedResult));
            });
        }

        [Test]
        public async Task DeleteGroup_IsNotAllowedCSComm_NotCallCasinoApi()
        {
            // Arrange
            var metaModel = FakeHelper.Create<DeleteGroupMetaModel>();
            metaModel.IsAllowedCSComm = false;

            var expectedResult = FakeHelper.Create<ResultInfo>();
            _groupDataAccess.DeleteGroup(
                metaModel.GroupId,
                metaModel.LicenseeId,
                metaModel.UserId,
                metaModel.Ip,
                metaModel.Language,
                metaModel.IsRemoveCommSetting,
                metaModel.IsResetDefault)
            .Returns(expectedResult);

            // Action
            var result = await _groupComService.DeleteGroup(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                _casinoCustomerService.DidNotReceive().UpdateCommission(Arg.Any<RNGCasinoUpdateMemberComMetaModel>());
                Assert.That(result, Is.EqualTo(expectedResult));
            });
        }

        [Test]
        public async Task DeleteGroup_CusttomerListIsEmpty_NotCallCasinoApi()
        {
            // Arrange
            var metaModel = FakeHelper.Create<DeleteGroupMetaModel>();
            metaModel.IsAllowedCSComm = true;
            metaModel.IsRemoveCommSetting = true;

            var expectedResult = new ResultInfo { ExtendedData = string.Empty };
            _groupDataAccess.DeleteGroup(
                metaModel.GroupId,
                metaModel.LicenseeId,
                metaModel.UserId,
                metaModel.Ip,
                metaModel.Language,
                metaModel.IsRemoveCommSetting,
                metaModel.IsResetDefault)
            .Returns(expectedResult);

            // Action
            var result = await _groupComService.DeleteGroup(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                _casinoCustomerService.DidNotReceive().UpdateCommission(Arg.Any<RNGCasinoUpdateMemberComMetaModel>());
                Assert.That(result, Is.EqualTo(expectedResult));
            });
        }

        [Test]
        public async Task DeleteGroup_IsUpdateCS_CallCasinoApi()
        {
            // Arrange
            var metaModel = FakeHelper.Create<DeleteGroupMetaModel>();
            metaModel.IsAllowedCSComm = true;
            metaModel.IsRemoveCommSetting = true;

            var expectedResult = new ResultInfo { ExtendedData = "1,2,3" };
            _groupDataAccess.DeleteGroup(
                metaModel.GroupId,
                metaModel.LicenseeId,
                metaModel.UserId,
                metaModel.Ip,
                metaModel.Language,
                metaModel.IsRemoveCommSetting,
                metaModel.IsResetDefault)
            .Returns(expectedResult);

            _casinoCustomerService.UpdateCommission(Arg.Any<RNGCasinoUpdateMemberComMetaModel>()).Returns(true);

            // Action
            var result = await _groupComService.DeleteGroup(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                _casinoCustomerService.Received().UpdateCommission(Arg.Is<RNGCasinoUpdateMemberComMetaModel>(
                    x => x.Comm == 0
                        && string.Join(",", x.CustomerId) == expectedResult.ExtendedData
                        && x.FromIp == metaModel.Ip
                        && x.ModifyBy == metaModel.UserId
                        && x.ModifyUserName == metaModel.UserName));
                Assert.That(result, Is.EqualTo(expectedResult));
            });
        }

        [Test]
        public async Task DeleteGroup_IsUpdateCS_CallCasinoApiFailed_ReturnErrorResult()
        {
            // Arrange
            var metaModel = FakeHelper.Create<DeleteGroupMetaModel>();
            metaModel.IsAllowedCSComm = true;
            metaModel.IsRemoveCommSetting = true;

            _groupDataAccess.DeleteGroup(
                metaModel.GroupId,
                metaModel.LicenseeId,
                metaModel.UserId,
                metaModel.Ip,
                metaModel.Language,
                metaModel.IsRemoveCommSetting,
                metaModel.IsResetDefault)
            .Returns(new ResultInfo { ExtendedData = "1,2,3" });

            _casinoCustomerService.UpdateCommission(Arg.Any<RNGCasinoUpdateMemberComMetaModel>()).Returns(false);

            var expectedResult = new ResultInfo { Code = 1, Message = "Having errors when calling CasinoAPI to update customer commission." };

            // Action
            var result = await _groupComService.DeleteGroup(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Code, Is.EqualTo(expectedResult.Code));
                Assert.That(result.Message, Is.EqualTo(expectedResult.Message));
            });
        }

        [Test]
        public void CheckByName_WhenCall_ReturnDataFromDataAccess()
        {
            // Arrange
            var licenseeId = FakeHelper.Create<int>();
            var groupName = FakeHelper.Create<string>();

            // Action
            var result = _groupComService.CheckExistingGroupByName(licenseeId, groupName);

            // Assert
            _groupDataAccess.Received().CheckByName(licenseeId, groupName);
        }
    }
}