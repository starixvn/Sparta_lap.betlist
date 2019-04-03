namespace Fanex.Athena.Controllers.Forecast.UnitTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Base.UnitTests;
    using DataAccess.Core.Common.Interfaces;
    using Model.Core.DTO.Common;
    using Model.Forecast.DTO.Forecast1X2;
    using Model.Forecast.MetaModel;
    using Model.Forecast.ViewModel;
    using NSubstitute;
    using NUnit.Framework;
    using Service.Forecast.Interfaces;

    public class OneXTwoForecastControllerTest : CultureBaseControllerHelperTest
    {
        private ILeagueDataAccess _leagueDataAccess;

        private IForecastOneXTwoService _forecastService;

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();

            SetUpDefaultFakeData();
        }

        [Test]
        public async Task Index_WhenGet_ReturnCorrectViewResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                AppSettings.LicenseeOfferNewForecast.Returns(Enumerable.Empty<int>());
                const int ExpectdLeagueId = -1;
                var leagueList = Enumerable.Empty<League>();
                var mainData = Enumerable.Empty<ForecastOneXTwo>();

                _leagueDataAccess.GetRunningLeagues(Arg.Any<string>(), Arg.Any<string>()).Returns(leagueList);
                _forecastService.GetForecastOneXTwoByLeague(Arg.Any<ForecastOneXTwoMetaModel>()).Returns(mainData);

                // Action
                var result = await controller.Index() as ViewResult;
                var viewModel = (ForecastOneXTwoViewModel)result.Model;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(result.ViewName, Is.EqualTo("~/Views/Desktop/Forecast/Forecast1X2/Main.cshtml"));
                    AssertViewModel(
                        viewModel,
                        new ForecastOneXTwoMetaModel(),
                        mainData,
                        leagueList,
                        ExpectdLeagueId);
                });
            }
        }

        [TestCase(new[] { 1, 2 }, 1)]
        [TestCase(new[] { 1, 2 }, 3)]
        [TestCase(new int[0], 3)]
        public async Task Index_WhenPost_ReturnCorrectViewResult(int[] leagueIDList, int leagueId)
        {
            using (var controller = CreateController())
            {
                // Arrange
                AppSettings.LicenseeOfferNewForecast.Returns(Enumerable.Empty<int>());
                var leagueList = leagueIDList.Select(x => new League { LeagueId = x }).ToList();
                var mainData = Enumerable.Empty<ForecastOneXTwo>();

                _leagueDataAccess.GetRunningLeagues(Arg.Any<string>(), Arg.Any<string>()).Returns(leagueList);
                _forecastService.GetForecastOneXTwoByMatch(Arg.Any<ForecastOneXTwoMetaModel>()).Returns(mainData);

                var metaModel = new ForecastOneXTwoMetaModel { LeagueId = leagueId, MatchId = 1 };

                // Action
                var result = await controller.Index(metaModel) as ViewResult;
                var viewModel = (ForecastOneXTwoViewModel)result.Model;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(result.ViewName, Is.EqualTo("~/Views/Desktop/Forecast/Forecast1X2/Main.cshtml"));
                    AssertViewModel(
                        viewModel,
                        metaModel,
                        mainData,
                        leagueList,
                        leagueId);
                });
            }
        }

        [TestCase(new[] { 1, 2 }, 1)]
        [TestCase(new[] { 1, 2 }, 3)]
        [TestCase(new int[0], 3)]
        public async Task Popup_WhenCall_ReturnCorrectViewResult(int[] leagueIDList, int leagueId)
        {
            using (var controller = CreateController())
            {
                // Arrange
                AppSettings.LicenseeOfferNewForecast.Returns(Enumerable.Empty<int>());
                var leagueList = leagueIDList.Select(x => new League { LeagueId = x }).ToList();
                var mainData = Enumerable.Empty<ForecastOneXTwo>();

                _leagueDataAccess.GetRunningLeagues(Arg.Any<string>(), Arg.Any<string>()).Returns(leagueList);
                _forecastService.GetForecastOneXTwoByMatch(Arg.Any<ForecastOneXTwoMetaModel>()).Returns(mainData);

                var metaModel = new ForecastOneXTwoMetaModel { LeagueId = leagueId, MatchId = 1 };

                // Action
                var result = await controller.Popup(metaModel) as ViewResult;
                var viewModel = (ForecastOneXTwoViewModel)result.Model;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(result.ViewName, Is.EqualTo("~/Views/Desktop/Forecast/Forecast1X2/Main.cshtml"));
                    AssertViewModel(
                        viewModel,
                        metaModel,
                        mainData,
                        leagueList,
                        leagueId);
                });
            }
        }

        private void AssertViewModel(
            ForecastOneXTwoViewModel viewModel,
            ForecastOneXTwoMetaModel metaModel,
            IEnumerable<ForecastOneXTwo> mainData,
            IEnumerable<League> leagueList,
            int leagueId)
        {
            Assert.That(viewModel.Param.LicenseeId, Is.EqualTo(DefaultLicenseeSession.LicenseeId));
            Assert.That(viewModel.Param.LeagueId, Is.EqualTo(leagueId));
            Assert.That(viewModel.MainData, Is.EqualTo(mainData));
            Assert.That(viewModel.LeagueList, Is.EqualTo(leagueList));
            Assert.That(viewModel.CurrencyList, Is.EqualTo(DefaultUserSession.CurrencyList));
        }

        private OneXTwoForecastController CreateController()
        {
            _forecastService = Substitute.For<IForecastOneXTwoService>();
            _leagueDataAccess = Substitute.For<ILeagueDataAccess>();

            var controller = new OneXTwoForecastController(
                LangService,
                SessionService,
                AppSettings,
                _leagueDataAccess,
                _forecastService)
            {
                ControllerContext = ControllerContext
            };

            return controller;
        }
    }
}