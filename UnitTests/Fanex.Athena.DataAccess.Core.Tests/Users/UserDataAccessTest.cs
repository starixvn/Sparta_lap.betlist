namespace Fanex.Athena.DataAccess.Core.Tests.Users
{
    using System;
    using System.Data;
    using System.Linq;
    using Core.Users;
    using Data;
    using Model.Core.DTO.Common;
    using Model.Core.DTO.Users;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class UserDataAccessTest
    {
        private IObjectDb _objectDb;
        private UserDataAccess _dataAccess;

        [SetUp]
        public void Setup()
        {
            _objectDb = Substitute.For<IObjectDb>();
            _dataAccess = Substitute.ForPartsOf<UserDataAccess>();
        }

        [Test]
        public void ChangePassword_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Licensee_UserPasswordUpd")).Returns(_objectDb);
            var requestUser = new User { Username = "test", Request = new Request { Ip = "::1" } };
            var oldPassword = "123456";
            var newPassword = "1234aa";
            var salt = "abcxyz";

            // Action
            _dataAccess.ChangePassword(requestUser, oldPassword, newPassword, salt);

            // Assert
            _objectDb.Received().AddInParameter("UserName", DbType.String, requestUser.Username);
            _objectDb.Received().AddInParameter("OldPwd", DbType.String, oldPassword);
            _objectDb.Received().AddInParameter("NewPwd", DbType.String, newPassword);
            _objectDb.Received().AddInParameter("LoginIP", DbType.String, requestUser.Request.Ip);
            _objectDb.Received().AddInParameter("UserSalt", DbType.String, salt);
        }

        [Test]
        public void GetByLicensee_WhenCall_ExecuteReaderMethodIsCalled()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_UserAccessSel")).Returns(_objectDb);
            var licenseeId = 1;
            var isInternal = true;

            // Action
            _dataAccess.GetByLicensee(licenseeId, isInternal);

            // Assert
            _objectDb.ReceivedWithAnyArgs().ExecuteReader(null);
        }

        [Test]
        public void GetByUsername_WhenCall_QueryMethodIsCalled()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_UserInfoSelByUserName")).Returns(_objectDb);
            var userName = "test";

            // Action
            _dataAccess.GetByUsername(userName);

            // Assert
            _objectDb.ReceivedWithAnyArgs().Query<User>(null).FirstOrDefault();
        }

        [Test]
        public void GetUserInfo_WhenCall_ExecuteReaderMethodIsCalled()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_UserInfoSel")).Returns(_objectDb);
            var licenseeId = 1;
            var userId = 2;

            // Action
            _dataAccess.GetUserInfo(licenseeId, userId);

            // Assert
            _objectDb.ReceivedWithAnyArgs().ExecuteReader(null);
        }

        [Test]
        public void GetUserSalt_WhenCall_ExecuteScalarMethodIsCalled()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Licensee_UserSaltSel")).Returns(_objectDb);
            var userName = "test";

            // Action
            _dataAccess.GetUserSalt(userName);

            // Assert
            _objectDb.ReceivedWithAnyArgs().ExecuteScalar(null);
        }

        [Test]
        public void IsUsernameExited_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_IsUserExisted")).Returns(_objectDb);
            var userName = "test";

            // Action
            _dataAccess.IsUsernameExited(userName);

            // Assert
            _objectDb.Received().AddInParameter("Username", DbType.String, userName);
            _objectDb.Received().AddOutParameter("IsUserExisted", DbType.Boolean, 1);
        }

        [Test]
        public void LoginOnUser_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_CheckIn")).Returns(_objectDb);
            var requestUser = new User { Id = 1, Request = new Request { Ip = "::1" } };
            var netSessionId = "1a2b3c";
            var serverId = 2;

            // Action
            _dataAccess.LoginOnUser(requestUser, netSessionId, serverId);

            // Assert
            _objectDb.Received().AddInParameter("CustID", DbType.Int32, requestUser.Id);
            _objectDb.Received().AddInParameter("NetSessionID", DbType.String, netSessionId);
            _objectDb.Received().AddInParameter("ServerID", DbType.Int32, serverId);
            _objectDb.Received().AddOutParameter("On_UserID", DbType.Int32, 4);
            _objectDb.Received().AddInParameter("IP", DbType.String, requestUser.Request.Ip);
        }

        [Test]
        public void Logout_WhenCall_ExecuteNonQueryMethodIsCalled()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_UserLogout")).Returns(_objectDb);
            var userId = 1;

            // Action
            _dataAccess.Logout(userId);

            // Assert
            _objectDb.ReceivedWithAnyArgs().ExecuteNonQuery(null);
        }

        [Test]
        public void ResetPassword_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_UserPwdReset")).Returns(_objectDb);
            var user = new User
            {
                Id = 1,
                Licensee = new Licensee { Id = 1 },
                Password = "1234aa",
                Request = new Request { Ip = "::1", Language = "en-US" }
            };
            var resetPwdBy = 2;
            var salt = "abcxyz";

            // Action
            _dataAccess.ResetPassword(user, salt, resetPwdBy);

            // Assert
            _objectDb.Received().AddInParameter("LicenseeID", DbType.Int32, user.Licensee.Id);
            _objectDb.Received().AddInParameter("UserId", DbType.Int32, user.Id);
            _objectDb.Received().AddInParameter("UpdateBy", DbType.Int32, resetPwdBy);
            _objectDb.Received().AddInParameter("Pwd", DbType.String, user.Password);
            _objectDb.Received().AddInParameter("UserSalt", DbType.String, salt);
            _objectDb.Received().AddInParameter("langkey", DbType.String, user.Request.Language);
            _objectDb.Received().AddInParameter("LoginIP", DbType.String, user.Request.Ip);
        }

        [Test]
        public void UpdateInfo_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_UserUpdAccount")).Returns(_objectDb);
            var user = new User
            {
                Id = 1,
                FirstName = "FirstName",
                LastName = "LastName",
                Status = "active",
                Licensee = new Licensee { Id = 1 },
                Password = "1234aa",
                Request = new Request { Ip = "::1", Language = "en-US" }
            };
            var updatedBy = 2;

            // Action
            _dataAccess.UpdateInfo(user, updatedBy);

            // Assert
            _objectDb.Received().AddInParameter("LicenseeID", DbType.Int32, user.Licensee.Id);
            _objectDb.Received().AddInParameter("UserId", DbType.Int32, user.Id);
            _objectDb.Received().AddInParameter("UpdateBy", DbType.Int32, updatedBy);
            _objectDb.Received().AddInParameter("FirstName", DbType.String, user.FirstName);
            _objectDb.Received().AddInParameter("LastName", DbType.String, user.LastName);
            _objectDb.Received().AddInParameter("Status", DbType.String, user.Status);
            _objectDb.Received().AddInParameter("langkey", DbType.String, user.Request.Language);
            _objectDb.Received().AddInParameter("LoginIP", DbType.String, user.Request.Ip);
            _objectDb.Received().AddOutParameter("ErrCode", DbType.Int32, 4);
            _objectDb.Received().AddOutParameter("ErrMsg", DbType.String, 4000);
        }

        [Test]
        public void ValidateInternalUser_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_InternalLogin")).Returns(_objectDb);
            var requestUser = new User
            {
                Username = "UserNameTest",
                Password = "1234aa",
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
            var licenseeName = "TestName";
            var timeStamp = TimeSpan.TicksPerMillisecond;
            IResultInfo result = new ResultInfo();

            // Action
            _dataAccess.ValidateInternalUser(requestUser, timeStamp, licenseeName, out result);

            // Assert
            _objectDb.Received().AddInParameter("UserName", DbType.String, requestUser.Username);
            _objectDb.Received().AddInParameter("EncryptedPassword", DbType.String, requestUser.Password);
            _objectDb.Received().AddInParameter("LicenseeName", DbType.String, licenseeName);
            _objectDb.Received().AddInParameter("LoginTime", DbType.DateTime, new DateTime(timeStamp));
            _objectDb.Received().AddInParameter("LoginIP", DbType.String, requestUser.Request.Ip);
            _objectDb.Received().AddInParameter("LoginDomain", DbType.String, requestUser.Request.Domain);
            _objectDb.Received().AddInParameter("BrowserName", DbType.String, requestUser.Request.Browser);
            _objectDb.Received().AddInParameter("Platform", DbType.String, requestUser.Request.Platform);
            _objectDb.Received().AddInParameter("CountryCode", DbType.String, requestUser.Request.CountryCode);
            _objectDb.Received().AddInParameter("Language", DbType.String, requestUser.Request.Language);
            _objectDb.Received().AddOutParameter("ServerID", DbType.Int32, 4);
            _objectDb.Received().AddOutParameter("ErrCode", DbType.Int32, 4);
            _objectDb.Received().AddOutParameter("ErrMsg", DbType.String, 4000);
        }

        [Test]
        public void ValidateUserForLrfSystem_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_UserLogin")).Returns(_objectDb);
            var requestUser = new User
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
            IResultInfo result = new ResultInfo();

            // Action
            _dataAccess.ValidateUserForLrfSystem(requestUser, out result);

            // Assert
            _objectDb.Received().AddInParameter("UserName", DbType.String, requestUser.Username);
            _objectDb.Received().AddInParameter("LoginIP", DbType.String, requestUser.Request.Ip);
            _objectDb.Received().AddInParameter("LoginDomain", DbType.String, requestUser.Request.Domain);
            _objectDb.Received().AddInParameter("BrowserName", DbType.String, requestUser.Request.Browser);
            _objectDb.Received().AddInParameter("Platform", DbType.String, requestUser.Request.Platform);
            _objectDb.Received().AddInParameter("CountryCode", DbType.String, requestUser.Request.CountryCode);
            _objectDb.Received().AddInParameter("langkey", DbType.String, requestUser.Request.Language);
            _objectDb.Received().AddOutParameter("ServerID", DbType.Int32, 4);
            _objectDb.Received().AddOutParameter("ErrCode", DbType.Int32, 4);
            _objectDb.Received().AddOutParameter("ErrMsg", DbType.String, 4000);
        }
    }
}