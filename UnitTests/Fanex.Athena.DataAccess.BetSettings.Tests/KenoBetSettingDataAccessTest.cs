namespace Fanex.Athena.DataAccess.BetSettings.Tests
{
    using System.Data;
    using Data;
    using Model.BetSettings.DTO;
    using Model.Core.DTO.Common;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class KenoBetSettingDataAccessTest
    {
        private IObjectDb _objectDb;
        private KenoBetSettingDataAccess _dataAccess;

        [SetUp]
        public void SetUp()
        {
            _objectDb = Substitute.For<IObjectDb>();
            _dataAccess = Substitute.ForPartsOf<KenoBetSettingDataAccess>();
        }

        [Test]
        public void GetBetSettings_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_CustomerSelEditKN")).Returns(_objectDb);
            var custId = 1;

            // Action
            _dataAccess.GetBetSettings(custId);

            // Assert
            _objectDb.Received().AddInParameter("CustID", DbType.Int32, custId);
        }

        [Test]
        public void GetBetSettings_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_CustomerSelEditKN")).Returns(_objectDb);
            var reader = Substitute.For<IDataReader>();           
            var custId = 1;
            var expected = new BetSetting
            {
                MaxBet = 100.5m,
                MinBet = 10m,
                MaxPerMatchMax = 60m,
                MaxPerMatch = 40m,
                BetValueMax = 20m,
                BetValueMin = 15m,
                SportId = "SportIdTest"
            };
            ConfigureReaderForReadBetSettingMethod(reader, expected);

            // Action
            var result = _dataAccess.GetBetSettings(custId)[0];

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
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_CustomerUpdMember_MinMaxBetKN")).Returns(_objectDb);
            var custId = 1;
            var userId = 2;
            var language = "LanguageTest";
            var ip = "::1";
            var xmlData = "XMLDataTest";

            // Action
            _dataAccess.UpdateBetSetting(custId, userId, language, ip, xmlData);

            // Assert
            _objectDb.Received().AddInParameter("custid", DbType.Int32, custId);
            _objectDb.Received().AddInParameter("userid", DbType.Int32, userId);
            _objectDb.Received().AddInParameter("language", DbType.String, language);
            _objectDb.Received().AddInParameter("ip", DbType.String, ip);
            _objectDb.Received().AddInParameter("dataxml", DbType.String, xmlData);
            _objectDb.Received().AddOutParameter("errcode", DbType.Int32, 4);
            _objectDb.Received().AddOutParameter("errmsg", DbType.String, 8000);
        }

        [Test]
        public void UpdateBetSetting_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_CustomerUpdMember_MinMaxBetKN")).Returns(_objectDb);
            var custId = 1;
            var userId = 2;
            var language = "LanguageTest";
            var ip = "::1";
            var xmlData = "XMLDataTest";
            var expected = new ResultInfo
            {
                Code = 100,
                Message = "Success"
            };
            _objectDb.GetParameterValue("ErrCode").Returns(expected.Code);
            _objectDb.GetParameterValue("ErrMsg").Returns(expected.Message);

            // Action
            var result = _dataAccess.UpdateBetSetting(custId, userId, language, ip, xmlData);

            // Assert
            Assert.That(result.Code, Is.EqualTo(expected.Code));
            Assert.That(result.Message, Is.EqualTo(expected.Message));
        }

        private void ConfigureReaderForReadBetSettingMethod(IDataReader reader, BetSetting expected)
        {
            _objectDb.ExecuteReader().Returns(reader);
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