namespace Fanex.Athena.Service.Report.UnitTests
{
    using System.Collections.Generic;
    using System.Linq;
    using DataAccess.Report.Interfaces;
    using Extension.Utility;
    using Model.Core.ResultModel;
    using Model.Report.DTO;
    using Model.Report.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using Report;
    using UnitTest.Common.Extensions;

    public class CashOutWinLossServiceTest
    {
        private ICashOutWinLossDataAccess _dataAccess;
        private CashOutWinLossService _service;

        [SetUp]
        public void SetUp()
        {
            _dataAccess = Substitute.For<ICashOutWinLossDataAccess>();
            _service = new CashOutWinLossService(_dataAccess);
        }

        [TearDown]
        public void TearDown()
        {
            _dataAccess = null;
            _service = null;
        }

        [Test]
        public void GetDetail_WhenCallWithValidModel_CallDataAccess()
        {
            // Arrange
            var metaModel = new ThirdPartyWinLossMetaModel();

            // Action
            _service.GetDetail(metaModel);

            // Assert
            _dataAccess.Received().GetDetail(metaModel);
        }

        [Test]
        public void GetSummary_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var metaModel = new ThirdPartyWinLossMetaModel();
            var fakeData = CreateFakeSummaryData();
            _dataAccess.GetSummary(metaModel).ReturnsForAnyArgs(fakeData);
            var expectedData = new SortableList<CashOutWinLoss>
            {
                new CashOutWinLoss
                {
                    RowSpan = -1
                },
                new CashOutWinLoss
                {
                    RowSpan = 1,
                    Index = 1
                }
            };

            // Action
            var result = _service.GetSummary(metaModel);

            // Assert
            Assert.That(result.DataList.ElementAt(0).RowSpan, Is.EqualTo(expectedData[0].RowSpan));
            Assert.That(result.DataList.ElementAt(1).RowSpan, Is.EqualTo(expectedData[1].RowSpan));
            Assert.That(result.DataList.ElementAt(1).Index, Is.EqualTo(expectedData[1].Index));
        }

        [Test]
        public void TotalCalc_WhenCallWithNullData_ReturnEmptyResult()
        {
            // Arrange
            List<CashOutWinLoss> dataList = null;

            // Action
            var result = _service.TotalCalc(dataList);

            // Assert
            Assert.That(result.Count, Is.EqualTo(0));
        }

        [Test]
        public void TotalCalc_WhenCallWithEmptyData_ReturnEmptyResult()
        {
            // Arrange
            var dataList = new List<CashOutWinLoss>();

            // Action
            var result = _service.TotalCalc(dataList);

            // Assert
            Assert.That(result.Count, Is.EqualTo(0));
        }

        [Test]
        public void TotalCalc_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var dataList = CreateFakeTotalData();

            var expectedResult = new List<CashOutWinLoss>
            {
                new CashOutWinLoss
                {
                    CurrencyName = "EUR",
                    CashOutTimes = 2,
                    CashOutTickets = 4,
                    CashOutValue = 20m,
                    BuyBackAmountSB = 22m,
                    BuyBackAmountBA = 24m,
                    TicketPayout = 26m,
                    CashOutCommission = 28m,
                    SettleAmountTicketPayout = 30m,
                    SettleAmountBuyBackComm = 32m,
                    SettleAmountTotal = 34m,
                }
            };

            // Action
            var result = _service.TotalCalc(dataList);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Count, Is.EqualTo(expectedResult.Count));
                Assert.That(result[0], Tobe.SameProperties(expectedResult[0]));
            });
        }

        private static SortableReportResultModel<CashOutWinLoss> CreateFakeSummaryData()
        {
            var dataList = new SortableList<CashOutWinLoss>
            {
                new CashOutWinLoss
                {
                    Date = new System.DateTime()
                },
                new CashOutWinLoss
                {
                    Date = new System.DateTime(2017, 4, 17)
                }
            };

            return new SortableReportResultModel<CashOutWinLoss>
            {
                DataList = dataList
            };
        }

        private static IList<CashOutWinLoss> CreateFakeTotalData()
        {
            return new List<CashOutWinLoss>
            {
                new CashOutWinLoss
                {
                    CurrencyName = "EUR",
                    CashOutTimes = 1,
                    CashOutTickets = 2,
                    CashOutValue = 10m,
                    BuyBackAmountSB = 11m,
                    BuyBackAmountBA = 12m,
                    TicketPayout = 13m,
                    CashOutCommission = 14m,
                    SettleAmountTicketPayout = 15m,
                    SettleAmountBuyBackComm = 16m,
                    SettleAmountTotal = 17m,
                },
                new CashOutWinLoss
                {
                    CurrencyName = "EUR",
                    CashOutTimes = 1,
                    CashOutTickets = 2,
                    CashOutValue = 10m,
                    BuyBackAmountSB = 11m,
                    BuyBackAmountBA = 12m,
                    TicketPayout = 13m,
                    CashOutCommission = 14m,
                    SettleAmountTicketPayout = 15m,
                    SettleAmountBuyBackComm = 16m,
                    SettleAmountTotal = 17m,
                }
            };
        }
    }
}