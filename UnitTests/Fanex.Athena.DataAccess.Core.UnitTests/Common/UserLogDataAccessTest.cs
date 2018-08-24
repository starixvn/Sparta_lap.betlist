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
                ObjectDb.Received(1).AddInParameter("UserID", DbType.Int32, metaModel.UserId);
                ObjectDb.Received(1).AddInParameter("CustID", DbType.Int32, metaModel.CustID);
                ObjectDb.Received(1).AddInParameter("SPName", DbType.String, metaModel.SPName);
                ObjectDb.Received(1).AddInParameter("ActionID", DbType.Int32, metaModel.ActionID);
                ObjectDb.Received(1).AddInParameter("Doing", DbType.String, metaModel.Doing);
                ObjectDb.Received(1).AddInParameter("TrackingValue", DbType.String, metaModel.TrackingValue1);
                ObjectDb.Received(1).AddInParameter("TrackingValue2", DbType.String, metaModel.TrackingValue2);
                ObjectDb.Received(1).AddInParameter("Duration", DbType.Int32, 0);
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