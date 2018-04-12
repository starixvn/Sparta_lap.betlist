namespace Fanex.Athena.DataAccess.Report.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Web.Script.Serialization;
    using Core.Configuration.Interfaces;
    using Core.UnitTests.TestHelpers;
    using Data;
    using Extension.Utility;
    using Fanex.Athena.Model.Core.DTO.Configuration;
    using Model.Core.ResultModel;
    using Model.Report.DTO;
    using Model.Report.MetaModel;
    using NSubstitute;
    using NUnit.Framework;

    public class CashOutWinLossDataAccessTest : BaseDataAccessHelperTest<CashOutWinLossDataAccess>
    {
        private const string GetSummarySp = "Lrf_Rpt_CashoutWinloss";

        private const string GetDetailSp = "Lrf_Rpt_CashoutWinloss_Detail";

        private BalanceUp _balance;
        private ThirdPartyWinLossMetaModel _metaModel;

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            var balanceUpDataAccess = Substitute.For<IBalanceUpDataAccess>();
            _balance = new BalanceUp
            {
                MoveBetsDay = new DateTime(2017, 1, 1)
            };
            balanceUpDataAccess.GetBalanceUp().Returns(_balance);

            DataAccess = Substitute.ForPartsOf<CashOutWinLossDataAccess>(balanceUpDataAccess);
            ObjectDb = Substitute.For<IObjectDb>();
        }

        [OneTimeTearDown]
        public override void FixtureTearDown()
        {
            _balance = null;
            _metaModel = null;

            base.FixtureTearDown();
        }

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            _metaModel = new ThirdPartyWinLossMetaModel
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

        [Test]
        public void GetSummary_WhenCall_ParameterPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(GetSummarySp)).Returns(ObjectDb);
            Reader.Read().Returns(false);

            // Action
            DataAccess.GetSummary(_metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                AssertUspParameters(isDrilldown: false);
                ObjectDb.Received(1).AddInParameter("ReportType", DbType.Int32, _metaModel.ReportBy);
            });
        }

        [Test]
        public void GetSummary_Level0AndConditionIsTrue_ReturnCorrectData()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(GetSummarySp)).Returns(ObjectDb);
            ConfigCheckingColumnForReader();
            Reader.Read().Returns(true, false);

            var fakeData = CreateFakeData();
            MapReaderData(fakeData);

            var serializer = new JavaScriptSerializer();
            var expectedCashoutWinLossData = serializer.Serialize(CreateExpectedCashoutWinLossData(1));
            var expectedSummaryTotal = serializer.Serialize(CreateExpectedSummaryTotalData());

            // Action
            var result = DataAccess.GetSummary(_metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(serializer.Serialize(result.DataList), Is.EqualTo(expectedCashoutWinLossData));
                Assert.That(serializer.Serialize(result.SummaryTotal), Is.EqualTo(expectedSummaryTotal));
            });
        }

        [Test]
        public void GetSummary_WhenCallWithLevel1AndConditionIsFalse_ReturnCorrectData()
        {
            // Arrange
            _metaModel.DrillDownLevel = 1;

            DataAccess.GetObjectDb(Arg.Is(GetSummarySp)).Returns(ObjectDb);
            Reader.Read().Returns(true, false);
            Reader.FieldCount.Returns(0);

            var fakeData = CreateFakeData();
            MapReaderData(fakeData);
            var serializer = new JavaScriptSerializer();

            var cashoutWinLossData = CreateExpectedCashoutWinLossData(1);
            cashoutWinLossData[0].Date = _metaModel.DrillDownFromDate;
            cashoutWinLossData[0].SystemId = string.Empty;
            cashoutWinLossData[0].ExternalId = string.Empty;
            cashoutWinLossData[0].CustomerId = 0;

            var expectedCashoutWinLossData = serializer.Serialize(cashoutWinLossData);

            // Action
            var result = DataAccess.GetSummary(_metaModel);

            // Assert
            Assert.That(serializer.Serialize(result.DataList), Is.EqualTo(expectedCashoutWinLossData));
        }

        [Test]
        public void GetSummary_WhenCall_ReturnCorrectRowSpan()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(GetSummarySp)).Returns(ObjectDb);
            ConfigCheckingColumnForReader();
            Reader.Read().Returns(true, true, true, false);

            const string GroupDate1 = "20170120";
            const string GroupDate2 = "20170122";
            var fakeData = CreateFakeData();
            MapReaderData(fakeData);
            Reader["reporttype"].Returns(GroupDate2, GroupDate1, GroupDate2);

            const int ExpectedRowSpan1 = 1;
            const int ExpectedRowSpan2 = 2;

            // Action
            var result = DataAccess.GetSummary(_metaModel);
            var firstGroupDate1 = result.DataList[0];
            var firstGroupDate2 = result.DataList[1];

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(firstGroupDate1.RowSpan, Is.EqualTo(ExpectedRowSpan1));
                Assert.That(firstGroupDate2.RowSpan, Is.EqualTo(ExpectedRowSpan2));
            });
        }

        [Test]
        public void GetSummary_WhenObjectDbThrowsException_ThrowDataAccessException()
        {
            TestException_WhenObjectDbThrowsException_ThrowDataAccessException(
                GetSummarySp,
                () => DataAccess.GetSummary(_metaModel));
        }

        [Test]
        public void GetDetail_WhenCall_ParameterPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(GetDetailSp)).Returns(ObjectDb);
            Reader.Read().Returns(false);

            // Action
            DataAccess.GetDetail(_metaModel);

            // Assert
            AssertUspParameters(isDrilldown: true);
        }

        [Test]
        public void GetDetail_WhenCall_ReturnCorrectData()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(GetDetailSp)).Returns(ObjectDb);
            Reader.Read().Returns(true, false);
            ConfigCheckingColumnForReader();

            var fakeData = CreateFakeData();
            MapReaderData(fakeData);
            var serializer = new JavaScriptSerializer();
            var expectedResult = new SortableReportResultModel<CashOutWinLoss>
            {
                DataList = CreateExpectedCashoutWinLossData(),
                TotalDataList = CreateExpectedCashoutWinLossTotalData(),
                SummaryTotal = CreateExpectedSummaryTotalData()
            };

            // Action
            var result = serializer.Serialize(DataAccess.GetDetail(_metaModel));

            // Assert
            Assert.That(result, Is.EqualTo(serializer.Serialize(expectedResult)));
        }

        [Test]
        public void GetDetail_WhenObjectDbThrowsException_ThrowDataAccessException()
        {
            TestException_WhenObjectDbThrowsException_ThrowDataAccessException(
                GetDetailSp,
                () => DataAccess.GetDetail(_metaModel));
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

        private static SortableList<CashOutWinLoss> CreateExpectedCashoutWinLossData(int rowSpan = 0)
        {
            return new SortableList<CashOutWinLoss>
            {
                new CashOutWinLoss
                {
                    RowSpan = rowSpan,
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

        private static SortableList<CashOutWinLoss> CreateExpectedCashoutWinLossTotalData(int rowSpan = 0)
        {
            var data = CreateExpectedCashoutWinLossData(rowSpan);
            data[0].Index = 0;
            data[0].Date = DateTime.MinValue;
            data[0].SystemId = null;
            data[0].ExternalId = null;
            data[0].CustomerId = 0;
            data[0].CurrencyId = 0;
            return data;
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

        private void ConfigCheckingColumnForReader()
        {
            Reader.FieldCount.Returns(3);
            Reader.GetName(0).Returns("systemid");
            Reader.GetName(1).Returns("externalid");
            Reader.GetName(2).Returns("custid");
        }

        private void AssertUspParameters(bool isDrilldown)
        {
            ObjectDb.Received(1).AddInParameter("fromdate", DbType.Date, isDrilldown ? _metaModel.DrillDownFromDate : _metaModel.FromDate);
            ObjectDb.Received(1).AddInParameter("todate", DbType.Date, isDrilldown ? _metaModel.DrillDownToDate : _metaModel.ToDate);
            ObjectDb.Received(1).AddInParameter("AgentList", DbType.String, _metaModel.AgentListXml);
            ObjectDb.Received(1).AddInParameter("MoveBetsDay", DbType.DateTime, _balance.MoveBetsDay);
        }
    }
}