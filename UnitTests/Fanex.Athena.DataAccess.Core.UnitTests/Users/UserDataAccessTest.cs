namespace Fanex.Athena.DataAccess.Core.UnitTests.Users
{
    using System.Collections.Generic;
    using System.Data;
    using Core.Users;
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

        [Test]
        public void ChangePassword_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            const string OldPassword = "123456";
            const string NewPassword = "1234aa";
            const string Salt = "abcxyz";
            var requestUser = new User { Username = "test", Request = new Request { Ip = "IP" } };

            DataAccess.GetObjectDb(Arg.Is(ChangePasswordSp)).Returns(ObjectDb);

            // Action
            DataAccess.ChangePassword(requestUser, OldPassword, NewPassword, Salt);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received(1).SetParameterValue("UserName", requestUser.Username);
                ObjectDb.Received(1).SetParameterValue("OldPwd", OldPassword);
                ObjectDb.Received(1).SetParameterValue("NewPwd", NewPassword);
                ObjectDb.Received(1).SetParameterValue("LoginIP", requestUser.Request.Ip);
                ObjectDb.Received(1).SetParameterValue("UserSalt", Salt);
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
            const int LicenseeId = 2;
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

        [TestCase(true, 1)]
        [TestCase(false, 1)]
        [TestCase(true, 0)]
        [TestCase(false, 0)]
        public void GetByUsername_WhenCall_QueryMethodIsCalled(bool isInternal, int licenseeId)
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is("Lrf_UserInfoSelByUserName")).Returns(ObjectDb);
            const string UserName = "test";

            // Action
            DataAccess.GetByUsername(UserName, isInternal, licenseeId);

            // Assert
            ObjectDb.Received(1).Query<User>(Arg.Any<dynamic>());
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
            const int LicenseeId = 3;
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
            DataAccess.RecheckIn(UserId);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received(1).SetParameterValue("On_Userid", UserId);
                ObjectDb.Received(1).SetParameterValue("IsKickedOut", null);
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
                Request = new Request { Ip = "IP", Language = "en-US" }
            };

            DataAccess.GetObjectDb(Arg.Is(ResetPasswordSp)).Returns(ObjectDb);

            // Action
            DataAccess.ResetPassword(user, Salt, ResetPwdBy);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received(1).SetParameterValue("LicenseeID", user.Licensee.Id);
                ObjectDb.Received(1).SetParameterValue("UserId", user.UserID);
                ObjectDb.Received(1).SetParameterValue("UpdateBy", ResetPwdBy);
                ObjectDb.Received(1).SetParameterValue("Pwd", user.Password);
                ObjectDb.Received(1).SetParameterValue("UserSalt", Salt);
                ObjectDb.Received(1).SetParameterValue("langkey", user.Request.Language);
                ObjectDb.Received(1).SetParameterValue("LoginIP", user.Request.Ip);
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
                Request = new Request { Ip = "IP", Language = "en-US" }
            };

            DataAccess.GetObjectDb(Arg.Is(UpdateInfoSp)).Returns(ObjectDb);

            // Action
            DataAccess.UpdateInfo(user, UpdatedBy);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received(1).SetParameterValue("LicenseeID", user.Licensee.Id);
                ObjectDb.Received(1).SetParameterValue("UserId", user.UserID);
                ObjectDb.Received(1).SetParameterValue("UpdateBy", UpdatedBy);
                ObjectDb.Received(1).SetParameterValue("FirstName", user.FirstName);
                ObjectDb.Received(1).SetParameterValue("LastName", user.LastName);
                ObjectDb.Received(1).SetParameterValue("Status", user.Status);
                ObjectDb.Received(1).SetParameterValue("langkey", user.Request.Language);
                ObjectDb.Received(1).SetParameterValue("LoginIP", user.Request.Ip);
                ObjectDb.Received(1).SetParameterValue("ErrCode", null);
                ObjectDb.Received(1).SetParameterValue("ErrMsg", null);
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