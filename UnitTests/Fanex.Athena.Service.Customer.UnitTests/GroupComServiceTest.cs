namespace Fanex.Athena.Service.Customer.UnitTests
{
    using System.Collections.Generic;
    using System.Data;
    using DataAccess.Customer.Interfaces;
    using Model.Core.DTO.Common;
    using Model.Core.DTO.Users;
    using Model.Customer.MetaModel;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class GroupComServiceTest
    {
        private IGroupComDataAccess _groupDataAccess;

        private GroupComService _groupComService;

        [SetUp]
        public void SetUp()
        {
            _groupDataAccess = Substitute.For<IGroupComDataAccess>();
            _groupComService = new GroupComService(_groupDataAccess);
        }

        [TearDown]
        public void TearDown()
        {
            _groupDataAccess = null;
            _groupComService = null;
        }

        [Test]
        public void GetAll_WhenCall_ReturnCorrectData()
        {
            // Arrange
            const int LicenseeId = 1;
            var expectedResult = new List<GroupCommMetaModel>();
            _groupDataAccess.GetAll(LicenseeId).Returns(expectedResult);

            // Action
            var result = _groupComService.GetAll(LicenseeId);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void GetDataForExcel_WhenCall_ReturnCorrectData()
        {
            // Arrange
            const int LicenseeId = 1;
            var expectedResult = new DataTable();
            _groupDataAccess.GetDataForExcel(LicenseeId).Returns(expectedResult);

            // Action
            var result = _groupComService.GetDataForExcel(LicenseeId);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void GetById_WhenCall_ReturnCorrectData()
        {
            // Arrange
            const int GroupCommId = 1;
            var expectedResult = new GroupCommMetaModel();
            _groupDataAccess.GetById(GroupCommId).Returns(expectedResult);

            // Action
            var result = _groupComService.GetById(GroupCommId);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void GetDefaultGroup_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var groupList = new List<GroupCommMetaModel>
            {
                new GroupCommMetaModel
                {
                    IsDefaultGroup = true,
                    GroupCommId = 1
                },
                new GroupCommMetaModel
                {
                    IsDefaultGroup = false,
                    GroupCommId = 2
                }
            };

            // Action
            var result = _groupComService.GetDefaultGroup(groupList);

            // Assert
            Assert.That(result, Is.EqualTo(1));
        }

        [Test]
        public void InsertGroup_WhenCall_ReturnCorrectData()
        {
            // Arrange
            _groupDataAccess.InsertGroup(Arg.Any<GroupCommMetaModel>(), Arg.Any<IRequest>(), Arg.Any<int>())
                .Returns(new ResultInfo());

            // Action
            var result = _groupComService.InsertGroup(Arg.Any<GroupCommMetaModel>(), Arg.Any<IRequest>(), Arg.Any<int>());

            // Assert
            Assert.That(result.IsSuccessful, Is.True);
        }

        [Test]
        public void UpdateGroup_IsUpdateCSInMetaModelIsFalse_ReturnCorrectData()
        {
            // Arrange
            var groupCommMetaModel = new GroupCommMetaModel { IsUpdateCS = false };
            var userSession = new UserSession { UserId = 1 };
            _groupDataAccess.UpdateGroup(Arg.Any<GroupCommMetaModel>(), Arg.Any<IRequest>(), Arg.Any<int>())
                .Returns(new ResultInfo());

            // Action
            var result = _groupComService.UpdateGroup(groupCommMetaModel, Arg.Any<IRequest>(), userSession);

            // Assert
            Assert.That(result.IsSuccessful, Is.True);
        }

        [Test]
        public void UpdateGroup_ExtendedDataIsEmpty_ReturnCorrectData()
        {
            // Arrange
            var groupCommMetaModel = new GroupCommMetaModel { IsUpdateCS = false };
            var userSession = new UserSession { UserId = 1 };
            _groupDataAccess.UpdateGroup(Arg.Any<GroupCommMetaModel>(), Arg.Any<IRequest>(), Arg.Any<int>())
                .Returns(new ResultInfo());

            // Action
            var result = _groupComService.UpdateGroup(groupCommMetaModel, Arg.Any<IRequest>(), userSession);

            // Assert
            Assert.That(result.IsSuccessful, Is.True);
        }

        [Test]
        public void DeleteGroup_IsRemoveCommSettingInMetaModelIsFalse_ReturnCorrectData()
        {
            // Arrange
            var metaModel = new DeleteGroupMetaModel
            {
                GroupId = 1,
                LicenseeId = 2,
                UserId = 3,
                Ip = "1.1.1.1",
                Language = "en-US",
                IsRemoveCommSetting = false,
                IsResetDefault = true,
                IsAllowedCSComm = true
            };
            _groupDataAccess.DeleteGroup(
                metaModel.GroupId,
                metaModel.LicenseeId,
                metaModel.UserId,
                metaModel.Ip,
                metaModel.Language,
                metaModel.IsRemoveCommSetting,
                metaModel.IsResetDefault)
                .Returns(new ResultInfo());

            // Action
            var result = _groupComService.DeleteGroup(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.IsSuccessful, Is.True);
            });
        }

        [Test]
        public void DeleteGroup_IsAllowedCSCommInMetaModelIsFalse_ReturnCorrectData()
        {
            // Arrange
            var metaModel = new DeleteGroupMetaModel
            {
                GroupId = 1,
                LicenseeId = 2,
                UserId = 3,
                Ip = "1.1.1.1",
                Language = "en-US",
                IsRemoveCommSetting = true,
                IsResetDefault = true,
                IsAllowedCSComm = false
            };
            _groupDataAccess.DeleteGroup(
                metaModel.GroupId,
                metaModel.LicenseeId,
                metaModel.UserId,
                metaModel.Ip,
                metaModel.Language,
                metaModel.IsRemoveCommSetting,
                metaModel.IsResetDefault)
                .Returns(new ResultInfo());

            // Action
            var result = _groupComService.DeleteGroup(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.IsSuccessful, Is.True);
            });
        }

        [Test]
        public void CheckByName_WhenCall_ReturnCorrectData()
        {
            // Arrange
            const int LicenseeId = 1;
            const string GroupName = "Group1";
            _groupDataAccess.CheckByName(LicenseeId, GroupName).Returns(true);

            // Action
            var result = _groupComService.CheckExistingGroupByName(LicenseeId, GroupName);

            // Assert
            Assert.That(result, Is.True);
        }
    }
}