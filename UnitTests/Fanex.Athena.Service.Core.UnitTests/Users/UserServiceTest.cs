namespace Fanex.Athena.Service.Core.UnitTests.Users
{
    using System.Collections.Generic;
    using Core.Users;
    using Core.Users.Interfaces;
    using DataAccess.Core.Users.Interfaces;
    using Model.Core.DTO.Users;
    using NSubstitute;
    using NUnit.Framework;
    using Resources;
    using UnitTest.Common.Helpers;

    [TestFixture]
    public class UserServiceTest
    {
        private IUserDataAccess _userDataAccess;
        private IUserService _userService;

        [OneTimeSetUp]
        public void SetUp()
        {
            _userDataAccess = Substitute.For<IUserDataAccess>();
            _userService = new UserService(_userDataAccess);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _userDataAccess = null;
            _userService = null;
        }

        [Test]
        public void GetByLicensee_WhenCall_ReturnDataFromDataAccess()
        {
            // Arrange
            var expectedResult = new List<User>
            {
                new User { UserID = FakeHelper.Create<int>() }
            };
            var licenseeId = FakeHelper.Create<int>();
            var isInternal = FakeHelper.Create<bool>();
            _userDataAccess.GetByLicensee(licenseeId, isInternal).Returns(expectedResult);

            // Action
            var result = _userService.GetByLicensee(licenseeId, isInternal);

            // Assert
            Assert.That(result[0].UserID, Is.EqualTo(expectedResult[0].UserID));
        }

        [Test]
        public void GetByUsername_WhenCall_ReturnDataFromDataAccess()
        {
            // Arrange
            var userName = FakeHelper.Create<string>();
            var licenseeId = FakeHelper.Create<int>();
            var isInternal = FakeHelper.Create<bool>();

            var expectedResult = new User { UserID = FakeHelper.Create<int>() };
            _userDataAccess.GetByUsername(userName, isInternal, licenseeId).Returns(expectedResult);

            // Action
            var result = _userService.GetByUsername(userName, isInternal, licenseeId);

            // Assert
            Assert.That(result.UserID, Is.EqualTo(expectedResult.UserID));
        }

        [Test]
        public void GetUserInfo_WhenCall_ReturnDataFromDataAccess()
        {
            // Arrange
            var licenseeId = FakeHelper.Create<int>();
            var userId = FakeHelper.Create<int>();

            var expectedResult = new User { UserID = FakeHelper.Create<int>() };
            _userDataAccess.GetUserInfo(licenseeId, userId).Returns(expectedResult);

            // Action
            var result = _userService.GetUserInfo(licenseeId, userId);

            // Assert
            Assert.That(result.UserID, Is.EqualTo(expectedResult.UserID));
        }

        [Test]
        public void RecheckIn_WhenCall_ExcuteDataAccessCorrectly()
        {
            // Arrange
            var onuserId = FakeHelper.Create<int>();

            // Action
            _userService.RecheckIn(onuserId);

            // Assert
            _userDataAccess.Received().RecheckIn(onuserId);
        }

        [Test]
        public void ResetPassword_WhenCall_ExcuteDataAccessCorrectly()
        {
            // Arrange
            var user = new User();
            var updatedBy = FakeHelper.Create<int>();

            // Action
            _userService.ResetPassword(user, updatedBy);

            // Assert
            _userDataAccess.Received().ResetPassword(Arg.Any<User>(), Arg.Any<string>(), updatedBy);
            Assert.That(user.Password, Is.Not.Null.Or.Empty);
        }

        [Test]
        public void UpdateInfo_WhenCall_ExcuteDataAccessCorrectly()
        {
            // Arrange
            var user = new User();
            var updatedBy = FakeHelper.Create<int>();

            // Action
            _userService.UpdateInfo(user, updatedBy);

            // Assert
            _userDataAccess.Received().UpdateInfo(Arg.Any<User>(), updatedBy);
        }

        [Test]
        public void ChangePassword_UserDoesNotExist_ReturnErrorResult()
        {
            // Arrange
            var userName = FakeHelper.Create<string>();
            var oldPass = FakeHelper.Create<string>();
            var newPass = FakeHelper.Create<string>();
            var ip = FakeHelper.Create<string>();

            _userDataAccess.GetByUsername(userName, false).Returns(x => { return null; });

            // Action
            var result = _userService.ChangePassword(userName, oldPass, newPass, ip);

            // Assert
            Assert.That(result.Message, Is.EqualTo(ViewResources.UserNotExist));
        }

        [Test]
        public void ChangePassword_WhenCall_CallDataAccessMethodToChangePassword()
        {
            // Arrange
            var userName = FakeHelper.Create<string>();
            var oldPass = FakeHelper.Create<string>();
            var newPass = FakeHelper.Create<string>();
            var ip = FakeHelper.Create<string>();

            _userDataAccess.GetByUsername(userName, false).Returns(new User { UserSalt = FakeHelper.Create<string>() });

            // Action
            var result = _userService.ChangePassword(userName, oldPass, newPass, ip);

            // Assert
            _userDataAccess.Received().ChangePassword(
                Arg.Is<User>(x => x.Username == userName && x.Request.Ip == ip),
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<string>());
        }
    }
}