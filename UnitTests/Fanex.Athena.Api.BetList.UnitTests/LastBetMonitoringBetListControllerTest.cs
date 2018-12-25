namespace Fanex.Athena.Api.BetList.UnitTests
{
    using System.Collections.Generic;
    using System.Linq;
    using Base.UnitTests;
    using Constant.DataDomain;
    using Fanex.BetList.Core.Entities;
    using Model.BetList.DTO;
    using Model.BetList.MetaModel;
    using Model.Core.DTO.Common;
    using NSubstitute;
    using NUnit.Framework;
    using Resources;
    using Service.BetList.Interfaces;
    using Service.Core.Common.Interfaces;
    using VendorAdapter.Interfaces;

    public class LastBetMonitoringBetListControllerTest : BaseApiControllerHelperTest
    {
        private IMatchService _matchService;

        private ILastBetsMonitoringBetListService _service;

        private ITicketHelper _ticketHelper;

        private ICacheRefAdapter _cacheRefAdapter;

        private LastBetMonitoringBetListController _controller;

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();

            _matchService = Substitute.For<IMatchService>();
            _service = Substitute.For<ILastBetsMonitoringBetListService>();
            _ticketHelper = Substitute.For<ITicketHelper>();
            _cacheRefAdapter = Substitute.For<ICacheRefAdapter>();

            _controller = Substitute.ForPartsOf<LastBetMonitoringBetListController>(
                SessionService,
                ProductService,
                LangService,
                _matchService,
                _service,
                _ticketHelper,
                _cacheRefAdapter);
        }

        [OneTimeTearDown]
        public override void FixtureTearDown()
        {
            base.FixtureTearDown();

            _matchService = null;
            _service = null;
            _ticketHelper = null;
            _cacheRefAdapter = null;
            _controller = null;
        }

        [Test]
        public void GetRunningMatchList_WhenCall_ReturnCorrectResult()
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
            _matchService.GetAllRunningMatch(SportId).Returns(matchList);

            _cacheRefAdapter.GetLeagueName(Arg.Any<int>()).Returns(x => { return x[0].ToString(); });

            var expectedMatchList = "Match1,Match2,Match3,Match4,Match5,Match6";
            var expectedLeagueList = "1,2,3,4,5";

            // Action
            var result = _controller.GetRunningMatchList(SportId);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(string.Join(",", ((IOrderedEnumerable<Match>)result.MatchList).Select(x => x.MatchName)), Is.EqualTo(expectedMatchList));
                Assert.That(string.Join(",", ((IOrderedEnumerable<League>)result.LeagueList).Select(x => x.LeagueName)), Is.EqualTo(expectedLeagueList));
            });
        }

        [Test]
        public void Index_NoCurrency_GenerateAgentListXmlCorrectly()
        {
            // Arrange
            var metaModel = new LastBetsMonitoringMetaModel { Currency = string.Empty };

            const string AgentListXml = "AgentListXmlWithoutCurrency";

            SessionService.GenerateAgentListXml(0, false).Returns(AgentListXml);

            _service.GetLastBetsMonitoringData(metaModel, UserCurrencyList).Returns(new TicketResult());

            // Action
            _controller.Index(metaModel);

            // Assert
            Assert.That(metaModel.AgentListXml, Is.EqualTo(AgentListXml));
        }

        [Test]
        public void Index_HasCurrency_GenerateAgentListXmlCorrectly()
        {
            // Arrange
            var metaModel = new LastBetsMonitoringMetaModel { Currency = "1,2" };

            const string AgentListXml = "AgentListXmlWithCurrency";

            SessionService.GenerateAgentListXml(Arg.Any<IEnumerable<int>>(), false).Returns(AgentListXml);

            _service.GetLastBetsMonitoringData(metaModel, UserCurrencyList).Returns(new TicketResult());

            // Action
            _controller.Index(metaModel);

            // Assert
            Assert.That(metaModel.AgentListXml, Is.EqualTo(AgentListXml));
        }

        [Test]
        public void Index_NoTicketResult_ReturnNoNewRow()
        {
            // Arrange
            var metaModel = new LastBetsMonitoringMetaModel { Currency = "1,2" };

            var ticketResult = new TicketResult();
            SetUpForIndexMethod(metaModel, ticketResult);

            // Action
            var result = _controller.Index(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.NewRow, Is.EqualTo(false));
            });
        }

        [Test]
        public void Index_HasTicketResult_ReturnNewRow()
        {
            // Arrange
            var metaModel = new LastBetsMonitoringMetaModel { Currency = "1,2" };

            var ticketResult = new TicketResult();
            ticketResult.MainTicketList.Add(new LrfTicket { TransId = 1, BetFrom = string.Empty });
            SetUpForIndexMethod(metaModel, ticketResult);

            // Action
            var result = _controller.Index(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.NewRow, Is.EqualTo(true));
                Assert.That(result.LastTransId, Is.EqualTo(1));
            });
        }

        [Test]
        public void GetLastMonitorBetListContent_GetClientDetailSystemParlayFunction_IsNotCombinationMixParlay_ReturnEmptyString()
        {
            // Arrange
            var metaModel = new LastBetsMonitoringMetaModel { Currency = "1,2" };

            var ticketResult = new TicketResult();
            ticketResult.MainTicketList.Add(new LrfTicket { TransId = 1, BetFrom = string.Empty });
            SetUpForIndexMethod(metaModel, ticketResult);

            // Action
            var result = _controller.Index(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(_ticketHelper.ClientDetailSystemParlayFunction, Is.EqualTo(string.Empty));
            });
        }

        [TestCase("a", nameof(ViewResources.China))]
        [TestCase("z", nameof(ViewResources.China))]
        [TestCase("d", nameof(ViewResources.Asia))]
        [TestCase("g", nameof(ViewResources.Asia))]
        [TestCase("e", nameof(ViewResources.Euro))]
        [TestCase("u", nameof(ViewResources.Euro))]
        [TestCase("y", nameof(ViewResources.Euro))]
        [TestCase("m", nameof(ViewResources.Wap))]
        [TestCase("s", nameof(ViewResources.Mobile))]
        [TestCase("l", nameof(ViewResources.Mobile))]
        [TestCase("r", nameof(ViewResources.Tablet))]
        [TestCase("x", nameof(ViewResources.NewAsia))]
        [TestCase("Others", nameof(ViewResources.Others))]
        public void GetLastMonitorBetListContent_GetPlatformText_CorrectPlatFormCode_ReturnCorrectText(string betFrom, string expectedText)
        {
            // Arrange
            var metaModel = new LastBetsMonitoringMetaModel { Currency = "1,2", OddsType = "Type" };

            var ticketResult = new TicketResult();
            var lrfTicket = new LrfTicket { TransId = 1, BetFrom = betFrom, BetTypeId = 1, Odds = 1 };
            ticketResult.MainTicketList.Add(lrfTicket);
            SetUpForIndexMethod(metaModel, ticketResult);

            var expectedPlatform = $"<td data-name='Platform'>{ViewResources.ResourceManager.GetString(expectedText)}</td>";

            // Action
            var result = _controller.Index(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Rows.Contains(expectedPlatform));
            });
        }

        [TestCase("usd", "usd", "ComboName", "BetType", "BetType - ComboName")]
        [TestCase("abc", "", "", "BetType", "BetType")]
        public void GetLastMonitorBetListContent_GetClientDetailSystemParlayFunction_IsCombinationMixParlay_ReturnCorrectString(
            string baseCurrency,
            string expectedBaseCurrency,
            string comboName,
            string betTypeName,
            string expectedBetTeam)
        {
            // Arrange
            var metaModel = new LastBetsMonitoringMetaModel { Currency = "1,2", BasedCurrency = baseCurrency };

            var ticketResult = new TicketResult();
            var lrfTicket = new LrfTicket { TransId = 1, BetFrom = string.Empty, RefNo = "RefNo", BetId = 2, BetTypeId = (int)BetType.CombinationMixParlay };
            ticketResult.MainTicketList.Add(lrfTicket);
            SetUpForIndexMethod(metaModel, ticketResult);

            _ticketHelper.GetCombinationMPOfferName(lrfTicket.BetTeam).Returns(comboName);
            _ticketHelper.GetBetTypeNameById(lrfTicket.BetTypeId).Returns(betTypeName);

            var expectedResult = $"showCombMPDetail({lrfTicket.TransId},{lrfTicket.RefNo},{lrfTicket.BetId},0,&quot;"
                + $"{expectedBetTeam}&quot;,&quot;{expectedBaseCurrency}&quot;)";

            // Action
            var result = _controller.Index(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(_ticketHelper.ClientDetailSystemParlayFunction, Is.EqualTo(expectedResult));
            });
        }

        [TestCase(1, 1.5, "")]
        [TestCase(2, 2, "")]
        [TestCase(7, -1, "negative")]
        [TestCase(12, -3, "negative")]
        [TestCase(20, -4, "negative")]
        public void GetLastMonitorBetListContent_BuildOdds_IsDecimalOddsBetType_ReturnCorrectString(int betTypeId, decimal odds, string oddClass)
        {
            // Arrange
            var metaModel = new LastBetsMonitoringMetaModel { Currency = "1,2", OddsType = "Type" };

            var ticketResult = new TicketResult();
            var lrfTicket = new LrfTicket { TransId = 1, BetFrom = string.Empty, BetTypeId = betTypeId, Odds = odds };
            ticketResult.MainTicketList.Add(lrfTicket);
            SetUpForIndexMethod(metaModel, ticketResult);

            var expectedOddClass = $"<td data-name=\"Odds\" class=\"bl-odds\"><span class=\"underdog {oddClass}\">";
            var expectedAddType = $"</span><br><span class=\"oddstype\">{metaModel.OddsType}</span></td>";

            // Action
            var result = _controller.Index(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Rows.Contains(expectedOddClass));
                Assert.That(result.Rows.Contains(expectedAddType));
            });
        }

        [TestCase(13)]
        public void GetLastMonitorBetListContent_BuildOdds_IsNotDecimalOddsBetType_ReturnCorrectString(int betTypeId)
        {
            // Arrange
            var metaModel = new LastBetsMonitoringMetaModel { Currency = "1,2", OddsType = "Type" };

            var ticketResult = new TicketResult();
            var lrfTicket = new LrfTicket { TransId = 1, BetFrom = string.Empty, BetTypeId = betTypeId, Odds = 1 };
            ticketResult.MainTicketList.Add(lrfTicket);
            SetUpForIndexMethod(metaModel, ticketResult);

            var expectedOddTypeName = "Test Odd Type Name";

            _cacheRefAdapter.GetOddsTypeName(Arg.Any<object>()).Returns(expectedOddTypeName);

            // Action
            var result = _controller.Index(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Rows.Contains(expectedOddTypeName));
            });
        }

        [TestCase(true, "#cc0033", nameof(ViewResources.Live))]
        [TestCase(false, "#000", nameof(ViewResources.NoneLive))]
        public void GetLastMonitorBetListContent_GetLasBetMonitoringStatus_IsNotMixParlay_ReturnCorrectString(bool isLive, string expectedColor, string expectedTitle)
        {
            // Arrange
            var metaModel = new LastBetsMonitoringMetaModel { Currency = "1,2", OddsType = "Type" };

            var ticketResult = new TicketResult();
            ticketResult.MainTicketList.Add(new LrfTicket { TransId = 1, BetFrom = string.Empty, Odds = 1, BetTypeId = 1, IsLive = isLive });
            SetUpForIndexMethod(metaModel, ticketResult);

            var expectedStatus = $"<label style='color:{expectedColor}'>{ViewResources.ResourceManager.GetString(expectedTitle)}</label>";

            // Action
            var result = _controller.Index(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Rows.Contains(expectedStatus));
            });
        }

        [TestCase(1, "#cc0033", nameof(ViewResources.Live))]
        [TestCase(2, "#0033ff", nameof(ViewResources.MixLiveAndNoneLive))]
        [TestCase(3, "#000", nameof(ViewResources.NoneLive))]
        public void GetLastMonitorBetListContent_GetLasBetMonitoringStatus_IsMixParlay_ReturnCorrectString(
            int liveHomeScore, 
            string expectedColor, 
            string expectedTitle)
        {
            // Arrange
            var metaModel = new LastBetsMonitoringMetaModel { Currency = "1,2" };

            var ticketResult = new TicketResult();
            ticketResult.MainTicketList.Add(new LrfTicket { TransId = 1, BetFrom = string.Empty, BetTypeId = (int)BetType.MixParlay, LiveHomeScore = liveHomeScore });
            SetUpForIndexMethod(metaModel, ticketResult);

            var expectedStatus = $"<label style='color:{expectedColor}'>{ViewResources.ResourceManager.GetString(expectedTitle)}</label>";

            // Action
            var result = _controller.Index(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Rows.Contains(expectedStatus));
            });
        }

        private void SetUpForIndexMethod(LastBetsMonitoringMetaModel metaModel, TicketResult ticketResult)
        {
            const string AgentListXml = "AgentListXmlWithCurrency";

            SessionService.GenerateAgentListXml(Arg.Any<IEnumerable<int>>(), false).Returns(AgentListXml);

            _service.GetLastBetsMonitoringData(metaModel, UserCurrencyList).Returns(ticketResult);
        }
    }
}