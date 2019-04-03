namespace Fanex.Athena.DataAccess.Forecast.UnitTests
{
    using System.Data;
    using Core.UnitTests.TestHelpers;
    using Forecast;
    using Model.Forecast.DTO.Forecast1X2;
    using Model.Forecast.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using UnitTest.Common.Helpers;
    using VendorAdapter.Interfaces;

    [TestFixture]
    public class ForecastOneXTwoDataAccessTest : BaseDataAccessHelperTest<ForecastOneXTwoDataAccess>
    {
        protected override void SetUpDataAccess()
        {
            DataAccess = Substitute.ForPartsOf<ForecastOneXTwoDataAccess>(
                Substitute.For<ICacheRefAdapter>());
        }

        [Test]
        public void GetForecastOneXTwoByLeague_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is("Lrf_Forecast1x2_SelByLeague")).Returns(ObjectDb);
            var metaModel = new ForecastOneXTwoMetaModel();

            // Action
            DataAccess.GetForecastOneXTwoByLeague(metaModel);

            // Assert
            ObjectDb.Received(1).SetParameterValue("AgentList", metaModel.AgentListXml);
            ObjectDb.Received(1).SetParameterValue("LeagueId", metaModel.LeagueId);
        }

        [Test]
        public void GetForecastOneXTwoByLeague_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is("Lrf_Forecast1x2_SelByLeague")).Returns(ObjectDb);
            var metaModel = new ForecastOneXTwoMetaModel();
            var expected = FakeHelper.Create<ForecastOneXTwo>();
            ConfifureReaderForGetForecastOneXTwoByLeagueMethod(Reader, expected, false);

            // Action
            var result = DataAccess.GetForecastOneXTwoByLeague(metaModel)[0];

            // Assert
            Assert.That(result.IsExtra, Is.False);
            Assert.That(result.MatchId, Is.EqualTo(expected.MatchId));
            Assert.That(result.HomeId, Is.EqualTo(expected.HomeId));
            Assert.That(result.AwayId, Is.EqualTo(expected.AwayId));
            Assert.That(result.LiveIndicator, Is.EqualTo(expected.LiveIndicator));
            Assert.That(result.LiveHomeScore, Is.EqualTo(expected.LiveHomeScore));
            Assert.That(result.LiveAwayScore, Is.EqualTo(expected.LiveAwayScore));
            Assert.That(result.ShowTime, Is.EqualTo(expected.ShowTime));
            Assert.That(result.IsNeutral, Is.EqualTo(expected.IsNeutral));
        }

        [Test]
        public void GetForecastOneXTwoByLeague_HasExtraColumn_DataReturnedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is("Lrf_Forecast1x2_SelByLeague")).Returns(ObjectDb);
            var metaModel = new ForecastOneXTwoMetaModel();
            var expected = FakeHelper.Create<ForecastOneXTwo>();
            ConfifureReaderForGetForecastOneXTwoByLeagueMethod(Reader, expected, true);

            // Action
            var result = DataAccess.GetForecastOneXTwoByLeague(metaModel)[0];

            // Assert
            Assert.That(result.IsExtra, Is.True);
            Assert.That(result.HalfTimeHomeWinLoss, Is.EqualTo(expected.HalfTimeHomeWinLoss));
            Assert.That(result.HalfTImeDrawWinLoss, Is.EqualTo(expected.HalfTImeDrawWinLoss));
            Assert.That(result.HalfTimeAwayWinLoss, Is.EqualTo(expected.HalfTimeAwayWinLoss));
        }

        [Test]
        public void GetForecastOneXTwoByMatch_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is("Lrf_Forecast_Sel1X2")).Returns(ObjectDb);
            var metaModel = new ForecastOneXTwoMetaModel();

            // Action
            DataAccess.GetForecastOneXTwoByMatch(metaModel);

            // Assert
            ObjectDb.Received(1).SetParameterValue("AgentList", metaModel.AgentListXml);
            ObjectDb.Received(1).SetParameterValue("matchid", metaModel.MatchId);
            ObjectDb.Received(1).SetParameterValue("betType", metaModel.BetType);
        }

        private static void ConfifureReaderForGetForecastOneXTwoByLeagueMethod(IDataReader reader, ForecastOneXTwo expected, bool hasExtraColumn)
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

            reader["hl"].Returns(expected.HomeWinLoss);
            reader["dl"].Returns(expected.DrawWinLoss);
            reader["al"].Returns(expected.AwayWinLoss);
            reader["ht_hl"].Returns(expected.HalfTimeHomeWinLoss);
            reader["ht_dl"].Returns(expected.HalfTImeDrawWinLoss);
            reader["ht_al"].Returns(expected.HalfTimeAwayWinLoss);

            reader["isExtra"].Returns(hasExtraColumn ? 1 : 0);

            if (hasExtraColumn)
            {
                reader.FieldCount.Returns(1);
                reader.GetName(0).Returns("IsExtra");
                reader["x_hl"].Returns(expected.HalfTimeHomeWinLoss);
                reader["x_dl"].Returns(expected.HalfTImeDrawWinLoss);
                reader["x_al"].Returns(expected.HalfTimeAwayWinLoss);
            }
        }
    }
}