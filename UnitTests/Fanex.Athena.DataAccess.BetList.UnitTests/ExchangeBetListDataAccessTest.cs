namespace Fanex.Athena.DataAccess.BetList.UnitTests
{
    using System;
    using System.Data;
    using System.Linq;
    using Core.UnitTests.TestHelpers;
    using Extension.Helper;
    using Model.BetList.DTO;
    using Model.BetList.MetaModel;
    using Model.Core.DataDomain;
    using NSubstitute;
    using NUnit.Framework;
    using UnitTest.Common.Helpers;

    public class ExchangeBetListDataAccessTest : BaseDataAccessHelperTest<ExchangeBetListDataAccess>
    {
        private const string BetListUsp = "Lrf_BetList_Exchange_Mem";

        private const string TicketDetailUsp = "Lrf_BetList_Exchange_Detail";

        [Test]
        public void GetBetListData_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(BetListUsp)).Returns(ObjectDb);

            var metaModel = new ExchangeBetListMetaModel();

            // Action
            DataAccess.GetBetListData(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received(1).AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
                ObjectDb.Received(1).AddInParameter("CustId", DbType.Int32, metaModel.CustId);
                ObjectDb.Received(1).AddInParameter("FromDate", DbType.Date, metaModel.FromDate);
                ObjectDb.Received(1).AddInParameter("ToDate", DbType.Date, metaModel.ToDate);
                ObjectDb.Received(1).AddOutParameter("SumWinLost", DbType.Currency, 8);
                ObjectDb.Received(1).AddOutParameter("SumWinLostEUR", DbType.Currency, 8);
                ObjectDb.Received(1).AddOutParameter("SumWinLostGBP", DbType.Currency, 8);
                ObjectDb.Received(1).AddOutParameter("SumWinLostUSD", DbType.Currency, 8);
            });
        }

        [Test]
        public void GetBetListData_NoData_ReturnEmptyResult()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(BetListUsp)).Returns(ObjectDb);
            Reader.Read().Returns(false);
            ObjectDb.GetParameterValue("SumWinLost").Returns(0);

            var metaModel = new ExchangeBetListMetaModel();

            // Action
            var result = DataAccess.GetBetListData(metaModel);

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
            DataAccess.GetObjectDb(Arg.Is(BetListUsp)).Returns(ObjectDb);
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
            DataAccess.GetObjectDb(Arg.Is(BetListUsp)).Returns(ObjectDb);

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
                Assert.That(result.BetPlacedDateString, Is.EqualTo(ticket.BetPlaced.ToString("MM/dd/yyyy hh:mm:ss")));
            });
        }

        [Test]
        public void GetBetListData_WhenObjectDbThrowsException_ThrowDataAccessException()
        {
            TestException_WhenObjectDbThrowsException_ThrowDataAccessException(
                BetListUsp,
                () => DataAccess.GetBetListData(new ExchangeBetListMetaModel()));
        }

        [Test]
        public void GetTicketDetailList_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(TicketDetailUsp)).Returns(ObjectDb);

            var metaModel = new ExchangeBetListDetailMetaModel();

            // Action
            DataAccess.GetTicketDetailList(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received(1).AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
                ObjectDb.Received(1).AddInParameter("CustId", DbType.Int32, metaModel.CustId);
                ObjectDb.Received(1).AddInParameter("BetID", DbType.Int64, metaModel.BetId);
            });
        }

        [Test]
        public void GetTicketDetailList_NoData_ReturnEmptyResult()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(TicketDetailUsp)).Returns(ObjectDb);
            Reader.Read().Returns(false);

            var metaModel = new ExchangeBetListDetailMetaModel();

            // Action
            var result = DataAccess.GetTicketDetailList(metaModel);

            // Assert
            Assert.That(result, Is.Empty);
        }

        [TestCase("test", 2.5, 3.5, 2.5)]
        [TestCase("usd", 1.5, 4.5, 4.5)]
        [TestCase("eur", 3.25, 5.1, 5.1)]
        [TestCase("gbp", 2.45, 2.58, 2.58)]
        public void GetTicketDetailList_HasData_MatchTypeIsLay_ReturnCorrectResult(string baseCurrency, decimal stake, decimal baseCurrencyStake, decimal expectedStake)
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(TicketDetailUsp)).Returns(ObjectDb);

            var ticket = new ExchangeTicketDetail
            {
                BetTaken = new DateTime(2018, 1, 1, 1, 2, 3),
                OddsReq = 1,
                Stake = stake,
                OddsMatched = 3
            };
            MapReaderData(ticket);

            Reader["OddsRequest"].Returns(ticket.OddsReq);
            Reader["Stake" + baseCurrency.ToUpperInvariant()].Returns(baseCurrencyStake);
            Reader.Read().Returns(true, false);

            var metaModel = new ExchangeBetListDetailMetaModel
            {
                BaseCurrency = baseCurrency,
                Type = ExchangeMatchType.Lay
            };

            // Action
            var result = DataAccess.GetTicketDetailList(metaModel).ElementAt(0);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.BetTaken, Is.EqualTo(ticket.BetTaken));
                Assert.That(result.BetTakenDateString, Is.EqualTo("01/01/2018 01:02:03"));
                Assert.That(result.OddsReq, Is.EqualTo(ticket.OddsReq));
                Assert.That(result.OddsMatched, Is.EqualTo(ticket.OddsMatched));
                Assert.That(result.Stake, Is.EqualTo(expectedStake));

                Assert.That(result.Liability, Is.EqualTo(NumberHelper.DecFormat((result.OddsMatched - 1) * result.Stake, 2)));
            });
        }

        [TestCase("test", 2.5, 3.5, 2.5)]
        [TestCase("USD", 1.5, 4.5, 4.5)]
        [TestCase("EUR", 3.25, 5.1, 5.1)]
        [TestCase("GBP", 2.45, 2.58, 2.58)]
        public void GetTicketDetailList_HasData_MatchTypeIsBack_ReturnCorrectResult(
            string baseCurrency, 
            decimal stake, 
            decimal baseCurrencyStake, 
            decimal expectedStake)
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(TicketDetailUsp)).Returns(ObjectDb);

            var ticket = new ExchangeTicketDetail
            {
                BetTaken = new DateTime(2018, 1, 1, 1, 2, 3),
                OddsReq = 1,
                Stake = stake,
                OddsMatched = 3
            };
            MapReaderData(ticket);

            Reader["OddsRequest"].Returns(ticket.OddsReq);
            Reader["Stake" + baseCurrency.ToUpperInvariant()].Returns(baseCurrencyStake);
            Reader.Read().Returns(true, false);

            var metaModel = new ExchangeBetListDetailMetaModel
            {
                BaseCurrency = baseCurrency,
                Type = ExchangeMatchType.Back
            };

            // Action
            var result = DataAccess.GetTicketDetailList(metaModel).ElementAt(0);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.BetTaken, Is.EqualTo(ticket.BetTaken));
                Assert.That(result.BetTakenDateString, Is.EqualTo("01/01/2018 01:02:03"));
                Assert.That(result.OddsReq, Is.EqualTo(ticket.OddsReq));
                Assert.That(result.OddsMatched, Is.EqualTo(ticket.OddsMatched));
                Assert.That(result.Stake, Is.EqualTo(expectedStake));

                Assert.That(result.Liability, Is.EqualTo("-"));
            });
        }

        [Test]
        public void GetTicketDetailList_WhenObjectDbThrowsException_ThrowDataAccessException()
        {
            TestException_WhenObjectDbThrowsException_ThrowDataAccessException(
                TicketDetailUsp,
                () => DataAccess.GetTicketDetailList(new ExchangeBetListDetailMetaModel()));
        }
    }
}