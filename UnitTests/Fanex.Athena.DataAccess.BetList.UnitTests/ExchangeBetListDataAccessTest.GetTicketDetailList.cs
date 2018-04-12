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

    public class ExchangeBetListDataAccessTest_GetTicketDetailList : BaseDataAccessHelperTest<ExchangeBetListDataAccess>
    {
        private const string TicketDetailUsp = "Lrf_BetList_Exchange_Detail";

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