namespace Fanex.Athena.DataAccess.BetList.UnitTests.Util
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using BetList.Util;
    using Data;
    using Model.BetList.MetaModel;
    using Model.Core.DataDomain;
    using NSubstitute;
    using NUnit.Framework;
    using UnitTest.Common.Helpers;

    [TestFixture]
    public class BetListParameterBuilderTest
    {
        private readonly BetListMetaModel _metaModel;
        private IObjectDb _objectDb;

        public BetListParameterBuilderTest()
        {
            _metaModel = FakeHelper.Create<BetListMetaModel>();
            _metaModel.ProductCodeList = "SB,NG";
        }

        [SetUp]
        public void SetUp()
        {
            _objectDb = Substitute.For<IObjectDb>();
        }

        [TearDown]
        public void TearDown()
        {
            _objectDb = null;
        }

        [Test]
        public void CreateBetListParameters_CalledByWinlossByLeague_ParametersPassedCorrectly()
        {
            // Arrange
            const string UspKey = "Lrf_BetList_WinlostByLeague_Mem";
            var metaModel = FakeHelper.Create<WinLossByLeagueBetListMetaModel>(x => x.ProductCodeList, "SB,NG");

            // Action
            BetListParameterBuilder.CreateBetListParameters(_objectDb, UspKey, metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                _objectDb.Received(1).AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
                _objectDb.Received(1).AddInParameter("CustID", DbType.Int32, metaModel.CustId);
                _objectDb.Received(1).AddInParameter("LeagueID", DbType.Int32, metaModel.LeagueId);
                _objectDb.Received(1).AddInParameter("MatchId", DbType.Int32, metaModel.MatchId);
                _objectDb.Received(1).AddInParameter("Odds", DbType.Decimal, metaModel.Odds);
                AssertDateRangeParameters(_objectDb, metaModel);
                AssertMoveBetParams(_objectDb, metaModel);
                _objectDb.Received(1).AddInParameter("ShowWin", DbType.Boolean, metaModel.ShowWin);
                _objectDb.Received(1).AddInParameter("ShowLoss", DbType.Boolean, metaModel.ShowLoss);
                _objectDb.Received(1).AddInParameter("ShowDraw", DbType.Boolean, metaModel.ShowDraw);
                _objectDb.Received(1).AddInParameter("ShowSB", DbType.Boolean, true);
                _objectDb.Received(1).AddInParameter("ShowBA", DbType.Boolean, false);
            });
        }

        [Test]
        public void CreateBetListParameters_CalledByCancelBetList_ParametersPassedCorrectly()
        {
            // Arrange
            const string UspKey = "Lrf_BetList_SelDeleted";

            // Action
            BetListParameterBuilder.CreateBetListParameters(_objectDb, UspKey, _metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                _objectDb.Received(1).AddInParameter("AgentList", DbType.String, _metaModel.AgentListXml);
                _objectDb.Received(1).AddInParameter("CustID", DbType.Int32, _metaModel.CustId);
                AssertPagingParameters(_objectDb, _metaModel);
            });
        }

        [Test]
        public void CreateBetListParameters_CalledByCorectScoreBetlist_ParametersPassedCorrectly()
        {
            // Arrange
            const string UspKey = "Lrf_BetList_CorrectScore";

            // Action
            BetListParameterBuilder.CreateBetListParameters(_objectDb, UspKey, _metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                _objectDb.Received(1).AddInParameter("AgentList", DbType.String, _metaModel.AgentListXml);
                AssertPagingParameters(_objectDb, _metaModel);
            });
        }

        [Test]
        public void CreateBetListParameters_CalledByCustomerWlBetlist_ParametersPassedCorrectly()
        {
            // Arrange
            const string UspKey = "Lrf_BetList_SelWinLossByDate_Mem";

            // Action
            BetListParameterBuilder.CreateBetListParameters(_objectDb, UspKey, _metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                _objectDb.Received(1).AddInParameter("AgentList", DbType.String, _metaModel.AgentListXml);
                _objectDb.Received(1).AddInParameter("CustID", DbType.Int32, _metaModel.CustId);
                AssertMoveBetParams(_objectDb, _metaModel);
                AssertDateRangeParameters(_objectDb, _metaModel);
                AssertPagingParameters(_objectDb, _metaModel);
                AssertSummaryParameters(_objectDb);
                _objectDb.Received(1).AddInParameter("showSB", DbType.Boolean, true);
                _objectDb.Received(1).AddInParameter("showNG", DbType.Boolean, true);
            });
        }

        [Test]
        public void CreateBetListParameters_CalledByRacingCommissionBetlist_ParametersPassedCorrectly()
        {
            // Arrange
            const string UspKey = "Lrf_BetList_SelWinLossRacingByDate_Mem";

            // Action
            BetListParameterBuilder.CreateBetListParameters(_objectDb, UspKey, _metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                _objectDb.Received(1).AddInParameter("CustID", DbType.Int32, _metaModel.CustId);
                _objectDb.Received(1).AddInParameter("AgentList", DbType.String, _metaModel.AgentListXml);
                AssertDateRangeParameters(_objectDb, _metaModel);
                AssertMoveBetParams(_objectDb, _metaModel);
                AssertPagingParameters(_objectDb, _metaModel);
                AssertSummaryParameters(_objectDb);
            });
        }

        [Test]
        public void CreateBetListParameters_CalledByDailyWLBetlist_ParametersPassedCorrectly()
        {
            // Arrange
            const string UspKey = "Lrf_BetList_SelWinlossDaily";
            var metaModel = FakeHelper.Create<DailyBetListMetaModel>();

            // Action
            BetListParameterBuilder.CreateBetListParameters(_objectDb, UspKey, metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                _objectDb.Received(1).AddInParameter("Sort", DbType.String, metaModel.SortBy + metaModel.BaseCurrency + " " + metaModel.SortOrder);
                _objectDb.Received(1).AddInParameter("date", DbType.DateTime, metaModel.Date);
                _objectDb.Received(1).AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
                _objectDb.Received(1).AddInParameter("product", DbType.String, metaModel.ProductCode);
                _objectDb.Received(1).AddInParameter("status", DbType.String, metaModel.DailyBetListStatus);
                AssertMoveBetParams(_objectDb, metaModel);
                AssertPagingParameters(_objectDb, metaModel);
                AssertSummaryParameters(_objectDb);
            });
        }

        [Test]
        public void CreateBetListParameters_CalledByDailyRacingWLBetlist_ParametersPassedCorrectly()
        {
            // Arrange
            const string UspKey = "Lrf_BetList_SelWinlostRacing";

            // Action
            BetListParameterBuilder.CreateBetListParameters(_objectDb, UspKey, _metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                _objectDb.Received(1).AddInParameter("date", DbType.DateTime, _metaModel.Date);
                _objectDb.Received(1).AddInParameter("AgentList", DbType.String, _metaModel.AgentListXml);
                _objectDb.Received(1).AddInParameter("leagueid", DbType.Int32, _metaModel.LeagueId);
                _objectDb.Received(1).AddInParameter("raceNo", DbType.String, _metaModel.RaceNo);
                AssertPagingParameters(_objectDb, _metaModel);
                AssertSummaryParameters(_objectDb);
                _objectDb.Received(1).AddOutParameter("SumStake", DbType.Currency, 8);
                _objectDb.Received(1).AddOutParameter("SumStakeUSD", DbType.Currency, 8);
                _objectDb.Received(1).AddOutParameter("SumStakeEUR", DbType.Currency, 8);
                _objectDb.Received(1).AddOutParameter("SumStakeGBP", DbType.Currency, 8);
            });
        }

        [Test]
        public void CreateBetListParameters_CalledByLicenseeWLBetlist_ParametersPassedCorrectly()
        {
            // Arrange
            const string UspKey = "Lrf_BetList_SelWinloss_Mem";

            // Action
            BetListParameterBuilder.CreateBetListParameters(_objectDb, UspKey, _metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                _objectDb.Received(1).AddInParameter("AgentList", DbType.String, _metaModel.AgentListXml);
                _objectDb.Received(1).AddInParameter("custid", DbType.Int32, _metaModel.CustId);
                AssertDateRangeParameters(_objectDb, _metaModel);
                AssertMoveBetParams(_objectDb, _metaModel);
                _objectDb.Received(1).AddInParameter("showSB", DbType.Boolean, true);
                _objectDb.Received(1).AddInParameter("showNG", DbType.Boolean, true);
                AssertPagingParameters(_objectDb, _metaModel);
                AssertSummaryParameters(_objectDb);
                _objectDb.Received(1).AddInParameter("drawfilter", DbType.Int16, _metaModel.DrawFilter);
            });
        }

        [Test]
        public void CreateBetListParameters_CalledByRunningBetListByBettype_ParametersPassedCorrectly()
        {
            // Arrange
            const string UspKey = "Lrf_BetList_SelRunningByBettype";

            // Action
            BetListParameterBuilder.CreateBetListParameters(_objectDb, UspKey, _metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                _objectDb.Received(1).AddInParameter("AgentList", DbType.String, _metaModel.AgentListXml);
                _objectDb.Received(1).AddInParameter("bettype", DbType.Int32, _metaModel.BetType);
                AssertPagingParameters(_objectDb, _metaModel);
            });
        }

        [Test]
        public void CreateBetListParameters_CalledByRunningBetListByMultipleBettype_ParametersPassedCorrectly()
        {
            // Arrange
            const string UspKey = "Lrf_BetList_SelRunningByBettypeList";

            // Action
            BetListParameterBuilder.CreateBetListParameters(_objectDb, UspKey, _metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                _objectDb.Received(1).AddInParameter("AgentList", DbType.String, _metaModel.AgentListXml);
                _objectDb.Received(1).AddInParameter("bettype", DbType.String, _metaModel.MultipleBettype);
                AssertPagingParameters(_objectDb, _metaModel);
            });
        }

        [Test]
        public void CreateBetListParameters_CalledByMatchWLRacingBetlist_ParametersPassedCorrectly()
        {
            // Arrange
            const string UspKey = "Lrf_BetList_MatchWL_Racing";

            // Action
            BetListParameterBuilder.CreateBetListParameters(_objectDb, UspKey, _metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                _objectDb.Received(1).AddInParameter("AgentList", DbType.String, _metaModel.AgentListXml);
                _objectDb.Received(1).AddInParameter("Bettype", DbType.Int32, _metaModel.BetType);
                _objectDb.Received(1).AddInParameter("LeagueId", DbType.Int32, _metaModel.LeagueId);
                _objectDb.Received(1).AddInParameter("RaceNo", DbType.String, _metaModel.RaceNo);
                AssertDateRangeParameters(_objectDb, _metaModel);
                _objectDb.Received(1).AddInParameter("MoveBetsDay", DbType.DateTime, _metaModel.MoveBetsDay);
                _objectDb.Received(1).AddInParameter("Sort", DbType.String, _metaModel.SortBy + _metaModel.BaseCurrency + " " + _metaModel.SortOrder);
                AssertPagingParameters(_objectDb, _metaModel);
                AssertSummaryParameters(_objectDb);
            });
        }

        [TestCase(2, 2)]
        [TestCase(null, -1)]
        public void CreateBetListParameters_CalledByMatchWLBettypeBetlist_ParametersPassedCorrectly(int? liveIndicator, int expectedLiveIndicator)
        {
            // Arrange
            const string UspKey = "Lrf_BetList_MatchWL_Bettype";

            _metaModel.LiveIndicator = liveIndicator;

            // Action
            BetListParameterBuilder.CreateBetListParameters(_objectDb, UspKey, _metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                _objectDb.Received(1).AddInParameter("AgentList", DbType.String, _metaModel.AgentListXml);
                _objectDb.Received(1).AddInParameter("bettype", DbType.Int32, _metaModel.BetType);
                _objectDb.Received(1).AddInParameter("MatchID", DbType.Int32, _metaModel.MatchId);
                _objectDb.Received(1).AddInParameter("liveindicator", DbType.Int32, expectedLiveIndicator);
                _objectDb.Received(1).AddInParameter("Sort", DbType.String, _metaModel.SortBy + _metaModel.BaseCurrency + " " + _metaModel.SortOrder);
                AssertPagingParameters(_objectDb, _metaModel);
                AssertSummaryParameters(_objectDb);
                _objectDb.Received(1).AddInParameter("Date", DbType.DateTime, _metaModel.Date);
                _objectDb.Received(1).AddInParameter("showSB", DbType.Boolean, true);
                _objectDb.Received(1).AddInParameter("showNG", DbType.Boolean, true);
            });
        }

        [Test]
        public void CreateBetListParameters_CalledByMatchWLOutRightBetList_ParametersPassedCorrectly()
        {
            // Arrange
            const string UspKey = "Lrf_BetList_MatchWL_Outright";

            // Action
            BetListParameterBuilder.CreateBetListParameters(_objectDb, UspKey, _metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                _objectDb.Received(1).AddInParameter("AgentList", DbType.String, _metaModel.AgentListXml);
                AssertDateRangeParameters(_objectDb, _metaModel);
                _objectDb.Received(1).AddInParameter("MoveBetsDay", DbType.DateTime, _metaModel.MoveBetsDay);
                _objectDb.Received(1).AddInParameter("Sort", DbType.String, _metaModel.SortBy + _metaModel.BaseCurrency + " " + _metaModel.SortOrder);
                AssertPagingParameters(_objectDb, _metaModel);
                AssertSummaryParameters(_objectDb);
                _objectDb.Received(1).AddInParameter("showSB", DbType.Boolean, true);
                _objectDb.DidNotReceive().AddInParameter("showNG", DbType.Boolean, true);
            });
        }

        [Test]
        public void CreateBetListParameters_CalledByMatchWLParlayBetList_ParametersPassedCorrectly()
        {
            // Arrange
            const string UspKey = "Lrf_BetList_MatchWL_Parlay";

            // Action
            BetListParameterBuilder.CreateBetListParameters(_objectDb, UspKey, _metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                _objectDb.Received(1).AddInParameter("AgentList", DbType.String, _metaModel.AgentListXml);
                _objectDb.Received(1).AddInParameter("Sort", DbType.String, _metaModel.SortBy + _metaModel.BaseCurrency + " " + _metaModel.SortOrder);
                _objectDb.Received(1).AddInParameter("bettype", DbType.Int32, _metaModel.BetType);
                _objectDb.Received(1).AddInParameter("MoveBetsDay", DbType.DateTime, _metaModel.MoveBetsDay);
                AssertDateRangeParameters(_objectDb, _metaModel);
                AssertPagingParameters(_objectDb, _metaModel);
                AssertSummaryParameters(_objectDb);
                _objectDb.Received(1).AddInParameter("showSB", DbType.Boolean, true);
                _objectDb.Received(1).AddInParameter("showNG", DbType.Boolean, true);
            });
        }

        [Test]
        public void CreateBetListParameters_CalledByPlatformWinLossBetList_ParametersPassedCorrectly()
        {
            // Arrange
            const string UspKey = "Lrf_BetList_PlatformWinLoss_Mem";

            // Action
            BetListParameterBuilder.CreateBetListParameters(_objectDb, UspKey, _metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                _objectDb.Received(1).AddInParameter("AgentList", DbType.String, _metaModel.AgentListXml);
                _objectDb.Received(1).AddInParameter("custid", DbType.Int32, _metaModel.CustId);
                AssertDateRangeParameters(_objectDb, _metaModel);
                AssertMoveBetParams(_objectDb, _metaModel);
                _objectDb.Received(1).AddInParameter("showSB", DbType.Boolean, true);
                _objectDb.Received(1).AddInParameter("showNG", DbType.Boolean, true);
                AssertPagingParameters(_objectDb, _metaModel);
                AssertSummaryParameters(_objectDb);
                _objectDb.Received(1).AddInParameter("drawfilter", DbType.Int16, _metaModel.DrawFilter);
                _objectDb.Received(1).AddInParameter("PlatformType", DbType.Byte, _metaModel.PlatformType);
            });
        }

        [Test]
        public void CreateBetListParameters_CalledByOutStandingCompletedBetList_ParametersPassedCorrectly()
        {
            // Arrange
            const string UspKey = "Lrf_BetList_CustomerList_Mem";

            // Action
            BetListParameterBuilder.CreateBetListParameters(_objectDb, UspKey, _metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                _objectDb.Received(1).AddInParameter("AgentList", DbType.String, _metaModel.AgentListXml);
                _objectDb.Received(1).AddInParameter("custid", DbType.Int32, _metaModel.CustId);
                AssertDateRangeParameters(_objectDb, _metaModel);
                AssertPagingParameters(_objectDb, _metaModel);
                AssertSummaryParameters(_objectDb, Enumerable.Empty<string>());
                _objectDb.Received(1).AddInParameter("showSB", DbType.Boolean, true);
                _objectDb.Received(1).AddInParameter("showNG", DbType.Boolean, true);
            });
        }

        [Test]
        public void CreateBetListParameters_CalledByDailyStatementBetList_ParametersPassedCorrectly()
        {
            // Arrange
            const string UspKey = "Lrf_BetList_SelByDayAll_Mem";

            // Action
            BetListParameterBuilder.CreateBetListParameters(_objectDb, UspKey, _metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                AssertDailyStatementBetList(_objectDb, _metaModel);
                _objectDb.Received(1).AddOutParameter("ErrCode", DbType.Int32, 4);
                _objectDb.Received(1).AddOutParameter("ErrMsg", DbType.String, 8000);
            });
        }

        [Test]
        public void CreateBetListParameters_CalledByDailyStatementBetListOld_ParametersPassedCorrectly()
        {
            // Arrange
            const string UspKey = "Lrf_BetList_SelByDayAll_Mem_Old";

            // Action
            BetListParameterBuilder.CreateBetListParameters(_objectDb, UspKey, _metaModel);

            // Assert
            AssertDailyStatementBetList(_objectDb, _metaModel);
        }

        [Test]
        public void CreateBetListParameters_CalledByRunningBetList_ParametersPassedCorrectly()
        {
            // Arrange
            const string UspKey = "Lrf_BetList_SelRunningByDate_Mem";

            // Action
            BetListParameterBuilder.CreateBetListParameters(_objectDb, UspKey, _metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                _objectDb.Received(1).AddInParameter("AgentList", DbType.String, _metaModel.AgentListXml);
                _objectDb.Received(1).AddInParameter("custid", DbType.Int32, _metaModel.CustId);
                AssertDateRangeParameters(_objectDb, _metaModel);
                AssertPagingParameters(_objectDb, _metaModel);
                _objectDb.Received(1).AddInParameter("showSB", DbType.Boolean, true);
                _objectDb.Received(1).AddInParameter("showNG", DbType.Boolean, true);
            });
        }

        [Test]
        public void CreateBetListParameters_CalledByMatchRunningBetListOfHR_ParametersPassedCorrectly()
        {
            // Arrange
            const string UspKey = "Lrf_BetList_SelAllByDateHR";

            // Action
            BetListParameterBuilder.CreateBetListParameters(_objectDb, UspKey, _metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                _objectDb.Received(1).AddInParameter("AgentList", DbType.String, _metaModel.AgentListXml);
                _objectDb.Received(1).AddInParameter("bettype", DbType.Int32, _metaModel.BetType);
                _objectDb.Received(1).AddInParameter("MatchID", DbType.Int32, _metaModel.MatchId);
                AssertPagingParameters(_objectDb, _metaModel);
                AssertSummaryParameters(_objectDb);
            });
        }

        [TestCase(2, 2)]
        [TestCase(null, -1)]
        public void CreateBetListParameters_CalledByMatchRunningBetLitByBettype_ParametersPassedCorrectly(int? liveIndicator, int expectedLiveIndicator)
        {
            // Arrange
            const string UspKey = "Lrf_BetList_SelRunningByBettypeMatch";

            _metaModel.LiveIndicator = liveIndicator;

            // Action
            BetListParameterBuilder.CreateBetListParameters(_objectDb, UspKey, _metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                _objectDb.Received(1).AddInParameter("AgentList", DbType.String, _metaModel.AgentListXml);
                _objectDb.Received(1).AddInParameter("bettype", DbType.Int32, _metaModel.BetType);
                _objectDb.Received(1).AddInParameter("MatchID", DbType.Int32, _metaModel.MatchId);
                AssertPagingParameters(_objectDb, _metaModel);
                _objectDb.Received(1).AddInParameter("liveindicator", DbType.Int32, expectedLiveIndicator);
                _objectDb.Received(1).AddInParameter("betteam", DbType.String, _metaModel.BetTeam);
            });
        }

        [TestCase("Test", "Test", 1, 1)]
        [TestCase(null, "", null, -1)]
        public void CreateBetListParameters_CalledByMatchRunningBetLitByMultipleBettypeOfMatch_ParametersPassedCorrectly(
            string multipleBettype,
            string expectedMultipleBettype,
            int? liveIndicator,
            int expectedLiveIndicator)
        {
            // Arrange
            const string UspKey = "Lrf_BetList_SelRunningByMultipleBettypeMatch";

            _metaModel.MultipleBettype = multipleBettype;
            _metaModel.LiveIndicator = liveIndicator;

            // Action
            BetListParameterBuilder.CreateBetListParameters(_objectDb, UspKey, _metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                _objectDb.Received(1).AddInParameter("AgentList", DbType.String, _metaModel.AgentListXml);
                _objectDb.Received(1).AddInParameter("bettype", DbType.String, expectedMultipleBettype);
                _objectDb.Received(1).AddInParameter("MatchID", DbType.Int32, _metaModel.MatchId);
                AssertPagingParameters(_objectDb, _metaModel);
                _objectDb.Received(1).AddInParameter("liveindicator", DbType.Int32, expectedLiveIndicator);
                _objectDb.Received(1).AddInParameter("betteam", DbType.String, _metaModel.BetTeam);
            });
        }

        [TestCase(2, 2)]
        [TestCase(null, -1)]
        public void CreateBetListParameters_CalledByRunningBetListByForeCastMatch_ParametersPassedCorrectly(int? liveIndicator, int expectedLiveIndicator)
        {
            // Arrange
            const string UspKey = "Lrf_BetList_Forecast_SelByMatch";

            _metaModel.LiveIndicator = liveIndicator;

            // Action
            BetListParameterBuilder.CreateBetListParameters(_objectDb, UspKey, _metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                _objectDb.Received(1).AddInParameter("AgentList", DbType.String, _metaModel.AgentListXml);
                _objectDb.Received(1).AddInParameter("bettype", DbType.Int32, _metaModel.BetType);
                _objectDb.Received(1).AddInParameter("MatchID", DbType.Int32, _metaModel.MatchId);
                AssertPagingParameters(_objectDb, _metaModel);
                _objectDb.Received(1).AddInParameter("liveindicator", DbType.Int32, expectedLiveIndicator);
                _objectDb.Received(1).AddInParameter("betteam", DbType.String, _metaModel.BetTeam);
                _objectDb.Received(1).AddInParameter("BetChoice", DbType.String, _metaModel.BetChoice);
                _objectDb.Received(1).AddInParameter("Handicap", DbType.Decimal, _metaModel.Handicap);
            });
        }

        [Test]
        public void CreateBetListParameters_CalledByMicroGamingRngDetail_ParametersPassedCorrectly()
        {
            // Arrange
            const string UspKey = "Lrf_BetList_RNG_Detail";

            // Action
            BetListParameterBuilder.CreateBetListParameters(_objectDb, UspKey, _metaModel);

            // Assert
            AssertCasinoDetailParameters(_objectDb, _metaModel);
        }

        [Test]
        public void CreateBetListParameters_CalledByAGCasinoDetail_ParametersPassedCorrectly()
        {
            // Arrange
            const string UspKey = "Lrf_BetList_LC_Detail";

            // Action
            BetListParameterBuilder.CreateBetListParameters(_objectDb, UspKey, _metaModel);

            // Assert
            AssertCasinoDetailParameters(_objectDb, _metaModel);
        }

        [Test]
        public void CreateBetListParameters_CalledByWinLossByBetType_ParametersPassedCorrectly()
        {
            // Arrange
            const string UspKey = "Lrf_BetList_SelWinLossByBetType";

            // Action
            BetListParameterBuilder.CreateBetListParameters(_objectDb, UspKey, _metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                _objectDb.Received(1).AddInParameter("AgentList", DbType.String, _metaModel.AgentListXml);
                _objectDb.Received(1).AddInParameter("Sort", DbType.String, _metaModel.SortBy + _metaModel.BaseCurrency + " " + _metaModel.SortOrder);
                _objectDb.Received(1).AddInParameter("bettype", DbType.Int32, _metaModel.BetType);
                AssertDateRangeParameters(_objectDb, _metaModel);
                AssertMoveBetParams(_objectDb, _metaModel);
                AssertPagingParameters(_objectDb, _metaModel);
                AssertSummaryParameters(_objectDb);
            });
        }

        [TestCase("nickname", "usd", "nickname ")]
        [TestCase("test", "usd", "testusd ")]
        public void CreateBetListParameters_CreateBetListSortingParameters_ParametersPassedCorrectly(string sortBy, string baseCurrency, string expectedSortBy)
        {
            // Arrange
            const string UspKey = "Lrf_BetList_SelWinLossByBetType";

            var metaModel = new BetListMetaModel { SortBy = sortBy, BaseCurrency = baseCurrency };

            // Action
            BetListParameterBuilder.CreateBetListParameters(_objectDb, UspKey, metaModel);

            // Assert
            _objectDb.Received(1).AddInParameter("Sort", DbType.String, expectedSortBy);
        }

        [Test]
        public void CreateBetListParameters_CreateBetListSortingParameters_SortByIsEmpty_SortByParameterIsNotPassed()
        {
            // Arrange
            const string UspKey = "Lrf_BetList_SelWinLossByBetType";

            var metaModel = new BetListMetaModel();

            // Action
            BetListParameterBuilder.CreateBetListParameters(_objectDb, UspKey, metaModel);

            // Assert
            _objectDb.DidNotReceive().AddInParameter("Sort", DbType.String, Arg.Any<string>());
        }

        [TestCase(true, 1, 100, 1, int.MaxValue - 1)]
        [TestCase(false, 2, 50, 2, 50)]
        public void CreateBetListParameters_CalledByLiveReportNumberGameBetlist_ParametersPassedPagingInfoCorrectly(
            bool isExportExcel,
            int pageIndex,
            int pageSize,
            int expectedIndex,
            int expectedSize)
        {
            // Arrange
            const string UspKey = "Lrf_BetList_LiveReport_SelWinlossNumberGame";
            var metaModel = new LiveReportBetListMetaModel
            {
                IsExportExcel = isExportExcel,
                PageIndex = pageIndex,
                PageSize = pageSize
            };

            // Action
            BetListParameterBuilder.CreateBetListParameters(_objectDb, UspKey, metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                _objectDb.Received(1).AddInParameter("PageSize", DbType.Int32, expectedSize);
                _objectDb.Received(1).AddInParameter("PageIndex", DbType.Int32, expectedIndex);
            });
        }

        private static void AssertSummaryParameters(IObjectDb objectDb, IEnumerable<string> baseCurrencyCols = null)
        {
            var baseCurrencyExpectedCols = baseCurrencyCols ?? SystemConstants.BaseCurrencyList;
            var summaryParams = new[] { "SumWinlost", "SumLicenseeWL", "SumComm", "SumPlayerComm" };

            var expectedParams = summaryParams.Join(baseCurrencyExpectedCols, b => true, s => true, (b, s) => b + s).Concat(summaryParams);

            foreach (var param in expectedParams)
            {
                objectDb.Received(1).AddOutParameter(param, DbType.Currency, 8);
            }
        }

        private static void AssertPagingParameters(IObjectDb objectDb, BetListMetaModel metaModel)
        {
            objectDb.Received(1).AddInParameter("PageSize", DbType.Int32, metaModel.PageSize);
            objectDb.Received(1).AddInParameter("PageIndex", DbType.Int32, metaModel.PageIndex);
            objectDb.Received(1).AddOutParameter("totalrows", DbType.Int32, 4);
        }

        private static void AssertDateRangeParameters(IObjectDb objectDb, BetListMetaModel metaModel)
        {
            objectDb.Received(1).AddInParameter("FromDate", DbType.DateTime, metaModel.FromDate);
            objectDb.Received(1).AddInParameter("ToDate", DbType.DateTime, metaModel.ToDate);
        }

        private static void AssertMoveBetParams(IObjectDb objectDb, BetListMetaModel metaModel)
        {
            objectDb.Received(1).AddInParameter("MoveBetsDay", DbType.DateTime, metaModel.MoveBetsDay);
            objectDb.Received(1).AddInParameter("BalanceUpDay", DbType.DateTime, metaModel.BalanceUpDay);
        }

        private static void AssertDailyStatementBetList(IObjectDb objectDb, BetListMetaModel metaModel)
        {
            objectDb.Received(1).AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
            objectDb.Received(1).AddInParameter("custid", DbType.Int32, metaModel.CustId);
            objectDb.Received(1).AddInParameter("fromdate", DbType.DateTime, metaModel.FromDate);
            objectDb.Received(1).AddInParameter("transtype", DbType.Int32, metaModel.TransType);
            AssertPagingParameters(objectDb, metaModel);
            AssertSummaryParameters(objectDb);
        }

        private static void AssertCasinoDetailParameters(IObjectDb objectDb, BetListMetaModel metaModel)
        {
            objectDb.Received(1).AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
            objectDb.Received(1).AddInParameter("refno", DbType.Int64, metaModel.TransId);
            objectDb.Received(1).AddInParameter("custid", DbType.Int32, metaModel.CustId);
            objectDb.Received(1).AddInParameter("transtype", DbType.Int16, metaModel.TransType);
        }
    }
}