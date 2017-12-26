namespace Fanex.Athena.Service.Core.UnitTests.Users
{
    using Core.Users;
    using Core.Users.Interfaces;
    using DataAccess.Core.Users.Interfaces;
    using Fanex.Common;
    using Model.Core.DTO.Common;
    using Model.Core.DTO.Users;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class AuthServiceTest
    {
        private IUserDataAccess _userDataAccess;
        private IAuthDataAccess _authDataAccess;
        private IAuthService _authService;

        [OneTimeSetUp]
        public void SetUp()
        {
            _userDataAccess = Substitute.For<IUserDataAccess>();
            _authDataAccess = Substitute.For<IAuthDataAccess>();
            _authService = new AuthService(_userDataAccess, _authDataAccess);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _authDataAccess = null;
            _authService = null;
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
            _authDataAccess.LoginOnUser(Arg.Any<User>(), netSessionId, serverId).Returns(expectedResult);

            // Action
            var result = _authService.LoginOnUser(user, netSessionId, serverId, string.Empty);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void Logout_WhenCall_ExcuteDataAccessCorrectly()
        {
            // Arrange
            var onuserId = 1;

            // Action
            _authService.Logout(onuserId);

            // Assert
            _authDataAccess.Received().Logout(onuserId);
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
            _authDataAccess.ValidateInternalUser(Arg.Any<User>(), timeStamp, licenseeName, out resultInfo).Returns(outputUser);

            // Action
            var result = _authService.ValidateInternalUser(new User(), licenseeName, timeStamp, out resultInfo);

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

            var resultInfo = new ResultInfo<User>
            {
                ExtendedData = outputUser
            };
            _userDataAccess.GetByUsername("user1", Arg.Any<bool>(), Arg.Any<int>()).Returns(outputUser);
            _authDataAccess.ValidateUserForLrfSystem(Arg.Any<User>()).Returns(resultInfo);

            // Action
            var result = _authService.ValidateUserForLrfSystem(inputUser, loginToken);

            // Assert
            Assert.That(result.ExtendedData.UserID, Is.EqualTo(outputUser.UserID));
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

            _userDataAccess.GetByUsername("user1", Arg.Any<bool>(), Arg.Any<int>()).Returns(outputUser);

            var expectedResult = new ResultInfo { Code = 104, Message = "Wrong username or password." };

            // Action
            var result = _authService.ValidateUserForLrfSystem(inputUser, loginToken);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Code, Is.EqualTo(expectedResult.Code));
                Assert.That(result.Message, Is.EqualTo(expectedResult.Message));
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

            _userDataAccess.GetByUsername("user1", Arg.Any<bool>(), Arg.Any<int>()).Returns(outputUser);
            var expectedResult = new ResultInfo { Code = 100, Message = "Not found user" };

            // Action
            var result = _authService.ValidateUserForLrfSystem(inputUser, loginToken);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Code, Is.EqualTo(expectedResult.Code));
                Assert.That(result.Message, Is.EqualTo(expectedResult.Message));
            });
        }
    }
}