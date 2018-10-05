namespace Fanex.Athena.DataAccess.BetSettings.UnitTests
{
    using System.Data;
    using Core.UnitTests.TestHelpers;
    using Model.BetSettings.DTO;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class AgCasinoBetSettingDataAccessTest : BaseDataAccessHelperTest<AgCasinoBetSettingDataAccess>
    {
        [Test]
        public void GetBetSetting_WhenCall_ParameterPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is("Lrf_AGC_SelCustSetting")).Returns(ObjectDb);
            var custId = 1;

            // Action
            DataAccess.GetBetSetting(custId);

            // Assert
            ObjectDb.Received(1).AddInParameter("CustID", DbType.Int32, custId);
            ObjectDb.Received(1).AddOutParameter("Currency", DbType.Int32, int.MaxValue);
            ObjectDb.Received(1).AddOutParameter("AGGroup", DbType.String, 20);
            ObjectDb.Received(1).AddOutParameter("IsEnabled", DbType.Boolean, 1);
            ObjectDb.Received(1).AddOutParameter("IsLineEnabled", DbType.Boolean, 1);
            ObjectDb.Received(1).AddOutParameter("CheckedGroups", DbType.String, 500);
        }

        [Test]
        public void UpdateBetSetting_WhenCall_ParameterPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is("Lrf_AGC_UpdCustSetting")).Returns(ObjectDb);
            var group = "GroupTest";
            var data = new CommonBetSetting
            {
                Ip = "IP",
                Language = "LanguageTest",
                UserId = 1,
                CustId = 2
            };

            // Action
            DataAccess.UpdateBetSetting(data, group);

            // Assert
            ObjectDb.Received(1).AddInParameter("aggroup", DbType.String, group);
            ObjectDb.Received(1).AddInParameter("ip", DbType.String, data.Ip);
            ObjectDb.Received(1).AddInParameter("language", DbType.String, data.Language);
            ObjectDb.Received(1).AddInParameter("userid", DbType.Int32, data.UserId);
            ObjectDb.Received(1).AddInParameter("custid", DbType.Int32, data.CustId);
            ObjectDb.Received(1).AddOutParameter("ErrCode", DbType.String, 600);
            ObjectDb.Received(1).AddOutParameter("ErrMsg", DbType.String, 8000);
        }

        [Test]
        public void UpdateStatus_WhenCall_ParameterPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is("Lrf_AGC_UpdCustStatus")).Returns(ObjectDb);
            var userId = 1;
            var custId = 2;
            var language = "LanguageTest";
            var ip = "IP";
            var isEnabled = true;

            // Action
            DataAccess.UpdateStatus(userId, custId, language, ip, isEnabled);

            // Assert
            ObjectDb.Received(1).AddInParameter("isenabled", DbType.Boolean, isEnabled);
            ObjectDb.Received(1).AddInParameter("ip", DbType.String, ip);
            ObjectDb.Received(1).AddInParameter("language", DbType.String, language);
            ObjectDb.Received(1).AddInParameter("userid", DbType.Int32, userId);
            ObjectDb.Received(1).AddInParameter("custid", DbType.Int32, custId);

            ObjectDb.Received(1).AddOutParameter("ErrCode", DbType.String, 600);
            ObjectDb.Received(1).AddOutParameter("ErrMsg", DbType.String, 8000);
        }
    }
}