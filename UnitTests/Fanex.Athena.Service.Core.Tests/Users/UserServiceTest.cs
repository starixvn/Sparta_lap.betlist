namespace Fanex.Athena.Service.Core.Tests.Common
{
    using System.Collections.Generic;
    using DataAccess.Core.Users.Interfaces;
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
        private IUserService _moveBetService;

        [SetUp]
        public void SetUp()
        {
            _userDataAccess = Substitute.For<IUserDataAccess>();
            _moveBetService = new UserService(_userDataAccess);
        }

        [Test]
        public void ChangePassword_WhenCall_CallRelevantMethodsOfDataAccess()
        {
            // Arrange
            var userName = "user";
            var oldPassword = "oldpwd";
            var newPassword = "newpwd";

            // Action
            var result = _moveBetService.ChangePassword(userName, oldPassword, newPassword, Arg.Any<string>());

            // Assert
            _userDataAccess.Received().GetUserSalt(userName);
            _userDataAccess.Received().ChangePassword(Arg.Any<User>(), oldPassword, newPassword, Arg.Any<string>());
        }

        [Test]
        public void GetByLicensee_WhenCall_ReturnDataFromDataAccess()
        {
            var expectedResult = new List<User>()
            {
                new User() { Id = 1 }
            };
            var licenseeId = 1;
            var isInternal = true;
            _userDataAccess.GetByLicensee(licenseeId, isInternal).Returns(expectedResult);

            // Action
            var result = _moveBetService.GetByLicensee(licenseeId, isInternal);

            // Assert
            Assert.That(result[0].Id, Is.EqualTo(expectedResult[0].Id));
        }

        [Test]
        public void GetByUsername_WhenCall_ReturnDataFromDataAccess()
        {
            var expectedResult = new User() { Id = 1 };
            var userName = "demo";
            _userDataAccess.GetByUsername(userName).Returns(expectedResult);

            // Action
            var result = _moveBetService.GetByUsername(userName);

            // Assert
            Assert.That(result.Id, Is.EqualTo(expectedResult.Id));
        }

        [Test]
        public void GetUserInfo_WhenCall_ReturnDataFromDataAccess()
        {
            var expectedResult = new User() { Id = 1 };
            var licenseeId = 1;
            var userId = 2;
            _userDataAccess.GetUserInfo(licenseeId, userId).Returns(expectedResult);

            // Action
            var result = _moveBetService.GetUserInfo(licenseeId, userId);

            // Assert
            Assert.That(result.Id, Is.EqualTo(expectedResult.Id));
        }

        [Test]
        public void IsUserNameExisted_WhenCall_ReturnDataFromDataAccess()
        {
            var expectedResult = true;
            var userName = "demo";
            _userDataAccess.IsUsernameExisted(userName).Returns(expectedResult);

            // Action
            var result = _moveBetService.IsUserNameExisted(userName);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void LoginOnUser_WhenCall_ReturnDataFromDataAccess()
        {
            var expectedResult = 100;
            var user = new User()
            {
                Request = new Request()
            };
            var netSessionId = "session1";
            var serverId = 1;
            _userDataAccess.LoginOnUser(Arg.Any<User>(), netSessionId, serverId).Returns(expectedResult);

            // Action
            var result = _moveBetService.LoginOnUser(user, netSessionId, serverId, string.Empty);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }
    }
}