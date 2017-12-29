namespace Fanex.Athena.DataAccess.BetList.UnitTests.Util
{
    using System;
    using System.Data;
    using BetList.Util;
    using Data;
    using Model.BetList.MetaModel;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class BetListParameterBuilderTest
    {
        private readonly BetListMetaModel _metaModel;
        private IObjectDb _objectDb;

        public BetListParameterBuilderTest()
        {
            _metaModel = GetBetListMetaModel();
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
            var metaModel = GetWinLossByLeagueBetListMetaModel();

            // Action
            BetListParameterBuilder.CreateBetListParameters(_objectDb, UspKey, metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                _objectDb.Received().AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
                _objectDb.Received().AddInParameter("CustID", DbType.Int32, metaModel.CustId);
                _objectDb.Received().AddInParameter("LeagueID", DbType.Int32, metaModel.LeagueId);
                _objectDb.Received().AddInParameter("MatchId", DbType.Int32, metaModel.MatchId);
                _objectDb.Received().AddInParameter("Odds", DbType.Decimal, metaModel.Odds);
                _objectDb.Received().AddInParameter("FromDate", DbType.DateTime, metaModel.FromDate);
                _objectDb.Received().AddInParameter("ToDate", DbType.DateTime, metaModel.ToDate);
                _objectDb.Received().AddInParameter("MoveBetsDay", DbType.DateTime, metaModel.MoveBetsDay);
                _objectDb.Received().AddInParameter("BalanceUpDay", DbType.DateTime, metaModel.BalanceUpDay);
                _objectDb.Received().AddInParameter("ShowWin", DbType.Boolean, metaModel.ShowWin);
                _objectDb.Received().AddInParameter("ShowLoss", DbType.Boolean, metaModel.ShowLoss);
                _objectDb.Received().AddInParameter("ShowDraw", DbType.Boolean, metaModel.ShowDraw);
                _objectDb.Received().AddInParameter("ShowSB", DbType.Boolean, true);
                _objectDb.Received().AddInParameter("ShowBA", DbType.Boolean, false);
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
                _objectDb.Received().AddInParameter("AgentList", DbType.String, _metaModel.AgentListXml);
                _objectDb.Received().AddInParameter("CustID", DbType.Int32, _metaModel.CustId);
                _objectDb.Received().AddInParameter("PageSize", DbType.Int32, _metaModel.PageSize);
                _objectDb.Received().AddInParameter("PageIndex", DbType.Int32, _metaModel.PageIndex);
                _objectDb.Received().AddOutParameter("totalrows", DbType.Int32, 4);
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
                _objectDb.Received().AddInParameter("AgentList", DbType.String, _metaModel.AgentListXml);
                _objectDb.Received().AddInParameter("PageSize", DbType.Int32, _metaModel.PageSize);
                _objectDb.Received().AddInParameter("PageIndex", DbType.Int32, _metaModel.PageIndex);
                _objectDb.Received().AddOutParameter("totalrows", DbType.Int32, 4);
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
                _objectDb.Received().AddInParameter("AgentList", DbType.String, _metaModel.AgentListXml);
                _objectDb.Received().AddInParameter("CustID", DbType.Int32, _metaModel.CustId);
                _objectDb.Received().AddInParameter("FromDate", DbType.DateTime, _metaModel.FromDate);
                _objectDb.Received().AddInParameter("ToDate", DbType.DateTime, _metaModel.ToDate);
                _objectDb.Received().AddInParameter("MoveBetsDay", DbType.DateTime, _metaModel.MoveBetsDay);
                _objectDb.Received().AddInParameter("BalanceUpDay", DbType.DateTime, _metaModel.BalanceUpDay);
                _objectDb.Received().AddInParameter("PageSize", DbType.Int32, _metaModel.PageSize);
                _objectDb.Received().AddInParameter("PageIndex", DbType.Int32, _metaModel.PageIndex);
                _objectDb.Received().AddOutParameter("totalrows", DbType.Int32, 4);
                _objectDb.Received().AddOutParameter("SumWinlost", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumLicenseeWL", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumComm", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumPlayerComm", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumWinlostUSD", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumLicenseeWLUSD", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumCommUSD", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumPlayerCommUSD", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumWinlostEUR", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumLicenseeWLEUR", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumCommEUR", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumPlayerCommEUR", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumWinlostGBP", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumLicenseeWLGBP", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumCommGBP", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumPlayerCommGBP", DbType.Currency, 8);
                _objectDb.Received().AddInParameter("showSB", DbType.Boolean, true);
                _objectDb.Received().AddInParameter("showNG", DbType.Boolean, true);
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
                _objectDb.Received().AddInParameter("CustID", DbType.Int32, _metaModel.CustId);
                _objectDb.Received().AddInParameter("AgentList", DbType.String, _metaModel.AgentListXml);
                _objectDb.Received().AddInParameter("FromDate", DbType.DateTime, _metaModel.FromDate);
                _objectDb.Received().AddInParameter("ToDate", DbType.DateTime, _metaModel.ToDate);
                _objectDb.Received().AddInParameter("MoveBetsDay", DbType.DateTime, _metaModel.MoveBetsDay);
                _objectDb.Received().AddInParameter("BalanceUpDay", DbType.DateTime, _metaModel.BalanceUpDay);
                _objectDb.Received().AddInParameter("PageSize", DbType.Int32, _metaModel.PageSize);
                _objectDb.Received().AddInParameter("PageIndex", DbType.Int32, _metaModel.PageIndex);
                _objectDb.Received().AddOutParameter("totalrows", DbType.Int32, 4);
                _objectDb.Received().AddOutParameter("SumWinlost", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumLicenseeWL", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumComm", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumPlayerComm", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumWinlostUSD", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumLicenseeWLUSD", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumCommUSD", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumPlayerCommUSD", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumWinlostEUR", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumLicenseeWLEUR", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumCommEUR", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumPlayerCommEUR", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumWinlostGBP", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumLicenseeWLGBP", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumCommGBP", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumPlayerCommGBP", DbType.Currency, 8);
            });
        }

        [Test]
        public void CreateBetListParameters_CalledByDailyWLBetlist_ParametersPassedCorrectly()
        {
            // Arrange
            const string UspKey = "Lrf_BetList_SelWinlossDaily";
            var metaModel = GetDailyBetListMetaModel();

            // Action
            BetListParameterBuilder.CreateBetListParameters(_objectDb, UspKey, metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                _objectDb.Received().AddInParameter("Sort", DbType.String, metaModel.SortBy + metaModel.BaseCurrency + " " + metaModel.SortOrder);
                _objectDb.Received().AddInParameter("date", DbType.DateTime, metaModel.Date);
                _objectDb.Received().AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
                _objectDb.Received().AddInParameter("product", DbType.String, metaModel.ProductCode);
                _objectDb.Received().AddInParameter("status", DbType.String, metaModel.DailyBetListStatus);
                _objectDb.Received().AddInParameter("MoveBetsDay", DbType.DateTime, metaModel.MoveBetsDay);
                _objectDb.Received().AddInParameter("BalanceUpDay", DbType.DateTime, metaModel.BalanceUpDay);
                _objectDb.Received().AddInParameter("PageSize", DbType.Int32, metaModel.PageSize);
                _objectDb.Received().AddInParameter("PageIndex", DbType.Int32, metaModel.PageIndex);
                _objectDb.Received().AddOutParameter("totalrows", DbType.Int32, 4);
                _objectDb.Received().AddOutParameter("SumWinlost", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumLicenseeWL", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumComm", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumPlayerComm", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumWinlostUSD", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumLicenseeWLUSD", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumCommUSD", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumPlayerCommUSD", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumWinlostEUR", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumLicenseeWLEUR", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumCommEUR", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumPlayerCommEUR", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumWinlostGBP", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumLicenseeWLGBP", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumCommGBP", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumPlayerCommGBP", DbType.Currency, 8);
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
                _objectDb.Received().AddInParameter("date", DbType.DateTime, _metaModel.Date);
                _objectDb.Received().AddInParameter("AgentList", DbType.String, _metaModel.AgentListXml);
                _objectDb.Received().AddInParameter("leagueid", DbType.Int32, _metaModel.LeagueId);
                _objectDb.Received().AddInParameter("raceNo", DbType.String, _metaModel.RaceNo);
                _objectDb.Received().AddInParameter("PageSize", DbType.Int32, _metaModel.PageSize);
                _objectDb.Received().AddInParameter("PageIndex", DbType.Int32, _metaModel.PageIndex);
                _objectDb.Received().AddOutParameter("totalrows", DbType.Int32, 4);
                _objectDb.Received().AddOutParameter("SumWinlost", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumLicenseeWL", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumComm", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumPlayerComm", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumWinlostUSD", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumLicenseeWLUSD", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumCommUSD", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumPlayerCommUSD", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumWinlostEUR", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumLicenseeWLEUR", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumCommEUR", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumPlayerCommEUR", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumWinlostGBP", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumLicenseeWLGBP", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumCommGBP", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumPlayerCommGBP", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumStake", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumStakeUSD", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumStakeEUR", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumStakeGBP", DbType.Currency, 8);
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
                _objectDb.Received().AddInParameter("AgentList", DbType.String, _metaModel.AgentListXml);
                _objectDb.Received().AddInParameter("custid", DbType.Int32, _metaModel.CustId);
                _objectDb.Received().AddInParameter("FromDate", DbType.DateTime, _metaModel.FromDate);
                _objectDb.Received().AddInParameter("ToDate", DbType.DateTime, _metaModel.ToDate);
                _objectDb.Received().AddInParameter("MoveBetsDay", DbType.DateTime, _metaModel.MoveBetsDay);
                _objectDb.Received().AddInParameter("BalanceUpDay", DbType.DateTime, _metaModel.BalanceUpDay);
                _objectDb.Received().AddInParameter("showSB", DbType.Boolean, true);
                _objectDb.Received().AddInParameter("showNG", DbType.Boolean, true);
                _objectDb.Received().AddInParameter("PageSize", DbType.Int32, _metaModel.PageSize);
                _objectDb.Received().AddInParameter("PageIndex", DbType.Int32, _metaModel.PageIndex);
                _objectDb.Received().AddOutParameter("totalrows", DbType.Int32, 4);
                _objectDb.Received().AddOutParameter("SumWinlost", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumLicenseeWL", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumComm", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumPlayerComm", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumWinlostUSD", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumLicenseeWLUSD", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumCommUSD", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumPlayerCommUSD", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumWinlostEUR", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumLicenseeWLEUR", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumCommEUR", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumPlayerCommEUR", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumWinlostGBP", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumLicenseeWLGBP", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumCommGBP", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumPlayerCommGBP", DbType.Currency, 8);
                _objectDb.Received().AddInParameter("drawfilter", DbType.Int16, _metaModel.DrawFilter);
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
                _objectDb.Received().AddInParameter("AgentList", DbType.String, _metaModel.AgentListXml);
                _objectDb.Received().AddInParameter("bettype", DbType.Int32, _metaModel.BetType);
                _objectDb.Received().AddInParameter("PageSize", DbType.Int32, _metaModel.PageSize);
                _objectDb.Received().AddInParameter("PageIndex", DbType.Int32, _metaModel.PageIndex);
                _objectDb.Received().AddOutParameter("totalrows", DbType.Int32, 4);
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
                _objectDb.Received().AddInParameter("AgentList", DbType.String, _metaModel.AgentListXml);
                _objectDb.Received().AddInParameter("bettype", DbType.String, _metaModel.MultipleBettype);
                _objectDb.Received().AddInParameter("PageSize", DbType.Int32, _metaModel.PageSize);
                _objectDb.Received().AddInParameter("PageIndex", DbType.Int32, _metaModel.PageIndex);
                _objectDb.Received().AddOutParameter("totalrows", DbType.Int32, 4);
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
                _objectDb.Received().AddInParameter("AgentList", DbType.String, _metaModel.AgentListXml);
                _objectDb.Received().AddInParameter("Bettype", DbType.Int32, _metaModel.BetType);
                _objectDb.Received().AddInParameter("LeagueId", DbType.Int32, _metaModel.LeagueId);
                _objectDb.Received().AddInParameter("RaceNo", DbType.String, _metaModel.RaceNo);
                _objectDb.Received().AddInParameter("FromDate", DbType.DateTime, _metaModel.FromDate);
                _objectDb.Received().AddInParameter("ToDate", DbType.DateTime, _metaModel.ToDate);
                _objectDb.Received().AddInParameter("MoveBetsDay", DbType.DateTime, _metaModel.MoveBetsDay);
                _objectDb.Received().AddInParameter("Sort", DbType.String, _metaModel.SortBy + _metaModel.BaseCurrency + " " + _metaModel.SortOrder);
                _objectDb.Received().AddInParameter("PageSize", DbType.Int32, _metaModel.PageSize);
                _objectDb.Received().AddInParameter("PageIndex", DbType.Int32, _metaModel.PageIndex);
                _objectDb.Received().AddOutParameter("totalrows", DbType.Int32, 4);
                _objectDb.Received().AddOutParameter("SumWinlost", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumLicenseeWL", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumComm", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumPlayerComm", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumWinlostUSD", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumLicenseeWLUSD", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumCommUSD", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumPlayerCommUSD", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumWinlostEUR", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumLicenseeWLEUR", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumCommEUR", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumPlayerCommEUR", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumWinlostGBP", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumLicenseeWLGBP", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumCommGBP", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumPlayerCommGBP", DbType.Currency, 8);
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
                _objectDb.Received().AddInParameter("AgentList", DbType.String, _metaModel.AgentListXml);
                _objectDb.Received().AddInParameter("bettype", DbType.Int32, _metaModel.BetType);
                _objectDb.Received().AddInParameter("MatchID", DbType.Int32, _metaModel.MatchId);
                _objectDb.Received().AddInParameter("liveindicator", DbType.Int32, expectedLiveIndicator);
                _objectDb.Received().AddInParameter("Sort", DbType.String, _metaModel.SortBy + _metaModel.BaseCurrency + " " + _metaModel.SortOrder);
                _objectDb.Received().AddInParameter("PageSize", DbType.Int32, _metaModel.PageSize);
                _objectDb.Received().AddInParameter("PageIndex", DbType.Int32, _metaModel.PageIndex);
                _objectDb.Received().AddOutParameter("totalrows", DbType.Int32, 4);
                _objectDb.Received().AddOutParameter("SumWinlost", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumLicenseeWL", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumComm", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumPlayerComm", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumWinlostUSD", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumLicenseeWLUSD", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumCommUSD", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumPlayerCommUSD", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumWinlostEUR", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumLicenseeWLEUR", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumCommEUR", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumPlayerCommEUR", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumWinlostGBP", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumLicenseeWLGBP", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumCommGBP", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumPlayerCommGBP", DbType.Currency, 8);
                _objectDb.Received().AddInParameter("Date", DbType.DateTime, _metaModel.Date);
                _objectDb.Received().AddInParameter("showSB", DbType.Boolean, true);
                _objectDb.Received().AddInParameter("showNG", DbType.Boolean, true);
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
                _objectDb.Received().AddInParameter("AgentList", DbType.String, _metaModel.AgentListXml);
                _objectDb.Received().AddInParameter("FromDate", DbType.DateTime, _metaModel.FromDate);
                _objectDb.Received().AddInParameter("ToDate", DbType.DateTime, _metaModel.ToDate);
                _objectDb.Received().AddInParameter("MoveBetsDay", DbType.DateTime, _metaModel.MoveBetsDay);
                _objectDb.Received().AddInParameter("Sort", DbType.String, _metaModel.SortBy + _metaModel.BaseCurrency + " " + _metaModel.SortOrder);
                _objectDb.Received().AddInParameter("PageSize", DbType.Int32, _metaModel.PageSize);
                _objectDb.Received().AddInParameter("PageIndex", DbType.Int32, _metaModel.PageIndex);
                _objectDb.Received().AddOutParameter("totalrows", DbType.Int32, 4);
                _objectDb.Received().AddOutParameter("SumWinlost", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumLicenseeWL", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumComm", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumPlayerComm", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumWinlostUSD", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumLicenseeWLUSD", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumCommUSD", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumPlayerCommUSD", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumWinlostEUR", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumLicenseeWLEUR", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumCommEUR", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumPlayerCommEUR", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumWinlostGBP", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumLicenseeWLGBP", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumCommGBP", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumPlayerCommGBP", DbType.Currency, 8);
                _objectDb.Received().AddInParameter("showSB", DbType.Boolean, true);
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
                _objectDb.Received().AddInParameter("AgentList", DbType.String, _metaModel.AgentListXml);
                _objectDb.Received().AddInParameter("Sort", DbType.String, _metaModel.SortBy + _metaModel.BaseCurrency + " " + _metaModel.SortOrder);
                _objectDb.Received().AddInParameter("bettype", DbType.Int32, _metaModel.BetType);
                _objectDb.Received().AddInParameter("MoveBetsDay", DbType.DateTime, _metaModel.MoveBetsDay);
                _objectDb.Received().AddInParameter("FromDate", DbType.DateTime, _metaModel.FromDate);
                _objectDb.Received().AddInParameter("ToDate", DbType.DateTime, _metaModel.ToDate);
                _objectDb.Received().AddInParameter("PageSize", DbType.Int32, _metaModel.PageSize);
                _objectDb.Received().AddInParameter("PageIndex", DbType.Int32, _metaModel.PageIndex);
                _objectDb.Received().AddOutParameter("totalrows", DbType.Int32, 4);
                _objectDb.Received().AddOutParameter("SumWinlost", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumLicenseeWL", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumComm", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumPlayerComm", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumWinlostUSD", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumLicenseeWLUSD", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumCommUSD", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumPlayerCommUSD", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumWinlostEUR", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumLicenseeWLEUR", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumCommEUR", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumPlayerCommEUR", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumWinlostGBP", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumLicenseeWLGBP", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumCommGBP", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumPlayerCommGBP", DbType.Currency, 8);
                _objectDb.Received().AddInParameter("showSB", DbType.Boolean, true);
                _objectDb.Received().AddInParameter("showNG", DbType.Boolean, true);
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
                _objectDb.Received().AddInParameter("AgentList", DbType.String, _metaModel.AgentListXml);
                _objectDb.Received().AddInParameter("custid", DbType.Int32, _metaModel.CustId);
                _objectDb.Received().AddInParameter("FromDate", DbType.DateTime, _metaModel.FromDate);
                _objectDb.Received().AddInParameter("ToDate", DbType.DateTime, _metaModel.ToDate);
                _objectDb.Received().AddInParameter("MoveBetsDay", DbType.DateTime, _metaModel.MoveBetsDay);
                _objectDb.Received().AddInParameter("BalanceUpDay", DbType.DateTime, _metaModel.BalanceUpDay);
                _objectDb.Received().AddInParameter("showSB", DbType.Boolean, true);
                _objectDb.Received().AddInParameter("showNG", DbType.Boolean, true);
                _objectDb.Received().AddInParameter("PageSize", DbType.Int32, _metaModel.PageSize);
                _objectDb.Received().AddInParameter("PageIndex", DbType.Int32, _metaModel.PageIndex);
                _objectDb.Received().AddOutParameter("totalrows", DbType.Int32, 4);
                _objectDb.Received().AddOutParameter("SumWinlost", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumLicenseeWL", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumComm", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumPlayerComm", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumWinlostUSD", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumLicenseeWLUSD", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumCommUSD", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumPlayerCommUSD", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumWinlostEUR", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumLicenseeWLEUR", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumCommEUR", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumPlayerCommEUR", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumWinlostGBP", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumLicenseeWLGBP", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumCommGBP", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumPlayerCommGBP", DbType.Currency, 8);
                _objectDb.Received().AddInParameter("drawfilter", DbType.Int16, _metaModel.DrawFilter);
                _objectDb.Received().AddInParameter("PlatformType", DbType.Byte, _metaModel.PlatformType);
            });
        }

        [Test]
        public void CreateBetListParameters_CalledByOutStandingCompletedBetList_ParametersPassedCorrectly()
        {
            // Arrange
            const string UspKey = "Lrf_BetList_SelAllByDate_Mem";

            // Action
            BetListParameterBuilder.CreateBetListParameters(_objectDb, UspKey, _metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                _objectDb.Received().AddInParameter("AgentList", DbType.String, _metaModel.AgentListXml);
                _objectDb.Received().AddInParameter("custid", DbType.Int32, _metaModel.CustId);
                _objectDb.Received().AddInParameter("FromDate", DbType.DateTime, _metaModel.FromDate);
                _objectDb.Received().AddInParameter("ToDate", DbType.DateTime, _metaModel.ToDate);
                _objectDb.Received().AddInParameter("PageSize", DbType.Int32, _metaModel.PageSize);
                _objectDb.Received().AddInParameter("PageIndex", DbType.Int32, _metaModel.PageIndex);
                _objectDb.Received().AddOutParameter("totalrows", DbType.Int32, 4);
                _objectDb.Received().AddOutParameter("SumWinlost", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumLicenseeWL", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumComm", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumPlayerComm", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumWinlostUSD", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumLicenseeWLUSD", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumCommUSD", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumPlayerCommUSD", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumWinlostEUR", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumLicenseeWLEUR", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumCommEUR", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumPlayerCommEUR", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumWinlostGBP", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumLicenseeWLGBP", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumCommGBP", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumPlayerCommGBP", DbType.Currency, 8);
                _objectDb.Received().AddInParameter("showSB", DbType.Boolean, true);
                _objectDb.Received().AddInParameter("showNG", DbType.Boolean, true);
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
                _objectDb.Received().AddInParameter("AgentList", DbType.String, _metaModel.AgentListXml);
                _objectDb.Received().AddInParameter("custid", DbType.Int32, _metaModel.CustId);
                _objectDb.Received().AddInParameter("fromdate", DbType.DateTime, _metaModel.FromDate);
                _objectDb.Received().AddInParameter("transtype", DbType.Int32, _metaModel.TransType);
                _objectDb.Received().AddInParameter("PageSize", DbType.Int32, _metaModel.PageSize);
                _objectDb.Received().AddInParameter("PageIndex", DbType.Int32, _metaModel.PageIndex);
                _objectDb.Received().AddOutParameter("totalrows", DbType.Int32, 4);
                _objectDb.Received().AddOutParameter("SumWinlost", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumLicenseeWL", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumComm", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumPlayerComm", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumWinlostUSD", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumLicenseeWLUSD", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumCommUSD", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumPlayerCommUSD", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumWinlostEUR", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumLicenseeWLEUR", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumCommEUR", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumPlayerCommEUR", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumWinlostGBP", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumLicenseeWLGBP", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumCommGBP", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumPlayerCommGBP", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("ErrCode", DbType.Int32, 4);
                _objectDb.Received().AddOutParameter("ErrMsg", DbType.String, 8000);
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
            Assert.Multiple(() =>
            {
                _objectDb.Received().AddInParameter("AgentList", DbType.String, _metaModel.AgentListXml);
                _objectDb.Received().AddInParameter("custid", DbType.Int32, _metaModel.CustId);
                _objectDb.Received().AddInParameter("fromdate", DbType.DateTime, _metaModel.FromDate);
                _objectDb.Received().AddInParameter("transtype", DbType.Int32, _metaModel.TransType);
                _objectDb.Received().AddInParameter("PageSize", DbType.Int32, _metaModel.PageSize);
                _objectDb.Received().AddInParameter("PageIndex", DbType.Int32, _metaModel.PageIndex);
                _objectDb.Received().AddOutParameter("totalrows", DbType.Int32, 4);
                _objectDb.Received().AddOutParameter("SumWinlost", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumLicenseeWL", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumComm", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumPlayerComm", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumWinlostUSD", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumLicenseeWLUSD", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumCommUSD", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumPlayerCommUSD", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumWinlostEUR", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumLicenseeWLEUR", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumCommEUR", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumPlayerCommEUR", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumWinlostGBP", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumLicenseeWLGBP", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumCommGBP", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumPlayerCommGBP", DbType.Currency, 8);
            });
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
                _objectDb.Received().AddInParameter("AgentList", DbType.String, _metaModel.AgentListXml);
                _objectDb.Received().AddInParameter("custid", DbType.Int32, _metaModel.CustId);
                _objectDb.Received().AddInParameter("FromDate", DbType.DateTime, _metaModel.FromDate);
                _objectDb.Received().AddInParameter("ToDate", DbType.DateTime, _metaModel.ToDate);
                _objectDb.Received().AddInParameter("PageSize", DbType.Int32, _metaModel.PageSize);
                _objectDb.Received().AddInParameter("PageIndex", DbType.Int32, _metaModel.PageIndex);
                _objectDb.Received().AddOutParameter("totalrows", DbType.Int32, 4);
                _objectDb.Received().AddInParameter("showSB", DbType.Boolean, true);
                _objectDb.Received().AddInParameter("showNG", DbType.Boolean, true);
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
                _objectDb.Received().AddInParameter("AgentList", DbType.String, _metaModel.AgentListXml);
                _objectDb.Received().AddInParameter("bettype", DbType.Int32, _metaModel.BetType);
                _objectDb.Received().AddInParameter("MatchID", DbType.Int32, _metaModel.MatchId);
                _objectDb.Received().AddInParameter("PageSize", DbType.Int32, _metaModel.PageSize);
                _objectDb.Received().AddInParameter("PageIndex", DbType.Int32, _metaModel.PageIndex);
                _objectDb.Received().AddOutParameter("totalrows", DbType.Int32, 4);
                _objectDb.Received().AddOutParameter("SumWinlost", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumLicenseeWL", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumComm", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumPlayerComm", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumWinlostUSD", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumLicenseeWLUSD", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumCommUSD", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumPlayerCommUSD", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumWinlostEUR", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumLicenseeWLEUR", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumCommEUR", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumPlayerCommEUR", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumWinlostGBP", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumLicenseeWLGBP", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumCommGBP", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumPlayerCommGBP", DbType.Currency, 8);
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
                _objectDb.Received().AddInParameter("AgentList", DbType.String, _metaModel.AgentListXml);
                _objectDb.Received().AddInParameter("bettype", DbType.Int32, _metaModel.BetType);
                _objectDb.Received().AddInParameter("MatchID", DbType.Int32, _metaModel.MatchId);
                _objectDb.Received().AddInParameter("PageSize", DbType.Int32, _metaModel.PageSize);
                _objectDb.Received().AddInParameter("PageIndex", DbType.Int32, _metaModel.PageIndex);
                _objectDb.Received().AddOutParameter("totalrows", DbType.Int32, 4);
                _objectDb.Received().AddInParameter("liveindicator", DbType.Int32, expectedLiveIndicator);
                _objectDb.Received().AddInParameter("betteam", DbType.String, _metaModel.BetTeam);
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
                _objectDb.Received().AddInParameter("AgentList", DbType.String, _metaModel.AgentListXml);
                _objectDb.Received().AddInParameter("bettype", DbType.String, expectedMultipleBettype);
                _objectDb.Received().AddInParameter("MatchID", DbType.Int32, _metaModel.MatchId);
                _objectDb.Received().AddInParameter("PageSize", DbType.Int32, _metaModel.PageSize);
                _objectDb.Received().AddInParameter("PageIndex", DbType.Int32, _metaModel.PageIndex);
                _objectDb.Received().AddOutParameter("totalrows", DbType.Int32, 4);
                _objectDb.Received().AddInParameter("liveindicator", DbType.Int32, expectedLiveIndicator);
                _objectDb.Received().AddInParameter("betteam", DbType.String, _metaModel.BetTeam);
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
                _objectDb.Received().AddInParameter("AgentList", DbType.String, _metaModel.AgentListXml);
                _objectDb.Received().AddInParameter("bettype", DbType.Int32, _metaModel.BetType);
                _objectDb.Received().AddInParameter("MatchID", DbType.Int32, _metaModel.MatchId);
                _objectDb.Received().AddInParameter("PageSize", DbType.Int32, _metaModel.PageSize);
                _objectDb.Received().AddInParameter("PageIndex", DbType.Int32, _metaModel.PageIndex);
                _objectDb.Received().AddOutParameter("totalrows", DbType.Int32, 4);
                _objectDb.Received().AddInParameter("liveindicator", DbType.Int32, expectedLiveIndicator);
                _objectDb.Received().AddInParameter("betteam", DbType.String, _metaModel.BetTeam);
                _objectDb.Received().AddInParameter("BetChoice", DbType.String, _metaModel.BetChoice);
                _objectDb.Received().AddInParameter("Handicap", DbType.Decimal, _metaModel.Handicap);
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
            Assert.Multiple(() =>
            {
                _objectDb.Received().AddInParameter("AgentList", DbType.String, _metaModel.AgentListXml);
                _objectDb.Received().AddInParameter("refno", DbType.Int64, _metaModel.TransId);
                _objectDb.Received().AddInParameter("custid", DbType.Int32, _metaModel.CustId);
                _objectDb.Received().AddInParameter("transtype", DbType.Int16, _metaModel.TransType);
            });
        }

        [Test]
        public void CreateBetListParameters_CalledByAGCasinoDetail_ParametersPassedCorrectly()
        {
            // Arrange
            const string UspKey = "Lrf_BetList_LC_Detail";

            // Action
            BetListParameterBuilder.CreateBetListParameters(_objectDb, UspKey, _metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                _objectDb.Received().AddInParameter("AgentList", DbType.String, _metaModel.AgentListXml);
                _objectDb.Received().AddInParameter("refno", DbType.Int64, _metaModel.TransId);
                _objectDb.Received().AddInParameter("custid", DbType.Int32, _metaModel.CustId);
                _objectDb.Received().AddInParameter("transtype", DbType.Int16, _metaModel.TransType);
            });
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
                _objectDb.Received().AddInParameter("AgentList", DbType.String, _metaModel.AgentListXml);
                _objectDb.Received().AddInParameter("Sort", DbType.String, _metaModel.SortBy + _metaModel.BaseCurrency + " " + _metaModel.SortOrder);
                _objectDb.Received().AddInParameter("bettype", DbType.Int32, _metaModel.BetType);
                _objectDb.Received().AddInParameter("fromdate", DbType.DateTime, _metaModel.FromDate);
                _objectDb.Received().AddInParameter("todate", DbType.DateTime, _metaModel.ToDate);
                _objectDb.Received().AddInParameter("movebetsday", DbType.DateTime, _metaModel.MoveBetsDay);
                _objectDb.Received().AddInParameter("balanceupday", DbType.DateTime, _metaModel.BalanceUpDay);
                _objectDb.Received().AddInParameter("PageSize", DbType.Int32, _metaModel.PageSize);
                _objectDb.Received().AddInParameter("PageIndex", DbType.Int32, _metaModel.PageIndex);
                _objectDb.Received().AddOutParameter("totalrows", DbType.Int32, 4);
                _objectDb.Received().AddOutParameter("SumWinlost", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumLicenseeWL", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumComm", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumPlayerComm", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumWinlostUSD", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumLicenseeWLUSD", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumCommUSD", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumPlayerCommUSD", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumWinlostEUR", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumLicenseeWLEUR", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumCommEUR", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumPlayerCommEUR", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumWinlostGBP", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumLicenseeWLGBP", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumCommGBP", DbType.Currency, 8);
                _objectDb.Received().AddOutParameter("SumPlayerCommGBP", DbType.Currency, 8);
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
            Assert.Multiple(() =>
            {
                _objectDb.Received().AddInParameter("Sort", DbType.String, expectedSortBy);
            });
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
            Assert.Multiple(() =>
            {
                _objectDb.DidNotReceive().AddInParameter("Sort", DbType.String, Arg.Any<string>());
            });
        }

        private static WinLossByLeagueBetListMetaModel GetWinLossByLeagueBetListMetaModel()
        {
            return new WinLossByLeagueBetListMetaModel
            {
                CustId = 1,
                FromDate = new DateTime(2016, 8, 10),
                ToDate = new DateTime(2016, 8, 19),
                MoveBetsDay = new DateTime(2016, 1, 1),
                BalanceUpDay = new DateTime(2016, 2, 1),
                ProductCodeList = "SB,NG",
                LeagueId = 5,
                Odds = 6,
                ShowDraw = true,
                ShowWin = false,
                ShowLoss = true
            };
        }

        private static DailyBetListMetaModel GetDailyBetListMetaModel()
        {
            return new DailyBetListMetaModel
            {
                BaseCurrency = "BaseCurrencyTest",
                MoveBetsDay = new DateTime(2016, 1, 1),
                BalanceUpDay = new DateTime(2016, 2, 1),
                PageSize = 10,
                PageIndex = 1,
                SortBy = "SortByTest",
                SortOrder = "SortOrderTest",
                ProductCode = "ProductCodeTest",
                DailyBetListStatus = "DailyBetListStatusTest"
            };
        }

        private static BetListMetaModel GetBetListMetaModel()
        {
            return new BetListMetaModel
            {
                BaseCurrency = "BaseCurrencyTest",
                CustId = 1,
                FromDate = new DateTime(2016, 8, 10),
                ToDate = new DateTime(2016, 8, 19),
                MoveBetsDay = new DateTime(2016, 1, 1),
                BalanceUpDay = new DateTime(2016, 2, 1),
                PageSize = 10,
                PageIndex = 1,
                ExternalId = "ExternalIdTest",
                SystemId = "SystemIdTest",
                LicenseeId = 2,
                UserId = 3,
                CurrencyId = 4,
                ProductCodeList = "SB,NG",
                SortBy = "SortByTest",
                SortOrder = "SortOrderTest",
                LeagueId = 5,
                RaceNo = "RaceNoTest",
                DrawFilter = 6,
                MultipleBettype = "MultipleBettypeTest",
                LiveIndicator = 7,
                PlatformType = 9
            };
        }
    }
}