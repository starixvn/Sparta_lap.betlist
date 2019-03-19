namespace Fanex.Athena.BetList.Controller.UnitTests
{
    using System.Linq;
    using System.Net;
    using System.Web;
    using System.Web.Mvc;
    using Common.Constant;
    using Common.Utility.Collections;
    using Common.Utility.Services.Interfaces;
    using Excel.Model;
    using Model.DTO;
    using Model.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using Resources;
    using Service.Interfaces;
    using UnitTest.Common.Helpers;

    public class BettingTicketCenterMultiCustomerControllerTest : BaseControllerHelperTest
    {
        private const int RowsExceedLimitErrorCode = 1001;

        private IAppSettings _appSettings;

        private IBettingTicketCenterService _service;

        private ITempFileService _tempFileService;

        [SetUp]
        public void SetUp()
        {
            _appSettings = Substitute.For<IAppSettings>();
            _service = Substitute.For<IBettingTicketCenterService>();
            _tempFileService = Substitute.For<ITempFileService>();
        }

        [TearDown]
        public void TearDown()
        {
            _appSettings = null;
            _service = null;
            _tempFileService = null;
        }

        [Test]
        public void GetData_WhenCall_ReturnCorrectData()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = FakeHelper.Create<MultipleCustomersBetListMetaModel>(x => x.IsExport, false);

                var ticketResult = new TicketResult
                {
                    TotalRows = FakeHelper.Create<int>(),
                    SumStake = FakeHelper.Create<decimal>(),
                    SumLicenseeWL = FakeHelper.Create<decimal>(),
                    SumComm = FakeHelper.Create<decimal>(),
                };
                _service.GetBetListForMultipleCustomers(metaModel, metaModel.UserCurrencyList).Returns(ticketResult);

                var ticketText = FakeHelper.CreateMany<TicketText>(2);
                _service.BuildTicketText(ticketResult, metaModel, metaModel.HasPlayerCom).Returns(ticketText.ToList());

                var sumWL = FakeHelper.Create<string>();
                _service.BuildSumWinLoss(ticketResult, metaModel.HasPlayerCom).Returns(sumWL);

                // Action
                var result = (controller.GetData(metaModel) as JsonResult).Data;
                var data = result.GetType().GetProperty("Data").GetValue(result, null);
                var total = result.GetType().GetProperty("Total").GetValue(result, null);
                var totalRows = result.GetType().GetProperty("TotalRows").GetValue(result, null);

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(data, Is.EqualTo(ticketText));
                    Assert.That(total.GetType().GetProperty("SumWinLoss").GetValue(total, null), Is.EqualTo(sumWL));
                    Assert.That(
                        total.GetType().GetProperty("SumLicenseeWL").GetValue(total, null),
                        Is.EqualTo((ticketResult.SumLicenseeWL + ticketResult.SumComm).ToString(FormatKey.DecimalFormat)));
                    Assert.That(
                        total.GetType().GetProperty("SumStake").GetValue(total, null),
                        Is.EqualTo(ticketResult.SumStake.ToString(FormatKey.DecimalFormat)));
                    Assert.That(totalRows, Is.EqualTo(ticketResult.TotalRows));
                    AssertClearData(ticketResult);
                });
            }
        }

        [Test]
        public void GetData_WhenExport_RowsExceedLimitation_ReturnError()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = FakeHelper.Create<MultipleCustomersBetListMetaModel>(x => x.IsExport, true);

                var totalRow = FakeHelper.Create<int>();
                _service.GetTotalRowForMultipleCustomers(metaModel).Returns(totalRow);
                _appSettings.GetValue("MaxRowSupportExportExcel").Returns((totalRow - 1).ToString());

                // Action
                var result = (controller.GetData(metaModel) as JsonResult).Data;
                var errorCode = result.GetType().GetProperty("ErrorCode").GetValue(result, null);

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(errorCode, Is.EqualTo(RowsExceedLimitErrorCode));
                });
            }
        }

        [Test]
        public void GetData_WhenExportExcel_RowsExceedMaxRowPerFile_NoMainTicket_ReturnError()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = FakeHelper.Create<MultipleCustomersBetListMetaModel>(x => x.IsExport, true);

                var totalRow = FakeHelper.Create<int>();
                _service.GetTotalRowForMultipleCustomers(metaModel).Returns(totalRow);
                _appSettings.GetValue("MaxRowSupportExportExcel").Returns((totalRow + 1).ToString());
                _appSettings.GetValue("MaxRowPerExcelFile").Returns((totalRow + 1).ToString());

                var ticketResult = new TicketResult();
                _service.GetBetListForMultipleCustomers(metaModel, metaModel.UserCurrencyList).Returns(ticketResult);

                // Action
                var result = controller.GetData(metaModel) as HttpStatusCodeResult;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.NoContent));
                });
            }
        }

        [Test]
        public void GetData_WhenExportExcel_ReturnExcelFile()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = FakeHelper.Create<MultipleCustomersBetListMetaModel>(x => x.IsExport, true);

                var totalRow = FakeHelper.Create<int>();
                _service.GetTotalRowForMultipleCustomers(metaModel).Returns(totalRow);
                _appSettings.GetValue("MaxRowSupportExportExcel").Returns((totalRow + 1).ToString());
                _appSettings.GetValue("MaxRowPerExcelFile").Returns((totalRow + 1).ToString());

                var ticket = FakeHelper.Create<LrfTicket>();
                var ticketResult = new TicketResult { MainTicketList = new SortableList<LrfTicket> { ticket } };
                _service.GetBetListForMultipleCustomers(metaModel, metaModel.UserCurrencyList).Returns(ticketResult);

                var title = FakeHelper.Create<string>();
                _service.GetTitle(metaModel).Returns(title);

                controller.WhenForAnyArgs(x => x.ReturnExcel(Arg.Any<BettingTicketCenterExcelModel>(), Arg.Any<TicketResult>())).DoNotCallBase();

                // Action
                controller.GetData(metaModel);

                // Assert
                Assert.Multiple(() =>
                {
                    controller.Received().ReturnExcel(
                        Arg.Is<BettingTicketCenterExcelModel>(x => x.Title == title
                            && x.Option.ShowTotal
                            && x.Option.ShowSumStake
                            && x.Option.ShowLicenseeWl
                            && x.Option.ShowWinLoss
                            && x.Option.TotalText == $"{ViewResources.Totalin} {metaModel.BaseCurrency.ToUpperInvariant()}"
                            && x.CustomerCommVisible == metaModel.HasPlayerCom
                            && x.Data == ticketResult),
                        ticketResult);
                });
            }
        }

        [Test]
        public void GetData_WhenExportZipFile_ClientIsNotConnected_ReturnNull()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = FakeHelper.Create<MultipleCustomersBetListMetaModel>(x => x.IsExport, true);

                var totalRow = FakeHelper.Create<int>();
                _service.GetTotalRowForMultipleCustomers(metaModel).Returns(totalRow);
                _appSettings.GetValue("MaxRowSupportExportExcel").Returns((totalRow + 1).ToString());

                var maxRowPerFile = totalRow - 1;
                _appSettings.GetValue("MaxRowPerExcelFile").Returns(maxRowPerFile.ToString());

                var ticketResult = new GroupTicketResult();
                _service.GetBetListForMultipleCustomers(metaModel, maxRowPerFile, metaModel.UserCurrencyList).Returns(ticketResult);

                var title = FakeHelper.Create<string>();
                _service.GetTitle(metaModel).Returns(title);

                controller.WhenForAnyArgs(x => x.ReturnExcel(Arg.Any<BettingTicketCenterExcelModel>(), Arg.Any<TicketResult>())).DoNotCallBase();

                SetUpForResponse(controller, false);

                // Action
                var result = controller.GetData(metaModel);

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(result, Is.Null);
                });
            }
        }

        private BettingTicketCenterMultiCustomerController CreateController()
        {
            return Substitute.ForPartsOf<BettingTicketCenterMultiCustomerController>(_appSettings, _service, _tempFileService);
        }

        private void SetUpForResponse(BettingTicketCenterMultiCustomerController controller, bool isClientConnected)
        {
            var response = Substitute.For<HttpResponseBase>();
            response.IsClientConnected.Returns(isClientConnected);

            var httpContext = Substitute.For<HttpContextBase>();
            httpContext.Response.Returns(response);

            var context = new ControllerContext(httpContext, new System.Web.Routing.RouteData(), controller);
            controller.ControllerContext = context;
        }
    }
}