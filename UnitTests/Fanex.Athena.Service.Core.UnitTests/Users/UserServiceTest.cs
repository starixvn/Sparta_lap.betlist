namespace Fanex.Athena.Service.Core.UnitTests.Users
{
    using System.Collections.Generic;
    using Core.Users;
    using Core.Users.Interfaces;
    using DataAccess.Core.Users.Interfaces;
    using Fanex.Common;
    using Model.Core.DTO.Common;
    using Model.Core.DTO.Users;
    using NSubstitute;
    using NUnit.Framework;

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
            var expectedResult = new List<User>
            {
                new User { UserID = 1 }
            };
            var licenseeId = 1;
            var isInternal = true;
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
            var expectedResult = new User { UserID = 1 };
            var userName = "demo";
            _userDataAccess.GetByUsername(userName).Returns(expectedResult);

            // Action
            var result = _userService.GetByUsername(userName);

            // Assert
            Assert.That(result.UserID, Is.EqualTo(expectedResult.UserID));
        }

        [Test]
        public void GetUserInfo_WhenCall_ReturnDataFromDataAccess()
        {
            // Arrange
            var expectedResult = new User { UserID = 1 };
            var licenseeId = 1;
            var userId = 2;
            _userDataAccess.GetUserInfo(licenseeId, userId).Returns(expectedResult);

            // Action
            var result = _userService.GetUserInfo(licenseeId, userId);

            // Assert
            Assert.That(result.UserID, Is.EqualTo(expectedResult.UserID));
        }

        [Test]
        public void LoginOnUser_WhenCall_ReturnDataFromDataAccess()
        {
            // Arrange
            var expectedResult = 100;
            var user = new User
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
            var outputUser = new User
            {
                UserID = 999
            };
            var licenseeName = "spondemo";
            long timeStamp = 100;
            IResultInfo resultInfo = null;
            _userDataAccess.ValidateInternalUser(Arg.Any<User>(), timeStamp, licenseeName, out resultInfo).Returns(outputUser);

            // Action
            var result = _userService.ValidateInternalUser(new User(), licenseeName, timeStamp, out resultInfo);

            // Assert
            Assert.That(result.UserID, Is.EqualTo(outputUser.UserID));
        }

        [Test]
        public void ValidateUserForLrfSystem_WhenCall_ReturnResultFromDataAccess()
        {
            // Arrange
            var outputUser = new User
            {
                UserID = 999,
                Password = "password1",
                UserSalt = "salt1"
            };
            var loginToken = "logintoken";
            var hashPassword = Cryptography.CreateHash(outputUser.Password + loginToken, outputUser.UserSalt);
            var inputUser = new User
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
            Assert.That(result.UserID, Is.EqualTo(outputUser.UserID));
        }

        [Test]
        public void ValidateUserForLrfSystem_WhenCallWithWrongPassword_GetCorrectResultInfo()
        {
            // Arrange
            var outputUser = new User
            {
                UserID = 999,
                Password = "password1",
                UserSalt = "salt1"
            };
            var loginToken = "logintoken";
            var hashPassword = Cryptography.CreateHash("WrongPassword", outputUser.UserSalt);
            var inputUser = new User
            {
                Username = "user1",
                Password = hashPassword
            };

            IResultInfo resultInfo = null;
            _userDataAccess.GetByUsername("user1").Returns(outputUser);

            var expectedResult = new ResultInfo { Code = 104, Message = "Wrong username or password." };

            // Action
            var result = _userService.ValidateUserForLrfSystem(inputUser, loginToken, out resultInfo);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(resultInfo.Code, Is.EqualTo(expectedResult.Code));
                Assert.That(resultInfo.Message, Is.EqualTo(expectedResult.Message));
            });
        }

        [Test]
        public void ValidateUserForLrfSystem_WhenCallWithWrongUserName_GetCorrectResultInfo()
        {
            // Arrange
            User outputUser = null;
            var loginToken = "logintoken";

            var inputUser = new User
            {
                Username = "user1",
                Password = "123"
            };

            IResultInfo resultInfo = null;
            _userDataAccess.GetByUsername("user1").Returns(outputUser);
            var expectedResult = new ResultInfo { Code = 100, Message = "Not found user" };

            // Action
            var result = _userService.ValidateUserForLrfSystem(inputUser, loginToken, out resultInfo);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(resultInfo.Code, Is.EqualTo(expectedResult.Code));
                Assert.That(resultInfo.Message, Is.EqualTo(expectedResult.Message));
            });
        }

        [Test]
        public void SyncUserInfo_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var expectedResult = new ResultInfo
            {
                Code = 100,
                Message = "Success"
            };
            const string LicenseeName = "12Bet", UserName = "alex";
            _userDataAccess.SyncUserInfo(LicenseeName, UserName).Returns(expectedResult);

            // Action
            var result = _userService.SyncUserInfo(LicenseeName, UserName);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }
    }
}