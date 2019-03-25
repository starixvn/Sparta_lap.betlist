namespace Fanex.Athena.DataAccess.Core.UnitTests.Users
{
    using System.Data;
    using Core.Users;
    using NSubstitute;
    using NUnit.Framework;
    using TestHelpers;

    [TestFixture]
    public class OneAuthDataAccessTest : BaseDataAccessHelperTest<OneAuthDataAccess>
    {
        private const int UserSEQ = 123;

        private const string VendorId = "sdsldfkjlsdkjfl";

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            DataAccess.GetObjectDb(Arg.Is("Lrf_GetUserLevelInfo")).Returns(ObjectDb);
        }

        [Test]
        public void GetUserInfo_WhenCall_ParametersPassedCorrectly()
        {
            // Action
            DataAccess.GetUserInfo(UserSEQ, VendorId);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received(1).SetParameterValue("VendorID", VendorId);
                ObjectDb.Received(1).SetParameterValue("UserSEQ", UserSEQ);
                ObjectDb.Received(1).SetParameterValue("UserLevel", null);
                ObjectDb.Received(1).SetParameterValue("SubsidiaryEnabled", null);
                ObjectDb.Received(1).SetParameterValue("OperatorID", null);
            });
        }

        [Test]
        public void LoginOnUser_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            const int ExpectedUserLevel = 10;
            const bool ExpectedSubsidiaryEnabled = true;
            const string ExpectedOperatorID = "s09df";
            ObjectDb.GetParameterValue("UserLevel").Returns(ExpectedUserLevel);
            ObjectDb.GetParameterValue("SubsidiaryEnabled").Returns(ExpectedSubsidiaryEnabled);
            ObjectDb.GetParameterValue("OperatorID").Returns(ExpectedOperatorID);

            // Action
            var result = DataAccess.GetUserInfo(UserSEQ, VendorId);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.UserLevel, Is.EqualTo(ExpectedUserLevel));
                Assert.That(result.IsEnabledSubsidiary, Is.EqualTo(ExpectedSubsidiaryEnabled));
                Assert.That(result.OperatorID, Is.EqualTo(ExpectedOperatorID));
            });
        }
    }
}