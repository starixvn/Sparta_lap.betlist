namespace Fanex.Athena.Api.Common.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Base;
    using DataAccess.Core.Common.Interfaces;
    using Model.Core.DTO.Common;
    using Model.Core.MetaModel.Common;
    using NSubstitute;
    using NUnit.Framework;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;
    using Session.Model;
    using UnitTest.Common.Helpers;

    public class CommonDataApiControllerTest
    {
        private ISportTypeDataAccess _sportTypeDataAccess = Substitute.For<ISportTypeDataAccess>();

        private ISessionService _sessionService = Substitute.For<ISessionService>();

        private ILeagueDataAccess _leagueDataAccess = Substitute.For<ILeagueDataAccess>();

        private IMatchDataAccess _matchDataAccess = Substitute.For<IMatchDataAccess>();

        private IBetTypeService _betTypeService = Substitute.For<IBetTypeService>();

        [OneTimeSetUp]
        public void FixtureSetUp()
        {
            _sessionService.LicenseeSession.Returns(new LicenseeSession { LicenseeId = FakeHelper.Create<int>() });
            _sessionService.UserSession.Returns(new UserSession { UserId = FakeHelper.Create<int>(), Language = FakeHelper.Create<string>() });
            _sessionService.GenerateAgentListXml(Arg.Any<int>()).Returns(FakeHelper.Create<string>());
        }

        [OneTimeTearDown]
        public void FixtureTearDown()
        {
            _sportTypeDataAccess = null;
            _sessionService = null;
            _leagueDataAccess = null;
            _matchDataAccess = null;
            _sessionService = null;
        }

        [Test]
        public void GetSports_WhenCall_ReturnCorrectData()
        {
            using (var controller = CreateController())
            {
                // arrange
                var productCode = FakeHelper.Create<string>();
                var parent = new SportProductItem { ParentId = -1, ID = FakeHelper.Create<int>(), Name = FakeHelper.Create<string>() };
                var child = new SportProductItem { ParentId = parent.ID, ID = FakeHelper.Create<int>(), Name = FakeHelper.Create<string>() };

                _sportTypeDataAccess.GetSportsAndProducts(
                 _sessionService.LicenseeSession.LicenseeId,
                 _sessionService.UserSession.Language,
                 productCode).Returns(new List<SportProductItem> { parent, child });

                // action
                var result = controller.GetSports(productCode).ElementAt(0);

                // assert
                Assert.Multiple(() =>
                {
                    Assert.That(result.GetType().GetProperty("value").GetValue(result, null), Is.EqualTo(child.ID));
                    Assert.That(result.GetType().GetProperty("label").GetValue(result, null), Is.EqualTo(child.Name));
                });
            }
        }

        [Test]
        public void GetBetTypes_WhenCall_ReturnCorrectData()
        {
            using (var controller = CreateController())
            {
                // arrange
                var productCode = FakeHelper.Create<string>();
                var item = new CheckedItem { ID = FakeHelper.Create<int>(), Name = FakeHelper.Create<string>() };

                _betTypeService.GetBettypesByProduct(
                 _sessionService.LicenseeSession.LicenseeId,
                 productCode,
                 _sessionService.UserSession.Language)
                 .Returns(new List<CheckedItem> { item });

                // action
                var result = controller.GetBetTypes(productCode).ElementAt(0);

                // assert
                Assert.Multiple(() =>
                {
                    Assert.That(result.GetType().GetProperty("value").GetValue(result, null), Is.EqualTo(item.ID));
                    Assert.That(result.GetType().GetProperty("label").GetValue(result, null), Is.EqualTo(item.Name));
                });
            }
        }

        [Test]
        public void GetLeagues_WhenCall_ReturnCorrectData()
        {
            using (var controller = CreateController())
            {
                // arrange
                var productCode = FakeHelper.Create<string>();
                var league = new League { LeagueId = FakeHelper.Create<int>(), LeagueName = FakeHelper.Create<string>() };

                _leagueDataAccess.GetLeaguesBySport(Arg.Any<LeagueMetaModel>())
                    .Returns(new List<League> { league });

                // action
                var result = controller.GetLeagues(
                    FakeHelper.Create<DateTime>(),
                    FakeHelper.Create<DateTime>(),
                    FakeHelper.Create<int>()).ElementAt(0);

                // assert
                Assert.Multiple(() =>
                {
                    Assert.That(result.GetType().GetProperty("value").GetValue(result, null), Is.EqualTo(league.LeagueId));
                    Assert.That(result.GetType().GetProperty("label").GetValue(result, null), Is.EqualTo(league.LeagueName));
                });
            }
        }

        [Test]
        public void GetMatchs_WhenCall_ReturnCorrectData()
        {
            using (var controller = CreateController())
            {
                // arrange
                var productCode = FakeHelper.Create<string>();
                var match = new Match { MatchId = FakeHelper.Create<int>(), MatchName = FakeHelper.Create<string>() };
                var fromDate = FakeHelper.Create<DateTime>();
                var toDate = FakeHelper.Create<DateTime>();
                var leagueId = FakeHelper.Create<int>();

                _matchDataAccess.GetMatchesByLeague(leagueId, fromDate, toDate)
                    .Returns(new List<Match> { match });

                // action
                var result = controller.GetMatches(fromDate, toDate, leagueId).ElementAt(0);

                // assert
                Assert.Multiple(() =>
                {
                    Assert.That(result.GetType().GetProperty("value").GetValue(result, null), Is.EqualTo(match.MatchId));
                    Assert.That(result.GetType().GetProperty("label").GetValue(result, null), Is.EqualTo(match.MatchName));
                });
            }
        }

        private CommonDataApiController CreateController()
        {
            return new CommonDataApiController(
                Substitute.For<ILanguageService>(),
                _sportTypeDataAccess,
                _sessionService,
                _leagueDataAccess,
                _matchDataAccess,
                _betTypeService);
        }
    }
}