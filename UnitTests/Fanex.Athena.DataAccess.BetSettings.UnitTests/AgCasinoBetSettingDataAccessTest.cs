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
            ObjectDb.Received(1).SetParameterValue("CustID", custId);
            ObjectDb.Received(1).SetParameterValue("Currency", null);
            ObjectDb.Received(1).SetParameterValue("AGGroup", null);
            ObjectDb.Received(1).SetParameterValue("IsEnabled", null);
            ObjectDb.Received(1).SetParameterValue("IsLineEnabled", null);
            ObjectDb.Received(1).SetParameterValue("CheckedGroups", null);
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
            ObjectDb.Received(1).SetParameterValue("aggroup", group);
            ObjectDb.Received(1).SetParameterValue("ip", data.Ip);
            ObjectDb.Received(1).SetParameterValue("language", data.Language);
            ObjectDb.Received(1).SetParameterValue("userid", data.UserId);
            ObjectDb.Received(1).SetParameterValue("custid", data.CustId);
            ObjectDb.Received(1).SetParameterValue("ErrCode", null);
            ObjectDb.Received(1).SetParameterValue("ErrMsg", null);
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
            ObjectDb.Received(1).SetParameterValue("isenabled", isEnabled);
            ObjectDb.Received(1).SetParameterValue("ip", ip);
            ObjectDb.Received(1).SetParameterValue("language", language);
            ObjectDb.Received(1).SetParameterValue("userid", userId);
            ObjectDb.Received(1).SetParameterValue("custid", custId);

            ObjectDb.Received(1).SetParameterValue("ErrCode", null);
            ObjectDb.Received(1).SetParameterValue("ErrMsg", null);
        }
    }
}