namespace Fanex.Athena.DataAccess.Forecast.UnitTests
{
    using System;
    using System.Data;
    using Core.UnitTests.TestHelpers;
    using Forecast;
    using Model.Forecast.DTO.ForecastHdou;
    using Model.Forecast.DTO.NewForecastHdpOu;
    using Model.Forecast.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using UnitTest.Common.Helpers;
    using VendorAdapter.Interfaces;

    [TestFixture]
    public class ForecastDataAccessTest : BaseDataAccessHelperTest<ForecastDataAccess>
    {
        protected override void SetUpDataAccess()
        {
            DataAccess = Substitute.ForPartsOf<ForecastDataAccess>(
                Substitute.For<ICacheRefAdapter>());
        }

        [Test]
        public void GetForecastHandicapOverUnder_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is("Lrf_Forecast_SelByLeague")).Returns(ObjectDb);
            var metaModel = new ForecastHandicapOverUnderMetaModel();

            // Action
            DataAccess.GetForecastHandicapOverUnder(metaModel);

            // Assert
            ObjectDb.Received(1).ExecuteReader(Arg.Any<object>());
        }

        [Test]
        public void GetForecastHandicapOverUnder_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is("Lrf_Forecast_SelByLeague")).Returns(ObjectDb);
            ObjectDb.ExecuteReader(Arg.Any<object>()).Returns(Reader);
            var metaModel = new ForecastHandicapOverUnderMetaModel();
            var expected = FakeHelper.Create<ForecastHandicapOverUnder>();
            ConfifureReaderForGetForecastHandicapOverUnderMethod(Reader, expected);

            // Action
            var result = DataAccess.GetForecastHandicapOverUnder(metaModel)[0];

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
            DataAccess.GetObjectDb(Arg.Is("Lrf_Forecast_MatchTotalForecast")).Returns(ObjectDb);
            var metaModel = new NewForecastHandicapOverUnderMetaModel();
            var matchInfo = new NewForecastHdpOuMatchInfo();
            ObjectDb.ExecuteDataSet().Returns(new DataSet());
            ObjectDb.GetParameterValue("MatchTime").Returns(new DateTime(2016, 8, 26));
            ObjectDb.GetParameterValue("HomeScore").Returns("HomeScoreTest");
            ObjectDb.GetParameterValue("AwayScore").Returns("AwayScoreTest");

            // Action
            DataAccess.GetNewForecastData(metaModel, matchInfo);

            // Assert
            ObjectDb.Received(1).SetParameterValue("AgentList", metaModel.AgentListXml);
            ObjectDb.Received(1).SetParameterValue("MatchId", metaModel.MatchId);
            ObjectDb.Received(1).SetParameterValue("HomeScore", null);
            ObjectDb.Received(1).SetParameterValue("AwayScore", null);
            ObjectDb.Received(1).SetParameterValue("MatchTime", null);
        }

        [Test]
        public void GetNewForecastData_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is("Lrf_Forecast_MatchTotalForecast")).Returns(ObjectDb);
            var metaModel = new NewForecastHandicapOverUnderMetaModel { BaseCurrency = "UUS" };
            var matchInfo = new NewForecastHdpOuMatchInfo();

            var dataSet = new DataSet();
            var totalItem = FakeHelper.Create<NewForecastHdpOuTotalItem>();
            dataSet.Tables.Add(GetTotalTableForGetNewForecastDataMethod(totalItem));

            var detailItem = FakeHelper.Create<NewForecastHdpOuDetailItem>();
            dataSet.Tables.Add(GetDetailTableForGetNewForecastDataMethod(detailItem));

            ObjectDb.ExecuteDataSet().Returns(dataSet);
            ObjectDb.GetParameterValue("MatchTime").Returns(new DateTime(2016, 8, 26, 1, 30, 0));
            ObjectDb.GetParameterValue("HomeScore").Returns(5);
            ObjectDb.GetParameterValue("AwayScore").Returns(6);

            // Action
            var result = DataAccess.GetNewForecastData(metaModel, matchInfo);

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

        private static DataTable GetTotalTableForGetNewForecastDataMethod(NewForecastHdpOuTotalItem expected)
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

        private static DataTable GetDetailTableForGetNewForecastDataMethod(NewForecastHdpOuDetailItem expected)
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

        private static void ConfifureReaderForGetForecastHandicapOverUnderMethod(IDataReader reader, ForecastHandicapOverUnder expected)
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
    }
}