namespace Fanex.Athena.Controllers.Forecast.UnitTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Base.UnitTests;
    using Model.Core.DTO.Common;
    using Model.Forecast.DTO.ForecastHdou;
    using Model.Forecast.MetaModel;
    using Model.Forecast.ViewModel;
    using NSubstitute;
    using NUnit.Framework;
    using Service.Forecast.Interfaces;

    public class HandicapOverUnderForecastControllerTest : BaseControllerHelperTest
    {
        private IForecastService _forecastService;

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();

            SetUpDefaultFakeData();
        }

        [Test]
        public void Index_WhenGet_LicenseeOffersNewForecast_ReturnNewView()
        {
            using (var controller = CreateController())
            {
                // Arrange
                AppSettings.LicenseeOfferNewForecast.Returns(new[] { DefaultLicenseeSession.LicenseeId });

                // Action
                var result = controller.Index() as RedirectToRouteResult;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(result.RouteValues["action"], Is.EqualTo("Index"));
                    Assert.That(result.RouteValues["controller"], Is.EqualTo("NewHandicapOverUnderForecast"));
                });
            }
        }

        [Test]
        public void Index_WhenGet_LicenseeDoesNotOfferNewForecast_ReturnCorrectViewResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                AppSettings.LicenseeOfferNewForecast.Returns(Enumerable.Empty<int>());
                const int ExpectdLeagueId = -1;
                var leagueList = Enumerable.Empty<League>();
                var mainData = Enumerable.Empty<ForecastHandicapOverUnder>();

                _forecastService.GetForecastRunningLeague(Arg.Any<ForecastHandicapOverUnderMetaModel>()).Returns(leagueList);
                _forecastService.GetForecastHandicapOverUnder(Arg.Any<ForecastHandicapOverUnderMetaModel>()).Returns(mainData);

                // Action
                var result = controller.Index() as ViewResult;
                var viewModel = (ForecastHdpOuViewModel)result.Model;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(result.ViewName, Is.EqualTo("~/Views/Desktop/Forecast/ForecastHDPOU/Main.cshtml"));
                    AssertViewModel(
                        viewModel,
                        new ForecastHandicapOverUnderMetaModel { LicenseeId = DefaultLicenseeSession.LicenseeId, UserId = DefaultUserSession.UserId },
                        mainData,
                        leagueList,
                        ExpectdLeagueId);
                });
            }
        }

        [TestCase(new[] { 1, 2 }, 1, 1)]
        [TestCase(new[] { 1, 2 }, 3, 1)]
        [TestCase(new int[0], 3, -1)]
        public void Index_WhenGetPost_ReturnCorrectViewResult(int[] leagueIDList, int leagueId, int expectedLeagueId)
        {
            using (var controller = CreateController())
            {
                // Arrange
                AppSettings.LicenseeOfferNewForecast.Returns(Enumerable.Empty<int>());
                var leagueList = leagueIDList.Select(x => new League { LeagueId = x }).ToList();
                var mainData = Enumerable.Empty<ForecastHandicapOverUnder>();

                _forecastService.GetForecastRunningLeague(Arg.Any<ForecastHandicapOverUnderMetaModel>()).Returns(leagueList);
                _forecastService.GetForecastHandicapOverUnder(Arg.Any<ForecastHandicapOverUnderMetaModel>()).Returns(mainData);

                var metaModel = new ForecastHandicapOverUnderMetaModel { LeagueId = leagueId };

                // Action
                var result = controller.Index(metaModel) as ViewResult;
                var viewModel = (ForecastHdpOuViewModel)result.Model;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(result.ViewName, Is.EqualTo("~/Views/Desktop/Forecast/ForecastHDPOU/Main.cshtml"));
                    AssertViewModel(
                        viewModel,
                        metaModel,
                        mainData,
                        leagueList,
                        expectedLeagueId);
                });
            }
        }

        private void AssertViewModel(
            ForecastHdpOuViewModel viewModel,
            ForecastHandicapOverUnderMetaModel metaModel,
            IEnumerable<ForecastHandicapOverUnder> mainData,
            IEnumerable<League> leagueList,
            int leagueId)
        {
            Assert.That(viewModel.Param.LicenseeId, Is.EqualTo(metaModel.LicenseeId));
            Assert.That(viewModel.Param.UserId, Is.EqualTo(metaModel.UserId));
            Assert.That(viewModel.Param.LeagueId, Is.EqualTo(leagueId));
            Assert.That(viewModel.MainData, Is.EqualTo(mainData));
            Assert.That(viewModel.LeagueList, Is.EqualTo(leagueList));
            Assert.That(viewModel.CurrencyList, Is.EqualTo(DefaultUserSession.CurrencyList));
        }

        private HandicapOverUnderForecastController CreateController()
        {
            _forecastService = Substitute.For<IForecastService>();

            var controller = new HandicapOverUnderForecastController(
                LangService,
                SessionService,
                AppSettings,
                _forecastService)
            {
                ControllerContext = ControllerContext
            };

            return controller;
        }
    }
}