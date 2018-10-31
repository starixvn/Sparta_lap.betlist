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
    using Resources;
    using UnitTest.Common.Helpers;

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
            var expectedResult = FakeHelper.Create<int>();
            var requestUser = new User
            {
                Request = new Request()
            };
            var netSessionId = FakeHelper.Create<string>();
            var serverId = FakeHelper.Create<int>();
            var ip = FakeHelper.Create<string>();

            _authDataAccess.LoginOnUser(Arg.Any<User>(), netSessionId, serverId).Returns(expectedResult);

            // Action
            var result = _authService.LoginOnUser(requestUser, netSessionId, serverId, ip);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.EqualTo(expectedResult));
                Assert.That(requestUser.Request.Ip, Is.EqualTo(ip));
            });
        }

        [Test]
        public void Logout_WhenCall_ExcuteDataAccessCorrectly()
        {
            // Arrange
            var onuserId = FakeHelper.Create<int>();

            // Action
            _authService.Logout(onuserId);

            // Assert
            _authDataAccess.Received().Logout(onuserId);
        }

        [Test]
        public void ValidateUserForLrfSystem_WhenCall_ReturnResultFromDataAccess()
        {
            // Arrange
            var outputUser = new User
            {
                UserID = FakeHelper.Create<int>(),
                Password = FakeHelper.Create<string>(),
                UserSalt = FakeHelper.Create<string>()
            };
            var loginToken = FakeHelper.Create<string>();
            var hashPassword = Cryptography.CreateHash(outputUser.Password + loginToken, outputUser.UserSalt);
            var inputUser = new User
            {
                Username = FakeHelper.Create<string>(),
                Password = hashPassword
            };

            var resultInfo = new ResultInfo<User>
            {
                ExtendedData = outputUser
            };
            _userDataAccess.GetByUsername(inputUser.Username, false, 0).Returns(outputUser);
            _authDataAccess.ValidateUserForLrfSystem(inputUser).Returns(resultInfo);

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
                UserID = FakeHelper.Create<int>(),
                Password = FakeHelper.Create<string>(),
                UserSalt = FakeHelper.Create<string>()
            };
            var loginToken = FakeHelper.Create<string>();
            var hashPassword = Cryptography.CreateHash("WrongPassword", outputUser.UserSalt);
            var inputUser = new User
            {
                Username = FakeHelper.Create<string>(),
                Password = hashPassword
            };

            _userDataAccess.GetByUsername(inputUser.Username, Arg.Any<bool>(), Arg.Any<int>()).Returns(outputUser);

            var expectedResult = new ResultInfo { Code = 104, Message = ViewResources.WrongUserNamePassword };

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
            var loginToken = FakeHelper.Create<string>();

            var inputUser = new User
            {
                Username = FakeHelper.Create<string>(),
                Password = FakeHelper.Create<string>()
            };

            _userDataAccess.GetByUsername("user1", Arg.Any<bool>(), Arg.Any<int>()).Returns(outputUser);
            var expectedResult = new ResultInfo { Code = 100, Message = ViewResources.UserNotExist };

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