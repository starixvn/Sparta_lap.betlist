namespace Fanex.Athena.Service.Core.UnitTests.Common
{
    using System.Collections.Generic;
    using DataAccess.Core.Users.Interfaces;
    using Fanex.Common;
    using Model.Core.DTO.Common;
    using Model.Core.DTO.Users;
    using NSubstitute;
    using NUnit.Framework;
    using Users;
    using Users.Interfaces;

    [TestFixture]
    public class UserServiceTest
    {
        private IUserDataAccess _userDataAccess;
        private IUserService _userService;

        [SetUp]
        public void SetUp()
        {
            _userDataAccess = Substitute.For<IUserDataAccess>();
            _userService = new UserService(_userDataAccess);
        }

        [Test]
        public void ChangePassword_WhenCall_CallRelevantMethodsOfDataAccess()
        {
            // Arrange
            var userName = "user";
            var oldPassword = "oldpwd";
            var newPassword = "newpwd";
            var oldSalt = "anything";
            var oldEncryptedPassword = Cryptography.CreateHash(oldPassword, oldSalt);
            _userDataAccess.GetUserSalt(userName).Returns(oldSalt);

            // Action
            var result = _userService.ChangePassword(userName, oldPassword, newPassword, Arg.Any<string>());

            // Assert
            _userDataAccess.Received().ChangePassword(Arg.Any<User>(), oldEncryptedPassword, Arg.Any<string>(), Arg.Any<string>());
        }

        [Test]
        public void GetByLicensee_WhenCall_ReturnDataFromDataAccess()
        {
            // Arrange
            var expectedResult = new List<User>()
            {
                new User() { Id = 1 }
            };
            var licenseeId = 1;
            var isInternal = true;
            _userDataAccess.GetByLicensee(licenseeId, isInternal).Returns(expectedResult);

            // Action
            var result = _userService.GetByLicensee(licenseeId, isInternal);

            // Assert
            Assert.That(result[0].Id, Is.EqualTo(expectedResult[0].Id));
        }

        [Test]
        public void GetByUsername_WhenCall_ReturnDataFromDataAccess()
        {
            // Arrange
            var expectedResult = new User() { Id = 1 };
            var userName = "demo";
            _userDataAccess.GetByUsername(userName).Returns(expectedResult);

            // Action
            var result = _userService.GetByUsername(userName);

            // Assert
            Assert.That(result.Id, Is.EqualTo(expectedResult.Id));
        }

        [Test]
        public void GetUserInfo_WhenCall_ReturnDataFromDataAccess()
        {
            // Arrange
            var expectedResult = new User() { Id = 1 };
            var licenseeId = 1;
            var userId = 2;
            _userDataAccess.GetUserInfo(licenseeId, userId).Returns(expectedResult);

            // Action
            var result = _userService.GetUserInfo(licenseeId, userId);

            // Assert
            Assert.That(result.Id, Is.EqualTo(expectedResult.Id));
        }

        [Test]
        public void IsUserNameExisted_WhenCall_ReturnDataFromDataAccess()
        {
            // Arrange
            var expectedResult = true;
            var userName = "demo";
            _userDataAccess.IsUsernameExisted(userName).Returns(expectedResult);

            // Action
            var result = _userService.IsUserNameExisted(userName);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void LoginOnUser_WhenCall_ReturnDataFromDataAccess()
        {
            // Arrange
            var expectedResult = 100;
            var user = new User()
            {
                Request = new Request()
            };
            var netSessionId = "session1";
            var serverId = 1;
            _userDataAccess.LoginOnUser(Arg.Any<User>(), netSessionId, serverId).Returns(expectedResult);

            // Action
            var result = _userService.LoginOnUser(user, netSessionId, serverId, string.Empty);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void Logout_WhenCall_ExcuteDataAccessCorrectly()
        {
            // Arrange
            var onuserId = 1;

            // Action
            _userService.Logout(onuserId);

            // Assert
            _userDataAccess.Received().Logout(onuserId);
        }

        [Test]
        public void RecheckIn_WhenCall_ExcuteDataAccessCorrectly()
        {
            // Arrange
            var onuserId = 1;

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
            var updatedBy = 1;

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
            var updatedBy = 1;

            // Action
            _userService.UpdateInfo(user, updatedBy);

            // Assert
            _userDataAccess.Received().UpdateInfo(Arg.Any<User>(), updatedBy);
        }

        [Test]
        public void ValidateInternalUser_WhenCall_ReturnResultFromDataAccess()
        {
            // Arrange
            var outputUser = new User()
            {
                Id = 999
            };
            var licenseeName = "spondemo";
            long timeStamp = 100;
            IResultInfo resultInfo = null;
            _userDataAccess.ValidateInternalUser(Arg.Any<User>(), timeStamp, licenseeName, out resultInfo).Returns(outputUser);

            // Action
            var result = _userService.ValidateInternalUser(new User(), licenseeName, timeStamp, out resultInfo);

            // Assert
            Assert.That(result.Id, Is.EqualTo(outputUser.Id));
        }

        [Test]
        public void ValidateUserForLrfSystem_WhenCall_ReturnResultFromDataAccess()
        {
            // Arrange
            var outputUser = new User()
            {
                Id = 999,
                Password = "password1",
                UserSalt = "salt1"
            };
            var loginToken = "logintoken";
            var hashPassword = Cryptography.CreateHash(outputUser.Password + loginToken, outputUser.UserSalt);
            var inputUser = new User()
            {
                Username = "user1",
                Password = hashPassword
            };
            
            IResultInfo resultInfo = null;
            _userDataAccess.GetByUsername("user1").Returns(outputUser);
            _userDataAccess.ValidateUserForLrfSystem(Arg.Any<User>(), out resultInfo).Returns(outputUser);

            // Action
            var result = _userService.ValidateUserForLrfSystem(inputUser, loginToken, out resultInfo);

            // Assert
            Assert.That(result.Id, Is.EqualTo(outputUser.Id));
        }
    }
}