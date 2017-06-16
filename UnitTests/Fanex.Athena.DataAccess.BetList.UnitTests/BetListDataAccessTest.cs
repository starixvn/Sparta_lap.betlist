namespace Fanex.Athena.DataAccess.BetList.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Data;
    using Model.BetList.DTO;
    using Model.BetList.MetaModel;
    using Model.Core.MetaModel.Product;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class BetListDataAccessTest
    {
        private readonly BetListMetaModel _metaModel;
        private IObjectDb _objectDb;
        private BetListDataAccess _dataAccess;

        public BetListDataAccessTest()
        {
            _metaModel = GetBetListMetaModel();
        }

        [SetUp]
        public void SetUp()
        {
            _objectDb = Substitute.For<IObjectDb>();
            _dataAccess = Substitute.ForPartsOf<BetListDataAccess>();
        }

        [TearDown]
        public void TearDown()
        {
            _objectDb = null;
            _dataAccess = null;
        }

        [Test]
        public void GetMixParlayDetailTicket_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_BetList_SelParlayDetail")).Returns(_objectDb);
            long transId = 1;

            // Action
            _dataAccess.GetMixParlayDetailTicket(transId);

            // Assert
            _objectDb.Received().AddInParameter("TransId", DbType.Int64, transId);
        }

        [Test]
        public void GetSystemParlayDetail_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_BetList_SelCombinationParlayDetail")).Returns(_objectDb);
            var metaModel = GetSystemParlayDetailMetaModel();

            var dataset = new DataSet();
            _objectDb.ExecuteDataSet().Returns(dataset);

            // Action
            _dataAccess.GetSystemParlayDetail(metaModel);

            // Assert
            _objectDb.Received().AddInParameter("refno", DbType.Int64, metaModel.RefNo);
            _objectDb.Received().AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
            _objectDb.Received().AddInParameter("movebetsday", DbType.DateTime, metaModel.MoveBetsDay);
            _objectDb.Received().AddInParameter("balanceupday", DbType.DateTime, metaModel.BalanceUpDay);
            _objectDb.Received().AddOutParameter("outstanding", DbType.Currency, 8);
            _objectDb.Received().AddOutParameter("outstandingeur", DbType.Currency, 8);
            _objectDb.Received().AddOutParameter("outstandingusd", DbType.Currency, 8);
            _objectDb.Received().AddOutParameter("outstandinggbp", DbType.Currency, 8);
        }

        [Test]
        public void GetDailyStatementData_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_Statement_Mem")).Returns(_objectDb);

            // Action
            _dataAccess.GetDailyStatementData(_metaModel, true);

            // Assert
            _objectDb.Received().AddInParameter("CustID", DbType.Int32, _metaModel.CustId);
            _objectDb.Received().AddInParameter("FromDate", DbType.DateTime, _metaModel.FromDate);
            _objectDb.Received().AddInParameter("ToDate", DbType.DateTime, _metaModel.ToDate);
            _objectDb.Received().AddInParameter("MoveBetsDay", DbType.DateTime, _metaModel.MoveBetsDay);
            _objectDb.Received().AddInParameter("BalanceUpDay", DbType.DateTime, _metaModel.BalanceUpDay);
            _objectDb.Received().AddInParameter("PageSize", DbType.Int32, _metaModel.PageSize);
            _objectDb.Received().AddInParameter("PageIndex", DbType.Int32, _metaModel.PageIndex);
            _objectDb.Received().AddOutParameter("totalrows", DbType.Int32, 4);
        }

        [Test]
        public void GetDailyStatementData_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_Statement_Mem")).Returns(_objectDb);
            var reader = Substitute.For<IDataReader>();
            _objectDb.ExecuteReader().Returns(reader);
            reader.Read().Returns(true, false);

            ConfigureReaderForGetDailyStatementData(reader, _metaModel);
            var expected = GetDailyStatementData(_metaModel);

            // Action
            var result = _dataAccess.GetDailyStatementData(_metaModel, true)[0];

            // Assert
            Assert.That(result.TransType, Is.EqualTo(expected.TransType));
            Assert.That(result.CustId, Is.EqualTo(expected.CustId));
            Assert.That(result.ExternalId, Is.EqualTo(expected.ExternalId));
            Assert.That(result.SystemId, Is.EqualTo(expected.SystemId));
            Assert.That(result.FullStatementDate, Is.EqualTo(expected.FullStatementDate));
            Assert.That(result.TransTypeHasBetList, Is.EqualTo(expected.TransTypeHasBetList));
            Assert.That(result.Index, Is.EqualTo(expected.Index));
            Assert.That(result.Date, Is.EqualTo(expected.Date));
            Assert.That(result.Remark, Is.EqualTo(expected.Remark));
            Assert.That(result.Turnover, Is.EqualTo(expected.Turnover));
            Assert.That(result.CreditDebit, Is.EqualTo(expected.CreditDebit));
            Assert.That(result.Commission, Is.EqualTo(expected.Commission));
            Assert.That(result.Balance, Is.EqualTo(expected.Balance));
        }

        [Test]
        public void GetBetListData_CalledByWinlossByLeague_ParametersPassedCorrectly()
        {
            // Arrange
            var uspKey = "Lrf_BetList_WinlostByLeague_Mem";
            _dataAccess.GetObjectDb(Arg.Is<string>(uspKey)).Returns(_objectDb);

            // Action
            _dataAccess.GetBetListData(_metaModel, uspKey);

            // Assert
            _objectDb.Received().AddInParameter("AgentList", DbType.String, _metaModel.AgentListXml);
            _objectDb.Received().AddInParameter("CustID", DbType.Int32, _metaModel.CustId);
            _objectDb.Received().AddInParameter("LeagueID", DbType.Int32, _metaModel.LeagueId);
            _objectDb.Received().AddInParameter("MatchId", DbType.Int32, _metaModel.MatchId);
            _objectDb.Received().AddInParameter("Odds", DbType.Decimal, _metaModel.Odds);
            _objectDb.Received().AddInParameter("FromDate", DbType.DateTime, _metaModel.FromDate);
            _objectDb.Received().AddInParameter("ToDate", DbType.DateTime, _metaModel.ToDate);
            _objectDb.Received().AddInParameter("MoveBetsDay", DbType.DateTime, _metaModel.MoveBetsDay);
            _objectDb.Received().AddInParameter("BalanceUpDay", DbType.DateTime, _metaModel.BalanceUpDay);
            _objectDb.Received().AddInParameter("ShowWin", DbType.Boolean, _metaModel.ShowWin);
            _objectDb.Received().AddInParameter("ShowLoss", DbType.Boolean, _metaModel.ShowLoss);
            _objectDb.Received().AddInParameter("ShowDraw", DbType.Boolean, _metaModel.ShowDraw);
            _objectDb.Received().AddInParameter("ShowSB", DbType.Boolean, true);
            _objectDb.Received().AddInParameter("ShowBA", DbType.Boolean, false);
        }

        [Test]
        public void GetBetListData_CalledByCancelBetList_ParametersPassedCorrectly()
        {
            // Arrange
            var uspKey = "Lrf_BetList_SelDeleted";
            _dataAccess.GetObjectDb(Arg.Is<string>(uspKey)).Returns(_objectDb);

            // Action
            _dataAccess.GetBetListData(_metaModel, uspKey);

            // Assert
            _objectDb.Received().AddInParameter("AgentList", DbType.String, _metaModel.AgentListXml);
            _objectDb.Received().AddInParameter("CustID", DbType.Int32, _metaModel.CustId);
            _objectDb.Received().AddInParameter("PageSize", DbType.Int32, _metaModel.PageSize);
            _objectDb.Received().AddInParameter("PageIndex", DbType.Int32, _metaModel.PageIndex);
            _objectDb.Received().AddOutParameter("totalrows", DbType.Int32, 4);
        }

        [Test]
        public void GetBetListData_CalledByCorectScoreBetlist_ParametersPassedCorrectly()
        {
            // Arrange
            var uspKey = "Lrf_BetList_CorrectScore";
            _dataAccess.GetObjectDb(Arg.Is<string>(uspKey)).Returns(_objectDb);

            // Action
            _dataAccess.GetBetListData(_metaModel, uspKey);

            // Assert
            _objectDb.Received().AddInParameter("AgentList", DbType.String, _metaModel.AgentListXml);
            _objectDb.Received().AddInParameter("PageSize", DbType.Int32, _metaModel.PageSize);
            _objectDb.Received().AddInParameter("PageIndex", DbType.Int32, _metaModel.PageIndex);
            _objectDb.Received().AddOutParameter("totalrows", DbType.Int32, 4);
        }

        [Test]
        public void GetBetListData_CalledByCustomerWlBetlist_ParametersPassedCorrectly()
        {
            // Arrange
            var uspKey = "Lrf_BetList_SelWinLossByDate_Mem";
            _dataAccess.GetObjectDb(Arg.Is<string>(uspKey)).Returns(_objectDb);

            // Action
            _dataAccess.GetBetListData(_metaModel, uspKey);

            // Assert
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
        }

        [Test]
        public void GetBetListData_CalledByRacingCommissionBetlist_ParametersPassedCorrectly()
        {
            // Arrange
            var uspKey = "Lrf_BetList_SelWinLossRacingByDate_Mem";
            _dataAccess.GetObjectDb(Arg.Is<string>(uspKey)).Returns(_objectDb);

            // Action
            _dataAccess.GetBetListData(_metaModel, uspKey);

            // Assert
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
        }

        [Test]
        public void GetBetListData_CalledByDailyWLBetlist_ParametersPassedCorrectly()
        {
            // Arrange
            var uspKey = "Lrf_BetList_SelWinlossDaily";
            _dataAccess.GetObjectDb(Arg.Is<string>(uspKey)).Returns(_objectDb);

            // Action
            _dataAccess.GetBetListData(_metaModel, uspKey);

            // Assert
            _objectDb.Received().AddInParameter("Sort", DbType.String, _metaModel.SortBy + _metaModel.BaseCurrency + " " + _metaModel.SortOrder);
            _objectDb.Received().AddInParameter("date", DbType.DateTime, _metaModel.Date);
            _objectDb.Received().AddInParameter("AgentList", DbType.String, _metaModel.AgentListXml);
            _objectDb.Received().AddInParameter("product", DbType.String, _metaModel.ProductCode);
            _objectDb.Received().AddInParameter("status", DbType.String, _metaModel.DailyBetListStatus);
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
        }

        [Test]
        public void GetBetListData_CalledByDailyRacingWLBetlist_ParametersPassedCorrectly()
        {
            // Arrange
            var uspKey = "Lrf_BetList_SelWinlostRacing";
            _dataAccess.GetObjectDb(Arg.Is<string>(uspKey)).Returns(_objectDb);

            // Action
            _dataAccess.GetBetListData(_metaModel, uspKey);

            // Assert
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
        }

        [Test]
        public void GetBetListData_CalledByLicenseeWLBetlist_ParametersPassedCorrectly()
        {
            // Arrange
            var uspKey = "Lrf_BetList_SelWinloss_Mem";
            _dataAccess.GetObjectDb(Arg.Is<string>(uspKey)).Returns(_objectDb);

            // Action
            _dataAccess.GetBetListData(_metaModel, uspKey);

            // Assert
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
        }

        [Test]
        public void GetBetListData_CalledByRunningBetListByBettype_ParametersPassedCorrectly()
        {
            // Arrange
            var uspKey = "Lrf_BetList_SelRunningByBettype";
            _dataAccess.GetObjectDb(Arg.Is<string>(uspKey)).Returns(_objectDb);

            // Action
            _dataAccess.GetBetListData(_metaModel, uspKey);

            // Assert
            _objectDb.Received().AddInParameter("AgentList", DbType.String, _metaModel.AgentListXml);
            _objectDb.Received().AddInParameter("bettype", DbType.Int32, _metaModel.BetType);
            _objectDb.Received().AddInParameter("PageSize", DbType.Int32, _metaModel.PageSize);
            _objectDb.Received().AddInParameter("PageIndex", DbType.Int32, _metaModel.PageIndex);
            _objectDb.Received().AddOutParameter("totalrows", DbType.Int32, 4);
        }

        [Test]
        public void GetBetListData_CalledByRunningBetListByMultipleBettype_ParametersPassedCorrectly()
        {
            // Arrange
            var uspKey = "Lrf_BetList_SelRunningByBettypeList";
            _dataAccess.GetObjectDb(Arg.Is<string>(uspKey)).Returns(_objectDb);

            // Action
            _dataAccess.GetBetListData(_metaModel, uspKey);

            // Assert
            _objectDb.Received().AddInParameter("AgentList", DbType.String, _metaModel.AgentListXml);
            _objectDb.Received().AddInParameter("bettype", DbType.String, _metaModel.MultipleBettype);
            _objectDb.Received().AddInParameter("PageSize", DbType.Int32, _metaModel.PageSize);
            _objectDb.Received().AddInParameter("PageIndex", DbType.Int32, _metaModel.PageIndex);
            _objectDb.Received().AddOutParameter("totalrows", DbType.Int32, 4);
        }

        [Test]
        public void GetBetListData_CalledByLiveReportBetListByMatch_ParametersPassedCorrectly()
        {
            // Arrange
            var uspKey = "Lrf_BetList_LiveReport_SelWinlossByMatch";
            _dataAccess.GetObjectDb(Arg.Is<string>(uspKey)).Returns(_objectDb);

            // Action
            _dataAccess.GetBetListData(_metaModel, uspKey);

            // Assert
            _objectDb.Received().AddInParameter("AgentList", DbType.String, _metaModel.AgentListXml);
            _objectDb.Received().AddInParameter("MatchID", DbType.Int32, _metaModel.MatchId);
            _objectDb.Received().AddInParameter("PageSize", DbType.Int32, _metaModel.PageSize);
            _objectDb.Received().AddInParameter("PageIndex", DbType.Int32, _metaModel.PageIndex);
            _objectDb.Received().AddInParameter("LiveIndicator", DbType.Int32, _metaModel.LiveIndicator);
            _objectDb.Received().AddInParameter("showSB", DbType.Boolean, true);
            _objectDb.DidNotReceive().AddInParameter("showNG", DbType.Boolean, true);
        }

        [Test]
        public void GetBetListData_CalledByLiveReportOutRightBetlist_ParametersPassedCorrectly()
        {
            // Arrange
            var uspKey = "Lrf_BetList_LiveReport_SelWinlossOutright";
            _dataAccess.GetObjectDb(Arg.Is<string>(uspKey)).Returns(_objectDb);

            // Action
            _dataAccess.GetBetListData(_metaModel, uspKey);

            // Assert
            _objectDb.Received().AddInParameter("AgentList", DbType.String, _metaModel.AgentListXml);
            _objectDb.Received().AddInParameter("FromDate", DbType.DateTime, _metaModel.FromDate);
            _objectDb.Received().AddInParameter("ToDate", DbType.DateTime, _metaModel.ToDate);
            _objectDb.Received().AddInParameter("MoveBetsDay", DbType.DateTime, _metaModel.MoveBetsDay);
            _objectDb.Received().AddInParameter("PageSize", DbType.Int32, _metaModel.PageSize);
            _objectDb.Received().AddInParameter("PageIndex", DbType.Int32, _metaModel.PageIndex);
            _objectDb.Received().AddInParameter("showSB", DbType.Boolean, true);
            _objectDb.DidNotReceive().AddInParameter("showNG", DbType.Boolean, true);
        }

        [Test]
        public void GetBetListData_CalledByLiveReportNumberGameBetlist_ParametersPassedCorrectly()
        {
            // Arrange
            var uspKey = "Lrf_BetList_LiveReport_SelWinlossNumberGame";
            _dataAccess.GetObjectDb(Arg.Is<string>(uspKey)).Returns(_objectDb);

            // Action
            _dataAccess.GetBetListData(_metaModel, uspKey);

            // Assert
            _objectDb.Received().AddInParameter("Sort", DbType.String, _metaModel.SortBy + _metaModel.BaseCurrency + " " + _metaModel.SortOrder);
            _objectDb.Received().AddInParameter("AgentList", DbType.String, _metaModel.AgentListXml);
            _objectDb.Received().AddInParameter("FromDate", DbType.DateTime, _metaModel.FromDate);
            _objectDb.Received().AddInParameter("ToDate", DbType.DateTime, _metaModel.ToDate);
            _objectDb.Received().AddInParameter("MoveBetsDay", DbType.DateTime, _metaModel.MoveBetsDay);
            _objectDb.Received().AddInParameter("PageSize", DbType.Int32, _metaModel.PageSize);
            _objectDb.Received().AddInParameter("PageIndex", DbType.Int32, _metaModel.PageIndex);
            _objectDb.Received().AddInParameter("LiveIndicator", DbType.Int32, _metaModel.LiveIndicator);
        }

        [Test]
        public void GetBetListData_CalledByLiveReportParlayBetlist_ParametersPassedCorrectly()
        {
            // Arrange
            var uspKey = "Lrf_BetList_LiveReport_SelWinlossParlay";
            _dataAccess.GetObjectDb(Arg.Is<string>(uspKey)).Returns(_objectDb);

            // Action
            _dataAccess.GetBetListData(_metaModel, uspKey);

            // Assert
            _objectDb.Received().AddInParameter("Sort", DbType.String, _metaModel.SortBy + _metaModel.BaseCurrency + " " + _metaModel.SortOrder);
            _objectDb.Received().AddInParameter("AgentList", DbType.String, _metaModel.AgentListXml);
            _objectDb.Received().AddInParameter("FromDate", DbType.DateTime, _metaModel.FromDate);
            _objectDb.Received().AddInParameter("ToDate", DbType.DateTime, _metaModel.ToDate);
            _objectDb.Received().AddInParameter("MoveBetsDay", DbType.DateTime, _metaModel.MoveBetsDay);
            _objectDb.Received().AddInParameter("PageSize", DbType.Int32, _metaModel.PageSize);
            _objectDb.Received().AddInParameter("PageIndex", DbType.Int32, _metaModel.PageIndex);
            _objectDb.Received().AddInParameter("showSB", DbType.Boolean, true);
            _objectDb.DidNotReceive().AddInParameter("showNG", DbType.Boolean, true);
            _objectDb.Received().AddInParameter("Bettype", DbType.Int32, _metaModel.BetType);
            _objectDb.Received().AddInParameter("LiveIndicator", DbType.Int32, _metaModel.LiveIndicator);
        }

        [TestCase("Lrf_BetList_LiveReport_SelWinlossRacing")]
        [TestCase("Lrf_BetList_LiveReport_SelWinlossVHRacing")]
        public void GetBetListData_CalledByLiveReportParlayBetlist_ParametersPassedCorrectly(string uspKey)
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>(key => key.Equals(uspKey))).Returns(_objectDb);

            // Action
            _dataAccess.GetBetListData(_metaModel, uspKey);

            // Assert
            _objectDb.Received().AddInParameter("Sort", DbType.String, _metaModel.SortBy + _metaModel.BaseCurrency + " " + _metaModel.SortOrder);
            _objectDb.Received().AddInParameter("AgentList", DbType.String, _metaModel.AgentListXml);
            _objectDb.Received().AddInParameter("FromDate", DbType.DateTime, _metaModel.FromDate);
            _objectDb.Received().AddInParameter("ToDate", DbType.DateTime, _metaModel.ToDate);
            _objectDb.Received().AddInParameter("MoveBetsDay", DbType.DateTime, _metaModel.MoveBetsDay);
            _objectDb.Received().AddInParameter("PageSize", DbType.Int32, _metaModel.PageSize);
            _objectDb.Received().AddInParameter("PageIndex", DbType.Int32, _metaModel.PageIndex);
            _objectDb.Received().AddInParameter("SportType", DbType.Int32, _metaModel.SportType);
            _objectDb.Received().AddOutParameter("SUMWL", DbType.Currency, 8);
            _objectDb.Received().AddOutParameter("SUMLicenseeWL", DbType.Currency, 8);
            _objectDb.Received().AddOutParameter("SUMWLUSD", DbType.Currency, 8);
            _objectDb.Received().AddOutParameter("SUMLicenseeWLUSD", DbType.Currency, 8);
            _objectDb.Received().AddOutParameter("SUMWLEUR", DbType.Currency, 8);
            _objectDb.Received().AddOutParameter("SUMLicenseeWLEUR", DbType.Currency, 8);
            _objectDb.Received().AddOutParameter("SUMWLGBP", DbType.Currency, 8);
            _objectDb.Received().AddOutParameter("SUMLicenseeWLGBP", DbType.Currency, 8);
        }

        [Test]
        public void GetBetListData_CalledByMatchWLRacingBetlist_ParametersPassedCorrectly()
        {
            // Arrange
            var uspKey = "Lrf_BetList_MatchWL_Racing";
            _dataAccess.GetObjectDb(Arg.Is<string>(uspKey)).Returns(_objectDb);

            // Action
            _dataAccess.GetBetListData(_metaModel, uspKey);

            // Assert
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
        }

        [Test]
        public void GetBetListData_CalledByMatchWLBettypeBetlist_ParametersPassedCorrectly()
        {
            // Arrange
            var uspKey = "Lrf_BetList_MatchWL_Bettype";
            _dataAccess.GetObjectDb(Arg.Is<string>(uspKey)).Returns(_objectDb);

            // Action
            _dataAccess.GetBetListData(_metaModel, uspKey);

            // Assert
            _objectDb.Received().AddInParameter("AgentList", DbType.String, _metaModel.AgentListXml);
            _objectDb.Received().AddInParameter("bettype", DbType.Int32, _metaModel.BetType);
            _objectDb.Received().AddInParameter("MatchID", DbType.Int32, _metaModel.MatchId);
            _objectDb.Received().AddInParameter("liveindicator", DbType.Int32, _metaModel.LiveIndicator.Value);
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
        }

        [Test]
        public void GetBetListData_CalledByMatchWLOutRightBetList_ParametersPassedCorrectly()
        {
            // Arrange
            var uspKey = "Lrf_BetList_MatchWL_Outright";
            _dataAccess.GetObjectDb(Arg.Is<string>(uspKey)).Returns(_objectDb);

            // Action
            _dataAccess.GetBetListData(_metaModel, uspKey);

            // Assert
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
        }

        [Test]
        public void GetBetListData_CalledByMatchWLParlayBetList_ParametersPassedCorrectly()
        {
            // Arrange
            var uspKey = "Lrf_BetList_MatchWL_Parlay";
            _dataAccess.GetObjectDb(Arg.Is<string>(uspKey)).Returns(_objectDb);

            // Action
            _dataAccess.GetBetListData(_metaModel, uspKey);

            // Assert
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
        }

        [Test]
        public void GetBetListData_CalledByPlatformWinLossBetList_ParametersPassedCorrectly()
        {
            // Arrange
            var uspKey = "Lrf_BetList_PlatformWinLoss_Mem";
            _dataAccess.GetObjectDb(Arg.Is<string>(uspKey)).Returns(_objectDb);

            // Action
            _dataAccess.GetBetListData(_metaModel, uspKey);

            // Assert
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
        }

        [Test]
        public void GetBetListData_CalledByOutStandingCompletedBetList_ParametersPassedCorrectly()
        {
            // Arrange
            var uspKey = "Lrf_BetList_SelAllByDate_Mem";
            _dataAccess.GetObjectDb(Arg.Is<string>(uspKey)).Returns(_objectDb);

            // Action
            _dataAccess.GetBetListData(_metaModel, uspKey);

            // Assert
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
        }

        [Test]
        public void GetBetListData_CalledByDailyStatementBetList_ParametersPassedCorrectly()
        {
            // Arrange
            var uspKey = "Lrf_BetList_SelByDayAll_Mem";
            _dataAccess.GetObjectDb(Arg.Is<string>(uspKey)).Returns(_objectDb);

            // Action
            _dataAccess.GetBetListData(_metaModel, uspKey);

            // Assert
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
        }

        [Test]
        public void GetBetListData_CalledByDailyStatementBetListOld_ParametersPassedCorrectly()
        {
            // Arrange
            var uspKey = "Lrf_BetList_SelByDayAll_Mem_Old";
            _dataAccess.GetObjectDb(Arg.Is<string>(uspKey)).Returns(_objectDb);

            // Action
            _dataAccess.GetBetListData(_metaModel, uspKey);

            // Assert
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
        }

        [Test]
        public void GetBetListData_CalledByRunningBetList_ParametersPassedCorrectly()
        {
            // Arrange
            var uspKey = "Lrf_BetList_SelRunningByDate_Mem";
            _dataAccess.GetObjectDb(Arg.Is<string>(uspKey)).Returns(_objectDb);

            // Action
            _dataAccess.GetBetListData(_metaModel, uspKey);

            // Assert
            _objectDb.Received().AddInParameter("AgentList", DbType.String, _metaModel.AgentListXml);
            _objectDb.Received().AddInParameter("custid", DbType.Int32, _metaModel.CustId);
            _objectDb.Received().AddInParameter("FromDate", DbType.DateTime, _metaModel.FromDate);
            _objectDb.Received().AddInParameter("ToDate", DbType.DateTime, _metaModel.ToDate);
            _objectDb.Received().AddInParameter("PageSize", DbType.Int32, _metaModel.PageSize);
            _objectDb.Received().AddInParameter("PageIndex", DbType.Int32, _metaModel.PageIndex);
            _objectDb.Received().AddOutParameter("totalrows", DbType.Int32, 4);
            _objectDb.Received().AddInParameter("showSB", DbType.Boolean, true);
            _objectDb.Received().AddInParameter("showNG", DbType.Boolean, true);
        }

        [Test]
        public void GetBetListData_CalledByMatchRunningBetListOfHR_ParametersPassedCorrectly()
        {
            // Arrange
            var uspKey = "Lrf_BetList_SelAllByDateHR";
            _dataAccess.GetObjectDb(Arg.Is<string>(uspKey)).Returns(_objectDb);

            // Action
            _dataAccess.GetBetListData(_metaModel, uspKey);

            // Assert
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
        }

        [Test]
        public void GetBetListData_CalledByMatchRunningBetLitByBettype_ParametersPassedCorrectly()
        {
            // Arrange
            var uspKey = "Lrf_BetList_SelRunningByBettypeMatch";
            _dataAccess.GetObjectDb(Arg.Is<string>(uspKey)).Returns(_objectDb);

            // Action
            _dataAccess.GetBetListData(_metaModel, uspKey);

            // Assert
            _objectDb.Received().AddInParameter("AgentList", DbType.String, _metaModel.AgentListXml);
            _objectDb.Received().AddInParameter("bettype", DbType.Int32, _metaModel.BetType);
            _objectDb.Received().AddInParameter("MatchID", DbType.Int32, _metaModel.MatchId);
            _objectDb.Received().AddInParameter("PageSize", DbType.Int32, _metaModel.PageSize);
            _objectDb.Received().AddInParameter("PageIndex", DbType.Int32, _metaModel.PageIndex);
            _objectDb.Received().AddOutParameter("totalrows", DbType.Int32, 4);
            _objectDb.Received().AddInParameter("liveindicator", DbType.Int32, _metaModel.LiveIndicator.Value);
            _objectDb.Received().AddInParameter("betteam", DbType.String, _metaModel.BetTeam);
        }

        [Test]
        public void GetBetListData_CalledByMatchRunningBetLitByMultipleBettypeOfMatch_ParametersPassedCorrectly()
        {
            // Arrange
            var uspKey = "Lrf_BetList_SelRunningByMultipleBettypeMatch";
            _dataAccess.GetObjectDb(Arg.Is<string>(uspKey)).Returns(_objectDb);

            // Action
            _dataAccess.GetBetListData(_metaModel, uspKey);

            // Assert
            _objectDb.Received().AddInParameter("AgentList", DbType.String, _metaModel.AgentListXml);
            _objectDb.Received().AddInParameter("bettype", DbType.String, _metaModel.MultipleBettype);
            _objectDb.Received().AddInParameter("MatchID", DbType.Int32, _metaModel.MatchId);
            _objectDb.Received().AddInParameter("PageSize", DbType.Int32, _metaModel.PageSize);
            _objectDb.Received().AddInParameter("PageIndex", DbType.Int32, _metaModel.PageIndex);
            _objectDb.Received().AddOutParameter("totalrows", DbType.Int32, 4);
            _objectDb.Received().AddInParameter("liveindicator", DbType.Int32, _metaModel.LiveIndicator.Value);
            _objectDb.Received().AddInParameter("betteam", DbType.String, _metaModel.BetTeam);
        }

        [Test]
        public void GetBetListData_CalledByRunningBetListByForeCastMatch_ParametersPassedCorrectly()
        {
            // Arrange
            var uspKey = "Lrf_BetList_Forecast_SelByMatch";
            _dataAccess.GetObjectDb(Arg.Is<string>(uspKey)).Returns(_objectDb);

            // Action
            _dataAccess.GetBetListData(_metaModel, uspKey);

            // Assert
            _objectDb.Received().AddInParameter("AgentList", DbType.String, _metaModel.AgentListXml);
            _objectDb.Received().AddInParameter("bettype", DbType.Int32, _metaModel.BetType);
            _objectDb.Received().AddInParameter("MatchID", DbType.Int32, _metaModel.MatchId);
            _objectDb.Received().AddInParameter("PageSize", DbType.Int32, _metaModel.PageSize);
            _objectDb.Received().AddInParameter("PageIndex", DbType.Int32, _metaModel.PageIndex);
            _objectDb.Received().AddOutParameter("totalrows", DbType.Int32, 4);
            _objectDb.Received().AddInParameter("liveindicator", DbType.Int32, _metaModel.LiveIndicator.Value);
            _objectDb.Received().AddInParameter("betteam", DbType.String, _metaModel.BetTeam);
            _objectDb.Received().AddInParameter("BetChoice", DbType.String, _metaModel.BetChoice);
            _objectDb.Received().AddInParameter("Handicap", DbType.Decimal, _metaModel.Handicap);
        }

        [Test]
        public void GetBetListData_CalledByMicroGamingRngDetail_ParametersPassedCorrectly()
        {
            // Arrange
            var uspKey = "Lrf_BetList_RNG_Detail";
            _dataAccess.GetObjectDb(Arg.Is<string>(uspKey)).Returns(_objectDb);

            // Action
            _dataAccess.GetBetListData(_metaModel, uspKey);

            // Assert
            _objectDb.Received().AddInParameter("AgentList", DbType.String, _metaModel.AgentListXml);
            _objectDb.Received().AddInParameter("refno", DbType.Int64, _metaModel.TransId);
            _objectDb.Received().AddInParameter("custid", DbType.Int32, _metaModel.CustId);
            _objectDb.Received().AddInParameter("transtype", DbType.Int16, _metaModel.TransType);
        }

        [Test]
        public void GetBetListData_CalledByAGCasinoDetail_ParametersPassedCorrectly()
        {
            // Arrange
            var uspKey = "Lrf_BetList_LC_Detail";
            _dataAccess.GetObjectDb(Arg.Is<string>(uspKey)).Returns(_objectDb);

            // Action
            _dataAccess.GetBetListData(_metaModel, uspKey);

            // Assert
            _objectDb.Received().AddInParameter("AgentList", DbType.String, _metaModel.AgentListXml);
            _objectDb.Received().AddInParameter("refno", DbType.Int64, _metaModel.TransId);
            _objectDb.Received().AddInParameter("custid", DbType.Int32, _metaModel.CustId);
            _objectDb.Received().AddInParameter("transtype", DbType.Int16, _metaModel.TransType);
        }

        [Test]
        public void GetBetListData_CalledByWinLossByBetType_ParametersPassedCorrectly()
        {
            // Arrange
            var uspKey = "Lrf_BetList_SelWinLossByBetType";
            _dataAccess.GetObjectDb(Arg.Is<string>(uspKey)).Returns(_objectDb);

            // Action
            _dataAccess.GetBetListData(_metaModel, uspKey);

            // Assert
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
        }

        private SystemParlayDetailMetaModel GetSystemParlayDetailMetaModel()
        {
            return new SystemParlayDetailMetaModel
            {
                RefNo = 1,
                UserId = 2,
                LicenseeId = 3,
                MoveBetsDay = new DateTime(2016, 8, 10),
                BalanceUpDay = new DateTime(2016, 8, 18)
            };
        }

        private BetListMetaModel GetBetListMetaModel()
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
                ProductCode = "ProductCodeTest",
                DailyBetListStatus = "DailyBetListStatusTest",
                LeagueId = 5,
                RaceNo = "RaceNoTest",
                DrawFilter = 6,
                MultipleBettype = "MultipleBettypeTest",
                LiveIndicator = 7,
                SportType = 8,
                PlatformType = 9
            };
        }

        private void ConfigureReaderForGetDailyStatementData(IDataReader reader, BetListMetaModel _metaModel)
        {
            var commissionColumn = "commission" + _metaModel.BaseCurrency;
            var totalAmtColumn = "totalamt" + _metaModel.BaseCurrency;
            var turnoverColumn = "turnover" + _metaModel.BaseCurrency;

            reader["winlostdate"].Returns(new DateTime(2016, 8, 19));
            reader["transtype"].Returns("com");
            reader["transdesc"].Returns("betting statement");
            reader[commissionColumn].Returns(10.5m);
            reader[turnoverColumn].Returns(11.5m);
            reader[totalAmtColumn].Returns(1200.5m);
        }

        private DailyStatement GetDailyStatementData(BetListMetaModel _metaModel)
        {
            return new DailyStatement
            {
                TransType = "1",
                CustId = _metaModel.CustId,
                ExternalId = _metaModel.ExternalId,
                SystemId = _metaModel.SystemId,
                FullStatementDate = "Friday <br/> 8/19/2016 12:00:00 AM",
                TransTypeHasBetList = true,
                Index = 1,
                Date = "8/19/2016",
                Remark = "<span class='bl-underdog'>COM <br/>- betting statement</span>",
                Turnover = "<span class='bl-underdog'>11.50</span>",
                CreditDebit = "<span class='bl-underdog'>1,200.50</span>",
                Commission = "<span class='bl-underdog'>10.50</span>",
                Balance = "<span class='bl-underdog'>1,211.00</span>",
            };
        }
    }
}