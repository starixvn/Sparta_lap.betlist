namespace Fanex.Athena.DataAccess.Report.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Web.Script.Serialization;
    using Data;
    using Fanex.Athena.Model.Core.DTO.Configuration;
    using Model.Report.DTO;
    using Model.Report.MetaModel;
    using NSubstitute;
    using NUnit.Framework;

    public class CashOutWinLossDataAccessTest
    {
        private CashOutWinLossDataAccess _dataAccess;
        private BalanceUp _balance;
        private CashOutWinLossMetaModel _metaModel;
        private IObjectDb _objectDb;

        [SetUp]
        public void SetUp()
        {
            _balance = new BalanceUp
            {
                MoveBetsDay = new DateTime(2017, 1, 1)
            };
            _dataAccess = Substitute.ForPartsOf<CashOutWinLossDataAccess>(_balance);
            _objectDb = Substitute.For<IObjectDb>();
            _metaModel = new CashOutWinLossMetaModel
            {
                DrillDownFromDate = new DateTime(2017, 4, 1),
                DrillDownToDate = new DateTime(2017, 4, 30),
                FromDate = new DateTime(2017, 4, 10),
                ToDate = new DateTime(2017, 4, 20),
                AgentListXml = "xml string",
                ReportBy = 1,
                BaseCurrency = "EUR"
            };
        }

        [TearDown]
        public void TearDown()
        {
            _dataAccess = null;
            _balance = null;
            _metaModel = null;
            _objectDb = null;
        }

        [Test]
        public void GetSummary_WhenCall_ParameterPassedCorrectly()
        {
            // Arrange
            var reader = Substitute.For<IDataReader>();
            reader.Read().Returns(false);
            _objectDb.ExecuteReader().Returns(reader);
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_Rpt_CashoutWinloss")).Returns(_objectDb);
            IDictionary<string, CashOutWinLoss> summaryTotal = new Dictionary<string, CashOutWinLoss>();

            // Action
            _dataAccess.GetSummary(_metaModel, out summaryTotal);

            // Assert
            _objectDb.Received().AddInParameter("fromdate", DbType.Date, _metaModel.FromDate.Date);
            _objectDb.Received().AddInParameter("todate", DbType.Date, _metaModel.ToDate.Date);
            _objectDb.Received().AddInParameter("AgentList", DbType.String, _metaModel.AgentListXml);
            _objectDb.Received().AddInParameter("MoveBetsDay", DbType.DateTime, _balance.MoveBetsDay);
            _objectDb.Received().AddInParameter("ReportType", DbType.Int32, _metaModel.ReportBy);
        }

        [Test]
        public void GetSummary_WhenCallWithLevel0AndConditionIsTrue_ReturnCorrectData()
        {
            // Arrange
            var reader = Substitute.For<IDataReader>();
            reader.Read().Returns(true, false);
            _objectDb.ExecuteReader().Returns(reader);
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_Rpt_CashoutWinloss")).Returns(_objectDb);
            IDictionary<string, CashOutWinLoss> summaryTotal = new Dictionary<string, CashOutWinLoss>();
            ConfigCheckingColumnForReader(reader);

            var fakeData = CreateFakeData();
            MapReaderData(reader, fakeData);
            var serializer = new JavaScriptSerializer();
            var expectedCashoutWinLossData = serializer.Serialize(CreateExpectedCashoutWinLossData());
            var expectedSummaryTotal = serializer.Serialize(CreateExpectedSummaryTotalData());

            // Action
            var result = serializer.Serialize(_dataAccess.GetSummary(_metaModel, out summaryTotal));
            var summaryTotalResult = serializer.Serialize(summaryTotal);

            // Assert
            Assert.That(result, Is.EqualTo(expectedCashoutWinLossData));
            Assert.That(summaryTotalResult, Is.EqualTo(expectedSummaryTotal));
        }

        [Test]
        public void GetSummary_WhenCallWithLevel1AndConditionIsFalse_ReturnCorrectData()
        {
            // Arrange
            _metaModel.DrillDownLevel = 1;
            var reader = Substitute.For<IDataReader>();
            reader.Read().Returns(true, false);
            _objectDb.ExecuteReader().Returns(reader);
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_Rpt_CashoutWinloss")).Returns(_objectDb);
            IDictionary<string, CashOutWinLoss> summaryTotal = new Dictionary<string, CashOutWinLoss>();
            reader.FieldCount.Returns(0);

            var fakeData = CreateFakeData();
            MapReaderData(reader, fakeData);
            var serializer = new JavaScriptSerializer();

            var cashoutWinLossData = CreateExpectedCashoutWinLossData();
            cashoutWinLossData[0].Date = _metaModel.DrillDownFromDate;
            cashoutWinLossData[0].SystemId = string.Empty;
            cashoutWinLossData[0].ExternalId = string.Empty;
            cashoutWinLossData[0].CustomerId = 0;

            var expectedCashoutWinLossData = serializer.Serialize(cashoutWinLossData);

            // Action
            var result = serializer.Serialize(_dataAccess.GetSummary(_metaModel, out summaryTotal));

            // Assert
            Assert.That(result, Is.EqualTo(expectedCashoutWinLossData));
        }

        [Test]
        public void GetDetail_WhenCall_ParameterPassedCorrectly()
        {
            // Arrange
            var reader = Substitute.For<IDataReader>();
            reader.Read().Returns(false);
            _objectDb.ExecuteReader().Returns(reader);
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_Rpt_CashoutWinloss_Detail")).Returns(_objectDb);
            IDictionary<string, CashOutWinLoss> summaryTotal = new Dictionary<string, CashOutWinLoss>();

            // Action
            _dataAccess.GetDetail(_metaModel, out summaryTotal);

            // Assert
            _objectDb.Received().AddInParameter("fromdate", DbType.Date, _metaModel.DrillDownFromDate);
            _objectDb.Received().AddInParameter("todate", DbType.Date, _metaModel.DrillDownToDate);
            _objectDb.Received().AddInParameter("AgentList", DbType.String, _metaModel.AgentListXml);
            _objectDb.Received().AddInParameter("MoveBetsDay", DbType.DateTime, _balance.MoveBetsDay);
        }

        [Test]
        public void GetDetail_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var reader = Substitute.For<IDataReader>();
            reader.Read().Returns(true, false);
            _objectDb.ExecuteReader().Returns(reader);
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_Rpt_CashoutWinloss_Detail")).Returns(_objectDb);
            IDictionary<string, CashOutWinLoss> summaryTotal = new Dictionary<string, CashOutWinLoss>();
            ConfigCheckingColumnForReader(reader);

            var fakeData = CreateFakeData();
            MapReaderData(reader, fakeData);
            var serializer = new JavaScriptSerializer();
            var expectedCashoutWinLossData = serializer.Serialize(CreateExpectedCashoutWinLossData());
            var expectedSummaryTotal = serializer.Serialize(CreateExpectedSummaryTotalData());

            // Action
            var result = serializer.Serialize(_dataAccess.GetDetail(_metaModel, out summaryTotal));
            var summaryTotalResult = serializer.Serialize(summaryTotal);

            // Assert
            Assert.That(result, Is.EqualTo(expectedCashoutWinLossData));
            Assert.That(summaryTotalResult, Is.EqualTo(expectedSummaryTotal));
        }

        private static Dictionary<string, object> CreateFakeData()
        {
            return new Dictionary<string, object>
            {
                { "systemid", "1" }, { "externalid", "2" }, { "custid", 3 },
                { "cashoutvalueeur", 10m }, { "buybackamountSBeur", 10.5m }, { "buybackamountBAeur", 11m },
                { "ticketpayouteur", 11.5m }, { "cashoutcommeur", 12m }, { "AmountDue_TicketPayouteur", 13m },
                { "AmountDue_Commeur", 14m }, { "Amount_Totaleur", 15m }, { "currencyid", 1 }, { "currencyname", "USD" },
                { "cashouttime", 2 }, { "cashoutticket", 3 }, { "reporttype", "20170417" },
                { "cashoutvalueEUR", 10m }, { "buybackamountSBEUR", 10.5m }, { "buybackamountBAEUR", 11m },
                { "ticketpayoutEUR", 11.5m }, { "cashoutcommEUR", 12m }, { "AmountDue_TicketPayoutEUR", 13m },
                { "AmountDue_CommEUR", 14m }, { "Amount_TotalEUR", 15m },
                { "cashoutvalueUSD", 10m }, { "buybackamountSBUSD", 10.5m }, { "buybackamountBAUSD", 11m },
                { "ticketpayoutUSD", 11.5m }, { "cashoutcommUSD", 12m }, { "AmountDue_TicketPayoutUSD", 13m },
                { "AmountDue_CommUSD", 14m }, { "Amount_TotalUSD", 15m },
                { "cashoutvalueGBP", 10m }, { "buybackamountSBGBP", 10.5m }, { "buybackamountBAGBP", 11m },
                { "ticketpayoutGBP", 11.5m }, { "cashoutcommGBP", 12m }, { "AmountDue_TicketPayoutGBP", 13m },
                { "AmountDue_CommGBP", 14m }, { "Amount_TotalGBP", 15m },
            };
        }

        private static List<CashOutWinLoss> CreateExpectedCashoutWinLossData()
        {
            return new List<CashOutWinLoss>
            {
                new CashOutWinLoss
                {
                    Index = 1,
                    SystemId = "1",
                    ExternalId = "2",
                    CustomerId = 3,
                    CurrencyId = 1,
                    CurrencyName = "USD",
                    CashOutTimes = 2,
                    CashOutValue = 10m,
                    CashOutTickets = 3,
                    BuyBackAmountSB = 10.5m,
                    BuyBackAmountBA = 11m,
                    TicketPayout = 11.5m,
                    CashOutCommission = 12m,
                    SettleAmountTicketPayout = 13m,
                    SettleAmountBuyBackComm = 14m,
                    SettleAmountTotal = 15m,
                    Date = new DateTime(2017, 4, 17)
                }
            };
        }

        private static IDictionary<string, CashOutWinLoss> CreateExpectedSummaryTotalData()
        {
            return new Dictionary<string, CashOutWinLoss>
            {
                { "EUR",
                    new CashOutWinLoss
                    {
                        CashOutValue = 10m,
                        CashOutTickets = 3,
                        CashOutTimes = 2,
                        BuyBackAmountSB = 10.5m,
                        BuyBackAmountBA = 11m,
                        TicketPayout = 11.5m,
                        CashOutCommission = 12m,
                        SettleAmountTicketPayout = 13m,
                        SettleAmountBuyBackComm = 14m,
                        SettleAmountTotal = 15m
                    }
                },
                { "USD",
                    new CashOutWinLoss
                    {
                        CashOutValue = 10m,
                        CashOutTickets = 3,
                        CashOutTimes = 2,
                        BuyBackAmountSB = 10.5m,
                        BuyBackAmountBA = 11m,
                        TicketPayout = 11.5m,
                        CashOutCommission = 12m,
                        SettleAmountTicketPayout = 13m,
                        SettleAmountBuyBackComm = 14m,
                        SettleAmountTotal = 15m
                    }
                },
                { "GBP",
                    new CashOutWinLoss
                    {
                        CashOutValue = 10m,
                        CashOutTickets = 3,
                        CashOutTimes = 2,
                        BuyBackAmountSB = 10.5m,
                        BuyBackAmountBA = 11m,
                        TicketPayout = 11.5m,
                        CashOutCommission = 12m,
                        SettleAmountTicketPayout = 13m,
                        SettleAmountBuyBackComm = 14m,
                        SettleAmountTotal = 15m
                    }
                }
            };
        }

        private static void ConfigCheckingColumnForReader(IDataReader reader)
        {
            reader.FieldCount.Returns(3);
            reader.GetName(0).Returns("systemid");
            reader.GetName(1).Returns("externalid");
            reader.GetName(2).Returns("custid");
        }

        private static void MapReaderData(IDataReader reader, Dictionary<string, object> readerDataMapping)
        {
            reader[Arg.Any<string>()].Returns(x =>
            {
                var paramName = x.ArgAt<string>(0);
                if (readerDataMapping.ContainsKey(paramName))
                {
                    return readerDataMapping[paramName];
                }

                return string.Empty;
            });
        }
    }
}