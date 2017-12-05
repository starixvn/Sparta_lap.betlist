namespace Fanex.Athena.DataAccess.BetSettings.UnitTests
{
    using System.Data;
    using Data;
    using Model.BetSettings.MetaModel;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class CoreCasinoBetSettingDataAccessTest
    {
        [Test]
        public void UpdateCasinoStatus_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            var objectDb = Substitute.For<IObjectDb>();
            var dataAccess = Substitute.ForPartsOf<CoreCasinoBetSettingDataAccess>();
            dataAccess.GetObjectDb(Arg.Is("Lrf_CustomerUpdCasinoStatus")).Returns(objectDb);
            var objectDbWriteLog = Substitute.For<IObjectDb>();
            dataAccess.GetObjectDb(Arg.Is("WriteLog_UserLog_RunSP")).Returns(objectDbWriteLog);
            var metaModel = new CoreCasinoStatusMetaModel
            {
                UserId = 1,
                CustId = 2,
                Status = 0,
                Language = "LanguageTest",
                Ip = "::1"
            };
            var sportId = 3;

            // Action
            dataAccess.UpdateCasinoStatus(metaModel, sportId);

            // Assert
            objectDb.Received().AddInParameter("userid", DbType.Int32, metaModel.UserId);
            objectDb.Received().AddInParameter("custid", DbType.Int32, metaModel.CustId);
            objectDb.Received().AddInParameter("sportid", DbType.Int32, sportId);
            objectDb.Received().AddInParameter("language", DbType.String, metaModel.Language);
            objectDb.Received().AddInParameter("ip", DbType.String, metaModel.Ip);
            objectDb.Received().AddInParameter("status", DbType.Int32, metaModel.Status);
            objectDb.Received().AddOutParameter("ErrCode", DbType.String, 5);
            objectDb.Received().AddOutParameter("ErrMsg", DbType.String, 6000);
        }

        [Test]
        public void WriteLogUpdatingAction_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            var objectDb = Substitute.For<IObjectDb>();
            var dataAccess = Substitute.ForPartsOf<CoreCasinoBetSettingDataAccess>();
            dataAccess.GetObjectDb(Arg.Is<string>("Lrf_CustomerUpdCasinoStatus")).Returns(objectDb);
            var objectDbWriteLog = Substitute.For<IObjectDb>();
            dataAccess.GetObjectDb(Arg.Is<string>("WriteLog_UserLog_RunSP")).Returns(objectDbWriteLog);
            var metaModel = new CoreCasinoStatusMetaModel
            {
                UserId = 1,
                CustId = 2,
                Status = 0,
                Language = "LanguageTest",
                Ip = "::1"
            };
            var sportId = 3;

            // Action
            dataAccess.UpdateCasinoStatus(metaModel, sportId);

            // Assert
            objectDbWriteLog.Received().AddInParameter("UserID", DbType.Int32, metaModel.UserId);
            objectDbWriteLog.Received().AddInParameter("CustID", DbType.Int32, metaModel.CustId);
            objectDbWriteLog.Received().AddInParameter("SPName", DbType.String, "Lrf_CustomerUpdCasinoStatus");
            objectDbWriteLog.Received().AddInParameter("ActionID", DbType.Int32, 2037);
            objectDbWriteLog.Received().AddInParameter("Duration", DbType.Int32, 0);
        }
    }
}