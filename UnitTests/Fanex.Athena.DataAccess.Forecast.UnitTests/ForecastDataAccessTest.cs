namespace Fanex.Athena.DataAccess.Forecast.UnitTests
{
    using System.Data;
    using System.Threading.Tasks;
    using Core.UnitTests.TestHelpers;
    using Forecast;
    using Model.Forecast.DTO.ForecastHdou;
    using Model.Forecast.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using UnitTest.Common.Helpers;
    using VendorAdapter.Interfaces;

    [TestFixture]
    public class ForecastDataAccessTest : BaseDataAccessHelperTest<ForecastDataAccess>
    {
        protected override void SetUpDataAccess()
        {
            DataAccess = Substitute.ForPartsOf<ForecastDataAccess>(
                Substitute.For<ICacheRefAdapter>());
        }

        [Test]
        public async Task GetForecastHandicapOverUnder_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is("Lrf_Forecast_SelByLeague")).Returns(ObjectDb);
            var metaModel = new ForecastHandicapOverUnderMetaModel();

            // Action
            await DataAccess.GetForecastHandicapOverUnder(metaModel);

            // Assert
            await ObjectDb.Received(1).ExecuteReaderAsync(Arg.Any<object>());
        }

        [Test]
        public async Task GetForecastHandicapOverUnder_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is("Lrf_Forecast_SelByLeague")).Returns(ObjectDb);
            ObjectDb.ExecuteReaderAsync(Arg.Any<object>()).Returns(Reader);
            var metaModel = new ForecastHandicapOverUnderMetaModel();
            var expected = FakeHelper.Create<ForecastHandicapOverUnder>();
            ConfifureReaderForGetForecastHandicapOverUnderMethod(Reader, expected);

            // Action
            var result = (await DataAccess.GetForecastHandicapOverUnder(metaModel))[0];

            // Assert
            Assert.That(result.MatchId, Is.EqualTo(expected.MatchId));
            Assert.That(result.HomeId, Is.EqualTo(expected.HomeId));
            Assert.That(result.AwayId, Is.EqualTo(expected.AwayId));
            Assert.That(result.LiveIndicator, Is.EqualTo(expected.LiveIndicator));
            Assert.That(result.LiveHomeScore, Is.EqualTo(expected.LiveHomeScore));
            Assert.That(result.LiveAwayScore, Is.EqualTo(expected.LiveAwayScore));
            Assert.That(result.ShowTime, Is.EqualTo(expected.ShowTime));
            Assert.That(result.IsNeutral, Is.EqualTo(expected.IsNeutral));
            Assert.That(result.HandicapMinus1, Is.EqualTo(expected.HandicapMinus1));
            Assert.That(result.HandicapPlus1, Is.EqualTo(expected.HandicapPlus1));
            Assert.That(result.OverUnderPlus2, Is.EqualTo(expected.OverUnderPlus2));
            Assert.That(result.OverUnderPlus1, Is.EqualTo(expected.OverUnderPlus1));
            Assert.That(result.OverUnder, Is.EqualTo(expected.OverUnder));
            Assert.That(result.HtHandicapMinus1, Is.EqualTo(expected.HtHandicapMinus1));
            Assert.That(result.HtHandicap, Is.EqualTo(expected.HtHandicap));
        }

        private static void ConfifureReaderForGetForecastHandicapOverUnderMethod(IDataReader reader, ForecastHandicapOverUnder expected)
        {
            reader.Read().Returns(true, false);

            reader["matchid"].Returns(expected.MatchId);
            reader["homeid"].Returns(expected.HomeId);
            reader["awayid"].Returns(expected.AwayId);
            reader["liveindicator"].Returns(expected.LiveIndicator);
            reader["livehomescore"].Returns(expected.LiveHomeScore);
            reader["liveawayscore"].Returns(expected.LiveAwayScore);
            reader["showtime"].Returns(expected.ShowTime);
            reader["isNeutral"].Returns(expected.IsNeutral);

            reader["WinLost_M1_HDP"].Returns(expected.HandicapMinus1);
            reader["WinLost_0_HDP"].Returns(expected.Handicap);
            reader["WinLost_1_HDP"].Returns(expected.HandicapPlus1);
            reader["WinLost_2_OU"].Returns(expected.OverUnderPlus2);
            reader["WinLost_1_OU"].Returns(expected.OverUnderPlus1);
            reader["WinLost_0_OU"].Returns(expected.OverUnder);

            reader["WinLost_M1_HTHDP"].Returns(expected.HtHandicapMinus1);
            reader["WinLost_0_HTHDP"].Returns(expected.HtHandicap);
            reader["WinLost_1_HTHDP"].Returns(expected.HtHandicapPlus1);
            reader["WinLost_2_HTOU"].Returns(expected.HtOverUnderPlus2);
            reader["WinLost_1_HTOU"].Returns(expected.HtOverUnderPlus1);
            reader["WinLost_0_HTOU"].Returns(expected.HtOverUnder);
        }
    }
}