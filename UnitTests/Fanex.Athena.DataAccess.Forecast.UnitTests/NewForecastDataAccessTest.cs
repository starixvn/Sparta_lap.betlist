namespace Fanex.Athena.DataAccess.Forecast.UnitTests
{
    using System;
    using System.Data;
    using Core.UnitTests.TestHelpers;
    using Forecast;
    using Model.Forecast.DTO.NewForecastHdpOu;
    using Model.Forecast.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using UnitTest.Common.Helpers;

    [TestFixture]
    public class NewForecastDataAccessTest : BaseDataAccessHelperTest<NewForecastDataAccess>
    {
        protected override void SetUpDataAccess()
        {
            DataAccess = Substitute.ForPartsOf<NewForecastDataAccess>();
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
    }
}