namespace Fanex.Athena.DataAccess.BetSettings.UnitTests
{
    using System.Data;
    using ClassTest;
    using Data;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class CasinoBettingBaseDataAccessTest
    {
        [Test]
        public void UpdateCasinoStatus_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            var objectDb = Substitute.For<IObjectDb>();
            var dataAccess = Substitute.ForPartsOf<FakeCasinoBettingBaseDataAccess>();
            dataAccess.GetObjectDb(Arg.Is<string>("Lrf_CustomerUpdCasinoStatus")).Returns(objectDb);
            var objectDbWriteLog = Substitute.For<IObjectDb>();
            dataAccess.GetObjectDb(Arg.Is<string>("WriteLog_UserLog_RunSP")).Returns(objectDbWriteLog);
            var userId = 1;
            var custId = 2;
            var sportId = 3;
            var status = 0;
            var language = "LanguageTest";
            var ip = "::1";

            // Action
            dataAccess.UpdateCasinoStatus(userId, custId, sportId, status, language, ip);

            // Assert
            objectDb.Received().AddInParameter("userid", DbType.Int32, userId);
            objectDb.Received().AddInParameter("custid", DbType.Int32, custId);
            objectDb.Received().AddInParameter("sportid", DbType.Int32, sportId);
            objectDb.Received().AddInParameter("language", DbType.String, language);
            objectDb.Received().AddInParameter("ip", DbType.String, ip);
            objectDb.Received().AddInParameter("status", DbType.Int32, status);
            objectDb.Received().AddOutParameter("ErrCode", DbType.String, 5);
            objectDb.Received().AddOutParameter("ErrMsg", DbType.String, 6000);
        }

        [Test]
        public void WriteLogUpdatingAction_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            var objectDb = Substitute.For<IObjectDb>();
            var dataAccess = Substitute.ForPartsOf<FakeCasinoBettingBaseDataAccess>();
            dataAccess.GetObjectDb(Arg.Is<string>("Lrf_CustomerUpdCasinoStatus")).Returns(objectDb);
            var objectDbWriteLog = Substitute.For<IObjectDb>();
            dataAccess.GetObjectDb(Arg.Is<string>("WriteLog_UserLog_RunSP")).Returns(objectDbWriteLog);
            var userId = 1;
            var custId = 2;
            var sportId = 3;
            var status = 0;
            var language = "LanguageTest";
            var ip = "::1";

            // Action
            dataAccess.UpdateCasinoStatus(userId, custId, sportId, status, language, ip);

            // Assert
            objectDbWriteLog.Received().AddInParameter("UserID", DbType.Int32, userId);
            objectDbWriteLog.Received().AddInParameter("CustID", DbType.Int32, custId);
            objectDbWriteLog.Received().AddInParameter("SPName", DbType.String, "Lrf_CustomerUpdCasinoStatus");
            objectDbWriteLog.Received().AddInParameter("ActionID", DbType.Int32, 2037);
            objectDbWriteLog.Received().AddInParameter("Duration", DbType.Int32, 0);
        }
    }
}