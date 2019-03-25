namespace Fanex.Athena.DataAccess.BetSettings.UnitTests
{
    using System.Data;
    using Core.UnitTests.TestHelpers;
    using Model.BetSettings.DTO;
    using Model.Core.DTO.Common;
    using NSubstitute;
    using NUnit.Framework;
    using UnitTest.Common.Helpers;
    using VendorAdapter.Interfaces;

    [TestFixture]
    public class KenoBetSettingDataAccessTest : BaseDataAccessHelperTest<KenoBetSettingDataAccess>
    {
        protected override void SetUpDataAccess()
        {
            DataAccess = Substitute.ForPartsOf<KenoBetSettingDataAccess>(
                Substitute.For<ICacheRefAdapter>());
        }

        [Test]
        public void GetBetSettings_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is("Lrf_CustomerSelEditKN")).Returns(ObjectDb);
            var custId = 1;

            // Action
            DataAccess.GetBetSettings(custId);

            // Assert
            ObjectDb.Received(1).SetParameterValue("CustID", custId);
        }

        [Test]
        public void GetBetSettings_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is("Lrf_CustomerSelEditKN")).Returns(ObjectDb);
            var reader = Substitute.For<IDataReader>();
            var custId = 1;
            var expected = FakeHelper.Create<BetSetting>();
            ConfigureReaderForReadBetSettingMethod(reader, expected);

            // Action
            var result = DataAccess.GetBetSettings(custId)[0];

            // Assert
            Assert.That(result.MaxBet, Is.EqualTo(expected.MaxBet));
            Assert.That(result.MinBet, Is.EqualTo(expected.MinBet));
            Assert.That(result.MaxPerMatchMax, Is.EqualTo(expected.MaxPerMatchMax));
            Assert.That(result.MaxPerMatch, Is.EqualTo(expected.MaxPerMatch));
            Assert.That(result.BetValueMax, Is.EqualTo(expected.BetValueMax));
            Assert.That(result.BetValueMin, Is.EqualTo(expected.BetValueMin));
            Assert.That(result.SportId, Is.EqualTo(expected.SportId));
        }

        [Test]
        public void UpdateBetSetting_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is("Lrf_CustomerUpdMember_MinMaxBetKN")).Returns(ObjectDb);
            var custId = 1;
            var userId = 2;
            var language = "LanguageTest";
            var ip = "IP";
            var xmlData = "XMLDataTest";

            // Action
            DataAccess.UpdateBetSetting(custId, userId, language, ip, xmlData);

            // Assert
            ObjectDb.Received(1).SetParameterValue("custid", custId);
            ObjectDb.Received(1).SetParameterValue("userid", userId);
            ObjectDb.Received(1).SetParameterValue("language", language);
            ObjectDb.Received(1).SetParameterValue("ip", ip);
            ObjectDb.Received(1).SetParameterValue("dataxml", xmlData);
            ObjectDb.Received(1).SetParameterValue("errcode", null);
            ObjectDb.Received(1).SetParameterValue("errmsg", null);
        }

        [Test]
        public void UpdateBetSetting_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is("Lrf_CustomerUpdMember_MinMaxBetKN")).Returns(ObjectDb);
            var custId = 1;
            var userId = 2;
            var language = "LanguageTest";
            var ip = "IP";
            var xmlData = "XMLDataTest";
            var expected = new ResultInfo
            {
                Code = 100,
                Message = "Success"
            };
            ObjectDb.GetParameterValue("ErrCode").Returns(expected.Code);
            ObjectDb.GetParameterValue("ErrMsg").Returns(expected.Message);

            // Action
            var result = DataAccess.UpdateBetSetting(custId, userId, language, ip, xmlData);

            // Assert
            Assert.That(result.Code, Is.EqualTo(expected.Code));
            Assert.That(result.Message, Is.EqualTo(expected.Message));
        }

        private void ConfigureReaderForReadBetSettingMethod(IDataReader reader, BetSetting expected)
        {
            ObjectDb.ExecuteReader().Returns(reader);
            reader.Read().Returns(true, false);

            reader["GameTypeID"].Returns(expected.SportId);
            reader["MaxBet"].Returns(expected.MaxBet);
            reader["MinBet"].Returns(expected.MinBet);
            reader["MaxPerMatch_Max"].Returns(expected.MaxPerMatchMax);
            reader["MaxPerMatch"].Returns(expected.MaxPerMatch);
            reader["MaxBet_Min"].Returns(expected.BetValueMax);
            reader["MinBet_Max"].Returns(expected.BetValueMin);
            reader["IBCBetTypeID"].Returns(1);
        }
    }
}