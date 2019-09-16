namespace Fanex.Athena.BetList.Controller.UnitTests
{
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Mvc;
    using Common.Constant;
    using Common.Utility.Collections;
    using Common.Utility.Services.Interfaces;
    using Excel.Model;
    using Fanex.Athena.BetList.Excel;
    using Fanex.Athena.BetList.Excel.Builders;
    using Model.DTO;
    using Model.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using Resources;
    using Service.Interfaces;
    using UnitTest.Common.Extensions;
    using UnitTest.Common.Helpers;
    using Utility;

    public class BettingTicketCenterMultiCustomerControllerTest : BaseControllerHelperTest
    {
        private const int RowsExceedLimitErrorCode = 1001;

        private IAppSettings _appSettings;

        private IBettingTicketCenterService _service;

        private IBettingTicketCenterZipBuilder _zipBuilder;
        private IBettingTicketCenterExcel _bettingTicketCenterExcel;
        private  ITicketBuilderService _ticketBuilderService;
        [SetUp]
        public void SetUp()
        {
            _ticketBuilderService = Substitute.For<ITicketBuilderService>();
            _appSettings = Substitute.For<IAppSettings>();
            _service = Substitute.For<IBettingTicketCenterService>();
            _zipBuilder = Substitute.For<IBettingTicketCenterZipBuilder>();
            _bettingTicketCenterExcel = Substitute.For<IBettingTicketCenterExcel>();
        }

        [TearDown]
        public void TearDown()
        {
            _appSettings = null;
            _service = null;
            _zipBuilder = null;
            _bettingTicketCenterExcel = null;
        }

        [Test]
        public async Task GetData_WhenCall_ReturnCorrectData()
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
                _service.GetBetListForMultipleCustomersAsync(metaModel, metaModel.UserCurrencyList).Returns(ticketResult);

                var ticketText = FakeHelper.CreateMany<TicketText>(2);
                _ticketBuilderService.BuildTicketText(ticketResult, metaModel, metaModel.HasPlayerCom).Returns(ticketText.ToList());

                var sumWL = FakeHelper.Create<string>();
                _service.BuildSumWinLoss(ticketResult, metaModel.HasPlayerCom).Returns(sumWL);

                // Action
                var result = (await controller.GetData(metaModel) as JsonResult).Data;
                var data = result.GetValue("Data");
                var total = result.GetValue("Total");
                var totalRows = result.GetValue("TotalRows");

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(data, Is.EqualTo(ticketText));
                    Assert.That(total.GetValue("SumWinLoss"), Is.EqualTo(sumWL));
                    Assert.That(
                        total.GetValue("SumLicenseeWL"),
                        Is.EqualTo((ticketResult.SumLicenseeWL + ticketResult.SumComm).ToString(FormatKey.DecimalFormat)));
                    Assert.That(
                        total.GetValue("SumStake"),
                        Is.EqualTo(ticketResult.SumStake.ToString(FormatKey.DecimalFormat)));
                    Assert.That(totalRows, Is.EqualTo(ticketResult.TotalRows));
                    AssertClearData(ticketResult);
                });
            }
        }

        [Test]
        public async Task GetData_WhenExport_RowsExceedLimitation_ReturnError()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = FakeHelper.Create<MultipleCustomersBetListMetaModel>(x => x.IsExport, true);

                var totalRow = FakeHelper.Create<int>();
                _service.GetTotalRowForMultipleCustomersAsync(metaModel).Returns(totalRow);
                _appSettings.GetValue("MaxRowSupportExportExcel").Returns((totalRow - 1).ToString());

                // Action
                var result = (await controller.GetData(metaModel) as JsonResult).Data;
                var errorCode = result.GetValue("ErrorCode");

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(errorCode, Is.EqualTo(RowsExceedLimitErrorCode));
                });
            }
        }

        [Test]
        public async Task GetData_WhenExportExcel_RowsExceedMaxRowPerFile_NoMainTicket_ReturnError()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = FakeHelper.Create<MultipleCustomersBetListMetaModel>(x => x.IsExport, true);

                var totalRow = FakeHelper.Create<int>();
                _service.GetTotalRowForMultipleCustomersAsync(metaModel).Returns(totalRow);
                _appSettings.GetValue("MaxRowSupportExportExcel").Returns((totalRow + 1).ToString());
                _appSettings.GetValue("MaxRowPerExcelFile").Returns((totalRow + 1).ToString());

                var ticketResult = new TicketResult();
                _service.GetBetListForMultipleCustomersAsync(metaModel, metaModel.UserCurrencyList).Returns(ticketResult);

                // Action
                var result = await controller.GetData(metaModel) as HttpStatusCodeResult;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.NoContent));
                });
            }
        }

        [Test]
        public async Task GetData_WhenExportExcel_ReturnExcelFile()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = FakeHelper.Create<MultipleCustomersBetListMetaModel>(x => x.IsExport, true);

                var totalRow = FakeHelper.Create<int>();
                _service.GetTotalRowForMultipleCustomersAsync(metaModel).Returns(totalRow);
                _appSettings.GetValue("MaxRowSupportExportExcel").Returns((totalRow + 1).ToString());
                _appSettings.GetValue("MaxRowPerExcelFile").Returns((totalRow + 1).ToString());

                var ticket = FakeHelper.Create<LrfTicket>();
                var ticketResult = new TicketResult { MainTicketList = new SortableList<LrfTicket> { ticket } };
                _service.GetBetListForMultipleCustomersAsync(metaModel, metaModel.UserCurrencyList).Returns(ticketResult);

                var title = FakeHelper.Create<string>();
                _service.GetTitle(metaModel).Returns(title);

                controller.WhenForAnyArgs(x => x.ReturnExcel(Arg.Any<BettingTicketCenterExcelModel>(), Arg.Any<TicketResult>())).DoNotCallBase();

                // Action
                await controller.GetData(metaModel);

                // Assert
                Assert.Multiple(() =>
                {
                    controller.Received().ReturnExcel(
                        Arg.Is<BettingTicketCenterExcelModel>(x => x.Title == title
                            && x.Option.ShowTotal
                            && x.Option.ShowSumStake
                            && x.Option.ShowLicenseeWl
                            && x.Option.ShowWinLoss
                            && x.Option.TotalText == $"{ViewResources.Totalin} {metaModel.BaseCurrencyName}"
                            && x.CustomerCommVisible == metaModel.HasPlayerCom
                            && x.Data == ticketResult),
                        ticketResult);
                });
            }
        }

        [Test]
        public async Task GetData_ExportZipFile_ZipBuilderReturnNull_ReturnNull()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = FakeHelper.Create<MultipleCustomersBetListMetaModel>(x => x.IsExport, true);

                var totalRow = FakeHelper.Create<int>();
                _service.GetTotalRowForMultipleCustomersAsync(metaModel).Returns(totalRow);
                _appSettings.GetValue("MaxRowSupportExportExcel").Returns((totalRow + 1).ToString());

                var maxRowPerFile = totalRow - 1;
                _appSettings.GetValue("MaxRowPerExcelFile").Returns(maxRowPerFile.ToString());

                var ticketResult = new GroupTicketResult();
                _service.GetBetListForMultipleCustomersAsync(metaModel, maxRowPerFile, metaModel.UserCurrencyList).Returns(ticketResult);

                controller.WhenForAnyArgs(x => x.ReturnExcel(Arg.Any<BettingTicketCenterExcelModel>(), Arg.Any<TicketResult>())).DoNotCallBase();

                SetUpForResponse(controller, false);
                _zipBuilder.BuildZip(
                    controller.HttpContext.Response,
                    ticketResult,
                    Arg.Any<string>(),
                    Arg.Any<bool>(),
                    Arg.Any<string>()).Returns(x => null);

                // Action
                var result = await controller.GetData(metaModel);

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(result, Is.Null);
                });
            }
        }

        private BettingTicketCenterMultiCustomerController CreateController()
        {
            return Substitute.ForPartsOf<BettingTicketCenterMultiCustomerController>(
                _appSettings,
                _service,
                _zipBuilder,
                _bettingTicketCenterExcel,
                _ticketBuilderService
                );
        }

        private static void SetUpForResponse(BettingTicketCenterMultiCustomerController controller, bool isClientConnected)
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