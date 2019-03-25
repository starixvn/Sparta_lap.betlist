namespace Fanex.Athena.DataAccess.Core.UnitTests.Common
{
    using System.Data;
    using Core.Common;
    using Fanex.Athena.DataAccess.Core.UnitTests.TestHelpers;
    using Model.Core.MetaModel.Common;
    using NSubstitute;
    using NUnit.Framework;
    using UnitTest.Common.Helpers;

    public class UserLogDataAccessTest : BaseDataAccessHelperTest<UserLogDataAccess>
    {
        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();

            DataAccess.GetObjectDb(Arg.Is("WriteLog_UserLog_RunSP")).Returns(ObjectDb);
        }

        [Test]
        public void WriteLog_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            var metaModel = FakeHelper.Create<UserLogMetaModel>();

            // Action
            DataAccess.WriteLog(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received(1).SetParameterValue("UserID", metaModel.UserId);
                ObjectDb.Received(1).SetParameterValue("CustID", metaModel.CustID);
                ObjectDb.Received(1).SetParameterValue("SPName", metaModel.SPName);
                ObjectDb.Received(1).SetParameterValue("ActionID", metaModel.ActionID);
                ObjectDb.Received(1).SetParameterValue("Doing", metaModel.Doing);
                ObjectDb.Received(1).SetParameterValue("TrackingValue", metaModel.TrackingValue1);
                ObjectDb.Received(1).SetParameterValue("TrackingValue2", metaModel.TrackingValue2);
                ObjectDb.Received(1).SetParameterValue("Duration", 0);
            });
        }

        [Test]
        public void WriteLog_WhenCall_ExecuteNonQuery()
        {
            // Arrange
            var metaModel = new UserLogMetaModel();

            // Action
            DataAccess.WriteLog(metaModel);

            // Assert
            ObjectDb.Received(1).ExecuteNonQuery();
        }
    }
}