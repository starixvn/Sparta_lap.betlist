namespace Fanex.Athena.DataAccess.BetList.UnitTests
{
    using System;
    using System.Data;
    using Model.BetList.MetaModel;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public partial class BetListDataAccessTest
    {
        [Test]
        public void GetBetListData_CalledByLiveReportBetListByMatch_ParametersPassedCorrectly()
        {
            // Arrange
            var uspKey = "Lrf_BetList_LiveReport_SelWinlossByMatch";
            _dataAccess.GetObjectDb(Arg.Is<string>(uspKey)).Returns(_objectDb);
            var metaModel = GetLiveReportBetListMetaModel();

            // Action
            _dataAccess.GetBetListData(metaModel, uspKey);

            // Assert
            _objectDb.Received().AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
            _objectDb.Received().AddInParameter("MatchID", DbType.Int32, metaModel.MatchId);
            _objectDb.Received().AddInParameter("PageSize", DbType.Int32, metaModel.PageSize);
            _objectDb.Received().AddInParameter("PageIndex", DbType.Int32, metaModel.PageIndex);
            _objectDb.Received().AddInParameter("LiveIndicator", DbType.Int32, metaModel.LiveIndicator);
            _objectDb.Received().AddInParameter("showSB", DbType.Boolean, true);
            _objectDb.DidNotReceive().AddInParameter("showNG", DbType.Boolean, true);
        }

        [Test]
        public void GetBetListData_CalledByLiveReportOutRightBetlist_ParametersPassedCorrectly()
        {
            // Arrange
            var uspKey = "Lrf_BetList_LiveReport_SelWinlossOutright";
            _dataAccess.GetObjectDb(Arg.Is<string>(uspKey)).Returns(_objectDb);
            var metaModel = GetLiveReportBetListMetaModel();

            // Action
            _dataAccess.GetBetListData(metaModel, uspKey);

            // Assert
            _objectDb.Received().AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
            _objectDb.Received().AddInParameter("FromDate", DbType.DateTime, metaModel.FromDate);
            _objectDb.Received().AddInParameter("ToDate", DbType.DateTime, metaModel.ToDate);
            _objectDb.Received().AddInParameter("MoveBetsDay", DbType.DateTime, metaModel.MoveBetsDay);
            _objectDb.Received().AddInParameter("PageSize", DbType.Int32, metaModel.PageSize);
            _objectDb.Received().AddInParameter("PageIndex", DbType.Int32, metaModel.PageIndex);
            _objectDb.Received().AddInParameter("showSB", DbType.Boolean, true);
            _objectDb.DidNotReceive().AddInParameter("showNG", DbType.Boolean, true);
        }

        [Test]
        public void GetBetListData_CalledByLiveReportNumberGameBetlist_ParametersPassedCorrectly()
        {
            // Arrange
            var uspKey = "Lrf_BetList_LiveReport_SelWinlossNumberGame";
            _dataAccess.GetObjectDb(Arg.Is<string>(uspKey)).Returns(_objectDb);
            var metaModel = GetLiveReportBetListMetaModel();

            // Action
            _dataAccess.GetBetListData(metaModel, uspKey);

            // Assert
            _objectDb.Received().AddInParameter("Sort", DbType.String, metaModel.SortBy + metaModel.BaseCurrency + " " + metaModel.SortOrder);
            _objectDb.Received().AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
            _objectDb.Received().AddInParameter("FromDate", DbType.DateTime, metaModel.FromDate);
            _objectDb.Received().AddInParameter("ToDate", DbType.DateTime, metaModel.ToDate);
            _objectDb.Received().AddInParameter("MoveBetsDay", DbType.DateTime, metaModel.MoveBetsDay);
            _objectDb.Received().AddInParameter("PageSize", DbType.Int32, metaModel.PageSize);
            _objectDb.Received().AddInParameter("PageIndex", DbType.Int32, metaModel.PageIndex);
            _objectDb.Received().AddInParameter("LiveIndicator", DbType.Int32, metaModel.LiveIndicator);
        }

        [Test]
        public void GetBetListData_CalledByLiveReportParlayBetlist_ParametersPassedCorrectly()
        {
            // Arrange
            var uspKey = "Lrf_BetList_LiveReport_SelWinlossParlay";
            _dataAccess.GetObjectDb(Arg.Is<string>(uspKey)).Returns(_objectDb);
            var metaModel = GetLiveReportBetListMetaModel();

            // Action
            _dataAccess.GetBetListData(metaModel, uspKey);

            // Assert
            _objectDb.Received().AddInParameter("Sort", DbType.String, metaModel.SortBy + metaModel.BaseCurrency + " " + metaModel.SortOrder);
            _objectDb.Received().AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
            _objectDb.Received().AddInParameter("FromDate", DbType.DateTime, metaModel.FromDate);
            _objectDb.Received().AddInParameter("ToDate", DbType.DateTime, metaModel.ToDate);
            _objectDb.Received().AddInParameter("MoveBetsDay", DbType.DateTime, metaModel.MoveBetsDay);
            _objectDb.Received().AddInParameter("PageSize", DbType.Int32, metaModel.PageSize);
            _objectDb.Received().AddInParameter("PageIndex", DbType.Int32, metaModel.PageIndex);
            _objectDb.Received().AddInParameter("showSB", DbType.Boolean, true);
            _objectDb.DidNotReceive().AddInParameter("showNG", DbType.Boolean, true);
            _objectDb.Received().AddInParameter("Bettype", DbType.Int32, metaModel.BetType);
            _objectDb.Received().AddInParameter("LiveIndicator", DbType.Int32, metaModel.LiveIndicator);
        }

        [TestCase("Lrf_BetList_LiveReport_SelWinlossRacing")]
        [TestCase("Lrf_BetList_LiveReport_SelWinlossVHRacing")]
        public void GetBetListData_CalledByLiveReportParlayBetlist_ParametersPassedCorrectly(string uspKey)
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>(key => key.Equals(uspKey))).Returns(_objectDb);
            var metaModel = GetLiveReportBetListMetaModel();

            // Action
            _dataAccess.GetBetListData(metaModel, uspKey);

            // Assert
            _objectDb.Received().AddInParameter("Sort", DbType.String, metaModel.SortBy + metaModel.BaseCurrency + " " + metaModel.SortOrder);
            _objectDb.Received().AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
            _objectDb.Received().AddInParameter("FromDate", DbType.DateTime, metaModel.FromDate);
            _objectDb.Received().AddInParameter("ToDate", DbType.DateTime, metaModel.ToDate);
            _objectDb.Received().AddInParameter("MoveBetsDay", DbType.DateTime, metaModel.MoveBetsDay);
            _objectDb.Received().AddInParameter("PageSize", DbType.Int32, metaModel.PageSize);
            _objectDb.Received().AddInParameter("PageIndex", DbType.Int32, metaModel.PageIndex);
            _objectDb.Received().AddInParameter("SportType", DbType.Int32, metaModel.SportType);
            _objectDb.Received().AddOutParameter("SUMWL", DbType.Currency, 8);
            _objectDb.Received().AddOutParameter("SUMLicenseeWL", DbType.Currency, 8);
            _objectDb.Received().AddOutParameter("SUMWLUSD", DbType.Currency, 8);
            _objectDb.Received().AddOutParameter("SUMLicenseeWLUSD", DbType.Currency, 8);
            _objectDb.Received().AddOutParameter("SUMWLEUR", DbType.Currency, 8);
            _objectDb.Received().AddOutParameter("SUMLicenseeWLEUR", DbType.Currency, 8);
            _objectDb.Received().AddOutParameter("SUMWLGBP", DbType.Currency, 8);
            _objectDb.Received().AddOutParameter("SUMLicenseeWLGBP", DbType.Currency, 8);
        }

        private static LiveReportBetListMetaModel GetLiveReportBetListMetaModel()
        {
            return new LiveReportBetListMetaModel
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
                SportType = 8,
                PlatformType = 9
            };
        }
    }
}