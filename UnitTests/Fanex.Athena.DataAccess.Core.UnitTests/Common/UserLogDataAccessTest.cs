namespace Fanex.Athena.DataAccess.Core.UnitTests.Common
{
    using System.Data;
    using Core.Common;
    using Fanex.Athena.DataAccess.Core.UnitTests.TestHelpers;
    using Model.Core.MetaModel.Common;
    using NSubstitute;
    using NUnit.Framework;

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
            var metaModel = new UserLogMetaModel
            {
                UserId = 1,
                CustID = 2,
                SPName = "SpName",
                ActionID = 3,
                Doing = "Doing",
                TrackingValue1 = "V1",
                TrackingValue2 = "V2"
            };

            // Action
            DataAccess.WriteLog(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received().AddInParameter("UserID", DbType.Int32, metaModel.UserId);
                ObjectDb.Received().AddInParameter("CustID", DbType.Int32, metaModel.CustID);
                ObjectDb.Received().AddInParameter("SPName", DbType.String, metaModel.SPName);
                ObjectDb.Received().AddInParameter("ActionID", DbType.Int32, metaModel.ActionID);
                ObjectDb.Received().AddInParameter("Doing", DbType.String, metaModel.Doing);
                ObjectDb.Received().AddInParameter("TrackingValue", DbType.String, metaModel.TrackingValue1);
                ObjectDb.Received().AddInParameter("TrackingValue2", DbType.String, metaModel.TrackingValue2);
                ObjectDb.Received().AddInParameter("Duration", DbType.Int32, 0);
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
            ObjectDb.Received().ExecuteNonQuery();
        }
    }
}