namespace Fanex.Athena.DataAccess.Core.UnitTests.Users
{
    using System;
    using System.Data;
    using Core.Users;
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

            var requestUser = new User { UserID = 1, Request = new Request { Ip = "::1" } };
            const string NetSessionId = "1a2b3c";
            const int ServerId = 2;

            // Action
            DataAccess.LoginOnUser(requestUser, NetSessionId, ServerId);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received().AddInParameter("CustID", DbType.Int32, requestUser.UserID);
                ObjectDb.Received().AddInParameter("NetSessionID", DbType.String, NetSessionId);
                ObjectDb.Received().AddInParameter("ServerID", DbType.Int32, ServerId);
                ObjectDb.Received().AddOutParameter("On_UserID", DbType.Int32, 4);
                ObjectDb.Received().AddInParameter("IP", DbType.String, requestUser.Request.Ip);
            });
        }

        [Test]
        public void LoginOnUser_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is("Lrf_CheckIn")).Returns(ObjectDb);

            var requestUser = new User { UserID = 1, Request = new Request { Ip = "::1" } };
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
        public void LoginOnUser_WhenObjectDbThrowsException_ThrowDataAccessException()
        {
            TestException_WhenObjectDbThrowsException_ThrowDataAccessException(
                "Lrf_CheckIn",
                () => DataAccess.LoginOnUser(new User(), string.Empty, 0));
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
        public void ValidateInternalUser_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is("Lrf_InternalLogin")).Returns(ObjectDb);

            const string LicenseeName = "TestName";
            var requestUser = GetFakeRequestUserData();
            var timeStamp = TimeSpan.TicksPerMillisecond;

            // Action
            DataAccess.ValidateInternalUser(requestUser, timeStamp, LicenseeName);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received().AddInParameter("UserName", DbType.String, requestUser.Username);
                ObjectDb.Received().AddInParameter("EncryptedPassword", DbType.String, requestUser.Password);
                ObjectDb.Received().AddInParameter("LicenseeName", DbType.String, LicenseeName);
                ObjectDb.Received().AddInParameter("LoginTime", DbType.DateTime, new DateTime(timeStamp));
                ObjectDb.Received().AddInParameter("LoginIP", DbType.String, requestUser.Request.Ip);
                ObjectDb.Received().AddInParameter("LoginDomain", DbType.String, requestUser.Request.Domain);
                ObjectDb.Received().AddInParameter("BrowserName", DbType.String, requestUser.Request.Browser);
                ObjectDb.Received().AddInParameter("Platform", DbType.String, requestUser.Request.Platform);
                ObjectDb.Received().AddInParameter("CountryCode", DbType.String, requestUser.Request.CountryCode);
                ObjectDb.Received().AddInParameter("Language", DbType.String, requestUser.Request.Language);
                ObjectDb.Received().AddOutParameter("ServerID", DbType.Int32, 4);
                ObjectDb.Received().AddOutParameter("ErrCode", DbType.Int32, 4);
                ObjectDb.Received().AddOutParameter("ErrMsg", DbType.String, 4000);
            });
        }

        [Test]
        public void ValidateInternalUser_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is("Lrf_InternalLogin")).Returns(ObjectDb);

            const string LicenseeName = "TestName";
            var timeStamp = TimeSpan.TicksPerMillisecond;

            var requestUser = GetFakeRequestUserData();
            var expected = GetFakeUserData();

            ConfigureDataReader(expected);

            // Action
            var user = DataAccess.ValidateInternalUser(requestUser, timeStamp, LicenseeName).ExtendedData;

            // Assert
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

        [Test]
        public void ValidateInternalUser_WhenObjectDbThrowsException_ThrowDataAccessException()
        {
            TestException_WhenObjectDbThrowsException_ThrowDataAccessException(
                "Lrf_InternalLogin",
                () => DataAccess.ValidateInternalUser(new User(), 0, string.Empty));
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
                ObjectDb.Received().AddInParameter("UserName", DbType.String, requestUser.Username);
                ObjectDb.Received().AddInParameter("LoginIP", DbType.String, requestUser.Request.Ip);
                ObjectDb.Received().AddInParameter("LoginDomain", DbType.String, requestUser.Request.Domain);
                ObjectDb.Received().AddInParameter("BrowserName", DbType.String, requestUser.Request.Browser);
                ObjectDb.Received().AddInParameter("Platform", DbType.String, requestUser.Request.Platform);
                ObjectDb.Received().AddInParameter("CountryCode", DbType.String, requestUser.Request.CountryCode);
                ObjectDb.Received().AddInParameter("langkey", DbType.String, requestUser.Request.Language);
                ObjectDb.Received().AddOutParameter("ServerID", DbType.Int32, 4);
                ObjectDb.Received().AddOutParameter("ErrCode", DbType.Int32, 4);
                ObjectDb.Received().AddOutParameter("ErrMsg", DbType.String, 4000);
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
                    Ip = "::1",
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