namespace Fanex.Athena.DataAccess.Core.UnitTests.Users
{
    using System;
    using System.Data;
    using Core.Users;
    using Data;
    using Model.Core.DTO.Common;
    using Model.Core.DTO.Users;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class AuthDataAccessTest
    {
        private IObjectDb _objectDb;
        private AuthDataAccess _dataAccess;
        private IDataReader _reader;

        [OneTimeSetUp]
        public void Setup()
        {
            _objectDb = Substitute.For<IObjectDb>();
            _dataAccess = Substitute.ForPartsOf<AuthDataAccess>();
            _reader = Substitute.For<IDataReader>();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _objectDb = null;
            _dataAccess = null;
            _reader = null;
        }

        [Test]
        public void LoginOnUser_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_CheckIn")).Returns(_objectDb);
            var requestUser = new User { UserID = 1, Request = new Request { Ip = "::1" } };
            var netSessionId = "1a2b3c";
            var serverId = 2;

            // Action
            _dataAccess.LoginOnUser(requestUser, netSessionId, serverId);

            // Assert
            _objectDb.Received().AddInParameter("CustID", DbType.Int32, requestUser.UserID);
            _objectDb.Received().AddInParameter("NetSessionID", DbType.String, netSessionId);
            _objectDb.Received().AddInParameter("ServerID", DbType.Int32, serverId);
            _objectDb.Received().AddOutParameter("On_UserID", DbType.Int32, 4);
            _objectDb.Received().AddInParameter("IP", DbType.String, requestUser.Request.Ip);
        }

        [Test]
        public void LoginOnUser_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_CheckIn")).Returns(_objectDb);
            var requestUser = new User { UserID = 1, Request = new Request { Ip = "::1" } };
            var netSessionId = "1a2b3c";
            var serverId = 2;

            var expected = 10;
            _objectDb.GetParameterValue("On_UserID").Returns(expected);

            // Action
            var result = _dataAccess.LoginOnUser(requestUser, netSessionId, serverId);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
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

            // Action
            _dataAccess.ValidateInternalUser(requestUser, timeStamp, licenseeName);

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
        public void ValidateInternalUser_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_InternalLogin")).Returns(_objectDb);
            _objectDb.ExecuteReader().Returns(_reader);
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

            var expected = GetFakeUserData();
            expected.Licensee = new Licensee { Id = 1 };
            expected.ServerId = 12;
            expected.Email = "test@gmail.com";
            expected.Role = "RoleTest";
            ConfigureDataReader(expected);

            _reader.GetInt32(Arg.Any<int>()).Returns(expected.Licensee.Id);
            _reader["email"].Returns(expected.Email);
            _reader["userrole"].Returns(expected.Role);
            _objectDb.GetParameterValue("ServerId").Returns(expected.ServerId);

            // Action
            var user = _dataAccess.ValidateInternalUser(requestUser, timeStamp, licenseeName).ExtendedData;

            // Assert
            Assert.That(user.UserID, Is.EqualTo(expected.UserID));
            Assert.That(user.Username, Is.EqualTo(expected.Username));
            Assert.That(user.FirstName, Is.EqualTo(expected.FirstName));
            Assert.That(user.LastName, Is.EqualTo(expected.LastName));
            Assert.That(user.Email, Is.EqualTo(expected.Email));
            Assert.That(user.Role, Is.EqualTo(expected.Role));
            Assert.That(user.IsInternal, Is.EqualTo(expected.IsInternal));
            Assert.That(user.Licensee.Id, Is.EqualTo(expected.Licensee.Id));
            Assert.That(user.ServerId, Is.EqualTo(expected.ServerId));
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

            // Action
            _dataAccess.ValidateUserForLrfSystem(requestUser);

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

        [Test]
        public void ValidateUserForLrfSystem_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_UserLogin")).Returns(_objectDb);
            _objectDb.ExecuteReader().Returns(_reader);
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
            var expected = GetFakeUserData();
            expected.Licensee = new Licensee { Id = 1 };
            expected.ServerId = 12;
            expected.Email = "test@gmail.com";
            expected.Role = "RoleTest";
            ConfigureDataReader(expected);

            _reader.GetInt32(Arg.Any<int>()).Returns(expected.Licensee.Id);
            _reader["email"].Returns(expected.Email);
            _reader["userrole"].Returns(expected.Role);
            _objectDb.GetParameterValue("ServerId").Returns(expected.ServerId);

            // Action
            var result = _dataAccess.ValidateUserForLrfSystem(requestUser);
            var user = result.ExtendedData;

            // Assert
            Assert.That(user.UserID, Is.EqualTo(expected.UserID));
            Assert.That(user.Username, Is.EqualTo(expected.Username));
            Assert.That(user.FirstName, Is.EqualTo(expected.FirstName));
            Assert.That(user.LastName, Is.EqualTo(expected.LastName));
            Assert.That(user.Email, Is.EqualTo(expected.Email));
            Assert.That(user.Role, Is.EqualTo(expected.Role));
            Assert.That(user.IsInternal, Is.EqualTo(expected.IsInternal));
            Assert.That(user.Licensee.Id, Is.EqualTo(expected.Licensee.Id));
            Assert.That(user.ServerId, Is.EqualTo(expected.ServerId));
        }

        private User GetFakeUserData(int licenseeId = 1)
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
                IsInternal = true
            };
        }

        private void ConfigureDataReader(User expected)
        {
            _reader.Read().Returns(true, false);
            _reader["UserID"].Returns(expected.UserID);
            _reader["username"].Returns(expected.Username);
            _reader["firstname"].Returns(expected.FirstName);
            _reader["lastname"].Returns(expected.LastName);
            _reader["status"].Returns(expected.Status);
            _reader["ismanageaccount"].Returns(expected.IsManageAccount);
            _reader["isinternal"].Returns(expected.IsInternal);
        }
    }
}