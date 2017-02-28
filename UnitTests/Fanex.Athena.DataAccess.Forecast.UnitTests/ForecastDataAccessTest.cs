namespace Fanex.Athena.DataAccess.Forecast.UnitTests
{
    using System;
    using System.Data;
    using Data;
    using Fanex.Athena.DataAccess.Core.Extensions;
    using Forecast;
    using Model.Core.DTO.Common;
    using Model.Forecast.DTO.Forecast1X2;
    using Model.Forecast.DTO.ForecastHdou;
    using Model.Forecast.DTO.NewForecastHdpOu;
    using Model.Forecast.MetaModel;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class ForecastDataAccessTest
    {
        private IObjectDb _objectDb;
        private ForecastDataAccess _dataAccess;
        private IDataReader _reader;

        [OneTimeSetUp]
        public void Setup()
        {
            _objectDb = Substitute.For<IObjectDb>();
            _dataAccess = Substitute.ForPartsOf<ForecastDataAccess>();
            _reader = Substitute.For<IDataReader>();
            _objectDb.ExecuteReader().Returns(_reader);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _objectDb = null;
            _dataAccess = null;
            _reader = null;
        }

        [Test]
        public void GetForecastTopOddsByMatch_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_Forecast_OddsSelTopByMatch")).Returns(_objectDb);
            var matchId = 0;
            var betType = 0;

            // Action
            _dataAccess.GetForecastTopOddsByMatch(matchId, betType);

            // Assert
            _objectDb.Received().AddInParameter("matchid", DbType.Int32, matchId);
            _objectDb.Received().AddInParameter("betType", DbType.Int32, betType);
        }

        [Test]
        public void GetForecastHandicap_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_Forecast_SelHDP")).Returns(_objectDb);
            var metaModel = new ForecastAHMetaModel();
            var userId = 0;

            // Action
            _dataAccess.GetForecastHandicap(metaModel, userId);

            // Assert
            _objectDb.Received().AddInParameter("CurrencyId", DbType.Int32, metaModel.CurrencyId);
            _objectDb.Received().AddInParameter("UserId", DbType.Int32, metaModel.UserId);
            _objectDb.Received().AddInParameter("LicenseeId", DbType.Int32, metaModel.LicenseeId);
            _objectDb.Received().AddInParameter("matchid", DbType.Int32, metaModel.MatchId);
            _objectDb.Received().AddInParameter("betType", DbType.Int32, metaModel.BetType);
        }

        [Test]
        public void GetForecastOverUnder_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_Forecast_SelOU")).Returns(_objectDb);
            var metaModel = new ForecastAHMetaModel();
            var userId = 0;

            // Action
            _dataAccess.GetForecastOverUnder(metaModel, userId);

            // Assert
            _objectDb.Received().AddInParameter("CurrencyId", DbType.Int32, metaModel.CurrencyId);
            _objectDb.Received().AddInParameter("UserId", DbType.Int32, metaModel.UserId);
            _objectDb.Received().AddInParameter("LicenseeId", DbType.Int32, metaModel.LicenseeId);
            _objectDb.Received().AddInParameter("matchid", DbType.Int32, metaModel.MatchId);
            _objectDb.Received().AddInParameter("betType", DbType.Int32, metaModel.BetType);
        }

        [Test]
        public void GetForecastRunningLeague_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_LeagueSelRunning")).Returns(_objectDb);
            var metaModel = new ForecastBaseMetaModel();

            // Action
            _dataAccess.GetForecastRunningLeague(metaModel);

            // Assert
            _objectDb.Received().AddInParameter("CurrencyId", DbType.Int32, 0);
            _objectDb.Received().AddInParameter("UserId", DbType.Int32, metaModel.UserId);
            _objectDb.Received().AddInParameter("LicenseeId", DbType.Int32, metaModel.LicenseeId);
            _objectDb.Received().AddInParameter("betTypes", DbType.String, metaModel.BetTypeString);
        }

        [Test]
        public void GetForecastRunningLeague_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_LeagueSelRunning")).Returns(_objectDb);
            var metaModel = new ForecastBaseMetaModel();
            _reader.Read().Returns(true, false);
            var expected = new League { LeagueId = 1 };
            _reader["leagueid"].Returns(expected.LeagueId);

            // Action
            var result = _dataAccess.GetForecastRunningLeague(metaModel)[0];

            // Assert
            Assert.That(result.LeagueId, Is.EqualTo(expected.LeagueId));
        }

        [Test]
        public void GetForecastOneXTwoByLeague_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_Forecast1x2_SelByLeague")).Returns(_objectDb);
            var metaModel = new ForecastOneXTwoMetaModel();

            // Action
            _dataAccess.GetForecastOneXTwoByLeague(metaModel);

            // Assert
            _objectDb.Received().AddInParameter("CurrencyId", DbType.Int32, metaModel.CurrencyId);
            _objectDb.Received().AddInParameter("UserId", DbType.Int32, metaModel.UserId);
            _objectDb.Received().AddInParameter("LicenseeId", DbType.Int32, metaModel.LicenseeId);
            _objectDb.Received().AddInParameter("LeagueId", DbType.Int32, metaModel.LeagueId);
        }

        [Test]
        public void GetForecastOneXTwoByLeague_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_Forecast1x2_SelByLeague")).Returns(_objectDb);
            var metaModel = new ForecastOneXTwoMetaModel();
            var expected = new ForecastOneXTwo
            {
                MatchId = 1,
                HomeId = 2,
                AwayId = 3,
                LiveIndicator = true,
                LiveHomeScore = 4,
                LiveAwayScore = 5,
                ShowTime = "ShowTimeTest",
                IsNeutral = true
            };
            ConfifureReaderForGetForecastOneXTwoByLeagueMethod(_reader, expected, false);

            // Action
            var result = _dataAccess.GetForecastOneXTwoByLeague(metaModel)[0];

            // Assert
            Assert.That(result.IsExtra, Is.False);
            Assert.That(result.MatchId, Is.EqualTo(expected.MatchId));
            Assert.That(result.HomeId, Is.EqualTo(expected.HomeId));
            Assert.That(result.AwayId, Is.EqualTo(expected.AwayId));
            Assert.That(result.LiveIndicator, Is.EqualTo(expected.LiveIndicator));
            Assert.That(result.LiveHomeScore, Is.EqualTo(expected.LiveHomeScore));
            Assert.That(result.LiveAwayScore, Is.EqualTo(expected.LiveAwayScore));
            Assert.That(result.ShowTime, Is.EqualTo(expected.ShowTime));
            Assert.That(result.IsNeutral, Is.EqualTo(expected.IsNeutral));
        }

        [Test]
        public void GetForecastOneXTwoByLeague_HasExtraColumn_DataReturnedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_Forecast1x2_SelByLeague")).Returns(_objectDb);
            var metaModel = new ForecastOneXTwoMetaModel();
            var expected = new ForecastOneXTwo
            {
                MatchId = 1,
                HomeId = 2,
                AwayId = 3,
                LiveIndicator = true,
                LiveHomeScore = 4,
                LiveAwayScore = 5,
                ShowTime = "ShowTimeTest",
                IsNeutral = true
            };
            ConfifureReaderForGetForecastOneXTwoByLeagueMethod(_reader, expected, true);

            // Action
            var result = _dataAccess.GetForecastOneXTwoByLeague(metaModel)[0];

            // Assert
            Assert.That(result.IsExtra, Is.True);
            Assert.That(result.HalfTimeHomeWinLoss, Is.EqualTo(expected.HalfTimeHomeWinLoss));
            Assert.That(result.HalfTImeDrawWinLoss, Is.EqualTo(expected.HalfTImeDrawWinLoss));
            Assert.That(result.HalfTimeAwayWinLoss, Is.EqualTo(expected.HalfTimeAwayWinLoss));
        }

        [Test]
        public void GetForecastOneXTwoByMatch_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_Forecast_Sel1X2")).Returns(_objectDb);
            var metaModel = new ForecastOneXTwoMetaModel();
            var userId = 0;

            // Action
            _dataAccess.GetForecastOneXTwoByMatch(metaModel, userId);

            // Assert
            _objectDb.Received().AddInParameter("CurrencyId", DbType.Int32, metaModel.CurrencyId);
            _objectDb.Received().AddInParameter("UserId", DbType.Int32, metaModel.UserId);
            _objectDb.Received().AddInParameter("LicenseeId", DbType.Int32, metaModel.LicenseeId);
            _objectDb.Received().AddInParameter("matchid", DbType.Int32, metaModel.MatchId);
            _objectDb.Received().AddInParameter("betType", DbType.Int32, metaModel.BetType);
        }

        [Test]
        public void GetForecastHandicapOverUnder_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_Forecast_SelByLeague")).Returns(_objectDb);
            var metaModel = new ForecastHandicapOverUnderMetaModel();

            // Action
            _dataAccess.GetForecastHandicapOverUnder(metaModel);

            // Assert
            _objectDb.Received().ExecuteReader(Arg.Any<object>());
        }

        [Test]
        public void GetForecastHandicapOverUnder_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_Forecast_SelByLeague")).Returns(_objectDb);
            _objectDb.ExecuteReader(Arg.Any<object>()).Returns(_reader);
            var metaModel = new ForecastHandicapOverUnderMetaModel();
            var expected = new ForecastHandicapOverUnder
            {
                MatchId = 1,
                HomeId = 2,
                AwayId = 3,
                LiveIndicator = true,
                LiveHomeScore = 4,
                LiveAwayScore = 5,
                ShowTime = "ShowTimeTest",
                IsNeutral = true,
                HandicapMinus1 = 10m,
                Handicap = 10.5m,
                HandicapPlus1 = 11m,
                OverUnderPlus2 = 11.5m,
                OverUnderPlus1 = 12m,
                OverUnder = 13m,
                HtHandicapMinus1 = 14m,
                HtHandicap = 15m
            };
            ConfifureReaderForGetForecastHandicapOverUnderMethod(_reader, expected);

            // Action
            var result = _dataAccess.GetForecastHandicapOverUnder(metaModel)[0];

            // Assert
            Assert.That(result.MatchId, Is.EqualTo(expected.MatchId));
            Assert.That(result.HomeId, Is.EqualTo(expected.HomeId));
            Assert.That(result.AwayId, Is.EqualTo(expected.AwayId));
            Assert.That(result.LiveIndicator, Is.EqualTo(expected.LiveIndicator));
            Assert.That(result.LiveHomeScore, Is.EqualTo(expected.LiveHomeScore));
            Assert.That(result.LiveAwayScore, Is.EqualTo(expected.LiveAwayScore));
            Assert.That(result.ShowTime, Is.EqualTo(expected.ShowTime));
            Assert.That(result.IsNeutral, Is.EqualTo(expected.IsNeutral));
            Assert.That(result.HandicapMinus1, Is.EqualTo(expected.HandicapMinus1));
            Assert.That(result.HandicapPlus1, Is.EqualTo(expected.HandicapPlus1));
            Assert.That(result.OverUnderPlus2, Is.EqualTo(expected.OverUnderPlus2));
            Assert.That(result.OverUnderPlus1, Is.EqualTo(expected.OverUnderPlus1));
            Assert.That(result.OverUnder, Is.EqualTo(expected.OverUnder));
            Assert.That(result.HtHandicapMinus1, Is.EqualTo(expected.HtHandicapMinus1));
            Assert.That(result.HtHandicap, Is.EqualTo(expected.HtHandicap));
        }

        [Test]
        public void GetNewForecastData_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_Forecast_MatchTotalForecast")).Returns(_objectDb);
            var metaModel = new NewForecastHandicapOverUnderMetaModel();
            var matchInfo = new NewForecastHdpOuMatchInfo();
            _objectDb.ExecuteDataSet().Returns(new DataSet());
            _objectDb.GetParameterValue("MatchTime").Returns(new DateTime(2016, 8, 26));
            _objectDb.GetParameterValue("HomeScore").Returns("HomeScoreTest");
            _objectDb.GetParameterValue("AwayScore").Returns("AwayScoreTest");

            // Action
            _dataAccess.GetNewForecastData(metaModel, matchInfo);

            // Assert
            _objectDb.Received().AddInParameter("UserId", DbType.Int32, metaModel.UserId);
            _objectDb.Received().AddInParameter("LicenseeId", DbType.Int32, metaModel.LicenseeId);
            _objectDb.Received().AddInParameter("MatchId", DbType.Int32, metaModel.MatchId);
            _objectDb.Received().AddInParameter("CurrencyId", DbType.Int32, metaModel.CurrencyId);
            _objectDb.Received().AddOutParameter("HomeScore", DbType.Int32, 4);
            _objectDb.Received().AddOutParameter("AwayScore", DbType.Int32, 4);
            _objectDb.Received().AddOutParameter("MatchTime", DbType.DateTime, 4);
        }

        [Test]
        public void GetNewForecastData_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_Forecast_MatchTotalForecast")).Returns(_objectDb);
            var metaModel = new NewForecastHandicapOverUnderMetaModel { BaseCurrency = "UUS" };
            var matchInfo = new NewForecastHdpOuMatchInfo();

            var dataSet = new DataSet();
            var totalItem = new NewForecastHdpOuTotalItem
            {
                Type = 1,
                TicketNo = 2,
                TotalTurnoverHome = 10m,
                TotalTurnoverAway = 10.5m,
                TotalTurnoverOver = 20.5m,
                TotalTurnoverUnder = 30m
            };
            dataSet.Tables.Add(GetTotalTableForGetNewForecastDataMethod(totalItem));

            var detailItem = new NewForecastHdpOuDetailItem
            {
                Bettype = 1,
                Hdp = 10m,
                HomeWinLoss = 20m,
                AwayWinLoss = 20.5m
            };
            dataSet.Tables.Add(GetDetailTableForGetNewForecastDataMethod(detailItem));

            _objectDb.ExecuteDataSet().Returns(dataSet);
            _objectDb.GetParameterValue("MatchTime").Returns(new DateTime(2016, 8, 26, 1, 30, 0));
            _objectDb.GetParameterValue("HomeScore").Returns(5);
            _objectDb.GetParameterValue("AwayScore").Returns(6);

            // Action
            var result = _dataAccess.GetNewForecastData(metaModel, matchInfo);

            // Assert
            Assert.That(result.TotalItemList[0].Type, Is.EqualTo(totalItem.Type));
            Assert.That(result.TotalItemList[0].TicketNo, Is.EqualTo(totalItem.TicketNo));
            Assert.That(result.TotalItemList[0].TotalTurnoverHome, Is.EqualTo(totalItem.TotalTurnoverHome));
            Assert.That(result.TotalItemList[0].TotalTurnoverAway, Is.EqualTo(totalItem.TotalTurnoverAway));
            Assert.That(result.TotalItemList[0].TotalTurnoverOver, Is.EqualTo(totalItem.TotalTurnoverOver));
            Assert.That(result.TotalItemList[0].TotalTurnoverUnder, Is.EqualTo(totalItem.TotalTurnoverUnder));
            Assert.That(result.DetailItemList[0].Bettype, Is.EqualTo(detailItem.Bettype));
            Assert.That(result.DetailItemList[0].Hdp, Is.EqualTo(detailItem.Hdp));
            Assert.That(result.DetailItemList[0].HomeWinLoss, Is.EqualTo(detailItem.HomeWinLoss));
            Assert.That(result.DetailItemList[0].AwayWinLoss, Is.EqualTo(detailItem.AwayWinLoss));
            Assert.That(matchInfo.KickOffDateString, Is.EqualTo("08/26/2016 01:30 AM"));
            Assert.That(matchInfo.HomeScore, Is.EqualTo(5));
            Assert.That(matchInfo.AwayScore, Is.EqualTo(6));
        }

        private DataTable GetTotalTableForGetNewForecastDataMethod(NewForecastHdpOuTotalItem expected)
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add("Type");
            dataTable.Columns.Add("TotalTickets");
            dataTable.Columns.Add("HomeTurnoverUUS");
            dataTable.Columns.Add("AwayTurnoverUUS");
            dataTable.Columns.Add("OverTurnoverUUS");
            dataTable.Columns.Add("UnderTurnoverUUS");

            var row = dataTable.NewRow();
            row["Type"] = expected.Type;
            row["TotalTickets"] = expected.TicketNo;
            row["HomeTurnoverUUS"] = expected.TotalTurnoverHome;
            row["AwayTurnoverUUS"] = expected.TotalTurnoverAway;
            row["OverTurnoverUUS"] = expected.TotalTurnoverOver;
            row["UnderTurnoverUUS"] = expected.TotalTurnoverUnder;

            dataTable.Rows.Add(row);

            return dataTable;
        }

        private DataTable GetDetailTableForGetNewForecastDataMethod(NewForecastHdpOuDetailItem expected)
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add("Bettype");
            dataTable.Columns.Add("Hdp");
            dataTable.Columns.Add("HomeWinlossUUS");
            dataTable.Columns.Add("AwayWinlossUUS");

            var row = dataTable.NewRow();
            row["Bettype"] = expected.Bettype;
            row["Hdp"] = expected.Hdp;
            row["HomeWinlossUUS"] = expected.HomeWinLoss;
            row["AwayWinlossUUS"] = expected.AwayWinLoss;

            dataTable.Rows.Add(row);

            return dataTable;
        }

        private void ConfifureReaderForGetForecastHandicapOverUnderMethod(IDataReader reader, ForecastHandicapOverUnder expected)
        {
            reader.Read().Returns(true, false);

            reader["matchid"].Returns(expected.MatchId);
            reader["homeid"].Returns(expected.HomeId);
            reader["awayid"].Returns(expected.AwayId);
            reader["liveindicator"].Returns(expected.LiveIndicator);
            reader["livehomescore"].Returns(expected.LiveHomeScore);
            reader["liveawayscore"].Returns(expected.LiveAwayScore);
            reader["showtime"].Returns(expected.ShowTime);
            reader["isNeutral"].Returns(expected.IsNeutral);

            reader["WinLost_M1_HDP"].Returns(expected.HandicapMinus1);
            reader["WinLost_0_HDP"].Returns(expected.Handicap);
            reader["WinLost_1_HDP"].Returns(expected.HandicapPlus1);
            reader["WinLost_2_OU"].Returns(expected.OverUnderPlus2);
            reader["WinLost_1_OU"].Returns(expected.OverUnderPlus1);
            reader["WinLost_0_OU"].Returns(expected.OverUnder);

            reader["WinLost_M1_HTHDP"].Returns(expected.HtHandicapMinus1);
            reader["WinLost_0_HTHDP"].Returns(expected.HtHandicap);
            reader["WinLost_1_HTHDP"].Returns(expected.HtHandicapPlus1);
            reader["WinLost_2_HTOU"].Returns(expected.HtOverUnderPlus2);
            reader["WinLost_1_HTOU"].Returns(expected.HtOverUnderPlus1);
            reader["WinLost_0_HTOU"].Returns(expected.HtOverUnder);
        }

        private void ConfifureReaderForGetForecastOneXTwoByLeagueMethod(IDataReader reader, ForecastOneXTwo expected, bool hasExtraColumn)
        {
            reader.Read().Returns(true, false);

            reader["matchid"].Returns(expected.MatchId);
            reader["homeid"].Returns(expected.HomeId);
            reader["awayid"].Returns(expected.AwayId);
            reader["liveindicator"].Returns(expected.LiveIndicator);
            reader["livehomescore"].Returns(expected.LiveHomeScore);
            reader["liveawayscore"].Returns(expected.LiveAwayScore);
            reader["showtime"].Returns(expected.ShowTime);
            reader["isNeutral"].Returns(expected.IsNeutral);

            reader["hl"].Returns(expected.HomeWinLoss);
            reader["dl"].Returns(expected.DrawWinLoss);
            reader["al"].Returns(expected.AwayWinLoss);
            reader["ht_hl"].Returns(expected.HalfTimeHomeWinLoss);
            reader["ht_dl"].Returns(expected.HalfTImeDrawWinLoss);
            reader["ht_al"].Returns(expected.HalfTimeAwayWinLoss);

            reader["isExtra"].Returns(hasExtraColumn ? 1 : 0);

            if (hasExtraColumn)
            {
                reader.FieldCount.Returns(1);
                reader.GetName(0).Returns("IsExtra");
                reader["x_hl"].Returns(expected.HalfTimeHomeWinLoss);
                reader["x_dl"].Returns(expected.HalfTImeDrawWinLoss);
                reader["x_al"].Returns(expected.HalfTimeAwayWinLoss);
            }
        }
    }
}