namespace Fanex.Athena.Controllers.TotalBets.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Base.UnitTests;
    using Constant.DataDomain;
    using DataAccess.Core.Common.Interfaces;
    using Model.Core.DTO.Common;
    using Model.Core.MetaModel.Common;
    using Model.Report.ViewModel;
    using NSubstitute;
    using NUnit.Framework;
    using Report;
    using Service.Core.Common.Interfaces;
    using UnitTest.Common.Helpers;

    [TestFixture]
    public class WinLossByLeagueReportControllerTest : CultureBaseControllerHelperTest
    {
        private ISportTypeDataAccess _sportTypeDataAccess;

        private ILeagueDataAccess _leagueDataAccess;

        private IMatchService _matchService;

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();
            _sportTypeDataAccess = Substitute.For<ISportTypeDataAccess>();
            _leagueDataAccess = Substitute.For<ILeagueDataAccess>();
            _matchService = Substitute.For<IMatchService>();
            SetUpDefaultFakeData();
        }

        [Test]
        public async Task Index_WhenCall_ReturnCorrectViewResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                const int LeagueId = 2;
                var sports = new List<SportProductItem>
                {
                    new SportProductItem { ID = 1, Name = "A", ParentId = 0 }
                };
                var leagues = new List<League>
                {
                    new League { LeagueId = LeagueId, LeagueName = "B" }
                };
                var matches = new List<Match>
                {
                    new Match { MatchId = 3, MatchName = "C" }
                };
                _sportTypeDataAccess.GetSportsAndProducts(
                    SessionService.LicenseeSession.LicenseeId,
                    SessionService.UserSession.Language,
                    Product.SB).Returns(sports);

                _leagueDataAccess.GetLeaguesBySport(Arg.Any<LeagueMetaModel>()).Returns(leagues);
                _matchService.GetMatchesByLeague(LeagueId, Arg.Any<DateTime>(), Arg.Any<DateTime>()).Returns(matches);

                // Action
                var viewResult = await controller.Index() as ViewResult;
                var viewModel = viewResult.Model as WinLossByLeagueViewModel;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(viewResult.ViewName, Is.EqualTo("~/Views/Desktop/Report/WinLossByLeague/Main.cshtml"));
                    Assert.That(viewModel.Sports, Is.EqualTo(sports.ToDictionary(x => x.ID, y => y.Name)));
                    Assert.That(viewModel.Leagues, Is.EqualTo(leagues.ToDictionary(x => x.LeagueId, y => y.LeagueName)));
                    Assert.That(viewModel.Matches, Is.EqualTo(matches.ToDictionary(x => x.MatchId, y => y.MatchName)));
                });
            }
        }

        [Test]
        public async Task GetLeaguesBySport_WhenCall_ReturnCorrectResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                const string AgentListXml = "<xml>";
                var metaModel = new LeagueMetaModel
                {
                    CurrencyId = 111
                };
                SessionService.GenerateAgentListXml(metaModel.CurrencyId).Returns(AgentListXml);
                var data = new List<League>
                {
                    new League { LeagueId = 1, LeagueName = "League1" }
                };
                _leagueDataAccess.GetLeaguesBySport(metaModel).Returns(data);

                // Action
                var result = await controller.GetLeaguesBySport(metaModel) as JsonResult;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(metaModel.AgentListXml, Is.EqualTo(AgentListXml));
                    Assert.That(ObjectHelper.ToJson(result.Data), Is.EqualTo(@"[{""Key"":1,""Value"":""League1""}]"));
                });
            }
        }

        [Test]
        public async Task GetMatchesByLeague_WhenCall_ReturnCorrectResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                const int LeagueId = 1;
                var fromDate = new DateTime(2018, 1, 1);
                var toDate = new DateTime(2018, 1, 2);

                var data = new List<Match>
                {
                    new Match { MatchId = 2, MatchName = "abc" }
                };
                _matchService.GetMatchesByLeague(LeagueId, fromDate, toDate).Returns(data);

                // Action
                var result = await controller.GetMatchesByLeague(LeagueId, fromDate, toDate) as JsonResult;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(
                        ObjectHelper.ToJson(result.Data),
                        Is.EqualTo(@"[{""Key"":0,""Value"":""All""},{""Key"":2,""Value"":""abc""}]"));
                });
            }
        }

        private WinLossByLeagueReportController CreateController()
        {
            var productService = Substitute.For<IProductService>();
            productService.GetProductAccess(
                SessionService.LicenseeSession.LicenseeId,
                SessionService.UserSession.Language).Returns(new ProductAccess(new List<Product>()));
            return new WinLossByLeagueReportController(
                LangService,
                SessionService,
                AppSettings,
                productService,
                _sportTypeDataAccess,
                _leagueDataAccess,
                _matchService);
        }
    }
}