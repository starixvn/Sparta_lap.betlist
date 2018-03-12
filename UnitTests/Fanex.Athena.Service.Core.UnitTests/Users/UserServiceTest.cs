namespace Fanex.Athena.Service.Core.UnitTests.Users
{
    using System.Collections.Generic;
    using Core.Users;
    using Core.Users.Interfaces;
    using DataAccess.Core.Users.Interfaces;
    using Model.Core.DTO.Common;
    using Model.Core.DTO.Users;
    using NSubstitute;
    using NUnit.Framework;
    using Resources;

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

        [TestCase(0, true)]
        [TestCase(0, false)]
        [TestCase(1, true)]
        [TestCase(1, false)]
        public void GetByUsername_WhenCall_ReturnDataFromDataAccess(int licenseeId, bool isInternal)
        {
            // Arrange
            var expectedResult = new User { UserID = 1 };
            var userName = "demo";
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
        public void ChangePassword_UserDoesNotExist_ReturnErrorResult()
        {
            // Arrange
            const string UserName = "Test", OldPass = "sdkaskger", NewPass = "poujkg", Ip = "1.1.1.1";

            _userDataAccess.GetByUsername(UserName, false).Returns(x => { return null; });

            // Action
            var result = _userService.ChangePassword(UserName, OldPass, NewPass, Ip);

            // Assert
            Assert.That(result.Message, Is.EqualTo(ViewResources.UserNotExist));
        }

        [Test]
        public void ChangePassword_WhenCall_CallDataAccessMethodToChangePassword()
        {
            // Arrange
            const string UserName = "Test", OldPass = "sdkaskger", NewPass = "poujkg", Ip = "1.1.1.1";

            _userDataAccess.GetByUsername(UserName, false).Returns(new User { UserSalt = "sdwdxz" });

            // Action
            var result = _userService.ChangePassword(UserName, OldPass, NewPass, Ip);

            // Assert
            _userDataAccess.Received().ChangePassword(
                Arg.Is<User>(x => x.Username == UserName && x.Request.Ip == Ip),
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<string>());
        }
    }
}