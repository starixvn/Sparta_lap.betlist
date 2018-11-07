namespace Fanex.Athena.Controllers.Forecast.UnitTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Base.UnitTests;
    using Model.Core.DTO.Common;
    using Model.Forecast.DTO.NewForecastHdpOu;
    using Model.Forecast.MetaModel;
    using Model.Forecast.ViewModel;
    using NSubstitute;
    using NUnit.Framework;
    using Service.Core.Common.Interfaces;
    using Service.Core.Utility.Interfaces;
    using Service.Forecast.Interfaces;
    using VendorAdapter.Interfaces;

    public class NewHandicapOverUnderForecastControllerTest : BaseControllerHelperTest
    {
        private IForecastService _forecastService;

        private ICacheRefAdapter _cacheRefAdapter;

        private IMatchService _matchService;

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();

            SetUpDefaultFakeData();
        }

        [TestCase("chf", "chf")]
        [TestCase("", "usd")]
        public void Index_WhenGet_LicenseeOffersNewForecase_ReturnCorrectViewResult(string baseCurrency, string expectedBaseCurrency)
        {
            using (var controler = CreateController())
            {
                // Arrange
                controler.HttpContext.Request["X-Requested-With"].Returns("XMLHttpRequest");
                AppSettings.LicenseeOfferNewForecast.Returns(new[] { DefaultLicenseeSession.LicenseeId });
                StubGetValueAppSettings("BaseUsdLicensees", DefaultLicenseeSession.LicenseeId.ToString());

                const string TeamName = "";
                _cacheRefAdapter.GetTeamName(0).Returns(TeamName);

                NewForecastHdpOuMatchInfo headerInfo = null;
                var mainData = new NewForecastHdpOuData();
                _forecastService.GetNewForecastData(
                    Arg.Any<NewForecastHandicapOverUnderMetaModel>(),
                    Arg.Do<NewForecastHdpOuMatchInfo>(x => headerInfo = x)).Returns(mainData);

                // Action
                var result = controler.Index(baseCurrency: baseCurrency) as ViewResult;
                var viewModel = (NewForecastHdpOuViewModel)result.Model;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(result.ViewName, Is.EqualTo("~/Views/Forecast/NewForecastHdpOu/Main.cshtml"));
                    AssertViewModel(viewModel, TeamName, TeamName, mainData, headerInfo);
                    Assert.That(viewModel.Param.BaseCurrency, Is.EqualTo(expectedBaseCurrency));
                });
            }
        }

        [Test]
        public void Index_IsNotAjaxRequest_NoRunningLeague_NoDataIsAssigned()
        {
            using (var controler = CreateController())
            {
                // Arrange
                controler.HttpContext.Request["X-Requested-With"].Returns(x => { return null; });
                AppSettings.LicenseeOfferNewForecast.Returns(new[] { DefaultLicenseeSession.LicenseeId });

                _cacheRefAdapter.GetTeamName(0).Returns(string.Empty);

                var mainData = new NewForecastHdpOuData();
                _forecastService.GetNewForecastData(
                    Arg.Any<NewForecastHandicapOverUnderMetaModel>(),
                    Arg.Any<NewForecastHdpOuMatchInfo>()).Returns(mainData);

                _forecastService.GetForecastRunningLeague(Arg.Any<NewForecastHandicapOverUnderMetaModel>()).Returns(Enumerable.Empty<League>());

                // Action
                var result = controler.Index() as ViewResult;
                var viewModel = (NewForecastHdpOuViewModel)result.Model;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(viewModel.LeagueList, Is.Empty);
                    Assert.That(viewModel.MatchList, Is.Empty);
                    Assert.That(viewModel.Param.MatchId, Is.Zero);
                    Assert.That(viewModel.Param.HomeTeamId, Is.Zero);
                    Assert.That(viewModel.Param.AwayTeamId, Is.Zero);
                    Assert.That(viewModel.Param.IsNeutral, Is.False);
                });
            }
        }

        [Test]
        public void Index_IsNotAjaxRequest_NoValidLeague_NoMatchDataIsAssigned()
        {
            using (var controler = CreateController())
            {
                // Arrange
                controler.HttpContext.Request["X-Requested-With"].Returns(x => { return null; });
                AppSettings.LicenseeOfferNewForecast.Returns(new[] { DefaultLicenseeSession.LicenseeId });

                _cacheRefAdapter.GetTeamName(0).Returns(string.Empty);

                var mainData = new NewForecastHdpOuData();
                _forecastService.GetNewForecastData(
                    Arg.Any<NewForecastHandicapOverUnderMetaModel>(),
                    Arg.Any<NewForecastHdpOuMatchInfo>()).Returns(mainData);

                const int LeagueId = 1;

                var leagueList = new List<League> { new League { LeagueId = 0 } };
                _forecastService.GetForecastRunningLeague(Arg.Any<NewForecastHandicapOverUnderMetaModel>()).Returns(leagueList);

                // Action
                var result = controler.Index(leagueId: LeagueId) as ViewResult;
                var viewModel = (NewForecastHdpOuViewModel)result.Model;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(viewModel.LeagueList, Is.EqualTo(leagueList));
                    Assert.That(viewModel.MatchList, Is.Empty);
                    Assert.That(viewModel.Param.MatchId, Is.Zero);
                    Assert.That(viewModel.Param.HomeTeamId, Is.Zero);
                    Assert.That(viewModel.Param.AwayTeamId, Is.Zero);
                    Assert.That(viewModel.Param.IsNeutral, Is.False);
                });
            }
        }

        [Test]
        public void Index_IsNotAjaxRequest_NoMatchList_NoMatchDataIsAssigned()
        {
            using (var controler = CreateController())
            {
                // Arrange
                controler.HttpContext.Request["X-Requested-With"].Returns(x => { return null; });
                AppSettings.LicenseeOfferNewForecast.Returns(new[] { DefaultLicenseeSession.LicenseeId });

                _cacheRefAdapter.GetTeamName(0).Returns(string.Empty);

                var mainData = new NewForecastHdpOuData();
                _forecastService.GetNewForecastData(
                    Arg.Any<NewForecastHandicapOverUnderMetaModel>(),
                    Arg.Any<NewForecastHdpOuMatchInfo>()).Returns(mainData);

                const int LeagueId = 1;

                var leagueList = new List<League> { new League { LeagueId = LeagueId } };
                _forecastService.GetForecastRunningLeague(Arg.Any<NewForecastHandicapOverUnderMetaModel>()).Returns(leagueList);

                _matchService.GetForecastMatchListByLeague(LeagueId).Returns(Enumerable.Empty<Match>());

                // Action
                var result = controler.Index(leagueId: LeagueId) as ViewResult;
                var viewModel = (NewForecastHdpOuViewModel)result.Model;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(viewModel.LeagueList, Is.EqualTo(leagueList));
                    Assert.That(viewModel.MatchList, Is.Empty);
                    Assert.That(viewModel.Param.MatchId, Is.Zero);
                    Assert.That(viewModel.Param.HomeTeamId, Is.Zero);
                    Assert.That(viewModel.Param.AwayTeamId, Is.Zero);
                    Assert.That(viewModel.Param.IsNeutral, Is.False);
                });
            }
        }

        [Test]
        public void Index_IsNotAjaxRequest_NoMatchFoundInMatchList_MatchDataIsAssignedFromFirstMatchOfList()
        {
            using (var controler = CreateController())
            {
                // Arrange
                controler.HttpContext.Request["X-Requested-With"].Returns(x => { return null; });
                AppSettings.LicenseeOfferNewForecast.Returns(new[] { DefaultLicenseeSession.LicenseeId });

                _cacheRefAdapter.GetTeamName(0).Returns(string.Empty);

                var mainData = new NewForecastHdpOuData();
                _forecastService.GetNewForecastData(
                    Arg.Any<NewForecastHandicapOverUnderMetaModel>(),
                    Arg.Any<NewForecastHdpOuMatchInfo>()).Returns(mainData);

                const int LeagueId = 1;

                var leagueList = new List<League> { new League { LeagueId = LeagueId } };
                _forecastService.GetForecastRunningLeague(Arg.Any<NewForecastHandicapOverUnderMetaModel>()).Returns(leagueList);

                const int MatchId = 2;
                var match = new Match { MatchId = 0, HomeId = 1, AwayId = 2, IsNeutral = true };
                var matchList = new List<Match> { match };
                _matchService.GetForecastMatchListByLeague(LeagueId).Returns(matchList);

                // Action
                var result = controler.Index(leagueId: LeagueId, matchId: MatchId) as ViewResult;
                var viewModel = (NewForecastHdpOuViewModel)result.Model;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(viewModel.LeagueList, Is.EqualTo(leagueList));
                    Assert.That(viewModel.MatchList, Is.EqualTo(matchList));
                    Assert.That(viewModel.Param.MatchId, Is.EqualTo(match.MatchId));
                    Assert.That(viewModel.Param.HomeTeamId, Is.EqualTo(match.HomeId));
                    Assert.That(viewModel.Param.AwayTeamId, Is.EqualTo(match.AwayId));
                    Assert.That(viewModel.Param.IsNeutral, Is.EqualTo(match.IsNeutral));
                });
            }
        }

        [Test]
        public void Index_IsNotAjaxRequest_MatchFoundInMatchList_MatchDataIsAssignedFromFoundMatchOfList()
        {
            using (var controler = CreateController())
            {
                // Arrange
                controler.HttpContext.Request["X-Requested-With"].Returns(x => { return null; });
                AppSettings.LicenseeOfferNewForecast.Returns(new[] { DefaultLicenseeSession.LicenseeId });

                _cacheRefAdapter.GetTeamName(0).Returns(string.Empty);

                var mainData = new NewForecastHdpOuData();
                _forecastService.GetNewForecastData(
                    Arg.Any<NewForecastHandicapOverUnderMetaModel>(),
                    Arg.Any<NewForecastHdpOuMatchInfo>()).Returns(mainData);

                const int LeagueId = 1;

                var leagueList = new List<League> { new League { LeagueId = LeagueId } };
                _forecastService.GetForecastRunningLeague(Arg.Any<NewForecastHandicapOverUnderMetaModel>()).Returns(leagueList);

                const int MatchId = 2;
                var match = new Match { MatchId = MatchId, HomeId = 1, AwayId = 2, IsNeutral = true };
                var matchList = new List<Match> { match };
                _matchService.GetForecastMatchListByLeague(LeagueId).Returns(matchList);

                // Action
                var result = controler.Index(leagueId: LeagueId, matchId: MatchId) as ViewResult;
                var viewModel = (NewForecastHdpOuViewModel)result.Model;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(viewModel.LeagueList, Is.EqualTo(leagueList));
                    Assert.That(viewModel.MatchList, Is.EqualTo(matchList));
                    Assert.That(viewModel.Param.MatchId, Is.EqualTo(match.MatchId));
                    Assert.That(viewModel.Param.HomeTeamId, Is.EqualTo(match.HomeId));
                    Assert.That(viewModel.Param.AwayTeamId, Is.EqualTo(match.AwayId));
                    Assert.That(viewModel.Param.IsNeutral, Is.EqualTo(match.IsNeutral));
                });
            }
        }

        [Test]
        public void Index_WhenGet_LicenseeDoesNotOffersNewForecase_RedirectToAction()
        {
            using (var controler = CreateController())
            {
                // Arrange
                AppSettings.LicenseeOfferNewForecast.Returns(new int[0]);

                // Action
                var result = controler.Index() as RedirectToRouteResult;

                // Assert
                Assert.That(result.RouteValues["action"], Is.EqualTo("ForecastHandicapOverUnder"));
            }
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Index_WhenPost_ReturnCorrectViewResult(bool isNeutral)
        {
            using (var controler = CreateController())
            {
                // Arrange
                controler.HttpContext.Request["X-Requested-With"].Returns("XMLHttpRequest");

                var metaModel = new NewForecastHandicapOverUnderMetaModel
                {
                    HomeTeamId = 1,
                    AwayTeamId = 2,
                    IsNeutral = isNeutral
                };

                const string HomeTeamName = "Home";
                const string AwayTeamName = "Away";
                _cacheRefAdapter.GetTeamName(metaModel.HomeTeamId).Returns(HomeTeamName);
                _cacheRefAdapter.GetTeamName(metaModel.AwayTeamId).Returns(AwayTeamName);

                var expectedHomeName = HomeTeamName + (isNeutral ? " (n)" : string.Empty);

                const string TeamName = "";
                _cacheRefAdapter.GetTeamName(0).Returns(TeamName);

                NewForecastHdpOuMatchInfo headerInfo = null;
                var mainData = new NewForecastHdpOuData();
                _forecastService.GetNewForecastData(
                    Arg.Any<NewForecastHandicapOverUnderMetaModel>(),
                    Arg.Do<NewForecastHdpOuMatchInfo>(x => headerInfo = x)).Returns(mainData);

                // Action
                var result = controler.Index(metaModel) as ViewResult;
                var viewModel = (NewForecastHdpOuViewModel)result.Model;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(result.ViewName, Is.EqualTo("~/Views/Forecast/NewForecastHdpOu/Main.cshtml"));

                    AssertViewModel(viewModel, expectedHomeName, AwayTeamName, mainData, headerInfo);
                });
            }
        }

        [Test]
        public void GetMatchListByLeague_WhenCall_ReturnCorrectViewResult()
        {
            using (var controler = CreateController())
            {
                // Arrange
                _matchService.GetForecastMatchListByLeague(0).Returns(new List<Match>
                {
                    new Match { MatchName = "A" },
                    new Match { MatchName = "C" },
                    new Match { MatchName = "B" }
                });

                var expectedMatchNames = "A,B,C";

                // Action
                var result = controler.GetMatchListByLeague(0) as ViewResult;
                var viewModel = (List<Match>)result.Model;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(result.ViewName, Is.EqualTo("~/Views/Forecast/NewForecastHdpOu/MatchOptionList.cshtml"));
                    Assert.That(string.Join(",", viewModel.Select(x => x.MatchName)), Is.EqualTo(expectedMatchNames));
                });
            }
        }

        private void AssertViewModel(
            NewForecastHdpOuViewModel viewModel,
            string homeTeamName,
            string awayTeamName,
            NewForecastHdpOuData mainData,
            NewForecastHdpOuMatchInfo headerInfo)
        {
            Assert.That(viewModel.Param.LicenseeId, Is.EqualTo(DefaultLicenseeSession.LicenseeId));
            Assert.That(viewModel.Param.UserId, Is.EqualTo(DefaultUserSession.UserId));
            Assert.That(viewModel.Param.HomeTeamName, Is.EqualTo(homeTeamName));
            Assert.That(viewModel.Param.AwayTeamName, Is.EqualTo(awayTeamName));
            Assert.That(viewModel.MainData, Is.EqualTo(mainData));
            Assert.That(viewModel.HeaderInfo, Is.EqualTo(headerInfo));
        }

        private NewHandicapOverUnderForecastController CreateController()
        {
            _forecastService = Substitute.For<IForecastService>();
            _cacheRefAdapter = Substitute.For<ICacheRefAdapter>();
            _matchService = Substitute.For<IMatchService>();

            var controller = new NewHandicapOverUnderForecastController(
                LangService,
                SessionService,
                AppSettings,
                _forecastService,
                Substitute.For<ICoreLogger>(),
                _cacheRefAdapter,
                _matchService)
            {
                ControllerContext = ControllerContext
            };

            return controller;
        }
    }
}