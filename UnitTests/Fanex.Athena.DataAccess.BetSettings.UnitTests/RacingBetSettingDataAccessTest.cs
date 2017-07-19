namespace Fanex.Athena.DataAccess.BetSettings.UnitTests
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

        [OneTimeSetUp]
        public void SetUp()
        {
            _objectDb = Substitute.For<IObjectDb>();
            _dataAccess = Substitute.ForPartsOf<RacingBetSettingDataAccess>();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _objectDb = null;
            _dataAccess = null;
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

        [TestCase("151_31", "Lrf_CustomerUpdMember_MinMaxBet151_31")]
        [TestCase("151_41", "Lrf_CustomerUpdMember_MinMaxBet151_41")]
        [TestCase("151_51", "Lrf_CustomerUpdMember_MinMaxBet151_51")]
        [TestCase("152_41", "Lrf_CustomerUpdMember_MinMaxBet152_41")]
        [TestCase("152_51", "Lrf_CustomerUpdMember_MinMaxBet152_51")]
        [TestCase("153_51", "Lrf_CustomerUpdMember_MinMaxBet153_51")]
        public void UpdateBetSetting_WhenCallUpdate15131Method_ParametersPassedCorrectly(string key, string uspName)
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>(uspName)).Returns(_objectDb);
            var racingBetSetting = GetRacingBetSetting();
            var racingBetSettingList = new Dictionary<string, RacingBetSetting> { { key, racingBetSetting } };

            // Action
            _dataAccess.UpdateBetSetting(racingBetSettingList);

            // Assert
            _objectDb.Received().AddInParameter("custid", DbType.Int32, racingBetSetting.CustId);
            _objectDb.Received().AddInParameter("userid", DbType.Int32, racingBetSetting.UserId);
            _objectDb.Received().AddInParameter("minbet", DbType.Decimal, racingBetSetting.MinBet);
            _objectDb.Received().AddInParameter("maxbet", DbType.Decimal, racingBetSetting.MaxBet);
            _objectDb.Received().AddInParameter("maxpermatch", DbType.Decimal, racingBetSetting.MaxPerMatch);
            _objectDb.Received().AddInParameter("language", DbType.String, racingBetSetting.Language);
            _objectDb.Received().AddInParameter("ip", DbType.String, racingBetSetting.Ip);
            _objectDb.Received().AddOutParameter("ErrCode", DbType.Int32, 4);
            _objectDb.Received().AddOutParameter("ErrMsg", DbType.String, 4000);
        }

        [Test]
        public void IsAllowedSaToteCurrency_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_CheckHRAllowedCurrency")).Returns(_objectDb);
            var custId = 1;

            // Action
            _dataAccess.IsAllowedSaToteCurrency(custId);

            // Assert
            _objectDb.Received().AddInParameter("custid", DbType.Int32, custId);
            _objectDb.Received().AddOutParameter("allowed", DbType.Boolean, sizeof(bool));
        }

        [TestCase(true)]
        [TestCase(false)]
        public void IsAllowedSaToteCurrency_WhenCall_ParametersPassedCorrectly(bool expected)
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_CheckHRAllowedCurrency")).Returns(_objectDb);
            _objectDb.GetParameterValue("allowed").Returns(expected);
            var custId = 1;

            // Action
            var result = _dataAccess.IsAllowedSaToteCurrency(custId);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase(1, true)]
        [TestCase(2, false)]
        public void IsAllowedSaToteCurrency_WhenCall_ReturnCorrectValue(int custId, bool expected)
        {
            // Arrange
            _dataAccess.IsAllowedSaToteCurrency(custId).Returns(expected);

            // Action
            var result = _dataAccess.IsAllowedSaToteCurrency(custId);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
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
                MaxPerMatch = 30.5m,
                IsDataChange = true
            };
        }
    }
}