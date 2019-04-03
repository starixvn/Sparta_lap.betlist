namespace Fanex.Athena.Service.Forecast.UnitTests
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using DataAccess.Core.Common.Interfaces;
    using DataAccess.Forecast.Interfaces;
    using Model.Core.DTO.Common;
    using Model.Forecast.DTO.Forecast1X2;
    using Model.Forecast.DTO.ForecastHdou;
    using Model.Forecast.DTO.NewForecastHdpOu;
    using Model.Forecast.MetaModel;
    using Model.Forecast.ViewModel;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class ForecastServiceTest
    {
        private ILeagueDataAccess _leagueDataAccess;
        private IForecastDataAccess _forecastDataAccess;
        private ForecastService _forecastService;

        [SetUp]
        public void SetUp()
        {
            _forecastDataAccess = Substitute.For<IForecastDataAccess>();
            _leagueDataAccess = Substitute.For<ILeagueDataAccess>();
            _forecastService = new ForecastService(_leagueDataAccess, _forecastDataAccess);
        }

        [TearDown]
        public void TearDown()
        {
            _forecastDataAccess = null;
            _forecastService = null;
        }

        [Test]
        public async Task GetForecastRunningLeague_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var metaModel = new ForecastBaseMetaModel
            {
                AgentListXml = "sdfsdf",
                BetTypeString = "1234,5,6,"
            };
            var expectedResult = new List<League>();
            _leagueDataAccess.GetRunningLeagues(metaModel.AgentListXml, metaModel.BetTypeString).Returns(expectedResult);

            // Action
            var result = await _forecastService.GetForecastRunningLeague(metaModel);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void GetForecastOneXTwoByMatch_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var metaModel = new ForecastOneXTwoMetaModel();
            var expectedResult = new List<ForecastOneXTwo>();
            _forecastDataAccess.GetForecastOneXTwoByMatch(metaModel).Returns(expectedResult);

            // Action
            var result = _forecastService.GetForecastOneXTwoByMatch(metaModel);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void GetForecastOneXTwoByLeague_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var metaModel = new ForecastOneXTwoMetaModel();
            var expectedResult = new List<ForecastOneXTwo>();
            _forecastDataAccess.GetForecastOneXTwoByLeague(metaModel).Returns(expectedResult);

            // Action
            var result = _forecastService.GetForecastOneXTwoByLeague(metaModel);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void GetForecastHandicapOverUnder_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var metaModel = new ForecastHandicapOverUnderMetaModel();
            var expectedResult = new List<ForecastHandicapOverUnder>();
            _forecastDataAccess.GetForecastHandicapOverUnder(metaModel).Returns(expectedResult);

            // Action
            var result = _forecastService.GetForecastHandicapOverUnder(metaModel);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void GetNewForecastData_InvalidMetaModel_ResultIsNotNullAndDataAccessIsNotCalled()
        {
            // Arrange
            var metaModel = new NewForecastHandicapOverUnderMetaModel();

            // Action
            var result = _forecastService.GetNewForecastData(metaModel, Arg.Any<NewForecastHdpOuMatchInfo>());

            // Assert
            Assert.That(result, Is.Not.Null);
            _forecastDataAccess.DidNotReceiveWithAnyArgs().GetNewForecastData(metaModel, Arg.Any<NewForecastHdpOuMatchInfo>());
        }

        [Test]
        public void GetNewForecastData_WhenCallWithValidModel_ReturnCorrectData()
        {
            // Arrange
            var metaModel = new NewForecastHandicapOverUnderMetaModel { LeagueId = 1, MatchId = 2 };
            var headerInfo = new NewForecastHdpOuMatchInfo();
            var expectedResult = new NewForecastHdpOuData();
            _forecastDataAccess.GetNewForecastData(metaModel, headerInfo).Returns(expectedResult);

            // Action
            var result = _forecastService.GetNewForecastData(metaModel, headerInfo);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }
    }
}