namespace Fanex.Athena.DataAccess.Core.UnitTests.Users
{
    using System.Data;
    using Core.Users;
    using Data;
    using Model.Core.DTO.Common;
    using Model.Core.DTO.Users;
    using NSubstitute;
    using NUnit.Framework;
    using TestHelpers;

    [TestFixture]
    public class AuthDataAccessTest : BaseDataAccessHelperTest<AuthDataAccess>
    {
        [Test]
        public void LoginOnUser_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is("Lrf_CheckIn")).Returns(ObjectDb);

            var requestUser = new User { UserID = 1, Request = new Request { Ip = "IP" } };
            const string NetSessionId = "1a2b3c";
            const int ServerId = 2;

            // Action
            DataAccess.LoginOnUser(requestUser, NetSessionId, ServerId);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received(1).SetParameterValue("CustID", requestUser.UserID);
                ObjectDb.Received(1).SetParameterValue("NetSessionID", NetSessionId);
                ObjectDb.Received(1).SetParameterValue("ServerID", ServerId);
                ObjectDb.Received(1).SetParameterValue("On_UserID", null);
                ObjectDb.Received(1).SetParameterValue("IP", requestUser.Request.Ip);
            });
        }

        [Test]
        public void LoginOnUser_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is("Lrf_CheckIn")).Returns(ObjectDb);

            var requestUser = new User { UserID = 1, Request = new Request { Ip = "IP" } };
            const string NetSessionId = "1a2b3c";
            const int ServerId = 2;

            const int ExpectedOnUserID = 10;
            ObjectDb.GetParameterValue("On_UserID").Returns(ExpectedOnUserID);

            // Action
            var result = DataAccess.LoginOnUser(requestUser, NetSessionId, ServerId);

            // Assert
            Assert.That(result, Is.EqualTo(ExpectedOnUserID));
        }

        [Test]
        public void Logout_WhenCall_ExecuteNonQueryMethodIsCalled()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is("Lrf_UserLogout")).Returns(ObjectDb);
            const int UserId = 1;

            // Action
            DataAccess.Logout(UserId);

            // Assert
            ObjectDb.ReceivedWithAnyArgs().ExecuteNonQuery(null);
        }

        [Test]
        public void Logout_WhenObjectDbThrowsException_ThrowDataAccessException()
        {
            TestException_WhenObjectDbThrowsException_ThrowDataAccessException(
                "Lrf_UserLogout",
                () => DataAccess.Logout(0));
        }

        [Test]
        public void ValidateUserForLrfSystem_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is("Lrf_UserLogin")).Returns(ObjectDb);
            var requestUser = GetFakeRequestUserData();

            // Action
            DataAccess.ValidateUserForLrfSystem(requestUser);

            // Assert
            Assert.Multiple(() =>
            {
                AssertPassedCommonParams(ObjectDb, requestUser);
                ObjectDb.Received(1).SetParameterValue("langkey", requestUser.Request.Language);
            });
        }

        [Test]
        public void ValidateUserForLrfSystem_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is("Lrf_UserLogin")).Returns(ObjectDb);

            var requestUser = GetFakeRequestUserData();
            var expected = GetFakeUserData();

            ConfigureDataReader(expected);

            // Action
            var result = DataAccess.ValidateUserForLrfSystem(requestUser);
            var user = result.ExtendedData;

            // Assert
            AssertExpectedUser(expected, user);
        }

        private static void AssertExpectedUser(User expected, User user)
        {
            Assert.Multiple(() =>
            {
                Assert.That(user.UserID, Is.EqualTo(expected.UserID));
                Assert.That(user.Username, Is.EqualTo(expected.Username));
                Assert.That(user.FirstName, Is.EqualTo(expected.FirstName));
                Assert.That(user.LastName, Is.EqualTo(expected.LastName));
                Assert.That(user.Email, Is.EqualTo(expected.Email));
                Assert.That(user.Role, Is.EqualTo(expected.Role));
                Assert.That(user.IsInternal, Is.EqualTo(expected.IsInternal));
                Assert.That(user.Licensee.Id, Is.EqualTo(expected.Licensee.Id));
                Assert.That(user.ServerId, Is.EqualTo(expected.ServerId));
            });
        }

        private static void AssertPassedCommonParams(IObjectDb objectDb, User requestUser)
        {
            objectDb.Received(1).SetParameterValue("UserName", requestUser.Username);
            objectDb.Received(1).SetParameterValue("LoginIP", requestUser.Request.Ip);
            objectDb.Received(1).SetParameterValue("LoginDomain", requestUser.Request.Domain);
            objectDb.Received(1).SetParameterValue("BrowserName", requestUser.Request.Browser);
            objectDb.Received(1).SetParameterValue("Platform", requestUser.Request.Platform);
            objectDb.Received(1).SetParameterValue("CountryCode", requestUser.Request.CountryCode);
            objectDb.Received(1).SetParameterValue("ServerID", null);
            objectDb.Received(1).SetParameterValue("ErrCode", null);
            objectDb.Received(1).SetParameterValue("ErrMsg", null);
        }

        private static User GetFakeUserData(int licenseeId = 1)
        {
            return new User
            {
                UserID = 1,
                Username = "UserNameTest",
                FirstName = "FirstName Test",
                LastName = "LastName Test",
                Status = "Status Test",
                IsManageAccount = true,
                Licensee = new Licensee { Id = licenseeId },
                IsInternal = true,
                ServerId = 12,
                Email = "test@gmail.com",
                Role = "RoleTest"
            };
        }

        private static User GetFakeRequestUserData()
        {
            return new User
            {
                Username = "UserNameTest",
                Request = new Request
                {
                    Ip = "IP",
                    Language = "en-US",
                    Domain = "abc.xyz",
                    Browser = "Chrome",
                    Platform = "Web",
                    CountryCode = "VN"
                }
            };
        }

        private void ConfigureDataReader(User expected)
        {
            Reader.Read().Returns(true, false);

            MapReaderData(expected);
            Reader["userrole"].Returns(expected.Role);
            Reader.GetInt32(Arg.Any<int>()).Returns(expected.Licensee.Id);
            ObjectDb.GetParameterValue("ServerId").Returns(expected.ServerId);
        }
    }
}