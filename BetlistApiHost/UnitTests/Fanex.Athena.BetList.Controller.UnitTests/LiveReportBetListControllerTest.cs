namespace Fanex.Athena.BetList.Controller.UnitTests
{
    using System;
    using System.Linq;
    using Common.Utility.Helpers;
    using Model.DTO;
    using Model.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using Service.Interfaces;
    using UnitTest.Common.Helpers;

    public class LiveReportBetListControllerTest : BaseControllerHelperTest
    {
        private const string AgentListXml = "<xml/>";

        private IBetListService _betListService;

        [SetUp]
        public void SetUp()
        {
            _betListService = Substitute.For<IBetListService>();
        }

        [TearDown]
        public void TearDown()
        {
            _betListService = Substitute.For<IBetListService>();
        }

        [Test]
        public void GetData_WhenCall_ReturnCorrectData()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = FakeHelper.Create<LiveReportBetListMetaModel>();

                var ticketResult = new TicketResult
                {
                    SumWinLoss = FakeHelper.Create<decimal>(),
                    SumPlayerComm = FakeHelper.Create<decimal>(),
                    SumLicenseeWL = FakeHelper.Create<decimal>(),
                    SumComm = FakeHelper.Create<decimal>(),
                };
                _betListService.GetBetListData(metaModel).Returns(ticketResult);

                var ticketText = FakeHelper.CreateMany<TicketText>(2);
                _betListService.BuildTicketTextList(ticketResult).Returns(ticketText.ToList());

                // Action
                var result = controller.GetData(metaModel).Data;
                var main = result.GetType().GetProperty("Main").GetValue(result, null);
                var total = result.GetType().GetProperty("Total").GetValue(result, null);

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(main, Is.EqualTo(ticketText));
                    Assert.That(total.GetType().GetProperty("SumWinLoss").GetValue(total, null), Is.EqualTo(ticketResult.SumWinLoss + ticketResult.SumPlayerComm));
                    Assert.That(total.GetType().GetProperty("SumLicenseeWl").GetValue(total, null), Is.EqualTo(ticketResult.SumLicenseeWL + ticketResult.SumComm));
                    AssertClearData(ticketResult);
                });
            }
        }

        [TestCase(1, 20, 50, 1, 3)]
        [TestCase(2, 30, 60, 31, 2)]
        [TestCase(3, 10, 60, 21, 6)]
        public void GetPagingData_WhenCall_ReturnCorrectData(int pageIndex, int pageSize, int totalRows, int startIndex, int totalPage)
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = FakeHelper.Create<LiveReportBetListMetaModel>();
                metaModel.PageIndex = pageIndex;
                metaModel.PageSize = pageSize;
                metaModel.TotalRows = totalRows;

                var ticketResult = new TicketResult();
                _betListService.GetBetListData(metaModel).Returns(ticketResult);

                var ticketText = FakeHelper.CreateMany<TicketText>(2);
                _betListService.BuildTicketTextList(ticketResult, metaModel, startIndex).Returns(ticketText.ToList());

                // Action
                var result = controller.GetPagingData(metaModel).Data;
                var page = result.GetType().GetProperty("page").GetValue(result, null);
                var total = result.GetType().GetProperty("total").GetValue(result, null);
                var records = result.GetType().GetProperty("records").GetValue(result, null);
                var rows = result.GetType().GetProperty("rows").GetValue(result, null);
                var isHasCustComm = result.GetType().GetProperty("IsHasCustComm").GetValue(result, null);

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(page, Is.EqualTo(pageIndex));
                    Assert.That(total, Is.EqualTo(totalPage));
                    Assert.That(records, Is.EqualTo(totalRows));
                    Assert.That(rows, Is.EqualTo(ticketText));
                    Assert.That(isHasCustComm, Is.EqualTo(metaModel.HasPlayerCom));
                    AssertClearData(ticketResult);
                });
            }
        }

        [TestCase(1.123, 2.345, "3.47")]
        [TestCase(1.123, 2.34, "3.46")]
        [TestCase(-1.123, -2.345, "<span class='negative-number'>-3.47</span>")]
        public void GetPagingData_WhenCall_ReturnCorrectLicenseeWLText(decimal sumLicenseeWL, decimal sumComm, string expectedLicenseeWLText)
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = FakeHelper.Create<LiveReportBetListMetaModel>();
                metaModel.SumLicenseeWL = sumLicenseeWL;
                metaModel.SumComm = sumComm;

                var ticketResult = new TicketResult();
                _betListService.GetBetListData(metaModel).Returns(ticketResult);

                var ticketText = FakeHelper.CreateMany<TicketText>(2);
                _betListService.BuildTicketTextList(ticketResult, metaModel, Arg.Any<int>()).Returns(ticketText.ToList());

                var expectedWinloss = NumberHelper.DecFormat(metaModel.SumWinlost + metaModel.SumPlayerComm, 2);

                // Action
                var result = controller.GetPagingData(metaModel).Data;
                var userdata = result.GetType().GetProperty("userdata").GetValue(result, null);

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(userdata.GetType().GetProperty("WinLoss").GetValue(userdata, null), Is.EqualTo(expectedWinloss));
                    Assert.That(userdata.GetType().GetProperty("LicenseeWL").GetValue(userdata, null), Is.EqualTo(expectedLicenseeWLText));
                });
            }
        }

        private LiveReportBetListController CreateController()
        {
            return new LiveReportBetListController(_betListService);
        }
    }
}