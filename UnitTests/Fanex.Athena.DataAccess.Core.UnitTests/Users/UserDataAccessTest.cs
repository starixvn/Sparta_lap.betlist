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

    public class UserDataAccessTest : BaseDataAccessHelperTest<UserDataAccess>
    {
        private const string ChangePasswordSp = "Licensee_UserPasswordUpd";
        private const string GetByLicenseeSp = "Lrf_UserAccessSel";
        private const string GetUserInfoSp = "Lrf_UserInfoSel";
        private const string RecheckInSp = "Lrf_ReCheckIn";
        private const string ResetPasswordSp = "Lrf_UserPwdReset";
        private const string UpdateInfoSp = "Lrf_UserUpdAccount";

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            DataAccess = Substitute.ForPartsOf<UserDataAccess>();
            ObjectDb = Substitute.For<IObjectDb>();
        }

        [Test]
        public void ChangePassword_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            const string OldPassword = "123456";
            const string NewPassword = "1234aa";
            const string Salt = "abcxyz";
            var requestUser = new User { Username = "test", Request = new Request { Ip = "::1" } };

            DataAccess.GetObjectDb(Arg.Is(ChangePasswordSp)).Returns(ObjectDb);

            // Action
            DataAccess.ChangePassword(requestUser, OldPassword, NewPassword, Salt);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received().AddInParameter("UserName", DbType.String, requestUser.Username);
                ObjectDb.Received().AddInParameter("OldPwd", DbType.String, OldPassword);
                ObjectDb.Received().AddInParameter("NewPwd", DbType.String, NewPassword);
                ObjectDb.Received().AddInParameter("LoginIP", DbType.String, requestUser.Request.Ip);
                ObjectDb.Received().AddInParameter("UserSalt", DbType.String, Salt);
            });
        }

        [Test]
        public void ChangePassword_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(ChangePasswordSp)).Returns(ObjectDb);
            ResultInfoHelper.MockResultInfo(ObjectDb);

            // Action
            var result = DataAccess.ChangePassword(new User { Request = new Request() }, string.Empty, string.Empty, string.Empty);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Code, Is.EqualTo(ResultInfoHelper.ResultCode));
                Assert.That(result.Message, Is.EqualTo(ResultInfoHelper.ResultMessage));
            });
        }

        [Test]
        public void GetByLicensee_WhenCall_ExecuteReaderMethodIsCalled()
        {
            // Arrange
            const int LicenseeId = 1;
            const bool IsInternal = true;

            DataAccess.GetObjectDb(Arg.Is(GetByLicenseeSp)).Returns(ObjectDb);

            // Action
            DataAccess.GetByLicensee(LicenseeId, IsInternal);

            // Assert
            ObjectDb.ReceivedWithAnyArgs().ExecuteReader(null);
        }

        [Test]
        public void GetByLicensee_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            const int LicenseeId = 1;
            const bool IsInternal = true;

            var expected = GetFakeUserData(LicenseeId);
            ConfigureDataReader(expected);

            DataAccess.GetObjectDb(Arg.Is(GetByLicenseeSp)).Returns(ObjectDb);

            // Action
            var result = DataAccess.GetByLicensee(LicenseeId, IsInternal);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result[0].UserID, Is.EqualTo(expected.UserID));
                Assert.That(result[0].Username, Is.EqualTo(expected.Username));
                Assert.That(result[0].FirstName, Is.EqualTo(expected.FirstName));
                Assert.That(result[0].LastName, Is.EqualTo(expected.LastName));
                Assert.That(result[0].Status, Is.EqualTo(expected.Status));
                Assert.That(result[0].Licensee.Id, Is.EqualTo(expected.Licensee.Id));
                Assert.That(result[0].IsManageAccount, Is.EqualTo(expected.IsManageAccount));
                Assert.That(result[0].IsInternal, Is.EqualTo(expected.IsInternal));
            });
        }

        [Test]
        public void GetByLicensee_WhenObjectDbThrowsException_ThrowDataAccessException()
        {
            TestException_WhenObjectDbThrowsException_ThrowDataAccessException(
                GetByLicenseeSp,
                () => DataAccess.GetByLicensee(1, true));
        }

        [TestCase(true, 1)]
        [TestCase(false, 1)]
        [TestCase(true, 0)]
        [TestCase(false, 0)]
        public void GetByUsername_WhenCall_QueryMethodIsCalled(bool isInternal, int licenseeId)
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is<string>("Lrf_UserInfoSelByUserName")).Returns(ObjectDb);
            var userName = "test";

            // Action
            DataAccess.GetByUsername(userName, isInternal, licenseeId);

            // Assert
            ObjectDb.ReceivedWithAnyArgs().Query<User>(null).FirstOrDefault();
        }

        [TestCase(true, 1)]
        [TestCase(false, 1)]
        [TestCase(true, 0)]
        [TestCase(false, 0)]
        public void GetByUsername_WhenCall_DataReturnedCorrectly(bool isInternal, int licenseeId)
        {
            // Arrange
            const string UserName = "test";

            var expected = GetFakeUserData();
            ObjectDb.Query<User>(Arg.Any<object>()).Returns(new List<User> { expected });

            DataAccess.GetObjectDb(Arg.Is("Lrf_UserInfoSelByUserName")).Returns(ObjectDb);

            // Action
            var result = DataAccess.GetByUsername(UserName, isInternal, licenseeId);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void GetUserInfo_WhenCall_ExecuteReaderMethodIsCalled()
        {
            // Arrange
            const int LicenseeId = 1;
            const int UserId = 2;

            DataAccess.GetObjectDb(Arg.Is(GetUserInfoSp)).Returns(ObjectDb);

            // Action
            DataAccess.GetUserInfo(LicenseeId, UserId);

            // Assert
            ObjectDb.ReceivedWithAnyArgs().ExecuteReader(null);
        }

        [Test]
        public void GetUserInfo_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            const int LicenseeId = 1;
            const int UserId = 2;
            var expected = GetFakeUserData(LicenseeId);
            ConfigureDataReader(expected);

            DataAccess.GetObjectDb(Arg.Is(GetUserInfoSp)).Returns(ObjectDb);

            // Action
            var result = DataAccess.GetUserInfo(LicenseeId, UserId);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.UserID, Is.EqualTo(expected.UserID));
                Assert.That(result.Username, Is.EqualTo(expected.Username));
                Assert.That(result.FirstName, Is.EqualTo(expected.FirstName));
                Assert.That(result.LastName, Is.EqualTo(expected.LastName));
                Assert.That(result.Status, Is.EqualTo(expected.Status));
                Assert.That(result.Licensee.Id, Is.EqualTo(expected.Licensee.Id));
                Assert.That(result.IsInternal, Is.EqualTo(expected.IsInternal));
            });
        }

        [Test]
        public void RecheckIn_WhenCall_ParameterPassesCorrectly()
        {
            // Arrange
            const int UserId = 2;

            DataAccess.GetObjectDb(Arg.Is(RecheckInSp), Arg.Any<int>()).Returns(ObjectDb);
            
            // Action
            var result = DataAccess.RecheckIn(UserId);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received().AddInParameter("On_Userid", DbType.Int32, UserId);
                ObjectDb.Received().AddOutParameter("IsKickedOut", DbType.Boolean, 0);
            });
        }

        [Test]
        public void RecheckIn_WhenCall_ReturnDataCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(RecheckInSp), Arg.Any<int>()).Returns(ObjectDb);
            ObjectDb.GetParameterValue("IsKickedOut").Returns(true);

            // Action
            var result = DataAccess.RecheckIn(0);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void ResetPassword_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            const int ResetPwdBy = 2;
            const string Salt = "abcxyz";
            var user = new User
            {
                UserID = 1,
                Licensee = new Licensee { Id = 1 },
                Password = "1234aa",
                Request = new Request { Ip = "::1", Language = "en-US" }
            };

            DataAccess.GetObjectDb(Arg.Is(ResetPasswordSp)).Returns(ObjectDb);

            // Action
            DataAccess.ResetPassword(user, Salt, ResetPwdBy);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received().AddInParameter("LicenseeID", DbType.Int32, user.Licensee.Id);
                ObjectDb.Received().AddInParameter("UserId", DbType.Int32, user.UserID);
                ObjectDb.Received().AddInParameter("UpdateBy", DbType.Int32, ResetPwdBy);
                ObjectDb.Received().AddInParameter("Pwd", DbType.String, user.Password);
                ObjectDb.Received().AddInParameter("UserSalt", DbType.String, Salt);
                ObjectDb.Received().AddInParameter("langkey", DbType.String, user.Request.Language);
                ObjectDb.Received().AddInParameter("LoginIP", DbType.String, user.Request.Ip);
            });
        }

        [Test]
        public void ResetPassword_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(ResetPasswordSp)).Returns(ObjectDb);
            ResultInfoHelper.MockResultInfo(ObjectDb);

            // Action
            var user = new User
            {
                Licensee = new Licensee(),
                Request = new Request()
            };
            var result = DataAccess.ResetPassword(user, string.Empty, 0);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Code, Is.EqualTo(ResultInfoHelper.ResultCode));
                Assert.That(result.Message, Is.EqualTo(ResultInfoHelper.ResultMessage));
            });
        }

        [Test]
        public void UpdateInfo_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            const int UpdatedBy = 2;
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

            DataAccess.GetObjectDb(Arg.Is(UpdateInfoSp)).Returns(ObjectDb);

            // Action
            DataAccess.UpdateInfo(user, UpdatedBy);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received().AddInParameter("LicenseeID", DbType.Int32, user.Licensee.Id);
                ObjectDb.Received().AddInParameter("UserId", DbType.Int32, user.UserID);
                ObjectDb.Received().AddInParameter("UpdateBy", DbType.Int32, UpdatedBy);
                ObjectDb.Received().AddInParameter("FirstName", DbType.String, user.FirstName);
                ObjectDb.Received().AddInParameter("LastName", DbType.String, user.LastName);
                ObjectDb.Received().AddInParameter("Status", DbType.String, user.Status);
                ObjectDb.Received().AddInParameter("langkey", DbType.String, user.Request.Language);
                ObjectDb.Received().AddInParameter("LoginIP", DbType.String, user.Request.Ip);
                ObjectDb.Received().AddOutParameter("ErrCode", DbType.Int32, 4);
                ObjectDb.Received().AddOutParameter("ErrMsg", DbType.String, 4000);
            });
        }

        [Test]
        public void UpdateInfo_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(UpdateInfoSp)).Returns(ObjectDb);
            var user = new User
            {
                Licensee = new Licensee(),
                Request = new Request()
            };
            var updatedBy = 2;
            ResultInfoHelper.MockResultInfo(ObjectDb);

            // Action
            var result = DataAccess.UpdateInfo(user, updatedBy);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Code, Is.EqualTo(ResultInfoHelper.ResultCode));
                Assert.That(result.Message, Is.EqualTo(ResultInfoHelper.ResultMessage));
            });
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
            MapReaderData(expected);
            Reader.Read().Returns(true, false);
        }
    }
}