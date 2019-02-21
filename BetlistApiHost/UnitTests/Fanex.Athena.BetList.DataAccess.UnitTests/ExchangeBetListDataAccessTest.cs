namespace Fanex.Athena.DataAccess.BetList.UnitTests
{
    using System.Linq;
    using Athena.BetList.DataAccess;
    using Athena.BetList.Model.DTO;
    using Athena.BetList.Model.MetaModel;
    using Common.Constant;
    using Common.DataAcces.UnitTests;
    using NSubstitute;
    using NUnit.Framework;
    using UnitTest.Common.Helpers;

    public class ExchangeBetListDataAccessTest : BaseDataAccessHelperTest<ExchangeBetListDataAccess>
    {
        private const string BetListUsp = "Lrf_BetList_Exchange_Mem";

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            DataAccess.GetObjectDb(Arg.Is(BetListUsp)).Returns(ObjectDb);
        }

        [Test]
        public void GetBetListData_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            var metaModel = FakeHelper.Create<ExchangeBetListMetaModel>();

            // Action
            DataAccess.GetBetListData(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received(1).SetParameterValue("AgentList", metaModel.AgentListXml);
                ObjectDb.Received(1).SetParameterValue("CustId", metaModel.CustId);
                ObjectDb.Received(1).SetParameterValue("FromDate", metaModel.FromDate);
                ObjectDb.Received(1).SetParameterValue("ToDate", metaModel.ToDate);
                ObjectDb.Received(1).SetParameterValue("SumWinLost", null);
                ObjectDb.Received(1).SetParameterValue("SumWinLostEUR", null);
                ObjectDb.Received(1).SetParameterValue("SumWinLostGBP", null);
                ObjectDb.Received(1).SetParameterValue("SumWinLostUSD", null);
            });
        }

        [Test]
        public void GetBetListData_NoData_ReturnEmptyResult()
        {
            // Arrange
            Reader.Read().Returns(false);
            ObjectDb.GetParameterValue("SumWinLost").Returns(0);

            // Action
            var result = DataAccess.GetBetListData(new ExchangeBetListMetaModel());

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.SumWinLossList, Is.Empty);
                Assert.That(result.TicketList, Is.Empty);

                Assert.That(result.GrandTotal, Is.Zero);
            });
        }

        [TestCase("test", 2.5, 3.5, 2.5)]
        [TestCase("usd", 1.5, 4.5, 4.5)]
        [TestCase("eur", 3.25, 5.1, 5.1)]
        [TestCase("gbp", 2.45, 2.58, 2.58)]
        public void GetBetListData_HasSumWinLossListData_ReturnCorrectResult(
            string baseCurrency,
            decimal winlossComm,
            decimal baseCurrencyWinlossComm,
            decimal expectedWinlossComm)
        {
            // Arrange
            ObjectDb.GetParameterValue("SumWinLost").Returns(winlossComm);
            ObjectDb.GetParameterValue("SumWinLost" + baseCurrency.ToUpperInvariant()).Returns(baseCurrencyWinlossComm);

            var ticket = new ExchangeSumWinLoss
            {
                UserId = "UserId",
                EventId = 2,
                EventName = "EventName",
                EventTypeName = "EventTypeName",
                MarketId = "MarketId",
                MarketName = "MarketName",
                WinLost = winlossComm,
                Commission = winlossComm
            };
            MapReaderData(ticket);

            Reader["WinLost" + baseCurrency.ToUpperInvariant()].Returns(baseCurrencyWinlossComm);
            Reader["Commission" + baseCurrency.ToUpperInvariant()].Returns(baseCurrencyWinlossComm);
            Reader.Read().Returns(true, false, false);
            Reader.NextResult().Returns(true);

            var metaModel = new ExchangeBetListMetaModel
            {
                BaseCurrency = baseCurrency
            };

            // Action
            var result = DataAccess.GetBetListData(metaModel);
            var firstSumWL = result.SumWinLossList.ElementAt(0);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(firstSumWL.UserId, Is.EqualTo(ticket.UserId));
                Assert.That(firstSumWL.EventId, Is.EqualTo(ticket.EventId));
                Assert.That(firstSumWL.EventName, Is.EqualTo(ticket.EventName));
                Assert.That(firstSumWL.EventTypeName, Is.EqualTo(ticket.EventTypeName));
                Assert.That(firstSumWL.MarketId, Is.EqualTo(ticket.MarketId));
                Assert.That(firstSumWL.MarketName, Is.EqualTo(ticket.MarketName));
                Assert.That(firstSumWL.WinLost, Is.EqualTo(expectedWinlossComm));
                Assert.That(firstSumWL.Commission, Is.EqualTo(expectedWinlossComm));

                Assert.That(result.GrandTotal, Is.EqualTo(expectedWinlossComm));
            });
        }

        [TestCase("test", 2.5, 3.5, 2.5)]
        [TestCase("usd", 1.5, 4.5, 4.5)]
        [TestCase("eur", 3.25, 5.1, 5.1)]
        [TestCase("gbp", 2.45, 2.58, 2.58)]
        public void GetBetListData_HasTicketDataData_ReturnCorrectResult(string baseCurrency, decimal figure, decimal baseCurrencyFigure, decimal expectedFigure)
        {
            // Arrange
            var ticket = FakeHelper.Create<ExchangeTicket>();
            ticket.WinLost = figure;
            ticket.Stake = figure;
            ticket.BetPlaced = FakeHelper.TruncateMiliseconds(ticket.BetPlaced);
            MapReaderData(ticket);

            Reader["WinLost" + baseCurrency.ToUpperInvariant()].Returns(baseCurrencyFigure);
            Reader["Stake" + baseCurrency.ToUpperInvariant()].Returns(baseCurrencyFigure);
            Reader.Read().Returns(false, true, false);
            Reader.NextResult().Returns(true);

            var metaModel = new ExchangeBetListMetaModel
            {
                BaseCurrency = baseCurrency
            };

            // Action
            var result = DataAccess.GetBetListData(metaModel).TicketList.ElementAt(0);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.UserId, Is.EqualTo(ticket.UserId));
                Assert.That(result.EventId, Is.EqualTo(ticket.EventId));
                Assert.That(result.MarketId, Is.EqualTo(ticket.MarketId));
                Assert.That(result.BetId, Is.EqualTo(ticket.BetId));
                Assert.That(result.SelectionName, Is.EqualTo(ticket.SelectionName));
                Assert.That(result.OddsAvg, Is.EqualTo(ticket.OddsAvg));
                Assert.That(result.WinLost, Is.EqualTo(expectedFigure));
                Assert.That(result.Stake, Is.EqualTo(expectedFigure));
                Assert.That(result.BetPlaced, Is.EqualTo(ticket.BetPlaced));
                Assert.That(result.BetPlacedDateString, Is.EqualTo(ticket.BetPlaced.ToString(FormatKey.DateTimeFormat)));
            });
        }
    }
}