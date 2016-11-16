namespace Fanex.Athena.DataAccess.BetSettings.UnitTests
{
    using System.Data;
    using Data;
    using Model.BetSettings.DTO;
    using Model.Core.DTO.Common;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class NumberGameBetSettingDataAccessTest
    {
        private IObjectDb _objectDb;
        private NumberGameBetSettingDataAccess _dataAccess;

        [OneTimeSetUp]
        public void SetUp()
        {
            _objectDb = Substitute.For<IObjectDb>();
            _dataAccess = Substitute.ForPartsOf<NumberGameBetSettingDataAccess>();
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
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_CustomerSelEditNG")).Returns(_objectDb);
            var custId = 1;

            // Action
            _dataAccess.GetBetSetting(custId);

            // Assert
            _objectDb.Received().AddInParameter("CustID", DbType.Int32, custId);
        }

        [Test]
        public void GetBetSetting_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_CustomerSelEditNG")).Returns(_objectDb);
            var custId = 1;
            var expected = new BetSetting
            {
                MaxBet = 100.5m,
                MinBet = 10m,
                MaxPerMatchMax = 60m,
                MaxPerMatch = 40m,
                BetValueMax = 20m,
                BetValueMin = 15m,
                SportId = "161"
            };

            var reader = Substitute.For<IDataReader>();
            ConfigureReaderForGetBetSettingMethod(reader, expected);

            // Action
            var result = _dataAccess.GetBetSetting(custId)[0];

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
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_CustomerUpdMember_MinMaxBet161")).Returns(_objectDb);
            var param = GetNumberGameBetSetting();

            // Action
            _dataAccess.UpdateBetSetting(param);

            // Assert
            _objectDb.Received().AddInParameter("custid", DbType.Int32, param.CustId);
            _objectDb.Received().AddInParameter("userid", DbType.Int32, param.UserId);
            _objectDb.Received().AddInParameter("minbetv161", DbType.Decimal, param.MinBet);
            _objectDb.Received().AddInParameter("maxbetv161", DbType.Decimal, param.MaxBet);
            _objectDb.Received().AddInParameter("maxpermatchv161", DbType.Decimal, param.MaxPerMatch);
            _objectDb.Received().AddInParameter("maxperballv161", DbType.Decimal, param.MaxPerBall);
            _objectDb.Received().AddInParameter("language", DbType.String, param.Language);
            _objectDb.Received().AddInParameter("ip", DbType.String, param.Ip);
            _objectDb.Received().AddOutParameter("ErrCode", DbType.Int32, 4);
            _objectDb.Received().AddOutParameter("ErrMsg", DbType.String, 4000);
        }

        [Test]
        public void UpdateBetSetting_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_CustomerUpdMember_MinMaxBet161")).Returns(_objectDb);
            var param = GetNumberGameBetSetting();
            var expected = new ResultInfo
            {
                Code = 100,
                Message = "Success"
            };
            _objectDb.GetParameterValue("ErrCode").Returns(expected.Code);
            _objectDb.GetParameterValue("ErrMsg").Returns(expected.Message);

            // Action
            var result = _dataAccess.UpdateBetSetting(param);

            // Assert
            Assert.That(result.Code, Is.EqualTo(expected.Code));
            Assert.That(result.Message, Is.EqualTo(expected.Message));
        }

        private void ConfigureReaderForGetBetSettingMethod(IDataReader reader, BetSetting expected)
        {
            _objectDb.ExecuteReader().Returns(reader);
            reader.Read().Returns(true, false);
            reader.FieldCount.Returns(1);

            var sportSuffix = "v161";
            reader["maxbet" + sportSuffix].Returns(expected.MaxBet);
            reader["minbet" + sportSuffix].Returns(expected.MinBet);
            reader["MaxPerMatch" + sportSuffix].Returns(expected.MaxPerMatch);
            reader["MaxPerMatch" + sportSuffix + "_Max"].Returns(expected.MaxPerMatchMax);
            reader["MaxPerMatch" + sportSuffix + "_Min"].Returns(expected.MaxPerMatchMin);
            reader["MaxBet" + sportSuffix + "_Min"].Returns(expected.BetValueMax);
            reader["MinBet" + sportSuffix + "_Max"].Returns(expected.BetValueMin);
        }

        private NumberGameBetSetting GetNumberGameBetSetting()
        {
            return new NumberGameBetSetting
            {
                CustId = 1,
                UserId = 2,
                MinBet = 10m,
                MaxBet = 100.5m,
                MaxPerMatch = 40m,
                MaxPerBall = 30m,
                Language = "LanguageTest",
                Ip = "::1"
            };
        }
    }
}