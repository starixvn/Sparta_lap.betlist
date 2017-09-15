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

            // Action
            _dataAccess.GetBetSetting(custId);

            // Assert
            _objectDb.Received().AddInParameter("CustID", DbType.Int32, custId);
        }

        [Test]
        public void GetBetSetting_WhenCall_DataReturnCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_CustomerSelEditRB")).Returns(_objectDb);
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_CheckHRAllowedCurrency")).Returns(Substitute.For<IObjectDb>());
            var reader = Substitute.For<IDataReader>();
            _objectDb.ExecuteReader().Returns(reader);
            reader.Read().Returns(true, false);
            reader.FieldCount.Returns(1);

            MapReaderData(reader, new Dictionary<string, object>());
            var custId = 1;

            // Action
            var result = _dataAccess.GetBetSetting(custId);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Count, Is.EqualTo(3));
                Assert.That(result[0].SportId, Is.EqualTo("151_41"));
                Assert.That(result[1].SportId, Is.EqualTo("152_51"));
                Assert.That(result[2].SportId, Is.EqualTo("153_51"));
            });
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

        private static void MapReaderData(IDataReader reader, Dictionary<string, object> readerDataMapping)
        {
            reader[Arg.Any<string>()].Returns(x =>
            {
                var paramName = x.ArgAt<string>(0);
                if (readerDataMapping.ContainsKey(paramName))
                {
                    return readerDataMapping[paramName];
                }

                return string.Empty;
            });
        }
    }
}