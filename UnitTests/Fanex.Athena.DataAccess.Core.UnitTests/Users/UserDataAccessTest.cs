namespace Fanex.Athena.DataAccess.Core.UnitTests.Users
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Core.Users;
    using Data;
    using Logging;
    using Model.Core.DTO.Common;
    using Model.Core.DTO.Users;
    using NSubstitute;
    using NUnit.Framework;
    using TestHelpers;

    [TestFixture]
    public class UserDataAccessTest
    {
        private ILogger _logger;
        private IObjectDb _objectDb;
        private UserDataAccess _dataAccess;
        private IDataReader _reader;

        [OneTimeSetUp]
        public void Setup()
        {
            _logger = Substitute.For<ILogger>();
            _objectDb = Substitute.For<IObjectDb>();
            _dataAccess = Substitute.ForPartsOf<UserDataAccess>(_logger);
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
        public void ChangePassword_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Licensee_UserPasswordUpd")).Returns(_objectDb);
            var requestUser = new User { Username = "test", Request = new Request { Ip = "::1" } };
            var oldPassword = "123456";
            var newPassword = "1234aa";
            var salt = "abcxyz";
            ResultInfoHelper.MockResultInfo(_objectDb);

            // Action
            var result = _dataAccess.ChangePassword(requestUser, oldPassword, newPassword, salt);

            // Assert
            Assert.That(result.Code, Is.EqualTo(ResultInfoHelper.ResultCode));
            Assert.That(result.Message, Is.EqualTo(ResultInfoHelper.ResultMessage));
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
        public void GetByLicensee_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_UserAccessSel")).Returns(_objectDb);
            _objectDb.ExecuteReader(Arg.Any<object>()).Returns(_reader);
            var licenseeId = 1;
            var isInternal = true;
            var expected = GetFakeUserData(licenseeId);
            ConfigureDataReader(expected);

            // Action
            var result = _dataAccess.GetByLicensee(licenseeId, isInternal);

            // Assert
            Assert.That(result[0].UserID, Is.EqualTo(expected.UserID));
            Assert.That(result[0].Username, Is.EqualTo(expected.Username));
            Assert.That(result[0].FirstName, Is.EqualTo(expected.FirstName));
            Assert.That(result[0].LastName, Is.EqualTo(expected.LastName));
            Assert.That(result[0].Status, Is.EqualTo(expected.Status));
            Assert.That(result[0].Licensee.Id, Is.EqualTo(expected.Licensee.Id));
            Assert.That(result[0].IsManageAccount, Is.EqualTo(expected.IsManageAccount));
            Assert.That(result[0].IsInternal, Is.EqualTo(expected.IsInternal));
        }

        [TestCase(true, 1)]
        [TestCase(false, 1)]
        [TestCase(true, 0)]
        [TestCase(false, 0)]
        public void GetByUsername_WhenCall_QueryMethodIsCalled(bool isInternal, int licenseeId)
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_UserInfoSelByUserName")).Returns(_objectDb);
            var userName = "test";

            // Action
            _dataAccess.GetByUsername(userName, isInternal, licenseeId);

            // Assert
            _objectDb.ReceivedWithAnyArgs().Query<User>(null).FirstOrDefault();
        }

        [TestCase(true, 1)]
        [TestCase(false, 1)]
        [TestCase(true, 0)]
        [TestCase(false, 0)]
        public void GetByUsername_WhenCall_DataReturnedCorrectly(bool isInternal, int licenseeId)
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_UserInfoSelByUserName")).Returns(_objectDb);
            var userName = "test";
            var expected = GetFakeUserData();
            _objectDb.Query<User>(Arg.Any<object>()).Returns(new List<User> { expected });

            // Action
            var result = _dataAccess.GetByUsername(userName, isInternal, licenseeId);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
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
        public void GetUserInfo_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_UserInfoSel")).Returns(_objectDb);
            _objectDb.ExecuteReader(Arg.Any<object>()).Returns(_reader);

            var licenseeId = 1;
            var userId = 2;
            var expected = GetFakeUserData(licenseeId);
            ConfigureDataReader(expected);
            _reader["userid"].Returns(expected.UserID);

            // Action
            var result = _dataAccess.GetUserInfo(licenseeId, userId);

            // Assert
            Assert.That(result.UserID, Is.EqualTo(expected.UserID));
            Assert.That(result.Username, Is.EqualTo(expected.Username));
            Assert.That(result.FirstName, Is.EqualTo(expected.FirstName));
            Assert.That(result.LastName, Is.EqualTo(expected.LastName));
            Assert.That(result.Status, Is.EqualTo(expected.Status));
            Assert.That(result.Licensee.Id, Is.EqualTo(expected.Licensee.Id));
            Assert.That(result.IsInternal, Is.EqualTo(expected.IsInternal));
        }

        [Test]
        public void RecheckIn_WhenCall_ParameterPassesCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is("Lrf_ReCheckIn"), Arg.Any<int>()).Returns(_objectDb);
            const int UserId = 2;

            // Action
            var result = _dataAccess.RecheckIn(UserId);

            // Assert
            Assert.Multiple(() =>
            {
                _objectDb.Received().AddInParameter("On_Userid", DbType.Int32, UserId);
                _objectDb.Received().AddOutParameter("IsKickedOut", DbType.Boolean, 0);
            });
        }

        [Test]
        public void RecheckIn_WhenCall_ReturnDataCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is("Lrf_ReCheckIn"), Arg.Any<int>()).Returns(_objectDb);
            _objectDb.GetParameterValue("IsKickedOut").Returns(true);

            // Action
            var result = _dataAccess.RecheckIn(0);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void ResetPassword_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is("Lrf_UserPwdReset")).Returns(_objectDb);
            var user = new User
            {
                UserID = 1,
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
            _objectDb.Received().AddInParameter("UserId", DbType.Int32, user.UserID);
            _objectDb.Received().AddInParameter("UpdateBy", DbType.Int32, resetPwdBy);
            _objectDb.Received().AddInParameter("Pwd", DbType.String, user.Password);
            _objectDb.Received().AddInParameter("UserSalt", DbType.String, salt);
            _objectDb.Received().AddInParameter("langkey", DbType.String, user.Request.Language);
            _objectDb.Received().AddInParameter("LoginIP", DbType.String, user.Request.Ip);
        }

        [Test]
        public void ResetPassword_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is("Lrf_UserPwdReset")).Returns(_objectDb);
            var user = new User
            {
                UserID = 1,
                Licensee = new Licensee { Id = 1 },
                Password = "1234aa",
                Request = new Request { Ip = "::1", Language = "en-US" }
            };
            var resetPwdBy = 2;
            var salt = "abcxyz";
            ResultInfoHelper.MockResultInfo(_objectDb);

            // Action
            var result = _dataAccess.ResetPassword(user, salt, resetPwdBy);

            // Assert
            Assert.That(result.Code, Is.EqualTo(ResultInfoHelper.ResultCode));
            Assert.That(result.Message, Is.EqualTo(ResultInfoHelper.ResultMessage));
        }

        [Test]
        public void SyncUserInfo_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            const string LicenseeName = "spondemo";
            const string UserName = "user1";
            _dataAccess.GetObjectDb(Arg.Is("Lrf_SyncInternalUser")).Returns(_objectDb);

            // Action
            _dataAccess.SyncUserInfo(LicenseeName, UserName);

            // Assert
            Assert.Multiple(() =>
            {
                _objectDb.Received().AddInParameter("LicenseeName", DbType.String, LicenseeName);
                _objectDb.Received().AddInParameter("Username", DbType.String, UserName);
                _objectDb.Received().AddInParameter("EncryptedPassword", DbType.String, Arg.Any<string>());
                _objectDb.Received().AddOutParameter("ErrCode", DbType.Int32, 4);
                _objectDb.Received().AddOutParameter("ErrMsg", DbType.String, 4000);
                _logger.Received().Info($"Synced user from LAP system - userName={UserName}");
            });
        }

        [Test]
        public void SyncUserInfo_WhenCall_ReturnDataCorrectly()
        {
            // Arrange
            const int ErrCode = 1;
            const string ErrMsg = "Error";
            _dataAccess.GetObjectDb(Arg.Is("Lrf_SyncInternalUser")).Returns(_objectDb);
            _objectDb.GetParameterValue("ErrCode").Returns(ErrCode);
            _objectDb.GetParameterValue("ErrMsg").Returns(ErrMsg);

            // Action
            var result = _dataAccess.SyncUserInfo(null, null);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Code, Is.EqualTo(ErrCode));
                Assert.That(result.Message, Is.EqualTo(ErrMsg));
            });
        }

        [Test]
        public void UpdateInfo_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is("Lrf_UserUpdAccount")).Returns(_objectDb);
            var user = new User
            {
                UserID = 1,
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
            _objectDb.Received().AddInParameter("UserId", DbType.Int32, user.UserID);
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
        public void UpdateInfo_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_UserUpdAccount")).Returns(_objectDb);
            var user = new User
            {
                UserID = 1,
                FirstName = "FirstName",
                LastName = "LastName",
                Status = "active",
                Licensee = new Licensee { Id = 1 },
                Password = "1234aa",
                Request = new Request { Ip = "::1", Language = "en-US" }
            };
            var updatedBy = 2;
            ResultInfoHelper.MockResultInfo(_objectDb);

            // Action
            var result = _dataAccess.UpdateInfo(user, updatedBy);

            // Assert
            Assert.That(result.Code, Is.EqualTo(ResultInfoHelper.ResultCode));
            Assert.That(result.Message, Is.EqualTo(ResultInfoHelper.ResultMessage));
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