namespace Fanex.Athena.DataAccess.BetSettings.Tests
{
    using System.Data;
    using Data;
    using Model.BetSettings.DTO;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class LiveCasinoBetSettingDataAccessTest
    {
        private IObjectDb _objectDb;
        private LiveCasinoBetSettingDataAccess _dataAccess;

        [SetUp]
        public void SetUp()
        {
            _objectDb = Substitute.For<IObjectDb>();
            _dataAccess = Substitute.ForPartsOf<LiveCasinoBetSettingDataAccess>();
        }

        [Test]
        public void GetBetSetting_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("TP_Admin_StakeLevel_GetCustStakeLevelMulti")).Returns(_objectDb);
            var custId = 1;
            var language = "LanguageTest";

            // Action
            _dataAccess.GetBetSetting(custId, language);

            // Assert
            _objectDb.Received().AddInParameter("custid", DbType.Int32, custId);
        }

        [Test]
        public void GetBetSetting_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("TP_Admin_StakeLevel_GetCustStakeLevelMulti")).Returns(_objectDb);
            var custId = 1;
            var language = "LanguageTest";
            var expected = new LiveCasinoBetSetting
            {
                GameType = "GameTypeTest",
                GameTypeId = 1,
                StakeLevelId = 2,
                StakeLevelName = "StakeLevelNameTest",
                IsCheck = true
            };

            var reader = Substitute.For<IDataReader>();
            ConfigureReaderForGetBetSettingMethod(reader, expected);

            // Action
            var result = _dataAccess.GetBetSetting(custId, language)[0];

            // Assert
            Assert.That(result.GameType, Is.EqualTo(expected.GameType));
            Assert.That(result.GameTypeId, Is.EqualTo(expected.GameTypeId));
            Assert.That(result.StakeLevelId, Is.EqualTo(expected.StakeLevelId));
            Assert.That(result.StakeLevelName, Is.EqualTo(expected.StakeLevelName));
            Assert.That(result.IsCheck, Is.EqualTo(expected.IsCheck));
        }

        private void ConfigureReaderForGetBetSettingMethod(IDataReader reader, LiveCasinoBetSetting expected)
        {
            _objectDb.ExecuteReader().Returns(reader);
            reader.Read().Returns(true, false);

            reader["GameTypeName_En"].Returns(expected.GameType);
            reader["GameTypeId"].Returns(expected.GameTypeId);
            reader["StakeLevelID"].Returns(expected.StakeLevelId);
            reader["StakeLevelName"].Returns(expected.StakeLevelName);
            reader["IsChecked"].Returns(expected.IsCheck);
        }
    }
}