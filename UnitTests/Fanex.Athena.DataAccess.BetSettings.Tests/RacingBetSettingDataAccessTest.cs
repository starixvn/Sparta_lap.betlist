namespace Fanex.Athena.DataAccess.BetSettings.Tests
{
    using System.Collections.Generic;
    using System.Data;
    using Data;
    using Model.BetSettings.DTO;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class RacingBetSettingDataAccessTest
    {
        private IObjectDb _objectDb;
        private RacingBetSettingDataAccess _dataAccess;

        [SetUp]
        public void SetUp()
        {
            _objectDb = Substitute.For<IObjectDb>();
            _dataAccess = Substitute.ForPartsOf<RacingBetSettingDataAccess>();
        }

        [Test]
        public void GetBetSetting_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_CustomerSelEditRB")).Returns(_objectDb);
            var custId = 1;
            var isAllowSaToreCurrency = true;

            // Action
            _dataAccess.GetBetSetting(custId, isAllowSaToreCurrency);

            // Assert
            _objectDb.Received().AddInParameter("CustID", DbType.Int32, custId);
        }

        [Test]
        [Ignore("Cannot call 'this.GetObjectDb' in static method")]
        public void UpdateBetSetting_WhenCallUpdate15131Method_ParametersPassedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_CustomerUpdMember_MinMaxBet151_31")).Returns(_objectDb);
            var racingBetSetting = GetRacingBetSetting();
            var racingBetSettingList = new Dictionary<string, RacingBetSetting> { { "151_31", racingBetSetting } };

            // Action
            _dataAccess.UpdateBetSetting(racingBetSettingList);

            // Assert
        }

        private RacingBetSetting GetRacingBetSetting()
        {
            var custId = 1;
            var commonBetSetting = new CommonBetSetting
            {
                UserId = 2,
                Ip = "::1",
                Language = "LanguageTest"
            };

            return new RacingBetSetting(custId, commonBetSetting)
            {
                MinBet = 10m,
                MaxBet = 100m,
                MaxPerMatch = 30.5m
            };
        }
    }
}