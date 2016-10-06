namespace Fanex.Athena.DataAccess.BetSettings.UnitTests
{
    using System.Data;
    using Data;
    using Model.BetSettings.DTO;
    using Model.Core.DTO.Common;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class VirtualSportsBetSettingDataAccessTest
    {
        private IObjectDb _objectDb;
        private VirtualSportsBetSettingDataAccess _dataAccess;

        [SetUp]
        public void SetUp()
        {
            _objectDb = Substitute.For<IObjectDb>();
            _dataAccess = Substitute.ForPartsOf<VirtualSportsBetSettingDataAccess>();
        }

        [Test]
        public void GetBetSetting_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_CustomerSelMember_MinMaxBetVS")).Returns(_objectDb);
            var custId = 1;

            // Action
            _dataAccess.GetBetSetting(custId);

            // Assert
            _objectDb.Received().AddInParameter("custid", DbType.Int32, custId);
        }

        [Test]
        public void GetBetSetting_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_CustomerSelMember_MinMaxBetVS")).Returns(_objectDb);
            var custId = 1;
            var expected = new BetSetting
            {
                MaxBet = 100.5m,
                MinBet = 10m,
                MaxPerMatchMax = 60m,
                MaxPerMatchMin = 15m,
                MaxPerMatch = 40m,
                BetValueMax = 20m,
                BetValueMin = 15m,
                SportId = "SportIdTest"
            };

            var reader = Substitute.For<IDataReader>();
            ConfigureReaderForGetBetSettingMethod(reader, expected);

            // Action
            var result = _dataAccess.GetBetSetting(custId)[0];

            // Assert
            Assert.That(result.MaxBet, Is.EqualTo(expected.MaxBet));
            Assert.That(result.MinBet, Is.EqualTo(expected.MinBet));
            Assert.That(result.MaxPerMatchMax, Is.EqualTo(expected.MaxPerMatchMax));
            Assert.That(result.MaxPerMatchMin, Is.EqualTo(expected.MaxPerMatchMin));
            Assert.That(result.MaxPerMatch, Is.EqualTo(expected.MaxPerMatch));
            Assert.That(result.BetValueMax, Is.EqualTo(expected.BetValueMax));
            Assert.That(result.BetValueMin, Is.EqualTo(expected.BetValueMin));
            Assert.That(result.SportId, Is.EqualTo(expected.SportId));
        }

        [Test]
        public void UpdateBetSetting_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_CustomerUpdMember_MinMaxBetVS")).Returns(_objectDb);
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
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_CustomerUpdMember_MinMaxBetVS")).Returns(_objectDb);
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

        private void ConfigureReaderForGetBetSettingMethod(IDataReader reader, BetSetting expected)
        {
            _objectDb.ExecuteReader().Returns(reader);
            reader.Read().Returns(true, false);

            reader["sporttype"].Returns(expected.SportId);
            reader["minbet"].Returns(expected.MinBet);
            reader["minminbet"].Returns(expected.BetValueMin);
            reader["maxbet"].Returns(expected.MaxBet);
            reader["maxmaxbet"].Returns(expected.BetValueMax);
            reader["maxpermatch"].Returns(expected.MaxPerMatch);
            reader["maxmaxpermatch"].Returns(expected.MaxPerMatchMax);
            reader["maxpermatch_min"].Returns(expected.MaxPerMatchMin);
        }
    }
}