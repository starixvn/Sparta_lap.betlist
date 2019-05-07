namespace Fanex.Athena.DataAccess.Forecast.UnitTests
{
    using System.Threading.Tasks;
    using Core.UnitTests.TestHelpers;
    using Forecast;
    using Model.Forecast.DTO.ForecastAH;
    using Model.Forecast.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using VendorAdapter.Interfaces;

    [TestFixture]
    public class ForecastAhDataAccessTest : BaseDataAccessHelperTest<ForecastAhDataAccess>
    {
        protected override void SetUpDataAccess()
        {
            DataAccess = Substitute.ForPartsOf<ForecastAhDataAccess>(
                Substitute.For<ICacheRefAdapter>());
        }

        [Test]
        public async Task GetForecastTopOddsByMatch_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is("Lrf_Forecast_OddsSelTopByMatch")).Returns(ObjectDb);
            var matchId = 0;
            var betType = 0;
            ObjectDb.QueryEntity<ForecastAH>().Returns(new ForecastAH());

            // Action
            await DataAccess.GetForecastTopOddsByMatch(matchId, betType);

            // Assert
            ObjectDb.Received(1).SetParameterValue("matchid", matchId);
            ObjectDb.Received(1).SetParameterValue("betType", betType);
        }

        [Test]
        public async Task GetForecastHandicap_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is("Lrf_Forecast_SelHDP")).Returns(ObjectDb);
            var metaModel = new ForecastAHMetaModel();
            var userId = 0;

            // Action
            await DataAccess.GetForecastHandicap(metaModel, userId);

            // Assert
            ObjectDb.Received(1).SetParameterValue("AgentList", metaModel.AgentListXml);
            ObjectDb.Received(1).SetParameterValue("matchid", metaModel.MatchId);
            ObjectDb.Received(1).SetParameterValue("betType", metaModel.BetType);
        }

        [Test]
        public async Task GetForecastOverUnder_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is("Lrf_Forecast_SelOU")).Returns(ObjectDb);
            var metaModel = new ForecastAHMetaModel();
            var userId = 0;

            // Action
            await DataAccess.GetForecastOverUnder(metaModel, userId);

            // Assert
            ObjectDb.Received(1).SetParameterValue("AgentList", metaModel.AgentListXml);
            ObjectDb.Received(1).SetParameterValue("matchid", metaModel.MatchId);
            ObjectDb.Received(1).SetParameterValue("betType", metaModel.BetType);
        }
    }
}