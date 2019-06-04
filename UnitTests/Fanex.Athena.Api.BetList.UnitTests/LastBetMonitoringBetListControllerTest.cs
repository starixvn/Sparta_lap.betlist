namespace Fanex.Athena.Api.BetList.UnitTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Base.UnitTests;
    using Model.BetList.MetaModel;
    using Model.Core.DTO.Common;
    using NSubstitute;
    using NUnit.Framework;
    using Service.Core.Common.Interfaces;
    using Service.Core.Utility.Interfaces;
    using VendorAdapter.Interfaces;

    public class LastBetMonitoringBetListControllerTest : BaseApiControllerHelperTest
    {
        private const string ApiPath = "/LastBetMonitoringBetList/GetData";

        private IMatchService _matchService;

        private ICacheRefAdapter _cacheRefAdapter;

        private IBetListApiClient _betListApiClient;

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();

            _betListApiClient = Substitute.For<IBetListApiClient>();
            _cacheRefAdapter = Substitute.For<ICacheRefAdapter>();
            _matchService = Substitute.For<IMatchService>();

            MockSessionData();
        }

        [OneTimeTearDown]
        public override void FixtureTearDown()
        {
            base.FixtureTearDown();

            _betListApiClient = null;
        }

        [Test]
        public async Task GetRunningMatchList_WhenCall_ReturnCorrectResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                const int SportId = 1;
                var matchList = new List<Match>
                {
                    new Match { MatchName = "Match3", IsOutRight = false, LeagueId = 2 },
                    new Match { MatchName = "Match5", IsOutRight = false, LeagueId = 2 },
                    new Match { MatchName = "Match6", IsOutRight = false, LeagueId = 1 },
                    new Match { MatchName = "Match1", IsOutRight = true, LeagueId = 4 },
                    new Match { MatchName = "Match4", IsOutRight = true, LeagueId = 3 },
                    new Match { MatchName = "Match2", IsOutRight = true, LeagueId = 5 },
                };
                _matchService.GetAllRunningMatch(SportId, Arg.Any<string>()).Returns(matchList);

                _cacheRefAdapter.GetLeagueName(Arg.Any<int>()).Returns(x => { return x[0].ToString(); });

                var expectedMatchList = "Match1,Match2,Match3,Match4,Match5,Match6";
                var expectedLeagueList = "1,2,3,4,5";

                // Action
                var result = await controller.GetRunningMatchList(SportId);

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(string.Join(",", ((IOrderedEnumerable<Match>)result.MatchList).Select(x => x.MatchName)), Is.EqualTo(expectedMatchList));
                    Assert.That(string.Join(",", ((IOrderedEnumerable<League>)result.LeagueList).Select(x => x.LeagueName)), Is.EqualTo(expectedLeagueList));
                });
            }
        }

        [Test]
        public async Task Index_NoCurrency_GenerateAgentListXmlCorrectly()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new LastBetsMonitoringMetaModel { Currency = string.Empty };

                const string AgentListXml = "AgentListXmlWithoutCurrency";
                SessionService.GenerateAgentListXml(0, false).Returns(AgentListXml);

                var expect = new HttpResponseMessage();
                _betListApiClient.PostAsync(ApiPath, metaModel, SessionLanguage).Returns(Task.FromResult(expect));

                // Action
                var actual = await controller.Index(metaModel);

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(expect, Is.EqualTo(actual));
                    Assert.That(metaModel.AgentListXml, Is.EqualTo(AgentListXml));
                    _betListApiClient.Received().PostAsync(ApiPath, metaModel, SessionLanguage);
                });
            }
        }

        [Test]
        public async Task Index_HasCurrency_GenerateAgentListXmlCorrectly()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new LastBetsMonitoringMetaModel { Currency = "1,2" };

                const string AgentListXml = "AgentListXmlWithCurrency";
                SessionService.GenerateAgentListXml(Arg.Any<IEnumerable<int>>(), false).Returns(AgentListXml);

                var expect = new HttpResponseMessage();
                _betListApiClient.PostAsync(ApiPath, metaModel, SessionLanguage).Returns(Task.FromResult(expect));

                // Action
                var actual = await controller.Index(metaModel);

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(expect, Is.EqualTo(actual));
                    Assert.That(metaModel.AgentListXml, Is.EqualTo(AgentListXml));
                    _betListApiClient.Received().PostAsync(ApiPath, metaModel, SessionLanguage);
                });
            }
        }

        private LastBetMonitoringBetListController CreateController()
        {
            return new LastBetMonitoringBetListController(SessionService, ProductService, LangService, _betListApiClient, _matchService, _cacheRefAdapter);
        }
    }
}