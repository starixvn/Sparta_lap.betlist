namespace Fanex.Athena.DataAccess.BetList.UnitTests
{
    using System;
    using System.Collections;
    using System.Data;
    using Fanex.BetList.Core.Entities;
    using Model.BetList.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using UnitTest.Common.Helpers;
    using UnitTest.Common.NUnitExtensions;

    public partial class BetListDataAccessTest
    {
        private const string LiveReportWinlossByMatchSp = "Lrf_BetList_LiveReport_SelWinlossByMatch";
        private const string LiveReportWinlossOutrightSp = "Lrf_BetList_LiveReport_SelWinlossOutright";
        private const string LiveReportWinlossNumberGameSp = "Lrf_BetList_LiveReport_SelWinlossNumberGame";
        private const string LiveReportWinlossParlaySp = "Lrf_BetList_LiveReport_SelWinlossParlay";

        [Test]
        public void GetBetListData_CalledByLiveReportBetListByMatch_ParametersPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(LiveReportWinlossByMatchSp)).Returns(ObjectDb);
            var metaModel = GetLiveReportBetListMetaModel();

            ObjectDb.GetOuputParams().Returns(x => { return null; });

            // Action
            DataAccess.GetBetListData(metaModel, LiveReportWinlossByMatchSp);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received(1).AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
                ObjectDb.Received(1).AddInParameter("MatchID", DbType.Int32, metaModel.MatchId);
                ObjectDb.Received(1).AddInParameter("PageSize", DbType.Int32, metaModel.PageSize);
                ObjectDb.Received(1).AddInParameter("PageIndex", DbType.Int32, metaModel.PageIndex);
                ObjectDb.Received(1).AddInParameter("LiveIndicator", DbType.Int32, metaModel.LiveIndicator);
                ObjectDb.Received(1).AddInParameter("showSB", DbType.Boolean, true);
                ObjectDb.DidNotReceive().AddInParameter("showNG", DbType.Boolean, true);
            });
        }

        [Test]
        public void GetBetListData_CalledByLiveReportBetListByMatch_OutSqlParamIsNull_ReturnCorrectTicketDataList()
        {
            // Arrange
            var expectResult = new TicketData
            {
                RefNo = "2",
                Status = "3",
                BetTypeId = 4,
                Odds = 5
            };
            DataAccess.GetObjectDb(Arg.Is(LiveReportWinlossByMatchSp)).Returns(ObjectDb);
            ObjectDb.GetOuputParams().Returns(x => { return null; });
            Reader.NextResult().Returns(true);
            Reader.Read().Returns(false, true, false);
            MapReaderData(new
            {
                refno = expectResult.RefNo,
                status = expectResult.Status,
                bettype = expectResult.BetTypeId,
                odds = expectResult.Odds
            });

            // Action
            var result = DataAccess.GetBetListData(new LiveReportBetListMetaModel(), LiveReportWinlossByMatchSp);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.TotalRows, Is.Zero);
                Assert.That(result.TicketDataList[0], Tobe.SameProperties(expectResult));
            });
        }

        [Test]
        public void GetBetListData_CalledByLiveReportBetListByMatch_ReturnCorrectTicketDataList()
        {
            // Arrange
            var expectResult = new TicketData
            {
                RefNo = "2",
                Status = "3",
                BetTypeId = 4,
                Odds = 5
            };
            const int TotalRows = 10;
            DataAccess.GetObjectDb(Arg.Is(LiveReportWinlossByMatchSp)).Returns(ObjectDb);
            ObjectDb.GetOuputParams().Returns(x => { return new Hashtable { { "totalrows", TotalRows } }; });
            Reader.NextResult().Returns(true);
            Reader.Read().Returns(false, true, false);
            MapReaderData(new
            {
                refno = expectResult.RefNo,
                status = expectResult.Status,
                bettype = expectResult.BetTypeId,
                odds = expectResult.Odds
            });

            // Action
            var result = DataAccess.GetBetListData(new LiveReportBetListMetaModel(), LiveReportWinlossByMatchSp);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.TotalRows, Is.EqualTo(TotalRows));
                Assert.That(result.TicketDataList[0], Tobe.SameProperties(expectResult));
            });
        }

        [Test]
        public void GetBetListData_CalledByLiveReportOutRightBetlist_ParametersPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(LiveReportWinlossOutrightSp)).Returns(ObjectDb);
            var metaModel = GetLiveReportBetListMetaModel();

            // Action
            DataAccess.GetBetListData(metaModel, LiveReportWinlossOutrightSp);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received(1).AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
                ObjectDb.Received(1).AddInParameter("FromDate", DbType.DateTime, metaModel.FromDate);
                ObjectDb.Received(1).AddInParameter("ToDate", DbType.DateTime, metaModel.ToDate);
                ObjectDb.Received(1).AddInParameter("MoveBetsDay", DbType.DateTime, metaModel.MoveBetsDay);
                ObjectDb.Received(1).AddInParameter("PageSize", DbType.Int32, metaModel.PageSize);
                ObjectDb.Received(1).AddInParameter("PageIndex", DbType.Int32, metaModel.PageIndex);
                ObjectDb.Received(1).AddInParameter("showSB", DbType.Boolean, true);
                ObjectDb.DidNotReceive().AddInParameter("showNG", DbType.Boolean, true);
            });
        }

        [Test]
        public void GetBetListData_CalledByLiveReportNumberGameBetlist_ParametersPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(LiveReportWinlossNumberGameSp)).Returns(ObjectDb);
            var metaModel = GetLiveReportBetListMetaModel();

            // Action
            DataAccess.GetBetListData(metaModel, LiveReportWinlossNumberGameSp);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received(1).AddInParameter("Sort", DbType.String, metaModel.SortBy + metaModel.BaseCurrency + " " + metaModel.SortOrder);
                ObjectDb.Received(1).AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
                ObjectDb.Received(1).AddInParameter("FromDate", DbType.DateTime, metaModel.FromDate);
                ObjectDb.Received(1).AddInParameter("ToDate", DbType.DateTime, metaModel.ToDate);
                ObjectDb.Received(1).AddInParameter("MoveBetsDay", DbType.DateTime, metaModel.MoveBetsDay);
                ObjectDb.Received(1).AddInParameter("PageSize", DbType.Int32, metaModel.PageSize);
                ObjectDb.Received(1).AddInParameter("PageIndex", DbType.Int32, metaModel.PageIndex);
                ObjectDb.Received(1).AddInParameter("LiveIndicator", DbType.Int32, metaModel.LiveIndicator);
            });
        }

        [Test]
        public void GetBetListData_CalledByLiveReportParlayBetlist_ParametersPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(LiveReportWinlossParlaySp)).Returns(ObjectDb);
            var metaModel = GetLiveReportBetListMetaModel();

            // Action
            DataAccess.GetBetListData(metaModel, LiveReportWinlossParlaySp);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received(1).AddInParameter("Sort", DbType.String, metaModel.SortBy + metaModel.BaseCurrency + " " + metaModel.SortOrder);
                ObjectDb.Received(1).AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
                ObjectDb.Received(1).AddInParameter("FromDate", DbType.DateTime, metaModel.FromDate);
                ObjectDb.Received(1).AddInParameter("ToDate", DbType.DateTime, metaModel.ToDate);
                ObjectDb.Received(1).AddInParameter("MoveBetsDay", DbType.DateTime, metaModel.MoveBetsDay);
                ObjectDb.Received(1).AddInParameter("PageSize", DbType.Int32, metaModel.PageSize);
                ObjectDb.Received(1).AddInParameter("PageIndex", DbType.Int32, metaModel.PageIndex);
                ObjectDb.Received(1).AddInParameter("showSB", DbType.Boolean, true);
                ObjectDb.DidNotReceive().AddInParameter("showNG", DbType.Boolean, true);
                ObjectDb.Received(1).AddInParameter("Bettype", DbType.Int32, metaModel.BetType);
                ObjectDb.Received(1).AddInParameter("LiveIndicator", DbType.Int32, metaModel.LiveIndicator);
            });
        }

        [TestCase("Lrf_BetList_LiveReport_SelWinlossRacing")]
        [TestCase("Lrf_BetList_LiveReport_SelWinlossVHRacing")]
        public void GetBetListData_CalledByLiveReportParlayBetlist_ParametersPassedCorrectly(string uspKey)
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is<string>(key => key.Equals(uspKey))).Returns(ObjectDb);
            var metaModel = GetLiveReportBetListMetaModel();

            // Action
            DataAccess.GetBetListData(metaModel, uspKey);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received(1).AddInParameter("Sort", DbType.String, metaModel.SortBy + metaModel.BaseCurrency + " " + metaModel.SortOrder);
                ObjectDb.Received(1).AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
                ObjectDb.Received(1).AddInParameter("FromDate", DbType.DateTime, metaModel.FromDate);
                ObjectDb.Received(1).AddInParameter("ToDate", DbType.DateTime, metaModel.ToDate);
                ObjectDb.Received(1).AddInParameter("MoveBetsDay", DbType.DateTime, metaModel.MoveBetsDay);
                ObjectDb.Received(1).AddInParameter("PageSize", DbType.Int32, metaModel.PageSize);
                ObjectDb.Received(1).AddInParameter("PageIndex", DbType.Int32, metaModel.PageIndex);
                ObjectDb.Received(1).AddInParameter("SportType", DbType.Int32, metaModel.SportType);
                ObjectDb.Received(1).AddOutParameter("SUMWL", DbType.Currency, 8);
                ObjectDb.Received(1).AddOutParameter("SUMLicenseeWL", DbType.Currency, 8);
                ObjectDb.Received(1).AddOutParameter("SUMWLUSD", DbType.Currency, 8);
                ObjectDb.Received(1).AddOutParameter("SUMLicenseeWLUSD", DbType.Currency, 8);
                ObjectDb.Received(1).AddOutParameter("SUMWLEUR", DbType.Currency, 8);
                ObjectDb.Received(1).AddOutParameter("SUMLicenseeWLEUR", DbType.Currency, 8);
                ObjectDb.Received(1).AddOutParameter("SUMWLGBP", DbType.Currency, 8);
                ObjectDb.Received(1).AddOutParameter("SUMLicenseeWLGBP", DbType.Currency, 8);
            });
        }

        [TestCase(true, "Team1", true, 2)]
        [TestCase(false, "", false, 0)]
        public void GetBetListData_ReadLrfReferneceTicket_ReturnCorrectData(bool hasOrdinalBetTeam, string expectedBetTeam, bool hasOrdinalStake, decimal expectedStake)
        {
            // Arrange
            var expectResult = new TicketData
            {
                RefNo = "2",
                Status = "3",
                BetTypeId = 4,
                Odds = 5,
                BetTeam = expectedBetTeam,
                Stake = expectedStake
            };
            DataAccess.GetObjectDb(Arg.Is(LiveReportWinlossByMatchSp)).Returns(ObjectDb);
            MapReaderData(new
            {
                refno = expectResult.RefNo,
                status = expectResult.Status,
                bettype = expectResult.BetTypeId,
                odds = expectResult.Odds,
                betteam = expectedBetTeam,
                stake = expectedStake
            });
            Reader.NextResult().Returns(true);
            Reader.Read().Returns(false, true, false);
            Reader.FieldCount.Returns(2);
            Reader.GetName(0).Returns(hasOrdinalBetTeam ? "betteam" : string.Empty);
            Reader.GetName(1).Returns(hasOrdinalStake ? "stake" : string.Empty);

            // Action
            var result = DataAccess.GetBetListData(new LiveReportBetListMetaModel(), LiveReportWinlossByMatchSp);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.TicketDataList[0], Tobe.SameProperties(expectResult));
            });
        }

        [TestCase("WinLoss", "WinLossComm")]
        [TestCase("LicenseeWL", "LicenseeWLComm")]
        [TestCase("nickname", "ExternalId")]
        [TestCase("Status", "Status")]
        public void GetBetListData_SortData_SortCorrectProperty(string sortBy, string expectedSortedProp)
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(LiveReportWinlossNumberGameSp)).Returns(ObjectDb);
            var metaModel = GetLiveReportBetListMetaModel();
            metaModel.SortBy = sortBy;

            // Action
            DataAccess.GetBetListData(metaModel, LiveReportWinlossNumberGameSp);

            // Assert
            Assert.That(metaModel.SortBy, Is.EqualTo(expectedSortedProp));
        }

        [Test]
        public void GetBetListData_WhenObjectDbThrowsException_ThrowDataAccessException()
        {
            const string UspName = "GetBetListData_Sp";
            TestException_WhenObjectDbThrowsException_ThrowDataAccessException(
                UspName,
                () => DataAccess.GetBetListData(new BetListMetaModel(), UspName));
        }

        private static LiveReportBetListMetaModel GetLiveReportBetListMetaModel()
        {
            var metaModel = FakeHelper.Create<LiveReportBetListMetaModel>();
            metaModel.ProductCodeList = "SB,NG";
            metaModel.IsExportExcel = false;
            return metaModel;
        }
    }
}