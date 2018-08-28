namespace Fanex.Athena.DataAccess.BetSettings.UnitTests
{
    using System.Data;
    using Constant.DataDomain;
    using Model.BetSettings.DTO;
    using NSubstitute;
    using NUnit.Framework;
    using Utility;
    using VendorAdapter.Interfaces;

    [TestFixture]
    public class BetSettingReaderTest
    {
        private BetSettingReader _betSettingReader;

        [SetUp]
        public void SetUp()
        {
            var cacheRefAdapter = Substitute.For<ICacheRefAdapter>();
            _betSettingReader = Substitute.ForPartsOf<BetSettingReader>(cacheRefAdapter);
        }

        [Test]
        public void ReadForSport_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            const int SportId = (int)SportType.NumberGame;
            var reader = Substitute.For<IDataReader>();
            var expected = new BetSetting
            {
                MaxBet = 100.5m,
                MinBet = 10m,
                MaxPerMatchMax = 60m,
                MaxPerMatch = 40m,
                BetValueMax = 20m,
                BetValueMin = 15m,
                MaxPerBall = 16,
                MaxPerBallMax = 17,
                SportId = SportId.ToString()
            };

            ConfigDataReader(reader, expected);

            // Action
            var result = _betSettingReader.ReadForSport(reader, SportId.ToString(), true);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.MaxBet, Is.EqualTo(expected.MaxBet));
                Assert.That(result.MinBet, Is.EqualTo(expected.MinBet));
                Assert.That(result.MaxPerMatchMax, Is.EqualTo(expected.MaxPerMatchMax));
                Assert.That(result.MaxPerMatch, Is.EqualTo(expected.MaxPerMatch));
                Assert.That(result.BetValueMax, Is.EqualTo(expected.BetValueMax));
                Assert.That(result.BetValueMin, Is.EqualTo(expected.BetValueMin));
                Assert.That(result.MaxPerBall, Is.EqualTo(expected.MaxPerBall));
                Assert.That(result.MaxPerBallMax, Is.EqualTo(expected.MaxPerBallMax));
                Assert.That(result.SportId, Is.EqualTo(expected.SportId));
            });
        }

        private static void ConfigDataReader(IDataReader reader, BetSetting expected)
        {
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
            reader["maxperball" + sportSuffix].Returns(expected.MaxPerBall);
            reader["maxperball" + sportSuffix + "_Max"].Returns(expected.MaxPerBallMax);
        }
    }
}